using MISA.WEB08.AMIS.Common.Entities;

namespace MISA.WEB08.AMIS.BL
{
    /// <summary>
    /// Dữ liệu thao tác với Database và trả về với bảng Unit từ tầng BL
    /// </summary>
    /// Create by: Nguyễn Khắc Tiềm (21/09/2022)
    public interface ICouponBL : IBaseBL<Coupon>
    {
        public object GetRecordByCode(string code);
    }
}
