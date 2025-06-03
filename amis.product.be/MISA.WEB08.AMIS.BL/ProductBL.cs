using MISA.WEB08.AMIS.Common.Entities;
using MISA.WEB08.AMIS.DL;
using MISA.WEB08.AMIS.Common.Enums;
using MISA.WEB08.AMIS.Common.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using MISA.WEB08.AMIS.Common.Attributes;
using System.Reflection;
using OfficeOpenXml;
using MISA.WEB08.AMIS.Common.Resources;
using OfficeOpenXml.Style;

namespace MISA.WEB08.AMIS.BL
{
    /// <summary>
    /// Dữ liệu thao tác với Database và trả về với bảng Product từ tầng BL
    /// </summary>
    /// Create by: Nguyễn Khắc Tiềm (21/09/2022)
    public class ProductBL : BaseBL<Product>, IProductBL
    {
        #region Field

        private IProductDL _productDL;

        #endregion

        #region Contructor

        public ProductBL(IProductDL productDL) : base(productDL)
        {
            _productDL = productDL;
        }

        #endregion

        #region Method

        #region overidve

        /// <summary>
        /// Hàm xử lý custom kết quả validate cho bản ghi cần validate
        /// </summary>
        /// <param name="record">Record cần custom validate</param>
        /// <param name="errorDetail">Lỗi chi tiết khi nhập</param>
        /// <param name="status">Trạng thái nhập khẩu</param>
        /// CreatedBy: Nguyễn Khắc Tiềm (5/10/2022)
        public override void CustomResultValidate(ref Product record, string? errorDetail, string? status)
        {
            record.ProductID = Guid.NewGuid();
            record.ErrorDetail = errorDetail;
            record.StatusImportExcel = status;
        }

        /// <summary>
        /// Hàm xử lý custom validate đối với nhập từ tệp
        /// </summary>
        /// <param name="listRecord">Danh sách từ tệp</param>
        /// <param name="record">Record cần custom validate</param>
        /// CreatedBy: Nguyễn Khắc Tiềm (5/10/2022)
        public override ServiceResponse CustomValidateImportXlsx(Product record, List<Product> listRecord)
        {
            var validateFailures = "";
            int count = listRecord.Count(e => e.ProductCode == record.ProductCode);
            if (count >= 2)
            {
                // Có ít nhất 2 bản ghi trong danh sách "DSProduct" có ProductCode trùng với ProductCode của đối tượng "Product"
                validateFailures = $"validate.unique_import MESSAGE.VALID.SPLIT ProductCode MESSAGE.VALID.SPLIT {record.ProductCode}";
            }
            if (!string.IsNullOrEmpty(validateFailures))
            {
                return new ServiceResponse
                {
                    Success = false,
                    Data = validateFailures,
                    ErrorCode = MisaAmisErrorCode.InvalidInput
                };
            }
            return new ServiceResponse
            {
                Success = true
            };
        }

        /// <summary>
        /// Hàm xử lý đưa những bản ghi không hợp lệ vào list Fail, xoá bản ghi không hợp lệ ở list pass sau khi nhận kết quả từ proc
        /// </summary>
        /// <param name="listFail">Danh sách bản ghi không hợp lệ</param>
        /// <param name="listPass">Danh sách bản ghi  hợp lệ</param>
        /// <param name="listFailResultProc">Danh sách bản ghi không hơp lệ trả về từ Proc</param>
        /// CreatedBy: Nguyễn Khắc Tiềm (5/10/2022)
        public override void CustomListFailResultImportXlsx(ref List<object> listFail, ref List<Product> listPass, List<Product> listFailResultProc)
        {
            foreach (var item in listFailResultProc)
            {
                item.ProductID = Guid.NewGuid();
                listFail.Add(item);
                listPass.RemoveAll(x => x.ProductCode == item.ProductCode);
            }
        }

        /// <summary>
        /// Hàm xử lý lấy những dữ liệu chuẩn đưa vào list để tiến hành nhập từ tệp
        /// </summary>
        /// <param name="json">Dữ liệu sẽ được chuẩn hoá</param>
        /// <param name="listFail">Danh sách dữ liệu không hợp lệ</param>
        /// <param name="list">Danh sách dữ liệu đúng kiểu</param>
        /// CreatedBy: Nguyễn Khắc Tiềm (5/10/2022)
        public override void CustomListTypeImportXlsx(string json, ref List<object> listFail, ref List<Product> list)
        {
            void setFail(ref ProductImport item, ref List<object> listFail, ref bool check, string propertyName)
            {
                item.ProductID = Guid.NewGuid().ToString();
                item.ErrorDetail = $"validate.malformed MESSAGE.VALID.SPLIT {propertyName}";
                item.StatusImportExcel = "common.illegal";
                listFail.Add(item);
                check = true;
            }
            List<ProductImport> listData = JsonConvert.DeserializeObject<List<ProductImport>>(json.ToString());
            int count = 1;
            foreach (var temp in listData)
            {
                var item = temp;
                count++;
                item.LineExcel = count;
                var properties = typeof(ProductImport).GetProperties();
                bool check = false;
                foreach (var property in properties)
                {
                    var propertyValue = property.GetValue(item)?.ToString();
                    var validateString = (ValidateString?)Attribute.GetCustomAttribute(property, typeof(ValidateString));
                    if (validateString != null)
                    {
                        if (validateString.IsDate && !string.IsNullOrEmpty(propertyValue))
                        {
                            if (!Validate<ProductImport>.IsDateFormatValid(propertyValue))
                            {
                                setFail(ref item, ref listFail, ref check, property.Name);
                                break;
                            }
                        }
                        if (validateString.IsBoolean && !string.IsNullOrEmpty(propertyValue))
                        {
                            if (!Validate<ProductImport>.IsBoolean(propertyValue))
                            {
                                setFail(ref item, ref listFail, ref check, property.Name);
                                break;
                            }
                        }
                        if (validateString.IsNumber && !string.IsNullOrEmpty(propertyValue))
                        {
                            if (!Validate<ProductImport>.IsNumeric(propertyValue))
                            {
                                setFail(ref item, ref listFail, ref check, property.Name);
                                break;
                            }
                        }
                    }
                }
                if (check)
                {
                    continue;
                }
                if (!string.IsNullOrEmpty(item.Gender))
                {
                    if (!Validate<ProductImport>.IsGender(item.Gender))
                    {
                        setFail(ref item, ref listFail, ref check, "Gender");
                        continue;
                    }
                }
                item.ProductID = null;
                item.DepotID = null;
                item.CategoryID = null;
                item.OriginID = null;
                item.TrademarkID = null;
                list.Add(JsonConvert.DeserializeObject<Product>(JsonConvert.SerializeObject(item).ToString()));
            }
        }

        /// <summary>
        /// Hàm custom dữ liệu xuất file
        /// </summary>
        /// <param name="property">Cột dữ liệu cần custom</param>
        /// <returns>Trả ra false khi không custom gì</returns>
        ///  NK Tiềm 05/10/2022
        public override bool CustomValuePropertieExport(PropertyInfo property, ref ExcelWorksheet sheet, int indexRow, int indexBody, Product product)
        {
            if (property.Name == "Gender")
            {
                sheet.Cells[indexRow + 4, indexBody].Value = product.Gender == Gender.Male ? Resource.GenderMale : product.Gender == Gender.Female ? Resource.GenderFemale : Resource.GenderOther;
                sheet.Cells[indexRow + 4, indexBody].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                return true;
            }
            return false;
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
                FileName = "Danh sách hàng hoá",
                Header = "DANH SÁCH HÀNG HOÁ"
            };
        }

        #endregion

        /// <summary>
        /// API lấy ra 10 sản phẩm mới nhất
        /// </summary>
        /// <param name="formData">Từ khoá tìm kiếm</param>
        /// <returns>Danh sách record và tổng số bản ghi</returns>
        /// Create by: Nguyễn Khắc Tiềm (26/09/2022)
        public object GetProductHome(Dictionary<string, object> formData)
        {
            var v_KeyWord = formData.Keys.Contains("v_KeyWord") ? Convert.ToString(formData["v_KeyWord"]) : "";
            return _productDL.GetProductHome(v_KeyWord);
        }

        /// <summary>
        /// API lấy ra 10 sản phẩm mới nhất
        /// </summary>
        /// <param name="formData">Từ khoá tìm kiếm</param>
        /// <returns>Danh sách record và tổng số bản ghi</returns>
        /// Create by: Nguyễn Khắc Tiềm (26/09/2022)
        public object GetProductHot()
        {
            return _productDL.GetProductHot();
        }

        /// <summary>
        /// API lấy ra 10 sản phẩm mới nhất
        /// </summary>
        /// <param name="formData">Từ khoá tìm kiếm</param>
        /// <returns>Danh sách record và tổng số bản ghi</returns>
        /// Create by: Nguyễn Khắc Tiềm (26/09/2022)
        public object GetProductPrice()
        {
            return _productDL.GetProductPrice();
        }

        /// <summary>
        /// Hàm lấy ra danh sách record có lọc và phân trang
        /// </summary>
        /// <param name="formData">Từ khoá tìm kiếm</param>
        /// <returns>Danh sách record và tổng số bản ghi</returns>
        /// Create by: Nguyễn Khắc Tiềm (26/09/2022)
        public Paging GetFitterShops(Dictionary<string, object> formData)
        {
            var v_PriceStart = double.Parse(formData["v_PriceStart"].ToString());
            var v_PriceEnd = double.Parse(formData["v_PriceEnd"].ToString());
            var v_Page = int.Parse(formData["v_Page"].ToString());
            var v_KeyWord = formData.Keys.Contains("v_KeyWord") ? Convert.ToString(formData["v_KeyWord"]).Trim() : "";
            var v_CategoryID = formData.Keys.Contains("v_CategoryID") ? Convert.ToString(formData["v_CategoryID"]).Trim() : "";
            var v_TrademarkID = formData.Keys.Contains("v_TrademarkID") ? Convert.ToString(formData["v_TrademarkID"]).Trim() : "";
            var v_OriginID = formData.Keys.Contains("v_OriginID") ? Convert.ToString(formData["v_OriginID"]).Trim() : "";
            var v_DepotID = formData.Keys.Contains("v_DepotID") ? Convert.ToString(formData["v_DepotID"]).Trim() : "";
            return _productDL.GetFitterShops(v_KeyWord, v_PriceStart, v_PriceEnd, v_CategoryID, v_TrademarkID, v_OriginID, v_DepotID, v_Page);
        }

        #endregion
    }
}
