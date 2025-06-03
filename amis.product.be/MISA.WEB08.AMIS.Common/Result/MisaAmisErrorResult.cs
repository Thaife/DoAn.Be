using MISA.WEB08.AMIS.Common.Enums;

namespace MISA.WEB08.AMIS.Common.Result
{
    /// <summary>
    /// Lỗi tuỳ chỉnh
    /// Create by: Nguyễn Khắc Tiềm 21.09.2022  
    /// </summary>
    public class MisaAmisErrorResult
    {
        #region Field

        /// <summary>
        /// Mã lỗi enum
        /// </summary>
        /// Created by : Nguyễn Khắc Tiềm 21.09.2022
        public MisaAmisErrorCode ErrorCode { get; set; }

        /// <summary>
        /// Mã lỗi cho dev
        /// </summary>
        /// Created by : Nguyễn Khắc Tiềm 21.09.2022
        public string DevMsg { get; set; }

        /// <summary>
        /// Mã lỗi cho người dùng
        /// </summary>
        /// Created by : Nguyễn Khắc Tiềm 21.09.2022
        public dynamic? UserMsg { get; set; }

        /// <summary>
        /// Thông tin thêm
        /// </summary>
        /// Created by : Nguyễn Khắc Tiềm 21.09.2022
        public string MoreInfo { get; set; }

        /// <summary>
        /// ID kết nối để trace sau này để dò lỗi
        /// </summary>
        /// Created by : Nguyễn Khắc Tiềm 21.09.2022
        public string TraceId { get; set; }

        #endregion

        #region Contructor

        /// <summary>
        /// Hàm khởi tạo
        /// </summary>
        /// <param name="errorCode"></param>
        /// <param name="devMsg"></param>
        /// <param name="userMsg"></param>
        /// <param name="moreInfo"></param>
        /// <param name="traceId">ID kết nối</param>
        /// Created by : Nguyễn Khắc Tiềm 21.09.2022
        public MisaAmisErrorResult(MisaAmisErrorCode errorCode, string devMsg, dynamic? userMsg, string moreInfo, string traceId)
        {
            UserMsg = userMsg;
            ErrorCode = errorCode;
            DevMsg = devMsg;
            MoreInfo = moreInfo;
            TraceId = traceId;
        }

        #endregion

        #region Method
        /// ctrl m o : đóng
        /// ctrl k s : thêm region
        /// ctrl m l : mở
        #endregion
    }
}
