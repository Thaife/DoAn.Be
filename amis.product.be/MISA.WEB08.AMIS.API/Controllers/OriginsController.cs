using MISA.WEB08.AMIS.Common.Entities;
using MISA.WEB08.AMIS.BL;
using Microsoft.AspNetCore.Authorization;

namespace MISA.WEB08.AMIS.API.Controllers
{
    /// <summary>
    /// API dữ liệu với bảng Origin
    /// </summary>
    /// Created by : Nguyễn Khắc Tiềm (21/09/2022)
    [Authorize]
    public class OriginsController : BasesController<Origin>
    {
        #region Field

        private IOriginBL _originBL;

        #endregion

        #region Contructor

        public OriginsController(IOriginBL originBL) : base(originBL)
        {
            _originBL = originBL;
        }

        #endregion

        #region Method

        #endregion
    }
}