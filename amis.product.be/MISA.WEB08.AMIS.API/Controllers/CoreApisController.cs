using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.WEB08.AMIS.Common.Enums;
using MISA.WEB08.AMIS.Common.Resources;
using MISA.WEB08.AMIS.Common.Result;
using MISA.WEB08.AMIS.DL;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MISA.WEB08.AMIS.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CoreApisController : ControllerBase
    {
        /// <summary>
        /// API post một hình ảnh
        /// </summary>
        /// <param name="file">File</param>
        /// <returns></returns>
        /// CreatedBy: Nguyễn Khắc Tiềm (6/10/2022)
        [Route("PostImage")]
        [HttpPost]
        public virtual async Task<IActionResult> PostImage(IFormFile file)
        {
            if (file.Length > 0)
            {
                var imageName = $"/Images/Upload/{Guid.NewGuid()}_{file.FileName}";
                using var stream = new FileStream(DataContext.Path_root + imageName, FileMode.Create);
                await file.CopyToAsync(stream);

                return StatusCode(StatusCodes.Status200OK, new ServiceResponse
                {
                    Success = true,
                    Data = imageName
                });
            }
            return StatusCode(StatusCodes.Status200OK, new ServiceResponse
            {
                Success = false,
                ErrorCode = MisaAmisErrorCode.FileNotCorrect,
                Data = new MisaAmisErrorResult(
                        MisaAmisErrorCode.FileNotCorrect,
                        "validate.empty_file",
                        "validate.empty_file",
                        Resource.MoreInfo_Exception,
                        HttpContext.TraceIdentifier
                    )
            });
        }
    }
}
