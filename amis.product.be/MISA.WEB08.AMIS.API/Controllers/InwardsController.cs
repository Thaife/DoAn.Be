using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.WEB08.AMIS.BL;
using MISA.WEB08.AMIS.Common.Entities;
using MISA.WEB08.AMIS.Common.Enums;
using MISA.WEB08.AMIS.Common.Resources;
using MISA.WEB08.AMIS.Common.Result;
using System.Threading.Tasks;
using System;
using Dapper;
using System.Net.WebSockets;
using System.Text.Json;
using System.Collections.Generic;

namespace MISA.WEB08.AMIS.API.Controllers
{
    /// <summary>
    /// API dữ liệu với bảng nhập kho
    /// </summary>
    /// Created by : TVTHAI (21/09/2022)
    [Authorize]
    public class InwardsController : BasesController<Inward>
    {
        #region Field

        private IInwardBL _inwardBL;

        #endregion

        #region Contructor

        public InwardsController(IInwardBL inwardBL) : base(inwardBL)
        {
            _inwardBL = inwardBL;
        }

        #endregion

        #region Method
        /// <summary>
        /// Hàm lấy ra bản ghi theo ID
        /// </summary>
        /// <param name="recordID"></param>
        /// <returns>Thông tin chi tiết một bản ghi</returns>
        /// Create by: TVTHAI (26/09/2022)
        [HttpGet("{recordID}")]
        public override async Task<IActionResult> GetRecordByID([FromRoute] Guid recordID, [FromQuery] string? stateForm)
        {
            var record = await Task.FromResult(_inwardBL.GetRecordByID(recordID.ToString(), stateForm));
            var detail = await Task.FromResult(_inwardBL.GetDetailByID(recordID.ToString(), stateForm));
            if (record != null)
            {
                var res = JsonSerializer.Deserialize<Inward>(JsonSerializer.Serialize(record));
                var resDetail = JsonSerializer.Deserialize<List<InwardDetail>>(JsonSerializer.Serialize(detail));
                res.InwardDetail = resDetail;
                return StatusCode(StatusCodes.Status200OK, new ServiceResponse
                {
                    Success = true,
                    Data = res
                });
            }
            return StatusCode(StatusCodes.Status200OK, new ServiceResponse
            {
                Success = false,
                ErrorCode = MisaAmisErrorCode.NotFoundData,
                Data = new MisaAmisErrorResult(
                            MisaAmisErrorCode.NotFoundData,
                            Resource.DevMsg_ValidateFailed,
                            Resource.Message_notFoundData,
                            Resource.MoreInfo_Exception,
                            HttpContext.TraceIdentifier
                        )
            });
        }

        [HttpGet("approve/{recordID}")]
        public async Task<IActionResult> Approve([FromRoute] Guid recordID)
        {
            var record = await Task.FromResult(_inwardBL.Approve(recordID));
            return StatusCode(StatusCodes.Status200OK, new ServiceResponse
            {
                Success = true,
            });
        }

        #region API GET

        #endregion

        #endregion
    }
}
