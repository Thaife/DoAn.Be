using Dapper;
using MISA.WEB08.AMIS.Common.Entities;
using MISA.WEB08.AMIS.Common.Enums;
using MISA.WEB08.AMIS.Common.Resources;
using MISA.WEB08.AMIS.Common.Result;
using MySqlConnector;
using System;
using System.Data;
using System.Linq;

namespace MISA.WEB08.AMIS.DL
{
    /// <summary>
    /// Dữ liệu thao tác với Database và trả về với bảng Unit từ tầng DL
    /// </summary>
    /// Create by: Nguyễn Khắc Tiềm (21/09/2022)
    public class CartDL : BaseDL<Cart>, ICartDL
    {
        #region Field

        private IDatabaseHelper<Cart> _dbHelper;

        #endregion

        #region Contructor

        public CartDL(IDatabaseHelper<Cart> dbHelper) : base(dbHelper)
        {
            _dbHelper = dbHelper;
        }

        #endregion

        #region Method



        /// <summary>
        /// Hàm lấy ra bản ghi theo ID
        /// </summary>
        /// <param name="recordID">ID bản ghi</param>
        /// <param name="stateForm">Trạng thái lấy (sửa hay nhân bản, ...)</param>
        /// <returns>Thông tin chi tiết một bản ghi</returns>
        /// Create by: Nguyễn Khắc Tiềm (26/09/2022)
        public override object GetRecordByID(string recordID, string? stateForm)
        {
            Cart result;
            // Khởi tạo các parameter để chèn vào trong Proc
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add($"v_CurrentUser", recordID);
            // Khai báo stored procedure
            string storeProcedureName = string.Format(Resource.Proc_GetDetailOne, typeof(Cart).Name);
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
                result = records.Read<Cart>().First();
                result.CartDetail = records.Read<CartDetail>().ToList();
                if (mysqlConnection.State == ConnectionState.Open)
                {
                    mysqlConnection.Close();
                }
            }
            return result;
        }

        /// <summary>
        /// Cập nhật giỏ hàng
        /// </summary>
        /// <param name="recordID">ID bản ghi</param>
        /// <param name="stateForm">Trạng thái lấy (sửa hay nhân bản, ...)</param>
        /// <returns>Thông tin chi tiết một bản ghi</returns>
        /// Create by: Nguyễn Khắc Tiềm (26/09/2022)
        public object UpdateCart(string v_CurrentUser, string v_ProductID, string v_State)
        {
            Cart result;
            // Khởi tạo các parameter để chèn vào trong Proc
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add($"v_CurrentUser", v_CurrentUser);
            parameters.Add($"v_ProductID", v_ProductID);
            parameters.Add($"v_State", v_State);
            // Khai báo stored procedure
            string storeProcedureName = "Proc_cart_AddCartDetail";
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
                result = records.Read<Cart>().First();
                result.CartDetail = records.Read<CartDetail>().ToList();
                if (mysqlConnection.State == ConnectionState.Open)
                {
                    mysqlConnection.Close();
                }
            }
            return result;
        }

        /// <summary>
        /// Thanh toán
        /// </summary>
        /// <returns>Thông tin chi tiết một bản ghi</returns>
        /// Create by: Nguyễn Khắc Tiềm (26/09/2022)
        public void Checkout(Order order)
        {
            var v_MessOut = "";
            // Khởi tạo các parameter để chèn vào trong Proc
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("v_CurrentUser", order.CurrentUser);
            parameters.Add("v_UserName", order.UserName);
            parameters.Add("v_PhoneNumber", order.PhoneNumber);
            parameters.Add("v_Email", order.Email);
            parameters.Add("v_Province", order.Province);
            parameters.Add("v_District", order.District);
            parameters.Add("v_Ward", order.Ward);
            parameters.Add("v_Address", order.Address);
            parameters.Add("v_Description", order.Description);
            parameters.Add("v_CouponID", order.CouponID);
            parameters.Add("v_TypeCheckout", order.TypeCheckout);
            // chuẩn bị câu lệnh MySQL
            string storeProcedureName = "Proc_order_Checkout";
            _dbHelper.RunProcWithExecute(storeProcedureName, parameters, ref v_MessOut);
        }

        /// <summary>
        /// API lấy ra đơn đặt hàng 
        /// </summary>
        /// <param name="formData">Từ khoá tìm kiếm</param>
        /// <returns>Danh sách record và tổng số bản ghi</returns>
        /// Create by: Nguyễn Khắc Tiềm (26/09/2022)
        public object GetOrderUser(string v_CurrentUser)
        {
            // Khởi tạo các parameter để chèn vào trong Proc
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add($"v_CurrentUser", v_CurrentUser);
            // Khai báo stored procedure
            string storeProcedureName = "Proc_order_GetListOrder";
            object result;
            // Khai báo stored procedure
            using (var mysqlConnection = new MySqlConnection(DataContext.MySqlConnectionString))
            {
                //nếu như kết nối đang đóng thì tiến hành mở lại
                if (mysqlConnection.State != ConnectionState.Open)
                {
                    mysqlConnection.Open();
                }
                // thực hiện gọi vào DB
                result = mysqlConnection.Query<Order>(
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
        /// Hàm lấy ra bản ghi theo ID
        /// </summary>
        /// <param name="recordID">ID bản ghi</param>
        /// <param name="stateForm">Trạng thái lấy (sửa hay nhân bản, ...)</param>
        /// <returns>Thông tin chi tiết một bản ghi</returns>
        /// Create by: Nguyễn Khắc Tiềm (26/09/2022)
        public object GetOrderByID(string v_OrderID)
        {
            Order result;
            // Khởi tạo các parameter để chèn vào trong Proc
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add($"v_OrderID", v_OrderID);
            // Khai báo stored procedure
            string storeProcedureName = "Proc_order_GetOrderDetail";
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
                result = records.Read<Order>().First();
                result.OrderDetail = records.Read<OrderDetail>().ToList();
                if (mysqlConnection.State == ConnectionState.Open)
                {
                    mysqlConnection.Close();
                }
            }
            return result;
        }

        /// <summary>
        /// API lấy ra địa chỉ
        /// </summary>
        /// <param name="formData">Từ khoá tìm kiếm</param>
        /// <returns>Danh sách record và tổng số bản ghi</returns>
        /// Create by: Nguyễn Khắc Tiềm (26/09/2022)
        public object GetAddress(string v_Address, int? v_ID)
        {
            // Khởi tạo các parameter để chèn vào trong Proc
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add($"v_Address", v_Address);
            parameters.Add($"v_ID", v_ID);
            // Khai báo stored procedure
            string storeProcedureName = "Proc_order_GetAddres";
            object result;
            // Khai báo stored procedure
            using (var mysqlConnection = new MySqlConnection(DataContext.MySqlConnectionString))
            {
                //nếu như kết nối đang đóng thì tiến hành mở lại
                if (mysqlConnection.State != ConnectionState.Open)
                {
                    mysqlConnection.Open();
                }
                // thực hiện gọi vào DB
                result = mysqlConnection.Query<object>(
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
        /// API lấy ra status order
        /// </summary>
        /// <param name="formData">Từ khoá tìm kiếm</param>
        /// <returns>Danh sách record và tổng số bản ghi</returns>
        /// Create by: Nguyễn Khắc Tiềm (26/09/2022)
        public object GetStatusOrder(string v_OrderID)
        {
            // Khởi tạo các parameter để chèn vào trong Proc
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add($"v_OrderID", v_OrderID);
            // Khai báo stored procedure
            string storeProcedureName = "Proc_statusorder_GetStatusOrder";
            object result;
            // Khai báo stored procedure
            using (var mysqlConnection = new MySqlConnection(DataContext.MySqlConnectionString))
            {
                //nếu như kết nối đang đóng thì tiến hành mở lại
                if (mysqlConnection.State != ConnectionState.Open)
                {
                    mysqlConnection.Open();
                }
                // thực hiện gọi vào DB
                result = mysqlConnection.Query<object>(
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
        public override Paging GetFitterRecords(int offset, int limit, string? keyword, string? sort, string v_Query, string v_Select)
        {
            Paging result;
            // Khởi tạo các parameter để chèn vào trong Proc
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("v_Offset", offset);
            parameters.Add("v_Limit", limit);
            parameters.Add("v_Sort", sort);
            parameters.Add("v_Where", keyword);
            parameters.Add("v_Query", v_Query);
            parameters.Add("v_Select", v_Select != "" ? ',' + v_Select : "");

            // chuẩn bị câu lệnh MySQL
            string storeProcedureName = "Proc_order_GetFilterPaging";

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
                    RecordList = records.Read<Order>().ToList(),
                };
                if (limit > 0)
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
        /// Xoá một bản ghi theo ID
        /// </summary>
        /// <param name="recordID"></param>
        /// <returns>ID record sau khi xoá</returns>
        /// Create by: Nguyễn Khắc Tiềm (26/09/2022)
        public override ServiceResponse DeleteRecord(Guid recordID)
        {
            var v_MessOut = "";
            // chuẩn bị câu lệnh MySQL
            string storeProcedureName = "Proc_order_DeleteOne";
            // Khởi tạo các parameter để chèn vào trong Proc
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add($"v_OrderID", recordID);
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
        public override ServiceResponse DeleteMultiple(string listRecordID, int count)
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
                        string storeProcedureName = "Proc_order_DeleteMultiple";
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
        /// xóa nhiều bản ghi
        /// </summary>
        /// <param name="listRecordID">danh sách bản ghi cần xoá</param>
        /// <param name="count">Số lượng bản ghi bị xoá</param>
        /// <returns>Số kết quả bản ghi đã xoá</returns>
        /// CreatedBy: Nguyễn Khắc Tiềm (5/10/2022)
        public ServiceResponse Actionall(string listRecordID, int count, string action)
        {
            var rowAffects = 0;
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
                        string storeProcedureName = "Proc_order_ActionAll";
                        // Khởi tạo các parameter để chèn vào trong Proc
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("v_ListID", listRecordID);
                        parameters.Add("v_Action", action == "waitConfirm" ? 0 : action == "confirm" ? 1 : action == "deliveryOrder" ? 2 : action == "delivered" ? 3 : action == "destroy" ? 4 : 0);
                        // thực hiện gọi vào DB
                        parameters.Add($"v_MessOut", DbType.String, direction: ParameterDirection.Output, size: 255);
                        rowAffects += mysqlConnection.Execute(storeProcedureName,
                            parameters,
                            transaction: transaction,
                            commandType: CommandType.StoredProcedure
                            );
                        transaction.Commit();
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
            return new ServiceResponse
            {
                Success = true,
                Data = rowAffects
            };
        }

        /// <summary>
        /// Thêm một status order
        /// </summary>
        public ServiceResponse AddStatusOrder(string orderID, string comment, string userID)
        {
            var v_MessOut = "";
            // tạo recordID
            // Khởi tạo các parameter để chèn vào trong Proc
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("v_OrderID", orderID);
            parameters.Add("v_Comment", comment);
            parameters.Add("v_UserID", userID);

            // chuẩn bị câu lệnh MySQL
            string storeProcedureName = "Proc_statusorder_InsertStatusOrder";
            int numberOfAffectdRows = _dbHelper.RunProcWithExecute(storeProcedureName, parameters, ref v_MessOut);
            if (numberOfAffectdRows > 0)
            {
                return new ServiceResponse
                {
                    Success = true,
                    Data = orderID
                };
            }
            return new ServiceResponse
            {
                Success = false,
                Data = !string.IsNullOrEmpty(v_MessOut) ? v_MessOut : Guid.Empty.ToString()
            };
        }

        #endregion
    }
}
