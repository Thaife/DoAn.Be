using MISA.WEB08.AMIS.Common.Entities;
using MISA.WEB08.AMIS.BL;
using Microsoft.AspNetCore.Authorization;

namespace MISA.WEB08.AMIS.API.Controllers
{
    /// <summary>
    /// API dữ liệu với bảng Trademark
    /// </summary>
    /// Created by : Nguyễn Khắc Tiềm (21/09/2022)
    [Authorize]
    public class TrademarksController : BasesController<Trademark>
    {
        #region Field

        private ITrademarkBL _trademarkBL;

        #endregion

        #region Contructor

        public TrademarksController(ITrademarkBL trademarkBL) : base(trademarkBL)
        {
            _trademarkBL = trademarkBL;
        }

        #endregion

        #region Method

        #endregion
    }
}