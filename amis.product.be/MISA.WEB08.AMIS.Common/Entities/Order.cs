using MISA.WEB08.AMIS.Common.Attributes;
using MISA.WEB08.AMIS.Common.Enums;
using System;
using System.Collections.Generic;

namespace MISA.WEB08.AMIS.Common.Entities
{
    public class Order : BaseEntity
    {
        [Validate(PrimaryKey = true)]
        public Guid? OrderID { get; set; }

        public Guid? CurrentUser { get; set; }

        [ColumnName(Name = "Tên khách hàng", Width = 45)]
        public string UserName { get; set; }

        [ColumnName(Name = "Số điện thoại", Width = 45)]
        public string PhoneNumber { get; set; }

        [ColumnName(Name = "Email", Width = 45)]
        public string Email { get; set; }

        [ColumnName(Name = "Tỉnh thành phố", Width = 45)]
        public string Province { get; set; }

        [ColumnName(Name = "Quận huyện", Width = 45)]
        public string District { get; set; }

        [ColumnName(Name = "Phường xã", Width = 45)]
        public string Ward { get; set; }

        [ColumnName(Name = "Địa chỉ chi tiết", Width = 45)]
        public string Address { get; set; }

        [ColumnName(Name = "Ghi chú", Width = 45)]
        public string Description { get; set; }

        public string? CouponID { get; set; } = "";

        public string? CouponCode { get; set; } = "";

        public int? Percent { get; set; } = 0;

        [ColumnName(Name = "Hình thức thanh toán", Width = 45)]
        public TypeCheckout? TypeCheckout { get; set; }

        [ColumnName(Name = "Trạng thái", Width = 45)]
        public StatusOrder? Status { get; set; }

        public List<OrderDetail> OrderDetail { get; set; } = new List<OrderDetail>();
    }
    public class OrderDetail : Product
    {
        [Validate(PrimaryKey = true)]
        public Guid? OrderDetailID { get; set; }

        public Guid? OrderID { get; set; }

        public int? Quantity { get; set; }

        public double? PriceOrder { get; set; }
    }
}
