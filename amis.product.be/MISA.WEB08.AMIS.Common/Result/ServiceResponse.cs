using MISA.WEB08.AMIS.Common.Enums;

namespace MISA.WEB08.AMIS.Common.Result
{
    /// <summary>
    /// Custom dữ liệu trả về từ tầng BL DL
    /// </summary>
    /// Create by: Nguyễn Khắc Tiềm (21/09/2022)
    public class ServiceResponse
    {
        #region Field

        /// <summary>
        /// trả về true hoặc false (thành công hoặc thất bại)
        /// </summary>
        /// Create by: Nguyễn Khắc Tiềm (21/09/2022)
        public bool Success { get; set; }

        /// <summary>
        /// Mã lỗi đi kèm
        /// </summary>
        /// Create by: Nguyễn Khắc Tiềm (21/09/2022)
        public MisaAmisErrorCode? ErrorCode { get; set; }

        /// <summary>
        /// Dữ liệu đi kèm
        /// </summary>
        /// Create by: Nguyễn Khắc Tiềm (21/09/2022)
        public dynamic? Data { get; set; }

        #endregion
    }

    /// <summary>
    /// Kiểu dữ liệu phân trang
    /// </summary>
    /// Create by: Nguyễn Khắc Tiềm (21/09/2022)
    public class Paging
    {
        /// <summary>
        /// Danh sách
        /// </summary>
        /// Create by: Nguyễn Khắc Tiềm (21/09/2022)
        public object RecordList { get; set; }

        /// <summary>
        /// Tổng số bản ghi
        /// </summary>
        /// Create by: Nguyễn Khắc Tiềm (21/09/2022)
        public object TotalCount { get; set; }

        /// <summary>
        /// Dữ liệu thêm
        /// </summary>
        /// Create by: Nguyễn Khắc Tiềm (21/09/2022)
        public object? DataMore { get; set; }
    }
}
