using MISA.WEB08.AMIS.Common.Entities;

namespace MISA.WEB08.AMIS.DL
{
    /// <summary>
    /// Dữ liệu thao tác với Database và trả về với bảng Unit từ tầng DL
    /// </summary>
    /// Create by: Nguyễn Khắc Tiềm (21/09/2022)
    public class CategoryDL : BaseDL<Category>, ICategoryDL
    {
        #region Field

        private IDatabaseHelper<Category> _dbHelper;

        #endregion

        #region Contructor

        public CategoryDL(IDatabaseHelper<Category> dbHelper) : base(dbHelper)
        {
            _dbHelper = dbHelper;
        }

        #endregion

        #region Method

        #endregion
    }
}
