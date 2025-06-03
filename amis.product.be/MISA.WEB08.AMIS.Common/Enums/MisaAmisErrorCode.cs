
namespace MISA.WEB08.AMIS.Common.Enums
{
    /// <summary>
    /// Mã lỗi
    /// </summary>
    /// Create by: Nguyễn Khắc Tiềm 21.09.2022
    public enum MisaAmisErrorCode : int
    {
        /// <summary>
        /// Lỗi do ngoại lệ
        /// </summary>
        /// Created by : Khắc Tiềm 21.09.2022
        Exception = 500,

        /// <summary>
        /// Lỗi trùng các trường
        /// </summary>
        /// Created by : Khắc Tiềm 21.09.2022
        Duplicate = 2,

        /// <summary>
        /// Lỗi mã để trống
        /// </summary>
        /// Created by : Khắc Tiềm 21.09.2022
        EmptyCode = 3,

        /// <summary>
        /// Lỗi sai dữ liệu đầu vào
        /// </summary>
        /// Created by : Khắc Tiềm 21.09.2022
        InvalidInput = 4,

        /// <summary>
        /// Mã lỗi xoá nhiều bản ghi thất bại
        /// Created by : Khắc Tiềm 21.09.2022
        /// </summary>
        DeleteMultiple = 5,

        /// <summary>
        /// Mã lỗi thêm mới không thành công
        /// Created by : Khắc Tiềm 21.09.2022
        /// </summary>
        InsertFailed = 6,

        /// <summary>
        /// Mã lỗi sửa không thành công
        /// Created by : Khắc Tiềm 21.09.2022
        /// </summary>
        UpdateFailed = 6,

        /// <summary>
        /// Mã lỗi xoá không thành công
        /// Created by : Khắc Tiềm 21.09.2022
        /// </summary>
        DeleteFailed = 7,

        /// <summary>
        /// Mã lỗi xoá không thành công, bị phát sinh dữ liệu
        /// Created by : Khắc Tiềm 21.09.2022
        /// </summary>
        Incurred = 8,

        /// <summary>
        /// Lỗi không tìm đấy dữ liệu
        /// Created by : Khắc Tiềm 21.09.2022
        /// </summary>
        NotFoundData = 9,

        /// <summary>
        /// Tệp không đúng định dạng
        /// Created by : Khắc Tiềm 21.09.2022
        /// </summary>
        FileNotCorrect = 10,

        /// <summary>
        /// Tài khoản ngưng hoạt động
        /// Created by : Khắc Tiềm 21.09.2022
        /// </summary>
        ActiveFalse = 11,

        /// <summary>
        /// Tài khoản mật khẩu sai
        /// Created by : Khắc Tiềm 21.09.2022
        /// </summary>
        LoginFail = 12,
    }
}
