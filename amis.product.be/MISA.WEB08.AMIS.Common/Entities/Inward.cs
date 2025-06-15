using MISA.WEB08.AMIS.Common.Attributes;
using System;
using System.Collections.Generic;

namespace MISA.WEB08.AMIS.Common.Entities
{
    /// <summary>
    /// Nhà kho ứng với bảng Inward trong database
    /// </summary>
    /// Created by : TVTHAI 21.09.2022
    public class Inward : BaseEntity
    {
        /// <summary>
        /// id kho
        /// </summary>
        [Validate(PrimaryKey = true)]
        public Guid? InwardID { get; set; }

        public string InwardName { get; set; }
        public string Description { get; set; }
        public Guid DepotID { get; set; }
        [Validate(NotMapParameterProc = true)]
        public bool? IsApprove { get; set; }
        [Validate(NotMapParameterProc = true)]
        public string IsApproveText { get; set; }
        [Validate(NotMapParameterProc = true)]
        public string DepotCode { get; set; }
        [Validate(NotMapParameterProc = true)]
        public string DepotName { get; set; }

        [Validate(NotMapParameterProc = true)]
        public List<InwardDetail> InwardDetail { get; set; }

    }

    public class InwardDetail
    {
        public Guid? InwardDetailID { get; set; }
        public Guid? InwardID { get; set; }
        public Guid? ProductID { get; set; }
        [Validate(NotMapParameterProc = true)]
        public string ProductCode { get; set; }
        [Validate(NotMapParameterProc = true)]
        public string ProductName { get; set; }
        public decimal? Quantity { get; set; }
        [Validate(NotMapParameterProc = true)]
        public decimal? Price { get; set; }
    }
}
