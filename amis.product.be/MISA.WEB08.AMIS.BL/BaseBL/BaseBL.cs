using Microsoft.AspNetCore.Http;
using MISA.WEB08.AMIS.Common.Attributes;
using MISA.WEB08.AMIS.Common.Entities;
using MISA.WEB08.AMIS.Common.Enums;
using MISA.WEB08.AMIS.Common.Resources;
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
    /// Dữ liệu thao tác với Database và trả về với bảng trong Database từ tầng BL
    /// </summary>
    /// Create by: Nguyễn Khắc Tiềm (21/09/2022)
    public class BaseBL<T> : CustomVirtual<T>, IBaseBL<T>
    {
        #region Field

        private IBaseDL<T> _baseDL;

        #endregion

        #region Contructor

        public BaseBL(IBaseDL<T> baseDL)
        {
            _baseDL = baseDL;
        }

        #endregion

        #region Method

        /// <summary>
        /// Hàm Lấy danh sách tất cả bản ghi của 1 bảng
        /// </summary>
        /// <returns>Danh sách tất cả bản ghi</returns>
        /// Create by: Nguyễn Khắc Tiềm (26/09/2022)
        public virtual object GetAllRecords()
        {
            return _baseDL.GetAllRecords();
        }

        /// <summary>
        /// Hàm Lấy danh sách dropdown
        /// </summary>
        /// <returns>Danh sách tất cả bản ghi</returns>
        /// Create by: Nguyễn Khắc Tiềm (26/09/2022)
        public object GetDropdown()
        {
            return _baseDL.GetDropdown();
        }

        /// <summary>
        /// Hàm lấy ra bản ghi theo ID
        /// </summary>
        /// <param name="recordID">ID bản ghi</param>
        /// <param name="stateForm">Trạng thái lấy (sửa hay nhân bản, ...)</param>
        /// <returns>Thông tin chi tiết một bản ghi</returns>
        /// Create by: Nguyễn Khắc Tiềm (26/09/2022)
        public virtual object GetRecordByID(string recordID, string? stateForm)
        {
            return _baseDL.GetRecordByID(recordID, stateForm);
        }

        /// <summary>
        /// Hàm lấy ra mã record tự sinh
        /// </summary>
        /// <returns>Mã bản ghi</returns>
        /// Create by: Nguyễn Khắc Tiềm (26/09/2022)
        public virtual object GetRecordCodeNew()
        {
            return _baseDL.GetRecordCodeNew();
        }

        /// <summary>
        /// Hàm lấy ra danh sách record có lọc và phân trang
        /// </summary>
        /// <param name="formData">Từ khoá tìm kiếm</param>
        /// <returns>Danh sách record và tổng số bản ghi</returns>
        /// Create by: Nguyễn Khắc Tiềm (26/09/2022)
        public virtual Paging GetFitterRecords(Dictionary<string, object> formData)
        {
            var v_Offset = int.Parse(formData["v_Offset"].ToString());
            var v_Limit = int.Parse(formData["v_Limit"].ToString());
            var v_Where = formData.Keys.Contains("v_Where") ? Convert.ToString(formData["v_Where"]).Trim() : null;
            var v_Sort = formData.Keys.Contains("v_Sort") ? Convert.ToString(formData["v_Sort"]) : null;
            var v_Select = formData.Keys.Contains("v_Select") ? JsonConvert.DeserializeObject<List<string>>(Convert.ToString(formData["v_Select"])) : new List<string>();

            List<ComparisonTypeSearch> listQuery = formData.Keys.Contains("CustomSearch") ? JsonConvert.DeserializeObject <List<ComparisonTypeSearch>>(Convert.ToString(formData["CustomSearch"])) : new List<ComparisonTypeSearch>();
            string v_Query = "";
            foreach (ComparisonTypeSearch item in listQuery)
            {
                if (!string.IsNullOrEmpty(item.ValueSearch) || item.ComparisonType == "!=Null" || item.ComparisonType == "=Null")
                {
                    v_Query += Validate<T>.FormatQuery(item.ColumnSearch, item.ValueSearch.Trim(), item.TypeSearch, item.ComparisonType);
                }
            }
            return _baseDL.GetFitterRecords(v_Offset, v_Limit, v_Where, v_Sort, v_Query, string.Join(", ", v_Select));
        }

        /// <summary>
        /// Hàm thêm mới một bản ghi
        /// </summary>
        /// <param name="record"></param>
        /// <returns>ID bản ghi sau khi thêm</returns>
        /// Create by: Nguyễn Khắc Tiềm (26/09/2022)
        public virtual ServiceResponse InsertRecord(T record, string userID)
        {
            var validateResult = Validate<T>.ValidateData(record);
            if (!validateResult.Success)
            {
                return validateResult;
            }
            var validateCustom = CustomValidate(record);
            if (!validateCustom.Success)
            {
                return validateCustom;
            }
            var result = _baseDL.InsertRecord(record, userID);
            if (!result.Success)
            {
                return new ServiceResponse
                {
                    Success = false,
                    ErrorCode = MisaAmisErrorCode.InsertFailed,
                    Data = result.Data == Guid.Empty.ToString() ? Resource.Message_data_change : result.Data,
                };
            }
            return new ServiceResponse
            {
                Success = true,
                Data = result.Data,
            };
        }

        /// <summary>
        /// Hàm cập nhật thông tin một bản ghi theo ID
        /// </summary>
        /// <param name="recordID"></param>
        /// <param name="record"></param>
        /// <returns>ID record sau khi cập nhật</returns>
        /// Create by: Nguyễn Khắc Tiềm (26/09/2022)
        public virtual ServiceResponse UpdateRecord(Guid recordID, T record, string userID)
        {
            var validateResult = Validate<T>.ValidateData(record);
            if (!validateResult.Success)
            {
                return validateResult;
            }
            var validateCustom = CustomValidate(record);
            if (!validateCustom.Success)
            {
                return validateCustom;
            }
            var result = _baseDL.UpdateRecord(recordID, record, userID);
            if (!result.Success)
            {
                return new ServiceResponse
                {
                    Success = false,
                    ErrorCode = MisaAmisErrorCode.UpdateFailed,
                    Data = result.Data == Guid.Empty.ToString() ? Resource.Message_data_change : result.Data,
                };
            }
            return new ServiceResponse
            {
                Success = true,
                Data = result.Data,
            };
        }

        /// <summary>
        /// Xoá một bản ghi theo ID
        /// </summary>
        /// <param name="recordID"></param>
        /// <returns>ID record sau khi xoá</returns>
        /// Create by: Nguyễn Khắc Tiềm (26/09/2022)
        public virtual ServiceResponse DeleteRecord(Guid recordID)
        {
            var result = _baseDL.DeleteRecord(recordID);
            if (!result.Success)
            {
                return new ServiceResponse
                {
                    Success = false,
                    ErrorCode = MisaAmisErrorCode.DeleteFailed,
                    Data = result.Data == Guid.Empty.ToString() ? Resource.Message_data_change : result.Data,
                };
            }
            return new ServiceResponse
            {
                Success = true,
                Data = result.Data,
            };
        }

        /// <summary>
        /// xóa nhiều bản ghi
        /// </summary>
        /// <param name="listRecordID">danh sách bản ghi cần xoá</param>
        /// <param name="count">Số lượng bản ghi bị xoá</param>
        /// <returns>Số kết quả bản ghi đã xoá</returns>
        /// CreatedBy: Nguyễn Khắc Tiềm (5/10/2022)
        public virtual ServiceResponse DeleteMultiple(string listRecordID, int count)
        {
            return _baseDL.DeleteMultiple(listRecordID, count);
        }

        /// <summary>
        /// Hàm cập nhật toggle active bản ghi
        /// </summary>
        /// <param name="recordID"></param>
        /// <returns>ID record sau khi cập nhật</returns>
        /// Create by: Nguyễn Khắc Tiềm (26/09/2022)
        public virtual ServiceResponse ToggleActive(Guid recordID, string userID)
        {
            Guid result = _baseDL.ToggleActive(recordID, userID).Data;
            if (result != Guid.Empty)
            {
                return new ServiceResponse
                {
                    Success = true,
                    Data = result,
                };
            }
            return new ServiceResponse
            {
                Success = false,
                ErrorCode = MisaAmisErrorCode.UpdateFailed,
                Data = Resource.Message_data_change,
            };
        }

        /// <summary>
        /// Hhập khẩu dữ liệu từ tệp
        /// </summary>
        /// <param name="file">File execl</param>
        /// <returns></returns>
        /// Create by: Nguyễn Khắc Tiềm (26/09/2022)
        public virtual ServiceResponse ImportXLSX(IFormFile file, string userID)
        {
            var messageError = "";
            if (file.Length > 0 && file.FileName.Contains(".xlsx"))
            {
                ServiceResponse result;
                //Tạo tên file và lưu file
                var filename = Guid.NewGuid().ToString().Replace("-", "") + ".xlsx";
                var filePath = DataContext.Path_root + SaveFileImage.SaveExcelFileToDisk(file.OpenReadStream(), filename);
                // Đọc file từ excel
                var data = SaveFileImage.ReadFromExcelFile(filePath, 1, out messageError);
                SaveFileImage.DeleteFile(filePath);
                string json = JsonConvert.SerializeObject(data, Formatting.Indented);
                List<T> list = new List<T>();
                // Danh sách dữ liệu sai
                List<object> listFail = new List<object>();
                // Danh sách dữ liệu đúng
                List<T> listPass = new List<T>();
                // Chuyển dữ liệu đọc được thành kiểu list<T>
                try
                {
                    CustomListTypeImportXlsx(json,ref listFail,ref list);
                }
                catch(Exception ex)
                {
                    messageError = ex.Message;
                }
                if (list != null && list.Count > 0 && string.IsNullOrEmpty(messageError))
                {
                    // Tiến hành validate
                    foreach (var temp in list)
                    {
                        var item = temp;
                        CustomParameterValidate(ref item);
                        var validateResult = Validate<T>.ValidateData(item);
                        if (!validateResult.Success)
                        {
                            CustomResultValidate(ref item, validateResult.Data, "common.illegal");
                            listFail.Add(item);
                            continue;
                        }
                        var validateCustom = CustomValidate(item);
                        if (!validateCustom.Success)
                        {
                            CustomResultValidate(ref item, validateCustom.Data, "common.illegal");
                            listFail.Add(item);
                            continue;
                        }
                        var validatCustomValidateImportXlsx = CustomValidateImportXlsx(item, list);
                        if (!validatCustomValidateImportXlsx.Success)
                        {
                            CustomResultValidate(ref item, validatCustomValidateImportXlsx.Data, "common.illegal");
                            listFail.Add(item);
                            continue;
                        }
                        CustomResultValidate(ref item, null, "common.valid");
                        listPass.Add(item);
                    }
                    if(listPass.Count > 0)
                    {
                        // Nhập từ tệp những bản ghi hợp lệ
                        result = _baseDL.ImportXLSX(listPass, userID);
                        if (!result.Success)
                        {
                            return result;
                        }
                        var listFailResultProc = (List<T>)result.Data;
                        if (listFailResultProc.Count > 0)
                        {
                            CustomListFailResultImportXlsx(ref listFail, ref listPass, listFailResultProc);
                        }
                    }
                    return new ServiceResponse
                    {
                        Success = true,
                        Data = new
                        {
                            listFail = listFail,
                            listPass = listPass
                        }
                    };
                }
            }
            return new ServiceResponse
            {
                Success = false,
                Data = !string.IsNullOrEmpty(messageError) ? messageError : Resource.DevMsg_Exception,
            };
        }

        /// <summary>
        /// Export data ra file excel
        /// </summary>
        /// <param name="formData">Trường muốn filter và sắp xếp</param>
        /// <returns>file Excel chứa dữ liệu danh sách </returns>
        /// CreatedBy: Nguyễn Khắc Tiềm (6/10/2022)
        public virtual string ExportData(Dictionary<string, object> formData)
        {
            var v_Select = formData.Keys.Contains("v_Select") ? JsonConvert.DeserializeObject<List<string>>(Convert.ToString(formData["v_Select"])) : new List<string>();
            List<T> records = (List<T>)GetFitterRecords(formData).RecordList;
            if (records == null || records.Count == 0 || v_Select.Count == 0) return null;
            // lấy các thuộc tính của nhân viên
            string listField = string.Join(",", v_Select);
            var properties = typeof(T).GetProperties();
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
                            if(!SaveFileImage.SetUpDataExportDefault(ref sheet, ref convertDate, property, indexRow, indexBody, displayNameAttributes, recordItem))
                            {
                                if (!CustomValuePropertieExport(property, ref sheet, indexRow, indexBody, recordItem))
                                {
                                    if ((displayNameAttributes[0] as ColumnName).IsNumber && !string.IsNullOrEmpty(Convert.ToString(property.GetValue(recordItem))))
                                    {
                                        sheet.Cells[indexRow + 4, indexBody].Value = Validate<T>.FormatNumber(double.Parse(property.GetValue(recordItem) + ""));
                                        sheet.Cells[indexRow + 4, indexBody].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                    }
                                    else
                                    {
                                        sheet.Cells[indexRow + 4, indexBody].Value = property.GetValue(recordItem);
                                    }
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

        #endregion
    }
}
