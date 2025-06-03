using MISA.WEB08.AMIS.Common.Entities;
using MISA.WEB08.AMIS.Common.Result;
using MISA.WEB08.AMIS.DL;

namespace MISA.WEB08.AMIS.BL
{
    /// <summary>
    /// Dữ liệu thao tác với Database và trả về với bảng Unit từ tầng BL
    /// </summary>
    /// Create by: Nguyễn Khắc Tiềm (21/09/2022)
    public class CouponBL : BaseBL<Coupon>, ICouponBL
    {
        #region Field

        private ICouponDL _couponDL;

        #endregion

        #region Contructor

        public CouponBL(ICouponDL couponDL) : base(couponDL)
        {
            _couponDL = couponDL;
        }

        #endregion

        #region Method

        /// <summary>
        /// Hàm custom dữ liệu tên file, header, ... khi xuất file
        /// </summary>
        /// <returns></returns>
        ///  NK Tiềm 05/10/2022
        public override OptionExport CustomOptionExport()
        {
            return new OptionExport
            {
                FileName = "Danh sách giảm giá",
                Header = "DANH SÁCH GIẢM GIÁ"
            };
        }

        public object GetRecordByCode(string code)
        {
            return _couponDL.GetRecordByCode(code);
        }

        #endregion
    }
}
