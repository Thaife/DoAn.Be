using MISA.WEB08.AMIS.Common.Entities;
using MISA.WEB08.AMIS.BL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MISA.WEB08.AMIS.Common.Result;
using MISA.WEB08.AMIS.Common.Enums;
using MISA.WEB08.AMIS.Common.Resources;

namespace MISA.WEB08.AMIS.API.Controllers
{
    /// <summary>
    /// API dữ liệu với bảng Coupon
    /// </summary>
    /// Created by : Nguyễn Khắc Tiềm (21/09/2022)
    [Authorize]
    public class CouponsController : BasesController<Coupon>
    {
        #region Field

        private ICouponBL _couponBL;

        #endregion

        #region Contructor

        public CouponsController(ICouponBL couponBL) : base(couponBL)
        {
            _couponBL = couponBL;
        }

        #endregion

        #region Method

        /// <summary>
        /// Hàm lấy ra bản ghi theo mã
        /// </summary>
        /// <param name="recordID"></param>
        /// <returns>Thông tin chi tiết một bản ghi</returns>
        /// Create by: Nguyễn Khắc Tiềm (26/09/2022)
        [AllowAnonymous]
        [HttpGet("GetByCode/{recordCode}")]
        public async Task<IActionResult> GetRecordByCode([FromRoute] string recordCode)
        {
            var record = await Task.FromResult(_couponBL.GetRecordByCode(recordCode.ToString()));
            return StatusCode(StatusCodes.Status200OK, new ServiceResponse
            {
                Success = true,
                Data = record
            });
        }

        #endregion
    }
}