using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MISA.WEB08.AMIS.BL;
using MISA.WEB08.AMIS.Common.Entities;
using MISA.WEB08.AMIS.Common.Enums;
using MISA.WEB08.AMIS.Common.Resources;
using MISA.WEB08.AMIS.Common.Result;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WEB08.AMIS.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Administrator,Seller")]
    public class LoginController : ControllerBase
    {
        #region Field

        private ILoginBL _loginBL;

        private IConfiguration _config;

        #endregion

        #region Contructor

        public LoginController(ILoginBL loginBL, IConfiguration config)
        {
            _loginBL = loginBL;
            _config = config;
        }

        #endregion

        #region Method

        //[AllowAnonymous]
        /// <summary>
        /// API thực hiện Login từ phía client, tiến hành Authentication và uỷ quyền
        /// </summary>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        [Route("Authentication")]
        [HttpPost]
        public async Task<IActionResult> Authentication([FromBody] Employee userLogin)
        {
            var result = await Task.FromResult(_loginBL.GetUserLogin(userLogin));
            if (result.Success)
            {
                Employee user = (Employee)result.Data;
                user.AccessToken = Generate(user);
                user.Password = null;
                return StatusCode(StatusCodes.Status200OK, new ServiceResponse
                {
                    Success = true,
                    Data = user
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
        /// API đổi mật khẩu
        /// </summary>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        [Route("change-password")]
        [HttpPost]
        public async Task<IActionResult> ChangePassword([FromBody] Employee userLogin)
        {
            var result = await Task.FromResult(_loginBL.ChangePassword(userLogin));
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

        /// <summary>
        /// Hàm xử lý Generate tokken từ người dùng đăng nhập hợp lệ
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private string Generate(Employee user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.EmployeeID.ToString()),
                new Claim(ClaimTypes.Name, user.EmployeeCode),
                new Claim(ClaimTypes.GivenName, user.EmployeeName),
                new Claim(ClaimTypes.Role, user.IsActive.ToString())
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Audience"],
              claims,
              expires: DateTime.Now.AddYears(10),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        #endregion
    }
}
