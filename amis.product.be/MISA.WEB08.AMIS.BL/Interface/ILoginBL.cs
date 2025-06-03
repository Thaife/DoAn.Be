using MISA.WEB08.AMIS.Common.Entities;
using MISA.WEB08.AMIS.Common.Result;

namespace MISA.WEB08.AMIS.BL
{
    public interface ILoginBL
    {
        /// <summary>
        /// Hàm lấy ra nhân viên đang đăng nhập và check mật khẩu
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public ServiceResponse GetUserLogin(Employee user);

        /// <summary>
        /// Hàm lấy ra nhân viên đang đăng nhập và đổi mk
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public ServiceResponse ChangePassword(Employee user);
    }
}
