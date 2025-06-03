using Microsoft.AspNetCore.Authorization;
using MISA.WEB08.AMIS.BL;
using MISA.WEB08.AMIS.Common.Entities;

namespace MISA.WEB08.AMIS.API.Controllers
{
    /// <summary>
    /// API dữ liệu với bảng depot
    /// </summary>
    /// Created by : Nguyễn Khắc Tiềm (21/09/2022)
    [Authorize]
    public class DepotsController : BasesController<Depot>
    {
        #region Field

        private IDepotBL _depotBL;

        #endregion

        #region Contructor

        public DepotsController(IDepotBL depotBL) : base(depotBL)
        {
            _depotBL = depotBL;
        }

        #endregion

        #region Method

        #region API GET

        #endregion

        #endregion
    }
}
