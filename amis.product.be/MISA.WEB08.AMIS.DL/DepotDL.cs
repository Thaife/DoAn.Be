using MISA.WEB08.AMIS.Common.Entities;

namespace MISA.WEB08.AMIS.DL
{
    /// <summary>
    /// Dữ liệu thao tác với Database và trả về với bảng depot từ tầng DL
    /// </summary>
    /// Create by: Nguyễn Khắc Tiềm (21/09/2022)
    public class DepotDL : BaseDL<Depot>, IDepotDL
    {
        #region Field

        private IDatabaseHelper<Depot> _dbHelper;

        #endregion

        #region Contructor

        public DepotDL(IDatabaseHelper<Depot> dbHelper) : base(dbHelper)
        {
            _dbHelper = dbHelper;
        }

        #endregion

        #region Method

        #endregion
    }
}
