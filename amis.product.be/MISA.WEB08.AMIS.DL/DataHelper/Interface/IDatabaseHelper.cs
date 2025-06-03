using Dapper;
using MySqlConnector;

namespace MISA.WEB08.AMIS.DL
{
    /// <summary>
    /// Các thao tác gọi proc
    /// </summary>
    /// Create by: Nguyễn Khắc Tiềm (21/09/2022)
    public interface IDatabaseHelper<T>
    {
        /// <summary>
        /// Chạy proc với query trong dapper
        /// </summary>
        /// <param name="storeProcedureName">Tên store</param>
        /// <param name="parameters">các parameter</param>
        /// <returns></returns>
        /// Create by: Nguyễn Khắc Tiềm (21/09/2022)
        public object RunProcWithQuery(string storeProcedureName, DynamicParameters? parameters);

        /// <summary>
        /// Chạy proc với QueryFirstOrDefault trong dapper
        /// </summary>
        /// <param name="storeProcedureName">Tên store</param>
        /// <param name="parameters">các parameter</param>
        /// <returns></returns>
        /// Create by: Nguyễn Khắc Tiềm (21/09/2022)
        public object RunProcWithQueryFirstOrDefault(string storeProcedureName, DynamicParameters? parameters);

        /// <summary>
        /// Chạy proc với Execute trong dapper
        /// </summary>
        /// <param name="storeProcedureName">Tên store</param>
        /// <param name="parameters">các parameter</param>
        /// <param name="v_MessOut">Message trả ra từ store</param>
        /// <returns></returns>
        /// Create by: Nguyễn Khắc Tiềm (21/09/2022)
        public int RunProcWithExecute(string storeProcedureName, DynamicParameters? parameters, ref string? v_MessOut);

        /// <summary>
        /// Chạy proc với Query trong dapper kết hợp với Transaction
        /// </summary>
        /// <param name="storeProcedureName">Tên store</param>
        /// <param name="parameters">các parameter</param>
        /// <param name="v_MessOut">Message trả ra từ store</param>
        /// <returns></returns>
        /// Create by: Nguyễn Khắc Tiềm (21/09/2022)
        public object RunProcWithQueryCombineTransaction(string storeProcedureName, DynamicParameters? parameters, ref string? v_MessOut);

        /// <summary>
        /// Hàm tạo mã tự sinh
        /// </summary>
        /// <param name="code">Mã</param>
        /// <param name="prefix">Phần chữ đầu</param>
        /// <param name="number">Số lượng số</param>
        /// <param name="last">Phần sau</param>
        /// Create by: Nguyễn Khắc Tiềm (21/09/2022)
        public void SaveCode(string code, ref string prefix, ref string number, ref string last);
    }
}
