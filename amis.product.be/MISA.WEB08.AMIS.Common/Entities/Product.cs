using MISA.WEB08.AMIS.Common.Attributes;
using MISA.WEB08.AMIS.Common.Enums;
using System;

namespace MISA.WEB08.AMIS.Common.Entities
{
    /// <summary>
    /// Đơn vị ứng với bảng unit trong database
    /// </summary>
    /// Created by : Nguyễn Khắc Tiềm 21.09.2022
    public class Product : BaseEntity
    {
        /// <summary>
        /// id đơn vị
        /// </summary>
        [Validate(PrimaryKey = true)]
        public Guid? ProductID { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public Guid? DepotID { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        [ColumnName(Name = "Mã kho", Width = 25)]
        public string DepotCode { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        [ColumnName(Name = "Tên kho", Width = 40)]
        public string DepotName { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public Guid? OriginID { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        [ColumnName(Name = "Mã xuất xứ", Width = 25)]
        public string OriginCode { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        [ColumnName(Name = "Tên xuất xứ", Width = 40)]
        public string OriginName { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public Guid? TrademarkID { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        [ColumnName(Name = "Mã thương hiệu", Width = 25)]
        public string TrademarkCode { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        [ColumnName(Name = "Tên thương hiệu", Width = 40)]
        public string TrademarkName { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public Guid? CategoryID { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        [ColumnName(Name = "Mã danh mục", Width = 25)]
        public string CategoryCode { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        [ColumnName(Name = "Tên danh mục", Width = 40)]
        public string CategoryName { get; set; }

        /// <summary>
        /// mã đơn vị
        /// </summary>
        [Validate(IsNotNullOrEmpty = true, ErrorMessage = "validate.empty", MaxLength = 25)]
        [ColumnName(Name = "Mã sản phẩm", Width = 16)]
        public string ProductCode { get; set; }

        /// <summary>
        /// tên đơn vị
        /// </summary>
        [Validate(IsNotNullOrEmpty = true, ErrorMessage = "validate.empty", MaxLength = 255)]
        [ColumnName(Name = "Tên sản phẩm", Width = 40)]
        public string ProductName { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        [ColumnName(Name = "Mô tả", Width = 25)]
        public string Description { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        [ColumnName(Name = "Giá bán", Width = 25, IsNumber = true)]
        public double? Price { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        [ColumnName(Name = "Chất liệu", Width = 45)]
        public string Material { get; set; }

        /// <summary>
        /// giới tính
        /// </summary>
        [Validate(IsNotNullOrEmpty = true, ErrorMessage = "validate.empty")]
        [ColumnName(Name = "Giới tính", Width = 10, IsGender = true)]
        public Gender? Gender { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        [ColumnName(Name = "Thời lượng pin", Width = 45)]
        public string BatteryLife { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        [ColumnName(Name = "Thời hạn bảo hành", Width = 40)]
        public string WarrantyPeriod { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public int? Visit { get; set; }

        /// <summary>
        /// Hình ảnh
        /// </summary>
        public string? Avatar { get; set; }

        /// <summary>
        /// Hoạt động hay không hoạt động
        /// </summary>
        [Validate(IsNotNullOrEmpty = true, ErrorMessage = "validate.empty")]
        [ColumnName(Name = "Trạng thái", Width = 20)]
        public bool? IsActive { get; set; }
    }

    /// <summary>
    /// nhân viên ứng với bảng employee trong database dùng để Import
    /// </summary>
    /// Created by : Nguyễn Khắc Tiềm 21.09.2022
    public class ProductImport : BaseEntity
    {
        /// <summary>
        /// id đơn vị
        /// </summary>
        public string? ProductID { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public string? DepotID { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public string DepotCode { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public string DepotName { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public string OriginID { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public string OriginCode { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public string OriginName { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public string TrademarkID { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public string TrademarkCode { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public string TrademarkName { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public string CategoryID { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public string CategoryCode { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// mã đơn vị
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// tên đơn vị
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        [ValidateString(IsNumber = true)]
        public double? Price { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public string Material { get; set; }

        /// <summary>
        /// giới tính
        /// </summary>
        public string? Gender { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public string BatteryLife { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public string WarrantyPeriod { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public int? Visit { get; set; }

        /// <summary>
        /// Hình ảnh
        /// </summary>
        public string? Avatar { get; set; }

        /// <summary>
        /// Hoạt động hay không hoạt động
        /// </summary>
        [ValidateString(IsBoolean = true)]
        public string? IsActive { get; set; }
    }
}