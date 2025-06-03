using MISA.WEB08.AMIS.Common.Entities;
using MISA.WEB08.AMIS.BL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MISA.WEB08.AMIS.Common.Result;
using Microsoft.AspNetCore.Http;
using MISA.WEB08.AMIS.Common.Enums;
using MISA.WEB08.AMIS.Common.Resources;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;

namespace MISA.WEB08.AMIS.API.Controllers
{
    /// <summary>
    /// API dữ liệu với bảng branch
    /// </summary>
    /// Created by : Nguyễn Khắc Tiềm (21/09/2022)
    //[Authorize]
    public class CartsController : BasesController<Cart>
    {
        #region Field

        private ICartBL _cartBL;

        #endregion

        #region Contructor

        public CartsController(ICartBL cartBL) : base(cartBL)
        {
            _cartBL = cartBL;
        }

        #endregion

        #region Method

        /// <summary>
        /// Hàm lấy ra bản ghi theo ID
        /// </summary>
        /// <param name="recordID"></param>
        /// <returns>Thông tin chi tiết một bản ghi</returns>
        /// Create by: Nguyễn Khắc Tiềm (26/09/2022)
        [HttpGet("UpdateCart")]
        public virtual async Task<IActionResult> UpdateCart([FromQuery] string? v_CurrentUser, [FromQuery] string? v_ProductID, [FromQuery] string? v_State)
        {
            var record = await Task.FromResult(_cartBL.UpdateCart(v_CurrentUser, v_ProductID, v_State));
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
        /// API sửa một bản ghi
        /// <summary>
        /// <param name="recordID">ID bản ghi muốn cập nhật</param>
        /// <param name="record">Kiểu dữ liệu bản ghi cập nhật</param>
        /// <return> ID bản ghi sau khi cập nhật <return>
        /// Create by: Nguyễn Khắc Tiềm (21/09/2022)
        [HttpPost("Checkout")]
        public virtual async Task<IActionResult> Checkout([FromBody] Order order)
        {
            var result = await Task.FromResult(_cartBL.Checkout(order));
            return StatusCode(StatusCodes.Status200OK, new ServiceResponse
            {
                Success = true,
                Data = result
            });
        }

        /// <summary> 
        /// API lấy ra đơn đặt hàng 
        /// <summary>
        /// <param name="formData">Trường muốn filter và sắp xếp</param>
        /// <return> Danh sách bản ghi sau khi phân trang, chỉ lấy ra số bản ghi và số trang yêu cầu, và tổng số lượg bản ghi có điều kiện <return>
        /// Create by: Nguyễn Khắc Tiềm (21/09/2022)
        [HttpGet("GetOrderUser")]
        public virtual async Task<IActionResult> GetOrderUser([FromQuery] string? v_CurrentUser)
        {
            var records = await Task.FromResult(_cartBL.GetOrderUser(v_CurrentUser));
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
        [HttpGet("GetOrderByID")]
        public async Task<IActionResult> GetOrderByID([FromQuery] string? v_OrderID)
        {
            var record = await Task.FromResult(_cartBL.GetOrderByID(v_OrderID));
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
        [Authorize]
        [HttpPost("fitter")]
        public override async Task<IActionResult> GetFitterRecords([FromBody] Dictionary<string, object> formData)
        {
            var records = await Task.FromResult(_cartBL.GetFitterRecords(formData));
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
        /// API lấy ra địa chỉ
        /// </summary>
        /// <param name="formData">Từ khoá tìm kiếm</param>
        /// <returns>Danh sách record và tổng số bản ghi</returns>
        /// Create by: Nguyễn Khắc Tiềm (26/09/2022)
        [HttpGet("GetAddress")]
        public virtual async Task<IActionResult> GetAddress([FromQuery] string? v_Address, [FromQuery] int? v_ID)
        {
            var records = await Task.FromResult(_cartBL.GetAddress(v_Address, v_ID));
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
        /// API xoá hàng loạt bản ghi
        /// <summary>
        /// <param name="listRecordID">Danh sách ID bản ghi muốn xoá</param>
        /// <return> danh sách ID bản ghi sau khi xoá <return>
        /// Create by: Nguyễn Khắc Tiềm (21/09/2022)
        [HttpPost("ActionMultipleApi")]
        public async Task<IActionResult> Actionall([FromBody] List<Guid> listRecordID, [FromQuery] string? action)
        {
            var result = await Task.FromResult(_cartBL.Actionall(JsonConvert.SerializeObject(listRecordID.Select(ds => new { id = ds })), listRecordID.Count, action));
            if (result.Success)
            {
                return StatusCode(StatusCodes.Status200OK, new ServiceResponse
                {
                    Success = true,
                    Data = result
                });
            }
            return StatusCode(StatusCodes.Status200OK, new ServiceResponse
            {
                Success = false,
                ErrorCode = MisaAmisErrorCode.DeleteMultiple,
                Data = new MisaAmisErrorResult(
                            MisaAmisErrorCode.DeleteMultiple,
                            Resource.DevMsg_DeleteMultipleFailed.ToString(),
                            result.Data,
                            Resource.MoreInfo_Exception,
                            HttpContext.TraceIdentifier
                        )
            });
        }

        /// <summary>
        /// Thêm một status order
        /// </summary>
        [Authorize]
        [HttpGet("AddStatusOrder")]
        public async Task<IActionResult> AddStatusOrder([FromQuery] string? orderID, [FromQuery] string? comment)
        {
            var result = await Task.FromResult(_cartBL.AddStatusOrder(orderID, comment, GetCurrentUser().EmployeeID.ToString()));
            if (result.Success)
            {
                return StatusCode(StatusCodes.Status200OK, new ServiceResponse
                {
                    Success = true,
                    Data = result.Data
                });
            }
            return StatusCode(StatusCodes.Status200OK, new ServiceResponse
            {
                Success = false,
                ErrorCode = MisaAmisErrorCode.DeleteMultiple,
                Data = new MisaAmisErrorResult(
                            MisaAmisErrorCode.DeleteMultiple,
                            Resource.DevMsg_DeleteMultipleFailed.ToString(),
                            result.Data,
                            Resource.MoreInfo_Exception,
                            HttpContext.TraceIdentifier
                        )
            });
        }

        /// <summary>
        /// API lấy ra địa chỉ
        /// </summary>
        /// <param name="formData">Từ khoá tìm kiếm</param>
        /// <returns>Danh sách record và tổng số bản ghi</returns>
        /// Create by: Nguyễn Khắc Tiềm (26/09/2022)
        [HttpGet("GetStatusOrder")]
        public virtual async Task<IActionResult> GetStatusOrder([FromQuery] string? v_OrderID)
        {
            var records = await Task.FromResult(_cartBL.GetStatusOrder(v_OrderID));
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