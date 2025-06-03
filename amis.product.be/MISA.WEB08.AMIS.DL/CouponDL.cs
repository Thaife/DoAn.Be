using Dapper;
using MISA.WEB08.AMIS.Common.Entities;

namespace MISA.WEB08.AMIS.DL
{
    /// <summary>
    /// Dữ liệu thao tác với Database và trả về với bảng Unit từ tầng DL
    /// </summary>
    /// Create by: Nguyễn Khắc Tiềm (21/09/2022)
    public class CouponDL : BaseDL<Coupon>, ICouponDL
    {
        #region Field

        private IDatabaseHelper<Coupon> _dbHelper;

        #endregion

        #region Contructor

        public CouponDL(IDatabaseHelper<Coupon> dbHelper) : base(dbHelper)
        {
            _dbHelper = dbHelper;
        }

        #endregion

        #region Method

        public object GetRecordByCode(string code)
        {
            // Khởi tạo các parameter để chèn vào trong Proc
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add($"v_CouponCode", code);
            // Khai báo stored procedure
            string storeProcedureName = "Proc_coupon_GetDetailByCode";
            return _dbHelper.RunProcWithQueryFirstOrDefault(storeProcedureName, parameters);
        }

        #endregion
    }
}
