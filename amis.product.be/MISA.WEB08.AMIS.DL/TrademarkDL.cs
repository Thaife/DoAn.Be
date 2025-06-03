using MISA.WEB08.AMIS.Common.Entities;

namespace MISA.WEB08.AMIS.DL
{
    /// <summary>
    /// Dữ liệu thao tác với Database và trả về với bảng Unit từ tầng DL
    /// </summary>
    /// Create by: Nguyễn Khắc Tiềm (21/09/2022)
    public class TrademarkDL : BaseDL<Trademark>, ITrademarkDL
    {
        #region Field

        private IDatabaseHelper<Trademark> _dbHelper;

        #endregion

        #region Contructor

        public TrademarkDL(IDatabaseHelper<Trademark> dbHelper) : base(dbHelper)
        {
            _dbHelper = dbHelper;
        }

        #endregion

        #region Method

        #endregion
    }
}
