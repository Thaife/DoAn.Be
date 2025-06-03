using MISA.WEB08.AMIS.Common.Entities;
using MISA.WEB08.AMIS.Common.Enums;
using MISA.WEB08.AMIS.Common.Result;
using MISA.WEB08.AMIS.DL;
using System;
using System.Security.Cryptography;
using System.Text;

namespace MISA.WEB08.AMIS.BL
{
    public class LoginBL : ILoginBL
    {
        #region Field

        private ILoginDL _loginDL;

        #endregion

        #region Contructor

        public LoginBL(ILoginDL loginDL)
        {
            _loginDL = loginDL;
        }

        #endregion

        #region Method

        /// <summary>
        /// Hàm lấy ra nhân viên đang đăng nhập và check mật khẩu
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public ServiceResponse GetUserLogin(Employee user)
        {
            Employee? result = _loginDL.GetUserLogin(user);
            if(result != null)
            {
                if (result.IsActive == false)
                {
                    return new ServiceResponse
                    {
                        Success = false,
                        ErrorCode = MisaAmisErrorCode.ActiveFalse,
                        Data = "login.active_fail"
                    };
                }
                if (string.Equals(user.EmployeeCode, result.EmployeeCode) && VerifyPassword(user.Password, result.Password))
                {
                    return new ServiceResponse
                    {
                        Success = true,
                        Data = result
                    };
                }
            }
            return new ServiceResponse
            {
                Success = false,
                ErrorCode = MisaAmisErrorCode.LoginFail,
                Data = "login.is_valid_user"
            };
        }

        /// <summary>
        /// Hàm lấy ra nhân viên đang đăng nhập và đổi mk
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public ServiceResponse ChangePassword(Employee user)
        {
            Employee? resultRecord = _loginDL.GetUserLogin(user);
            if (resultRecord != null)
            {
                if (string.Equals(user.EmployeeCode, resultRecord.EmployeeCode) && VerifyPassword(user.Password, resultRecord.Password))
                {
                    user.PasswordNew = HashPassword(user.PasswordNew);
                    var result = _loginDL.UpdatePassword(user);
                    if(result.Success)
                    {
                        return new ServiceResponse
                        {
                            Success = true,
                            Data = user
                        };
                    }    
                }
            }
            return new ServiceResponse
            {
                Success = false,
                ErrorCode = MisaAmisErrorCode.LoginFail,
                Data = "login.is_valid_password"
            };
        }

        /// <summary>
        /// Hàm mã hoá mật khẩu bằng SHA-256
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha256.ComputeHash(passwordBytes);
                string hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                return hash;
            }
        }

        /// <summary>
        /// Hàm kiểm tra mật khẩu đã mã hoá có khớp với mật khẩu gốc hay không
        /// </summary>
        /// <param name="password"></param>
        /// <param name="hashedPassword"></param>
        /// <returns></returns>
        private bool VerifyPassword(string password, string hashedPassword)
        {
            string hashedInput = HashPassword(password);
            return StringComparer.OrdinalIgnoreCase.Compare(hashedInput, hashedPassword) == 0;
        }

        #endregion
    }
}
