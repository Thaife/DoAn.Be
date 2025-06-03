using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.WEB08.AMIS.BL;
using MISA.WEB08.AMIS.Common.Entities;
using MISA.WEB08.AMIS.Common.Enums;
using MISA.WEB08.AMIS.Common.Resources;
using MISA.WEB08.AMIS.Common.Result;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MISA.WEB08.AMIS.API.Controllers
{
    /// <summary>
    /// API dữ liệu với bảng trong database
    /// </summary>
    /// Created by : Nguyễn Khắc Tiềm (21/09/2022)
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BasesController<T> : ControllerBase
    {
        #region Field

        private IBaseBL<T> _baseBL;

        #endregion

        #region Contructor

        public BasesController(IBaseBL<T> baseBL)
        {
            _baseBL = baseBL;
        }

        #endregion

        #region Method

        #region API GET

        /// <summary>
        /// API lấy ra danh sách tất bản ghi trong 1 bảng
        /// <summary>
        /// <return> Danh sách tất cả bản ghi <return>
        /// Create by: Nguyễn Khắc Tiềm (21/09/2022)
        [HttpGet]
        public virtual async Task<IActionResult> GetAllRecords()
        {
            var recordList = await Task.FromResult(_baseBL.GetAllRecords());
            if (recordList != null)
            {
                return StatusCode(StatusCodes.Status200OK, new ServiceResponse
                {
                    Success = true,
                    Data = recordList
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
        /// Hàm Lấy danh sách dropdown
        /// </summary>
        /// <returns>Danh sách tất cả bản ghi</returns>
        /// Create by: Nguyễn Khắc Tiềm (26/09/2022)
        [AllowAnonymous]
        [HttpGet("dropdown")]
        public virtual async Task<IActionResult> GetDropdown()
        {
            var recordList = await Task.FromResult(_baseBL.GetDropdown());
            if (recordList != null)
            {
                return StatusCode(StatusCodes.Status200OK, new ServiceResponse
                {
                    Success = true,
                    Data = recordList
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
        /// Hàm lấy ra bản ghi theo ID
        /// </summary>
        /// <param name="recordID"></param>
        /// <returns>Thông tin chi tiết một bản ghi</returns>
        /// Create by: Nguyễn Khắc Tiềm (26/09/2022)
        [HttpGet("{recordID}")]
        public virtual async Task<IActionResult> GetRecordByID([FromRoute] Guid recordID, [FromQuery] string? stateForm)
        {
            var record = await Task.FromResult(_baseBL.GetRecordByID(recordID.ToString(), stateForm));
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
        /// Hàm lấy ra mã record tự sinh
        /// </summary>
        /// <returns>Mã bản ghi</returns>
        /// Create by: Nguyễn Khắc Tiềm (26/09/2022)
        [HttpGet("next_value")]
        public virtual async Task<IActionResult> GetRecordCodeNew()
        {
            var newCode = await Task.FromResult(_baseBL.GetRecordCodeNew());
            return StatusCode(StatusCodes.Status200OK, new ServiceResponse
            {
                Success = true,
                Data = newCode
            });
        }

        /// <summary> 
        /// API trả về danh sách đã lọc và phân trang
        /// <summary>
        /// <param name="formData">Trường muốn filter và sắp xếp</param>
        /// <return> Danh sách bản ghi sau khi phân trang, chỉ lấy ra số bản ghi và số trang yêu cầu, và tổng số lượg bản ghi có điều kiện <return>
        /// Create by: Nguyễn Khắc Tiềm (21/09/2022)
        [HttpPost("fitter")]
        public virtual async Task<IActionResult> GetFitterRecords([FromBody] Dictionary<string, object> formData)
        {
            var records = await Task.FromResult(_baseBL.GetFitterRecords(formData));
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

        #region API POST

        /// <summary> 
        /// API thêm mới một bản ghi
        /// <summary>
        /// <param name="record">Kiểu dữ liệu bản ghi</param>
        /// <return> ID bản ghi sau khi thêm <return>
        /// Create by: Nguyễn Khắc Tiềm (21/09/2022)
        [HttpPost]
        public virtual async Task<IActionResult> InsertRecord([FromBody] T record)
        {
            var result = await Task.FromResult(_baseBL.InsertRecord(record, GetCurrentUser().EmployeeID.ToString()));
            if (result.Success)
            {
                return StatusCode(StatusCodes.Status201Created, new ServiceResponse
                {
                    Success = true,
                    Data = result.Data
                });
            }
            return StatusCode(StatusCodes.Status200OK, new ServiceResponse
            {
                Success = false,
                ErrorCode = result.ErrorCode,
                Data = new MisaAmisErrorResult(
                            (MisaAmisErrorCode)result.ErrorCode,
                            Resource.DevMsg_ValidateFailed,
                            result.Data,
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
        [HttpPost("bulk_delete")]
        public virtual async Task<IActionResult> DeleteMultiple([FromBody] List<Guid> listRecordID)
        {
            var result = await Task.FromResult(_baseBL.DeleteMultiple(JsonConvert.SerializeObject(listRecordID.Select(ds => new { id = ds })), listRecordID.Count));
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

        #endregion

        #region API PUT

        /// <summary> 
        /// API sửa một bản ghi
        /// <summary>
        /// <param name="recordID">ID bản ghi muốn cập nhật</param>
        /// <param name="record">Kiểu dữ liệu bản ghi cập nhật</param>
        /// <return> ID bản ghi sau khi cập nhật <return>
        /// Create by: Nguyễn Khắc Tiềm (21/09/2022)
        [HttpPut("{recordID}")]
        public virtual async Task<IActionResult> UpdateRecord([FromRoute] Guid recordID, [FromBody] T record)
        {
            var result = await Task.FromResult(_baseBL.UpdateRecord(recordID, record, GetCurrentUser().EmployeeID.ToString()));
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
                ErrorCode = result.ErrorCode,
                Data = new MisaAmisErrorResult(
                        (MisaAmisErrorCode)result.ErrorCode,
                        Resource.DevMsg_ValidateFailed,
                        result.Data,
                        Resource.MoreInfo_Exception,
                        HttpContext.TraceIdentifier
                    )
            });
        }

        #endregion

        #region API DELETE

        /// <summary>
        /// API xoá một bản ghi theo ID
        /// <summary>
        /// <param name="recordID">ID bản ghi</param>
        /// <return> ID bản ghi sau khi xoá <return>
        /// Create by: Nguyễn Khắc Tiềm (21/09/2022)
        [HttpDelete("{recordID}")]
        public virtual async Task<IActionResult> DeleteRecord([FromRoute] Guid recordID)
        {
            var result = await Task.FromResult(_baseBL.DeleteRecord(recordID));
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
                ErrorCode = result.ErrorCode,
                Data = new MisaAmisErrorResult(
                            (MisaAmisErrorCode)result.ErrorCode,
                            Resource.DevMsg_DeleteFailed.ToString(),
                            result.Data,
                            Resource.MoreInfo_Exception,
                            HttpContext.TraceIdentifier
                        )
            });
        }

        /// <summary>
        /// Toggle active
        /// </summary>
        /// <returns>ID bản ghi </returns>
        /// CreatedBy: Nguyễn Khắc Tiềm (6/10/2022)
        [HttpGet("ToggleActive/{recordID}")]
        public virtual async Task<IActionResult> ToggleActive([FromRoute] Guid recordID)
        {
            var result = await Task.FromResult(_baseBL.ToggleActive(recordID, GetCurrentUser().EmployeeID.ToString()));
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
                ErrorCode = MisaAmisErrorCode.Exception,
                Data = new MisaAmisErrorResult(
                            MisaAmisErrorCode.Exception,
                            Resource.DevMsg_Exception,
                            result.Data,
                            Resource.MoreInfo_Exception,
                            HttpContext.TraceIdentifier
                        )
            });
        }

        /// <summary>
        /// API nhập khẩu dữ liệu từ tệp
        /// </summary>
        /// <param name="file">File Excel</param>
        /// <returns></returns>
        /// CreatedBy: Nguyễn Khắc Tiềm (6/10/2022)
        [Route("import-xlsx")]
        [HttpPost, DisableRequestSizeLimit]
        public virtual async Task<IActionResult> ImportXLSX(IFormFile file)
        {
            var result = await Task.FromResult(_baseBL.ImportXLSX(file, GetCurrentUser().EmployeeID.ToString()));
            if (result.Success)
            {
                return StatusCode(StatusCodes.Status200OK, result);
            }
            return StatusCode(StatusCodes.Status200OK, new ServiceResponse
            {
                Success = false,
                ErrorCode = MisaAmisErrorCode.FileNotCorrect,
                Data = new MisaAmisErrorResult(
                        MisaAmisErrorCode.FileNotCorrect,
                        result.Data,
                        Resource.Message_import_fail,
                        Resource.MoreInfo_Exception,
                        HttpContext.TraceIdentifier
                    )
            });
        }

        /// <summary>
        /// Export data ra file excel
        /// </summary>
        /// <param name="formData">Trường muốn filter và sắp xếp</param>
        /// <returns>file Excel chứa dữ liệu danh sách </returns>
        /// CreatedBy: Nguyễn Khắc Tiềm (6/10/2022)
        [HttpPost("export_data")]
        public virtual async Task<IActionResult> ExportData([FromBody] Dictionary<string, object> formData)
        {
            var excelName = await Task.FromResult(_baseBL.ExportData(formData));
            if (!string.IsNullOrEmpty(excelName))
            {
                return StatusCode(StatusCodes.Status200OK, new ServiceResponse
                {
                    Success = true,
                    Data = excelName
                });
            }
            return StatusCode(StatusCodes.Status200OK, new ServiceResponse
            {
                Success = false,
                ErrorCode = MisaAmisErrorCode.InvalidInput,
                Data = new MisaAmisErrorResult(
                            MisaAmisErrorCode.InvalidInput,
                            Resource.DevMsg_ValidateFailed,
                            Resource.Message_export_null,
                            Resource.MoreInfo_Exception,
                            HttpContext.TraceIdentifier
                        )
            });
        }

        #endregion

        /// <summary>
        /// Hàm lấy ra thông tin người đang đăng nhập gọi api
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-current-user")]
        public Employee GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity != null)
            {
                var userClaims = identity.Claims;

                return new Employee
                {
                    EmployeeID = Guid.Parse(userClaims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value),
                    EmployeeCode = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Name)?.Value,
                    EmployeeName = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.GivenName)?.Value,
                    IsActive = bool.Parse(userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value)
                };
            }
            return null;
        }

        #endregion
    }
}