using Dapper;
using MISA.WEB08.AMIS.Common.Entities;
using MISA.WEB08.AMIS.Common.Result;
using System;

namespace MISA.WEB08.AMIS.DL
{
    public class LoginDL : ILoginDL
    {
        #region Field

        private IDatabaseHelper<Employee> _dbHelper;

        #endregion

        #region Contructor

        public LoginDL(IDatabaseHelper<Employee> dbHelper)
        {
            _dbHelper = dbHelper;
        }

        #endregion

        #region Method

        /// <summary>
        /// Hàm lấy ra nhân viên đang đăng nhập và check mật khẩu
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Employee GetUserLogin(Employee user)
        {
            // Khởi tạo các parameter để chèn vào trong Proc
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add($"v_EmployeeCode", user.EmployeeCode);
            // Khai báo stored procedure
            string storeProcedureName = "Proc_employee_GetLogin";
            return (Employee)_dbHelper.RunProcWithQueryFirstOrDefault(storeProcedureName, parameters);
        }

        /// <summary>
        /// Hàm cập nhật thông tin mật khẩu nhân viên
        /// </summary>
        /// <param name="recordID"></param>
        /// <param name="record"></param>
        /// <returns>ID record sau khi cập nhật</returns>
        public ServiceResponse UpdatePassword(Employee user)
        {
            var v_MessOut = "";
            // Khởi tạo các parameter để chèn vào trong Proc
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("v_EmployeeID", user.EmployeeID);
            parameters.Add("v_Password", user.PasswordNew);
            // chuẩn bị câu lệnh MySQL
            string storeProcedureName = "Proc_employee_ChangePassword";
            int numberOfAffectdRows = _dbHelper.RunProcWithExecute(storeProcedureName, parameters, ref v_MessOut);
            if (numberOfAffectdRows > 0)
            {
                return new ServiceResponse
                {
                    Success = true,
                    Data = user.EmployeeID
                };
            }
            return new ServiceResponse
            {
                Success = false,
                Data = !string.IsNullOrEmpty(v_MessOut) ? v_MessOut : Guid.Empty.ToString()
            };
        }

        #endregion

    }
}
