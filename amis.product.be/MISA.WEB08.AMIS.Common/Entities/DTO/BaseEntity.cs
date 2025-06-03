using MISA.WEB08.AMIS.Common.Attributes;
using System;

namespace MISA.WEB08.AMIS.Common.Entities
{
    /// <summary>
    /// Base của các class model mà model của bảng trong Database nào cũng thường có
    /// </summary>
    /// Create by: Nguyễn Khắc Tiềm (21/09/2022)
    public class BaseEntity
    {
        /// <summary>
        /// Người thêm
        /// </summary>
        [Validate(NotMapParameterProc = true)]
        public string? CreatedBy { get; set; }

        /// <summary>
        /// ngày thêm
        /// </summary>
        [Validate(NotMapParameterProc = true)]
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// người sửa
        /// </summary>
        [Validate(NotMapParameterProc = true)]
        public string? ModifiedBy { get; set; }

        /// <summary>
        /// ngày sửa
        /// </summary>
        [Validate(NotMapParameterProc = true)]
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// Vị trí cột khi nhập khẩu
        /// </summary>
        public int? LineExcel { get; set; }

        /// <summary>
        /// Trạng thái nhập khẩu
        /// </summary>
        public string? StatusImportExcel { get; set; }

        /// <summary>
        /// Chi tiết lỗi khi nhập từ excel
        /// </summary>
        public string? ErrorDetail { get; set; }
    }
}