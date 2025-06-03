using MISA.WEB08.AMIS.Common.Attributes;
using System;

namespace MISA.WEB08.AMIS.Common.Entities
{
    /// <summary>
    /// Đơn vị ứng với bảng unit trong database
    /// </summary>
    /// Created by : Nguyễn Khắc Tiềm 21.09.2022
    public class Origin : BaseEntity
    {
        /// <summary>
        /// id đơn vị
        /// </summary>
        [Validate(PrimaryKey = true)]
        public Guid? OriginID { get; set; }

        /// <summary>
        /// mã đơn vị
        /// </summary>
        [Validate(IsNotNullOrEmpty = true, ErrorMessage = "validate.empty", MaxLength = 25)]
        [ColumnName(Name = "Mã xuất xứ", Width = 16)]
        public string OriginCode { get; set; }

        /// <summary>
        /// tên đơn vị
        /// </summary>
        [Validate(IsNotNullOrEmpty = true, ErrorMessage = "validate.empty", MaxLength = 255)]
        [ColumnName(Name = "Tên xuất xứ", Width = 40)]
        public string OriginName { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        [Validate(MaxLength = 255)]
        [ColumnName(Name = "Mô tả", Width = 25)]
        public string Description { get; set; }

        /// <summary>
        /// Hoạt động hay không hoạt động
        /// </summary>
        [Validate(IsNotNullOrEmpty = true, ErrorMessage = "validate.empty")]
        [ColumnName(Name = "Trạng thái", Width = 20)]
        public bool? IsActive { get; set; }
    }
}