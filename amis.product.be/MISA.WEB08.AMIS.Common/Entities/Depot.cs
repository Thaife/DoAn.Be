using MISA.WEB08.AMIS.Common.Attributes;
using System;

namespace MISA.WEB08.AMIS.Common.Entities
{
    /// <summary>
    /// Nhà kho ứng với bảng depot trong database
    /// </summary>
    /// Created by : Nguyễn Khắc Tiềm 21.09.2022
    public class Depot : BaseEntity
    {
        /// <summary>
        /// id kho
        /// </summary>
        [Validate(PrimaryKey = true)]
        public Guid? DepotID { get; set; }

        /// <summary>
        /// Mã kho
        /// </summary>
        [Validate(IsNotNullOrEmpty = true, ErrorMessage = "validate.empty", MaxLength = 20)]
        [ColumnName(Name = "Mã kho", Width = 30)]
        public string DepotCode { get; set; }

        /// <summary>
        /// Tên kho
        /// </summary>
        [Validate(IsNotNullOrEmpty = true, ErrorMessage = "validate.empty", MaxLength = 255)]
        [ColumnName(Name = "Tên kho", Width = 50)]
        public string DepotName { get; set; }

        /// <summary>
        /// Địa chỉ
        /// </summary>
        [Validate(MaxLength = 255)]
        [ColumnName(Name = "Địa chỉ", Width = 100)]
        public string DepotDelivery { get; set; }

        /// <summary>
        /// Hoạt động hay không hoạt động
        /// </summary>
        [Validate(IsNotNullOrEmpty = true, ErrorMessage = "validate.empty")]
        [ColumnName(Name = "Trạng thái", Width = 20)]
        public bool? IsActive { get; set; }
    }

    /// <summary>
    /// Nhà kho ứng với bảng depot trong database Import
    /// </summary>
    /// Created by : Nguyễn Khắc Tiềm 21.09.2022
    public class DepotImport : BaseEntity
    {
        /// <summary>
        /// id kho
        /// </summary>
        public string? DepotID { get; set; }

        /// <summary>
        /// Mã kho
        /// </summary>
        public string DepotCode { get; set; }

        /// <summary>
        /// Tên kho
        /// </summary>
        public string DepotName { get; set; }

        /// <summary>
        /// Địa chỉ
        /// </summary>
        public string DepotDelivery { get; set; }

        /// <summary>
        /// Hoạt động hay không hoạt động
        /// </summary>
        [ValidateString(IsBoolean = true)]
        public string? IsActive { get; set; }
    }
}
