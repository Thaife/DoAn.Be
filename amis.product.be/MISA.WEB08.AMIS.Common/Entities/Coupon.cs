using MISA.WEB08.AMIS.Common.Attributes;
using System;

namespace MISA.WEB08.AMIS.Common.Entities
{
    /// <summary>
    /// Đơn vị ứng với bảng unit trong database
    /// </summary>
    /// Created by : Nguyễn Khắc Tiềm 21.09.2022
    public class Coupon : BaseEntity
    {
        /// <summary>
        /// id đơn vị
        /// </summary>
        [Validate(PrimaryKey = true)]
        public Guid? CouponID { get; set; }

        /// <summary>
        /// mã đơn vị
        /// </summary>
        [Validate(IsNotNullOrEmpty = true, ErrorMessage = "validate.empty", MaxLength = 25)]
        [ColumnName(Name = "Mã giảm giá", Width = 16)]
        public string CouponCode { get; set; }

        /// <summary>
        /// tên đơn vị
        /// </summary>
        [Validate(IsNotNullOrEmpty = true, ErrorMessage = "validate.empty", MaxLength = 255)]
        [ColumnName(Name = "Tên sự kiện giảm giá", Width = 40)]
        public string CouponName { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        [Validate(MaxLength = 255)]
        [ColumnName(Name = "Mô tả", Width = 25)]
        public string Description { get; set; }

        /// <summary>
        /// Giảm giá %
        /// </summary>
        [Validate(IsNotNullOrEmpty = true)]
        [ColumnName(Name = "Giảm giá (%)", Width = 25, IsNumber = true)]
        public int? Percent { get; set; } = 0;

        /// <summary>
        /// Giảm giá %
        /// </summary>
        [Validate(IsNotNullOrEmpty = true)]
        [ColumnName(Name = "Số lượng", Width = 25, IsNumber = true)]
        public int? Quantity { get; set; } = 0;

        /// <summary>
        /// ngày sinh
        /// </summary>
        [Validate(IsNotNullOrEmpty = true)]
        [ColumnName(Name = "Ngày bắt đầu", Width = 20, IsDate = true)]
        public DateTime? DateStart { get; set; }

        /// <summary>
        /// ngày sinh
        /// </summary>
        [Validate(IsNotNullOrEmpty = true)]
        [ColumnName(Name = "Ngày Kết thúc", Width = 20, IsDate = true)]
        public DateTime? DateEnd { get; set; }

        /// <summary>
        /// Hoạt động hay không hoạt động
        /// </summary>
        [Validate(IsNotNullOrEmpty = true, ErrorMessage = "validate.empty")]
        [ColumnName(Name = "Trạng thái", Width = 20)]
        public bool? IsActive { get; set; }
    }
}