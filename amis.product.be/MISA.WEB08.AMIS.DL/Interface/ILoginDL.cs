using MISA.WEB08.AMIS.Common.Entities;
using MISA.WEB08.AMIS.Common.Result;
using System;

namespace MISA.WEB08.AMIS.DL
{
    public interface ILoginDL
    {
        /// <summary>
        /// Hàm lấy ra nhân viên đang đăng nhập và check mật khẩu
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Employee GetUserLogin(Employee user);

        /// <summary>
        /// Hàm cập nhật thông tin mật khẩu nhân viên
        /// </summary>
        /// <param name="recordID"></param>
        /// <param name="record"></param>
        /// <returns>ID record sau khi cập nhật</returns>
        public ServiceResponse UpdatePassword(Employee user);
    }
}
