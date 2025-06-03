using MISA.WEB08.AMIS.Common.Entities;
using MISA.WEB08.AMIS.BL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using MISA.WEB08.AMIS.Common.Result;
using MISA.WEB08.AMIS.Common.Enums;
using MISA.WEB08.AMIS.Common.Resources;
using System;

namespace MISA.WEB08.AMIS.API.Controllers
{
    /// <summary>
    /// API dữ liệu với bảng Product
    /// </summary>
    /// Created by : Nguyễn Khắc Tiềm (21/09/2022)
    [Authorize]
    public class ProductsController : BasesController<Product>
    {
        #region Field

        private IProductBL _productBL;

        #endregion

        #region Contructor

        public ProductsController(IProductBL productBL) : base(productBL)
        {
            _productBL = productBL;
        }

        #endregion

        #region Method

        /// <summary> 
        /// API lấy ra 10 sản phẩm mới nhất
        /// <summary>
        /// <param name="formData">Trường muốn filter và sắp xếp</param>
        /// <return> Danh sách bản ghi sau khi phân trang, chỉ lấy ra số bản ghi và số trang yêu cầu, và tổng số lượg bản ghi có điều kiện <return>
        /// Create by: Nguyễn Khắc Tiềm (21/09/2022)
        [AllowAnonymous]
        [HttpPost("GetProductHome")]
        public virtual async Task<IActionResult> GetProductHome([FromBody] Dictionary<string, object> formData)
        {
            var records = await Task.FromResult(_productBL.GetProductHome(formData));
            if (records != null)
            {
                return StatusCode(StatusCodes.Status200OK, new ServiceResponse
                {
                    Success = true,
                    Data = records
                });
            }
            return StatusCode(StatusCodes.Status200OK, new ServiceResponse
            {
                Success = false,
                ErrorCode = MisaAmisErrorCode.NotFoundData,
                Data = new MisaAmisErrorResult(
                        MisaAmisErrorCode.NotFoundData,
                        Resource.DevMsg_Exception,
                        Resource.Message_data_change,
                        Resource.MoreInfo_Exception,
                        HttpContext.TraceIdentifier
                    )
            });
        }

        /// <summary> 
        /// API lấy ra 10 sản phẩm hot nhất
        /// <summary>
        /// <param name="formData">Trường muốn filter và sắp xếp</param>
        /// <return> Danh sách bản ghi sau khi phân trang, chỉ lấy ra số bản ghi và số trang yêu cầu, và tổng số lượg bản ghi có điều kiện <return>
        /// Create by: Nguyễn Khắc Tiềm (21/09/2022)
        [AllowAnonymous]
        [HttpGet("GetProductHot")]
        public virtual async Task<IActionResult> GetProductHot()
        {
            var records = await Task.FromResult(_productBL.GetProductHot());
            if (records != null)
            {
                return StatusCode(StatusCodes.Status200OK, new ServiceResponse
                {
                    Success = true,
                    Data = records
                });
            }
            return StatusCode(StatusCodes.Status200OK, new ServiceResponse
            {
                Success = false,
                ErrorCode = MisaAmisErrorCode.NotFoundData,
                Data = new MisaAmisErrorResult(
                        MisaAmisErrorCode.NotFoundData,
                        Resource.DevMsg_Exception,
                        Resource.Message_data_change,
                        Resource.MoreInfo_Exception,
                        HttpContext.TraceIdentifier
                    )
            });
        }

        /// <summary> 
        /// API lấy ra 10 sản phẩm giá tốt nhất
        /// <summary>
        /// <param name="formData">Trường muốn filter và sắp xếp</param>
        /// <return> Danh sách bản ghi sau khi phân trang, chỉ lấy ra số bản ghi và số trang yêu cầu, và tổng số lượg bản ghi có điều kiện <return>
        /// Create by: Nguyễn Khắc Tiềm (21/09/2022)
        [AllowAnonymous]
        [HttpGet("GetProductPrice")]
        public virtual async Task<IActionResult> GetProductPrice()
        {
            var records = await Task.FromResult(_productBL.GetProductPrice());
            if (records != null)
            {
                return StatusCode(StatusCodes.Status200OK, new ServiceResponse
                {
                    Success = true,
                    Data = records
                });
            }
            return StatusCode(StatusCodes.Status200OK, new ServiceResponse
            {
                Success = false,
                ErrorCode = MisaAmisErrorCode.NotFoundData,
                Data = new MisaAmisErrorResult(
                        MisaAmisErrorCode.NotFoundData,
                        Resource.DevMsg_Exception,
                        Resource.Message_data_change,
                        Resource.MoreInfo_Exception,
                        HttpContext.TraceIdentifier
                    )
            });
        }

        /// <summary>
        /// Hàm lấy ra bản ghi theo ID
        /// </summary>
        /// <param name="recordID"></param>
        /// <returns>Thông tin chi tiết một bản ghi</returns>
        /// Create by: Nguyễn Khắc Tiềm (26/09/2022)
        [AllowAnonymous]
        [HttpGet("{recordID}")]
        public override async Task<IActionResult> GetRecordByID([FromRoute] Guid recordID, [FromQuery] string? stateForm)
        {
            var record = await Task.FromResult(_productBL.GetRecordByID(recordID.ToString(), stateForm));
            if (record != null)
            {
                return StatusCode(StatusCodes.Status200OK, new ServiceResponse
                {
                    Success = true,
                    Data = record
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

        /// <summary> 
        /// API trả về danh sách đã lọc và phân trang
        /// <summary>
        /// <param name="formData">Trường muốn filter và sắp xếp</param>
        /// <return> Danh sách bản ghi sau khi phân trang, chỉ lấy ra số bản ghi và số trang yêu cầu, và tổng số lượg bản ghi có điều kiện <return>
        /// Create by: Nguyễn Khắc Tiềm (21/09/2022)
        [AllowAnonymous]
        [HttpPost("FilterShop")]
        public virtual async Task<IActionResult> GetFitterShops([FromBody] Dictionary<string, object> formData)
        {
            var records = await Task.FromResult(_productBL.GetFitterShops(formData));
            if (records != null)
            {
                return StatusCode(StatusCodes.Status200OK, new ServiceResponse
                {
                    Success = true,
                    Data = records
                });
            }
            return StatusCode(StatusCodes.Status200OK, new ServiceResponse
            {
                Success = false,
                ErrorCode = MisaAmisErrorCode.NotFoundData,
                Data = new MisaAmisErrorResult(
                        MisaAmisErrorCode.NotFoundData,
                        Resource.DevMsg_Exception,
                        Resource.Message_data_change,
                        Resource.MoreInfo_Exception,
                        HttpContext.TraceIdentifier
                    )
            });
        }

        #endregion
    }
}