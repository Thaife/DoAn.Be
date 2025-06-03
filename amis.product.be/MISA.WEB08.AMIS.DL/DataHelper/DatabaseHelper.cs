using Dapper;
using MISA.WEB08.AMIS.Common.Resources;
using MySqlConnector;
using System;
using System.Data;
using System.Linq;

namespace MISA.WEB08.AMIS.DL
{
    /// <summary>
    /// Các thao tác gọi proc
    /// </summary>
    /// Create by: Nguyễn Khắc Tiềm (21/09/2022)
    public class DatabaseHelper<T> : IDatabaseHelper<T>
    {
        #region Method

        /// <summary>
        /// Chạy proc với query trong dapper
        /// </summary>
        /// <param name="storeProcedureName">Tên store</param>
        /// <param name="parameters">các parameter</param>
        /// <returns></returns>
        /// Create by: Nguyễn Khắc Tiềm (21/09/2022)
        public virtual object RunProcWithQuery(string storeProcedureName, DynamicParameters? parameters)
        {
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
                result = mysqlConnection.Query<T>(
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
        /// Chạy proc với QueryFirstOrDefault trong dapper
        /// </summary>
        /// <param name="storeProcedureName">Tên store</param>
        /// <param name="parameters">các parameter</param>
        /// <returns></returns>
        /// Create by: Nguyễn Khắc Tiềm (21/09/2022)
        public virtual object RunProcWithQueryFirstOrDefault(string storeProcedureName, DynamicParameters? parameters)
        {
            object result;
            using (var mysqlConnection = new MySqlConnection(DataContext.MySqlConnectionString))
            {
                //nếu như kết nối đang đóng thì tiến hành mở lại
                if (mysqlConnection.State != ConnectionState.Open)
                {
                    mysqlConnection.Open();
                }
                // thực hiện gọi vào DB
                result = mysqlConnection.QueryFirstOrDefault<T>(
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
        /// Chạy proc với Execute trong dapper
        /// </summary>
        /// <param name="storeProcedureName">Tên store</param>
        /// <param name="parameters">các parameter</param>
        /// <param name="v_MessOut">Message trả ra từ store</param>
        /// <returns></returns>
        /// Create by: Nguyễn Khắc Tiềm (21/09/2022)
        public int RunProcWithExecute(string storeProcedureName, DynamicParameters? parameters, ref string? v_MessOut)
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
                        // thực hiện gọi vào DB
                        parameters.Add($"v_MessOut", DbType.String, direction: ParameterDirection.Output, size: 255);
                        rowAffects += mysqlConnection.Execute(storeProcedureName,
                            parameters,
                            transaction: transaction,
                            commandType: CommandType.StoredProcedure
                            );
                        v_MessOut = parameters.Get<string>("v_MessOut");
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        v_MessOut = Resource.UserMsg_Exception;
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
            return rowAffects;
        }

        /// <summary>
        /// Chạy proc với Query trong dapper kết hợp với Transaction
        /// </summary>
        /// <param name="storeProcedureName">Tên store</param>
        /// <param name="parameters">các parameter</param>
        /// <param name="v_MessOut">Message trả ra từ store</param>
        /// <returns></returns>
        /// Create by: Nguyễn Khắc Tiềm (21/09/2022)
        public virtual object RunProcWithQueryCombineTransaction(string storeProcedureName, DynamicParameters? parameters, ref string? v_MessOut)
        {
            object result;
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
                        // thực hiện gọi vào DB
                        parameters.Add($"v_MessOut", DbType.String, direction: ParameterDirection.Output, size: 255);
                        result = mysqlConnection.Query<T>(storeProcedureName,
                            parameters,
                            transaction: transaction,
                            commandType: CommandType.StoredProcedure
                            );
                        v_MessOut = parameters.Get<string>("v_MessOut");
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        v_MessOut = Resource.UserMsg_Exception;
                        //nếu thực hiện không thành công thì rollback
                        transaction.Rollback();
                        result = null;
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
            return result;
        }

        /// <summary>
        /// Hàm tạo mã tự sinh
        /// </summary>
        /// <param name="code">Mã</param>
        /// <param name="prefix">Phần chữ đầu</param>
        /// <param name="number">Số lượng số</param>
        /// <param name="last">Phần sau</param>
        /// Create by: Nguyễn Khắc Tiềm (21/09/2022)
        public void SaveCode(string code, ref string prefix, ref string number, ref string last)
        {
            for (int i = 0; i < code.Length; i++)
            {
                char[] keyNumber = { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };
                char temp = code[i];
                if ((keyNumber.Contains(temp)) && last == "")
                {
                    if (number == "" && temp == '0')
                    {
                        prefix += temp;
                    }
                    else
                    {
                        number += temp;
                    }
                }
                else
                {
                    if (number != "")
                    {
                        last += temp;
                    }
                    else
                    {
                        prefix += temp;
                    }
                }
            }
            if (number == "")
            {
                number = "0";
            }
        }

        #endregion
    }
}
