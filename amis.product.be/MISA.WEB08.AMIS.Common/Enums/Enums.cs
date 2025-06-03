
namespace MISA.WEB08.AMIS.Common.Enums
{
    /// <summary>
    /// Giới tính 
    /// Enum giới tính của thực thể người, 3 loại nam, nữ, khác
    /// </summary>
    /// Create by: Nguyễn Khắc Tiềm (21/09/2022)
    public enum Gender:int
    {
        /// <summary>
        /// nam
        /// Created by : Khắc Tiềm 21.09.2022
        /// </summary>
        Male = 0,

        /// <summary>
        /// nữ
        /// Created by : Khắc Tiềm 21.09.2022
        /// </summary>
        Female = 1,

        /// <summary>
        /// khác
        /// Created by : Khắc Tiềm 21.09.2022
        /// </summary>
        Other = 2,
    }


    /// <summary>
    /// Kiểu join bảng
    /// </summary>
    /// Create by: Nguyễn Khắc Tiềm (21/09/2022)
    public enum TypeJoin: int
    {
        /// <summary>
        /// Kiểu inner join
        /// </summary>
        InnerJoin = 1,

        /// <summary>
        /// Kiểu left join
        /// </summary>
        LeftJoin = 2,
    }


    /// <summary>
    /// Kiểu thanh toán
    /// </summary>
    /// Create by: Nguyễn Khắc Tiềm (21/09/2022)
    public enum TypeCheckout : int
    {
        /// <summary>
        /// Chuyển khoản trực tiếp
        /// </summary>
        Live = 0,

        /// <summary>
        /// Kiểm tra hàng và thanh toán
        /// </summary>
        Check = 1,
    }

    /// <summary>
    /// Trạng thái đơn hàng
    /// </summary>
    /// Create by: Nguyễn Khắc Tiềm (21/09/2022)
    public enum StatusOrder : int
    {
        /// <summary>
        /// Chờ xác nhận
        /// </summary>
        WaitConfirm = 0,

        /// <summary>
        /// Đã xác nhận
        /// </summary>
        Confirm = 1,

        /// <summary>
        /// Đang giao hàng
        /// </summary>
        Delivery = 2,

        /// <summary>
        /// Đã giao hàng
        /// </summary>
        Delivered = 3,

        /// <summary>
        /// Đơn bị huỷ
        /// </summary>
        Destroy = 4,
    }
}
