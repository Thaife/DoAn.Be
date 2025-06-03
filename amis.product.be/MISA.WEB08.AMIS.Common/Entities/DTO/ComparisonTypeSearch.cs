using MISA.WEB08.AMIS.Common.Enums;

namespace MISA.WEB08.AMIS.Common.Entities
{
    /// <summary>
    /// Dữ liệu tìm kiếm với các cài đặt
    /// </summary>
    /// Create by: Nguyễn Khắc Tiềm (21/09/2022)
    public class ComparisonTypeSearch
    {
        /// <summary>
        /// Kiểu dữ liệu
        /// </summary>
        public string? TypeSearch { get; set; }

        /// <summary>
        /// Value tìm kiếm
        /// </summary>
        public string? ValueSearch { get; set; }

        /// <summary>
        /// Tên cột tìm kiếm
        /// </summary>
        public string ColumnSearch { get; set; }

        /// <summary>
        /// Kiểu tìm kiếm
        /// </summary>
        public string? ComparisonType { get; set; }

        /// <summary>
        /// Lọc kết hợp join bảng
        /// </summary>
        public Join? Join { get; set; }
    }

    /// <summary>
    /// Option join
    /// </summary>
    /// Create by: Nguyễn Khắc Tiềm (21/09/2022)
    public class Join
    {
        /// <summary>
        /// Kiểu join
        /// </summary>
        public TypeJoin? TypeJoin { get; set; }

        /// <summary>
        /// Key join
        /// </summary>
        public string? KeyJoin { get; set; }

        /// <summary>
        /// Table Join
        /// </summary>
        public string? TableJoin { get; set; }
    }
}
