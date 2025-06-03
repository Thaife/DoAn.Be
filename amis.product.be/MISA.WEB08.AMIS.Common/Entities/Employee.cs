using System;
using MISA.WEB08.AMIS.Common.Attributes;
using MISA.WEB08.AMIS.Common.Enums;

namespace MISA.WEB08.AMIS.Common.Entities
{
    /// <summary>
    /// nhân viên ứng với bảng employee trong database
    /// </summary>
    /// Created by : Nguyễn Khắc Tiềm 21.09.2022
    public class Employee : BaseEntity
    {
        /// <summary>
        /// id nhân viên
        /// </summary>
        [Validate(PrimaryKey = true)]
        public Guid? EmployeeID { get; set; }

        /// <summary>
        /// id đơn vị
        /// </summary>7
        [Validate(IsNotNullOrEmpty = true, ErrorMessage = "validate.empty")]
        public Guid? BranchID { get; set; }

        /// <summary>
        /// mã nhân viên
        /// </summary>
        [Validate(IsNotNullOrEmpty = true, ErrorMessage = "validate.empty", MaxLength = 20)]
        [ColumnName(Name = "Mã nhân viên", Width = 16)]
        public string EmployeeCode { get; set; }

        /// <summary>
        /// tên nhân viên
        /// </summary>
        [Validate(IsNotNullOrEmpty = true, ErrorMessage = "validate.empty", MaxLength = 80)]
        [ColumnName(Name = "Tên nhân viên", Width = 40)]
        public string EmployeeName { get; set; }

        /// <summary>
        /// tên đơn vị
        /// </summary>
        [ColumnName(Name = "Tên đơn vị", Width = 50)]
        public string? BranchName { get; set; }

        /// <summary>
        /// mã đơn vị
        /// </summary>
        [ColumnName(Name = "Mã đơn vị", Width = 30)]
        public string? BranchCode { get; set; }

        /// <summary>
        /// ngày sinh
        /// </summary>
        [ColumnName(Name = "Ngày sinh", Width = 20, IsDate = true)]
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// giới tính
        /// </summary>
        [Validate(IsNotNullOrEmpty = true, ErrorMessage = "validate.empty")]
        [ColumnName(Name = "Giới tính", Width = 10, IsGender = true)]
        public Gender? Gender { get; set; }

        /// <summary>
        /// chứng minh thư
        /// </summary>
        [Validate(MaxLength = 80)]
        [ColumnName(Name = "Chứng minh thư", Width = 20)]
        public string IdentityCard { get; set; }

        /// <summary>
        /// chi nhánh ngân hàng
        /// </summary>
        [Validate(MaxLength = 255)]
        [ColumnName(Name = "Chi nhánh ngân hàng", Width = 40)]
        public string BranchBank { get; set; }

        /// <summary>
        /// chức danh
        /// </summary>
        [Validate(MaxLength = 100)]
        [ColumnName(Name = "Chức danh", Width = 20)]
        public string EmployeeTitle { get; set; }

        /// <summary>
        /// số tài khoản
        /// </summary>
        [Validate(MaxLength = 80)]
        [ColumnName(Name = "Số tài khoản", Width = 20)]
        public string BankAccount { get; set; }

        /// <summary>
        /// tên ngân hàng
        /// </summary>
        [Validate(MaxLength = 100)]
        [ColumnName(Name = "Tên ngân hàng", Width = 40)]
        public string NameBank { get; set; }

        /// <summary>
        /// ngày cấp cmnd
        /// </summary>
        [ColumnName(Name = "Ngày cấp chứng minh thư", Width = 30, IsDate = true)]
        public DateTime? DayForIdentity { get; set; }

        /// <summary>
        /// địa chỉ cấp cmnd
        /// </summary>
        [Validate(MaxLength = 255)]
        [ColumnName(Name = "Nơi cấp", Width = 40)]
        public string GrantAddressIdentity { get; set; }

        /// <summary>
        /// số điện thoại
        /// </summary>
        [ColumnName(Name = "Điện thoại", Width = 20)]
        [Validate(MaxLength = 50)]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// số điện thoại cố định
        /// </summary>
        [ColumnName(Name = "Điện thoại cố định", Width = 20)]
        [Validate(MaxLength = 50)]
        public string LandlinePhone { get; set; }

        /// <summary>
        /// điạ chỉ email
        /// </summary>
        [ColumnName(Name = "Địa chỉ Email", Width = 30)]
        [Validate(MaxLength = 100)]
        public string EmployeeEmail { get; set; }

        /// <summary>
        /// địa chỉ nhân viên
        /// </summary>
        [Validate(MaxLength = 255)]
        [ColumnName(Name = "Địa chỉ", Width = 40)]
        public string EmployeeAddress { get; set; }

        /// <summary>
        /// Hoạt động hay không hoạt động
        /// </summary>
        [Validate(IsNotNullOrEmpty = true, ErrorMessage = "validate.empty")]
        [ColumnName(Name = "Là khách hàng", Width = 20, IsBollen = true)]
        public bool? IsCustomer { get; set; } = false;

        /// <summary>
        /// Hoạt động hay không hoạt động
        /// </summary>
        [Validate(IsNotNullOrEmpty = true, ErrorMessage = "validate.empty")]
        [ColumnName(Name = "Là nhà cung cấp", Width = 20, IsBollen = true)]
        public bool? IsVendor { get; set; } = false;

        /// <summary>
        /// Hoạt động hay không hoạt động
        /// </summary>
        [Validate(IsNotNullOrEmpty = true, ErrorMessage = "validate.empty")]
        [ColumnName(Name = "Trạng thái", Width = 20)]
        public bool? IsActive { get; set; } = true;

        /// <summary>
        /// Mật khẩu
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// Mật khẩu
        /// </summary>
        public string? PasswordNew { get; set; }

        /// <summary>
        /// Mật khẩu
        /// </summary>
        public string? PasswordAccess { get; set; }

        /// <summary>
        /// Token
        /// </summary>
        public string? AccessToken { get; set; }
    }

    /// <summary>
    /// nhân viên ứng với bảng employee trong database dùng để Import
    /// </summary>
    /// Created by : Nguyễn Khắc Tiềm 21.09.2022
    public class EmployeeImport : BaseEntity
    {
        /// <summary>
        /// id nhân viên
        /// </summary>
        public string? EmployeeID { get; set; }

        /// <summary>
        /// id đơn vị
        /// </summary>7
        public string? BranchID { get; set; }

        /// <summary>
        /// mã nhân viên
        /// </summary>
        public string EmployeeCode { get; set; }

        /// <summary>
        /// tên nhân viên
        /// </summary>
        public string EmployeeName { get; set; }

        /// <summary>
        /// tên đơn vị
        /// </summary>
        public string? BranchName { get; set; }

        /// <summary>
        /// mã đơn vị
        /// </summary>
        public string? BranchCode { get; set; }

        /// <summary>
        /// ngày sinh
        /// </summary>
        [ValidateString(IsDate = true)]
        public string? DateOfBirth { get; set; }

        /// <summary>
        /// giới tính
        /// </summary>
        public string? Gender { get; set; }

        /// <summary>
        /// chứng minh thư
        /// </summary>
        public string IdentityCard { get; set; }

        /// <summary>
        /// chi nhánh ngân hàng
        /// </summary>
        public string BranchBank { get; set; }

        /// <summary>
        /// chức danh
        /// </summary>
        public string EmployeeTitle { get; set; }

        /// <summary>
        /// số tài khoản
        /// </summary>
        public string BankAccount { get; set; }

        /// <summary>
        /// tên ngân hàng
        /// </summary>
        public string NameBank { get; set; }

        /// <summary>
        /// ngày cấp cmnd
        /// </summary>
        [ValidateString(IsDate = true)]
        public string? DayForIdentity { get; set; }

        /// <summary>
        /// địa chỉ cấp cmnd
        /// </summary>
        public string GrantAddressIdentity { get; set; }

        /// <summary>
        /// số điện thoại
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// số điện thoại cố định
        /// </summary>
        public string LandlinePhone { get; set; }

        /// <summary>
        /// điạ chỉ email
        /// </summary>
        public string EmployeeEmail { get; set; }

        /// <summary>
        /// địa chỉ nhân viên
        /// </summary>
        public string EmployeeAddress { get; set; }

        /// <summary>
        /// Hoạt động hay không hoạt động
        /// </summary>
        [ValidateString(IsBoolean = true)]
        public string? IsCustomer { get; set; }

        /// <summary>
        /// Hoạt động hay không hoạt động
        /// </summary>
        [ValidateString(IsBoolean = true)]
        public string? IsVendor { get; set; }

        /// <summary>
        /// Hoạt động hay không hoạt động
        /// </summary>
        [ValidateString(IsBoolean = true)]
        public string? IsActive { get; set; }

        /// <summary>
        /// Mật khẩu
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// Mật khẩu
        /// </summary>
        public string? PasswordNew { get; set; }

        /// <summary>
        /// Mật khẩu
        /// </summary>
        public string? PasswordAccess { get; set; }

        /// <summary>
        /// Token
        /// </summary>
        public string? AccessToken { get; set; }
    }
}