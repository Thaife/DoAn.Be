using Dapper;
using MISA.WEB08.AMIS.Common.Attributes;
using MISA.WEB08.AMIS.Common.Resources;
using MISA.WEB08.AMIS.Common.Result;
using MySqlConnector;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MISA.WEB08.AMIS.DL
{
    /// <summary>
    /// Dữ liệu thao tác với Database và trả về với bảng trong Database từ tầng DL
    /// </summary>
    /// Create by: Nguyễn Khắc Tiềm (21/09/2022)
    public class BaseDL<T> : CustomParameter<T>, IBaseDL<T>
    {

        #region Field

        private IDatabaseHelper<T> _dbHelper;

        #endregion

        #region Contructor

        public BaseDL(IDatabaseHelper<T> dbHelper)
        {
            _dbHelper = dbHelper;
        }

        #endregion

        #region Method

        /// <summary>
        /// Hàm Lấy danh sách tất cả bản ghi trong 1 bảng
        /// </summary>
        /// <returns>Danh sách tất cả bản ghi</returns>
        /// Create by: Nguyễn Khắc Tiềm (26/09/2022)
        public virtual object GetAllRecords()
        {
            string storeProcedureName = string.Format(Resource.Proc_GetAll, typeof(T).Name);
            return _dbHelper.RunProcWithQuery(storeProcedureName, null);
        }

        /// <summary>
        /// Hàm Lấy danh sách dropdown
        /// </summary>
        /// <returns>Danh sách tất cả bản ghi</returns>
        /// Create by: Nguyễn Khắc Tiềm (26/09/2022)
        public virtual object GetDropdown()
        {
            string storeProcedureName = string.Format(Resource.Proc_GetDropdown, typeof(T).Name);
            return _dbHelper.RunProcWithQuery(storeProcedureName, null);
        }

        /// <summary>
        /// Hàm lấy ra bản ghi theo ID
        /// </summary>
        /// <param name="recordID">ID bản ghi</param>
        /// <param name="stateForm">Trạng thái lấy (sửa hay nhân bản, ...)</param>
        /// <returns>Thông tin chi tiết một bản ghi</returns>
        /// Create by: Nguyễn Khắc Tiềm (26/09/2022)
        public virtual object GetRecordByID(string recordID, string? stateForm)
        {
            // Khởi tạo các parameter để chèn vào trong Proc
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add($"v_{typeof(T).Name}ID", recordID);
            parameters.Add($"v_StateForm", stateForm);
            // Khai báo stored procedure
            string storeProcedureName = string.Format(Resource.Proc_GetDetailOne, typeof(T).Name);
            return _dbHelper.RunProcWithQueryFirstOrDefault(storeProcedureName, parameters);
        }

        /// <summary>
        /// Hàm lấy ra mã record tự sinh
        /// </summary>
        /// <returns>Mã bản ghi</returns>
        /// Create by: Nguyễn Khắc Tiềm (26/09/2022)
        public virtual object GetRecordCodeNew()
        {
            object result;
            // chuẩn bị câu lệnh MySQL
            string storeProcedureName = "Proc_coderecord_GetNewCode";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("v_TableName", typeof(T).Name);
            using (var mysqlConnection = new MySqlConnection(DataContext.MySqlConnectionString))
            {
                //nếu như kết nối đang đóng thì tiến hành mở lại
                if (mysqlConnection.State != ConnectionState.Open)
                {
                    mysqlConnection.Open();
                }
                // thực hiện gọi vào DB
                result = mysqlConnection.QueryFirstOrDefault<string>(
                    storeProcedureName,
                    parameters,
                    commandType: CommandType.StoredProcedure
                    );
                if (mysqlConnection.State == ConnectionState.Open)
                {
                    mysqlConnection.Close();
                }
            }
            return result;
        }

        /// <summary>
        /// Hàm lấy ra danh sách record có lọc và phân trang
        /// </summary>
        /// <param name="offset">Thứ tự bản ghi bắt đầu lấy</param>
        /// <param name="limit">Số lượng bản ghi muốn lấy</param>
        /// <param name="keyword">Từ khoá tìm kiếm</param>
        /// <param name="sort">Trường muốn sắp xếp</param>
        /// <param name="v_Query">Lọc theo yêu cầu</param>
        /// <param name="v_Select">Trường muốn select</param>
        /// <returns>Danh sách record và tổng số bản ghi</returns>
        /// Create by: Nguyễn Khắc Tiềm (26/09/2022)
        public virtual Paging GetFitterRecords(int offset, int limit, string? keyword, string? sort, string v_Query, string v_Select)
        {
            Paging result;
            // Khởi tạo các parameter để chèn vào trong Proc
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("v_Offset", offset);
            parameters.Add("v_Limit", limit);
            parameters.Add("v_Sort", sort);
            parameters.Add("v_Where", keyword);
            parameters.Add("v_Query", v_Query);
            parameters.Add("v_Select", v_Select != "" ?  ','+v_Select : "");

            // chuẩn bị câu lệnh MySQL
            string storeProcedureName = string.Format(Resource.Proc_GetFilterPaging, typeof(T).Name);

            using (var mysqlConnection = new MySqlConnection(DataContext.MySqlConnectionString))
            {
                //nếu như kết nối đang đóng thì tiến hành mở lại
                if (mysqlConnection.State != ConnectionState.Open)
                {
                    mysqlConnection.Open();
                }
                // thực hiện gọi vào DB
                var records = mysqlConnection.QueryMultiple(
                    storeProcedureName,
                    parameters,
                    commandType: CommandType.StoredProcedure
                    );
                result = new Paging
                {
                    RecordList = records.Read<T>().ToList(),
                };
                if(limit > 0)
                {
                    result.TotalCount = records.ReadSingle().totalCount;
                    CustomResultProc(records, ref result);
                }
                if (mysqlConnection.State == ConnectionState.Open)
                {
                    mysqlConnection.Close();
                }
            }
            return result;
        }

        /// <summary>
        /// Hàm thêm mới một bản ghi
        /// </summary>
        /// <param name="record"></param>
        /// <returns>ID bản ghi sau khi thêm</returns>
        /// Create by: Nguyễn Khắc Tiềm (26/09/2022)
        public virtual ServiceResponse InsertRecord(T record, string userID)
        {
            var v_MessOut = "";
            // tạo recordID
            Guid recordID = Guid.NewGuid();
            // Khởi tạo các parameter để chèn vào trong Proc
            DynamicParameters parameters = new DynamicParameters();
            CustomParameterForCreate(ref parameters, record);
            parameters.Add("v_UserID", userID);
            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                string propertyName = property.Name;
                // Kiểm tra trường nào là khoá chính thì thêm param là id tự sinh Guid
                var primaryKeyAttribute = (ValidateAttribute?)Attribute.GetCustomAttribute(property, typeof(ValidateAttribute));
                if (primaryKeyAttribute != null && primaryKeyAttribute.PrimaryKey)
                {
                    parameters.Add($"v_{propertyName}", recordID);
                }
                else if(primaryKeyAttribute != null && primaryKeyAttribute.NotMapParameterProc == true)
                {
                    continue;
                }
                else
                {
                    var propertyValue = property.GetValue(record, null);
                    parameters.Add($"v_{propertyName}", propertyValue);
                }
            }
            // chuẩn bị câu lệnh MySQL
            string storeProcedureName = string.Format(Resource.Proc_InsertOne, typeof(T).Name);
            int numberOfAffectdRows = _dbHelper.RunProcWithExecute(storeProcedureName, parameters, ref v_MessOut);
            if (numberOfAffectdRows > 0)
            {
                return new ServiceResponse
                {
                    Success = true,
                    Data = recordID
                };
            }
            return new ServiceResponse
            {
                Success = false,
                Data = !string.IsNullOrEmpty(v_MessOut) ? v_MessOut : Guid.Empty.ToString()
            };
        }

        /// <summary>
        /// Hàm cập nhật thông tin một bản ghi theo ID
        /// </summary>
        /// <param name="recordID"></param>
        /// <param name="record"></param>
        /// <returns>ID record sau khi cập nhật</returns>
        /// Create by: Nguyễn Khắc Tiềm (26/09/2022)
        public virtual ServiceResponse UpdateRecord(Guid recordID, T record, string userID)
        {
            var v_MessOut = "";
            // Khởi tạo các parameter để chèn vào trong Proc
            DynamicParameters parameters = new DynamicParameters();
            CustomParameterForUpdate(ref parameters, record);
            parameters.Add("v_UserID", userID);
            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                string propertyName = property.Name;
                // Kiểm tra trường nào là khoá chính thì thêm param là id tự sinh Guid
                var primaryKeyAttribute = (ValidateAttribute?)Attribute.GetCustomAttribute(property, typeof(ValidateAttribute));
                if (primaryKeyAttribute != null && primaryKeyAttribute.PrimaryKey)
                {
                    parameters.Add($"v_{propertyName}", recordID);
                }
                else if (primaryKeyAttribute != null && primaryKeyAttribute.NotMapParameterProc == true)
                {
                    continue;
                }
                else
                {
                    var propertyValue = property.GetValue(record, null);
                    parameters.Add($"v_{propertyName}", propertyValue);
                }
            }

            // chuẩn bị câu lệnh MySQL
            string storeProcedureName = string.Format(Resource.Proc_UpdateOne, typeof(T).Name);
            int numberOfAffectdRows = _dbHelper.RunProcWithExecute(storeProcedureName, parameters,ref v_MessOut);
            if (numberOfAffectdRows > 0)
            {
                return new ServiceResponse
                {
                    Success = true,
                    Data = recordID
                };
            }
            return new ServiceResponse
            {
                Success = false,
                Data = !string.IsNullOrEmpty(v_MessOut) ? v_MessOut : Guid.Empty.ToString()
            };
        }

        /// <summary>
        /// Xoá một bản ghi theo ID
        /// </summary>
        /// <param name="recordID"></param>
        /// <returns>ID record sau khi xoá</returns>
        /// Create by: Nguyễn Khắc Tiềm (26/09/2022)
        public virtual ServiceResponse DeleteRecord(Guid recordID)
        {
            var v_MessOut = "";
            // chuẩn bị câu lệnh MySQL
            string storeProcedureName = string.Format(Resource.Proc_DeleteOne, typeof(T).Name);
            // Khởi tạo các parameter để chèn vào trong Proc
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add($"v_{typeof(T).Name}ID", recordID);
            int numberOfAffectdRows = _dbHelper.RunProcWithExecute(storeProcedureName, parameters, ref v_MessOut);
            if (numberOfAffectdRows > 0)
            {
                return new ServiceResponse
                {
                    Success = true,
                    Data = recordID
                };
            }
            return new ServiceResponse
            {
                Success = false,
                Data = !string.IsNullOrEmpty(v_MessOut) ? v_MessOut : Guid.Empty.ToString()
            };
        }

        /// <summary>
        /// xóa nhiều bản ghi
        /// </summary>
        /// <param name="listRecordID">danh sách bản ghi cần xoá</param>
        /// <param name="count">Số lượng bản ghi bị xoá</param>
        /// <returns>Số kết quả bản ghi đã xoá</returns>
        /// CreatedBy: Nguyễn Khắc Tiềm (5/10/2022)
        public virtual ServiceResponse DeleteMultiple(string listRecordID, int count)
        {
            var rowAffects = 0;
            var v_MessOut = "";
            using (var mysqlConnection = new MySqlConnection(DataContext.MySqlConnectionString))
            {
                //nếu như kết nối đang đóng thì tiến hành mở lại
                if (mysqlConnection.State != ConnectionState.Open)
                {
                    mysqlConnection.Open();
                }
                //mở một giao dịch( nếu xóa thành công thì xóa hết, nếu lỗi giữa chừng thì dừng lại và khôi phục các dữ liệu đã bị xóa)
                using (var transaction = mysqlConnection.BeginTransaction())
                {
                    try
                    {
                        // chuẩn bị câu lệnh MySQL
                        string storeProcedureName = string.Format(Resource.Proc_DeleteMultiple, typeof(T).Name);
                        // Khởi tạo các parameter để chèn vào trong Proc
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("v_ListID", listRecordID);
                        // thực hiện gọi vào DB
                        parameters.Add($"v_MessOut", DbType.String, direction: ParameterDirection.Output, size: 255);
                        rowAffects += mysqlConnection.Execute(storeProcedureName,
                            parameters,
                            transaction: transaction,
                            commandType: CommandType.StoredProcedure
                            );
                        v_MessOut = parameters.Get<string>("v_MessOut");
                        if (rowAffects >= count)
                        {
                            transaction.Commit();
                        }
                        else
                        {
                            transaction.Rollback();
                            rowAffects = 0;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        //nếu thực hiện không thành công thì rollback
                        transaction.Rollback();
                        rowAffects = 0;
                    }
                    finally
                    {
                        if (mysqlConnection.State == ConnectionState.Open)
                        {
                            mysqlConnection.Close();
                        }
                    }
                }
            }
            //trả về kết quả(số bản ghi xóa được)
            if (rowAffects >= count)
            {
                return new ServiceResponse
                {
                    Success = true,
                    Data = v_MessOut
                };
            }
            return new ServiceResponse
            {
                Success = false,
                Data = v_MessOut
            };
        }

        /// <summary>
        /// Hàm cập nhật toggle active bản ghi
        /// </summary>
        /// <param name="recordID"></param>
        /// <returns>ID record sau khi cập nhật</returns>
        /// Create by: Nguyễn Khắc Tiềm (26/09/2022)
        public virtual ServiceResponse ToggleActive(Guid recordID, string userID)
        {
            var v_MessOut = "";
            // Khởi tạo các parameter để chèn vào trong Proc
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("v_ID", recordID);
            parameters.Add("v_UserID", userID);
            // chuẩn bị câu lệnh MySQL
            string storeProcedureName = string.Format(Resource.Proc_UpdateActive, typeof(T).Name);
            int numberOfAffectdRows = _dbHelper.RunProcWithExecute(storeProcedureName, parameters, ref v_MessOut);
            if (numberOfAffectdRows > 0)
            {
                return new ServiceResponse
                {
                    Success = true,
                    Data = recordID
                };
            }
            return new ServiceResponse
            {
                Success = false,
                Data = Guid.Empty
            };
        }

        /// <summary>
        /// Hhập khẩu dữ liệu từ tệp
        /// </summary>
        /// <param name="data">Json danh sách</param>
        /// <param name="count">Số lượng record</param>
        /// <returns></returns>
        /// Create by: Nguyễn Khắc Tiềm (26/09/2022)
        public ServiceResponse ImportXLSX(List<T> listData, string userID)
        {
            var v_MessOut = "";
            // chuẩn bị câu lệnh MySQL
            string storeProcedureName = string.Format(Resource.Proc_Import, typeof(T).Name);
            // Khởi tạo các parameter để chèn vào trong Proc
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("v_Data", JsonConvert.SerializeObject(listData));
            parameters.Add("v_UserID", userID);
            object result = _dbHelper.RunProcWithQueryCombineTransaction(storeProcedureName, parameters, ref v_MessOut);
            if (string.IsNullOrEmpty(v_MessOut))
            {
                return new ServiceResponse
                {
                    Success = true,
                    Data = result
                };
            }
            return new ServiceResponse
            {
                Success = false,
                Data = v_MessOut
            };
        }

        #endregion
    }
}