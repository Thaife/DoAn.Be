using MISA.WEB08.AMIS.Common.Attributes;
using MISA.WEB08.AMIS.Common.Entities;
using MISA.WEB08.AMIS.Common.Enums;
using MISA.WEB08.AMIS.Common.Result;
using MISA.WEB08.AMIS.DL;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MISA.WEB08.AMIS.BL
{
    /// <summary>
    /// Dữ liệu thao tác với Database và trả về với bảng Unit từ tầng BL
    /// </summary>
    /// Create by: Nguyễn Khắc Tiềm (21/09/2022)
    public class CartBL : BaseBL<Cart>, ICartBL
    {
        #region Field

        private ICartDL _cartDL;

        #endregion

        #region Contructor

        public CartBL(ICartDL cartDL) : base(cartDL)
        {
            _cartDL = cartDL;
        }

        #endregion

        #region Method


        /// <summary>
        /// Cập nhật giỏ hàng
        /// </summary>
        /// <param name="recordID">ID bản ghi</param>
        /// <param name="stateForm">Trạng thái lấy (sửa hay nhân bản, ...)</param>
        /// <returns>Thông tin chi tiết một bản ghi</returns>
        /// Create by: Nguyễn Khắc Tiềm (26/09/2022)
        public object UpdateCart(string v_CurrentUser, string v_ProductID, string v_State)
        {
            return _cartDL.UpdateCart(v_CurrentUser, v_ProductID, v_State);
        }

        /// <summary>
        /// Thanh toán
        /// </summary>
        /// <returns>Thông tin chi tiết một bản ghi</returns>
        /// Create by: Nguyễn Khắc Tiềm (26/09/2022)
        public bool Checkout(Order order)
        {
            _cartDL.Checkout(order);
            return true;
        }

        /// <summary>
        /// API lấy ra đơn đặt hàng 
        /// </summary>
        /// <param name="formData">Từ khoá tìm kiếm</param>
        /// <returns>Danh sách record và tổng số bản ghi</returns>
        /// Create by: Nguyễn Khắc Tiềm (26/09/2022)
        public object GetOrderUser(string v_CurrentUser)
        {
            return _cartDL.GetOrderUser(v_CurrentUser);
        }

        /// <summary>
        /// Hàm lấy ra bản ghi theo ID
        /// </summary>
        /// <param name="recordID">ID bản ghi</param>
        /// <param name="stateForm">Trạng thái lấy (sửa hay nhân bản, ...)</param>
        /// <returns>Thông tin chi tiết một bản ghi</returns>
        /// Create by: Nguyễn Khắc Tiềm (26/09/2022)
        public object GetOrderByID(string v_OrderID)
        {
            return _cartDL.GetOrderByID(v_OrderID);
        }

        /// <summary>
        /// API lấy ra địa chỉ
        /// </summary>
        /// <param name="formData">Từ khoá tìm kiếm</param>
        /// <returns>Danh sách record và tổng số bản ghi</returns>
        /// Create by: Nguyễn Khắc Tiềm (26/09/2022)
        public object GetAddress(string v_Address, int? v_ID)
        {
            return _cartDL.GetAddress(v_Address, v_ID);
        }

        /// <summary>
        /// xóa nhiều bản ghi
        /// </summary>
        /// <param name="listRecordID">danh sách bản ghi cần xoá</param>
        /// <param name="count">Số lượng bản ghi bị xoá</param>
        /// <returns>Số kết quả bản ghi đã xoá</returns>
        /// CreatedBy: Nguyễn Khắc Tiềm (5/10/2022)
        public ServiceResponse Actionall(string listRecordID, int count, string action)
        {
            return _cartDL.Actionall(listRecordID, count, action);
        }

        /// <summary>
        /// Export data ra file excel
        /// </summary>
        /// <param name="formData">Trường muốn filter và sắp xếp</param>
        /// <returns>file Excel chứa dữ liệu danh sách </returns>
        /// CreatedBy: Nguyễn Khắc Tiềm (6/10/2022)
        public override string ExportData(Dictionary<string, object> formData)
        {
            var v_Select = formData.Keys.Contains("v_Select") ? JsonConvert.DeserializeObject<List<string>>(Convert.ToString(formData["v_Select"])) : new List<string>();
            List<Order> records = (List<Order>)GetFitterRecords(formData).RecordList;
            if (records == null || records.Count == 0 || v_Select.Count == 0) return null;
            // lấy các thuộc tính của nhân viên
            string listField = string.Join(",", v_Select);
            var properties = typeof(Order).GetProperties();
            int indexCol = SaveFileImage.CalcIndexCol(properties, listField);
            string column = SaveFileImage.GetColumnName(indexCol + 1);
            var stream = new MemoryStream();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(stream ?? new MemoryStream()))
            {
                var sheet = package.Workbook.Worksheets.Add(CustomOptionExport().Header);
                // style header customize tên header của file excel
                SaveFileImage.SetUpExportHeaderData(ref sheet, column, CustomOptionExport().Header, records.Count(), properties, listField);
                int index = 4;
                int indexRow = 0;
                string convertDate = "M/d/yyyy hh:mm:ss tt";
                // duyệt các nhân viên thêm dữ liệu vào excel (phần body)
                foreach (var recordItem in records)
                {
                    int indexBody = 2;
                    sheet.Cells[index, 1].Value = indexRow + 1;
                    sheet.Cells[index, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    index++;
                    foreach (var property in properties)
                    {
                        var displayNameAttributes = property.GetCustomAttributes(typeof(ColumnName), true);
                        if (listField.Contains(property.Name) && displayNameAttributes != null && displayNameAttributes.Length > 0)
                        {
                            // các kiểu dữ liệu khác datatime và giới tính
                            if (!SaveFileImage.SetUpDataExportDefault(ref sheet, ref convertDate, property, indexRow, indexBody, displayNameAttributes, recordItem))
                            {
                                if (property.Name == "TypeCheckout")
                                {
                                    sheet.Cells[indexRow + 4, indexBody].Value = recordItem.TypeCheckout == TypeCheckout.Live ? "Chuyển khoản trực tiếp" : "Kiểm tra hàng và thanh toán";
                                }
                                else if (property.Name == "Status")
                                {
                                    sheet.Cells[indexRow + 4, indexBody].Value = recordItem.Status == StatusOrder.WaitConfirm ? "Chờ xác nhận" : recordItem.Status == StatusOrder.Confirm ? "Đã xác nhận" : recordItem.Status == StatusOrder.Delivery ? "Đang vận chuyển" : recordItem.Status == StatusOrder.Delivered ? "Đã vận chuyển" : "Đã huỷ đơn";
                                }
                                else if ((displayNameAttributes[0] as ColumnName).IsNumber && !string.IsNullOrEmpty(Convert.ToString(property.GetValue(recordItem))))
                                {
                                    sheet.Cells[indexRow + 4, indexBody].Value = Validate<Order>.FormatNumber(double.Parse(property.GetValue(recordItem) + ""));
                                    sheet.Cells[indexRow + 4, indexBody].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                }
                                else
                                {
                                    sheet.Cells[indexRow + 4, indexBody].Value = property.GetValue(recordItem);
                                }
                            }
                            indexBody++;
                        }
                    }
                    indexRow++;
                }
                package.Save();

                package.Stream.Position = 0;
                string execlName = $"{CustomOptionExport().FileName} ({DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss")}).xlsx";
                return SaveFileImage.SaveExcelFileToDisk(package.Stream, execlName);
            }
        }

        /// <summary>
        /// Hàm custom dữ liệu tên file, header, ... khi xuất file
        /// </summary>
        /// <returns></returns>
        ///  NK Tiềm 05/10/2022
        public override OptionExport CustomOptionExport()
        {
            return new OptionExport
            {
                FileName = "Danh sách đơn hàng",
                Header = "DANH SÁCH ĐƠN HÀNG"
            };
        }

        /// <summary>
        /// Thêm một status order
        /// </summary>
        public ServiceResponse AddStatusOrder(string orderID, string comment, string userID)
        {
            return _cartDL.AddStatusOrder(orderID, comment, userID);
        }

        /// <summary>
        /// API lấy ra status order
        /// </summary>
        /// <param name="formData">Từ khoá tìm kiếm</param>
        /// <returns>Danh sách record và tổng số bản ghi</returns>
        /// Create by: Nguyễn Khắc Tiềm (26/09/2022)
        public object GetStatusOrder(string v_OrderID)
        {
            return _cartDL.GetStatusOrder(v_OrderID);
        }

        #endregion
    }
}
