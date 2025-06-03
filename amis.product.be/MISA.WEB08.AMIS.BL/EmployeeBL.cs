using MISA.WEB08.AMIS.Common.Entities;
using MISA.WEB08.AMIS.DL;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using MISA.WEB08.AMIS.Common.Resources;
using MISA.WEB08.AMIS.Common.Enums;
using MISA.WEB08.AMIS.Common.Result;
using System.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using MISA.WEB08.AMIS.Common.Attributes;
using System.Text.RegularExpressions;

namespace MISA.WEB08.AMIS.BL
{
    /// <summary>
    /// Dữ liệu thao tác với Database và trả về với bảng employee từ tầng BL
    /// </summary>
    /// Create by: Nguyễn Khắc Tiềm (21/09/2022)
    public class EmployeeBL : BaseBL<Employee>, IEmployeeBL
    {
        #region Field

        private IEmployeeDL _employeeDL;

        #endregion

        #region Contructor

        public EmployeeBL(IEmployeeDL employeeDL) : base(employeeDL)
        {
            _employeeDL = employeeDL;
        }

        #endregion

        #region Method

        /// <summary>
        /// Hàm xử lý custom override validate model employee
        /// </summary>
        /// <param name="employee">Record cần custom validate</param>
        /// CreatedBy: Nguyễn Khắc Tiềm (5/10/2022)
        public override ServiceResponse? CustomValidate(Employee employee)
        {
            var validateFailures = "";
            // Kiểm tra phải đúng định dạng số điện thoại
            if (!string.IsNullOrEmpty(employee.PhoneNumber))
            {
                if (!Regex.IsMatch(employee.PhoneNumber, @"(03|02|05|07|08|09|01[2|6|8|9])+([0-9]{8})\b"))
                {
                    validateFailures = $"validate.malformed MESSAGE.VALID.SPLIT PhoneNumber";
                }
            }
            else if (!string.IsNullOrEmpty(employee.LandlinePhone))
            {
                if (!Regex.IsMatch(employee.LandlinePhone, @"(03|02|05|07|08|09|01[2|6|8|9])+([0-9]{8})\b"))
                {
                    validateFailures = $"validate.malformed MESSAGE.VALID.SPLIT LandlinePhone";
                }
            }
            // kiểm tra phải đúng định dạng email
            else if (!string.IsNullOrEmpty(employee.EmployeeEmail))
            {
                if (!Regex.IsMatch(employee.EmployeeEmail, @"^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$"))
                {
                    validateFailures = $"validate.malformed MESSAGE.VALID.SPLIT EmployeeEmail";
                }
            }
            // Kiểm tra ngày không được lớn hơn ngày hiện tại
            else if(!string.IsNullOrEmpty(employee.DayForIdentity.ToString()))
            {
                if (employee.DayForIdentity > DateTime.Now)
                {
                    validateFailures = $"validate.max_date_now MESSAGE.VALID.SPLIT DayForIdentity";
                }
            }
            else if (!string.IsNullOrEmpty(employee.DateOfBirth.ToString()))
            {
                if (employee.DateOfBirth > DateTime.Now)
                {
                    validateFailures = $"validate.max_date_now MESSAGE.VALID.SPLIT DateOfBirth";
                }
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
            else
            {
                return new ServiceResponse
                {
                    Success = true
                };
            }
        }

        /// <summary>
        /// Hàm xử lý custom tham số cho bản ghi cần validate
        /// </summary>
        /// <param name="record">Record cần custom validate</param>
        /// CreatedBy: Nguyễn Khắc Tiềm (5/10/2022)
        public override void CustomParameterValidate(ref Employee record)
        {
            record.BranchID = Guid.NewGuid();
        }

        /// <summary>
        /// Hàm xử lý custom kết quả validate cho bản ghi cần validate
        /// </summary>
        /// <param name="record">Record cần custom validate</param>
        /// <param name="errorDetail">Lỗi chi tiết khi nhập</param>
        /// <param name="status">Trạng thái nhập khẩu</param>
        /// CreatedBy: Nguyễn Khắc Tiềm (5/10/2022)
        public override void CustomResultValidate(ref Employee record, string? errorDetail, string? status)
        {
            record.EmployeeID = Guid.NewGuid();
            record.ErrorDetail = errorDetail;
            record.StatusImportExcel = status;
        }

        /// <summary>
        /// Hàm xử lý custom validate đối với nhập từ tệp
        /// </summary>
        /// <param name="listRecord">Danh sách từ tệp</param>
        /// <param name="record">Record cần custom validate</param>
        /// CreatedBy: Nguyễn Khắc Tiềm (5/10/2022)
        public override ServiceResponse CustomValidateImportXlsx(Employee record, List<Employee> listRecord)
        {
            var validateFailures = "";
            int count = listRecord.Count(e => e.EmployeeCode == record.EmployeeCode);
            if (count >= 2)
            {
                // Có ít nhất 2 bản ghi trong danh sách "DSEmployee" có EmployeeCode trùng với EmployeeCode của đối tượng "employee"
                validateFailures = $"validate.unique_import MESSAGE.VALID.SPLIT EmployeeCode MESSAGE.VALID.SPLIT {record.EmployeeCode}";
            }
            else if (string.IsNullOrEmpty(record.BranchCode))
            {
                validateFailures = $"validate.empty MESSAGE.VALID.SPLIT BranchCode";
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
        public override void CustomListFailResultImportXlsx(ref List<object> listFail, ref List<Employee> listPass, List<Employee> listFailResultProc)
        {
            foreach(var item in listFailResultProc)
            {
                item.EmployeeID = Guid.NewGuid();
                listFail.Add(item);
                listPass.RemoveAll(x => x.EmployeeCode == item.EmployeeCode);
            }
        }

        /// <summary>
        /// Hàm xử lý lấy những dữ liệu chuẩn đưa vào list để tiến hành nhập từ tệp
        /// </summary>
        /// <param name="json">Dữ liệu sẽ được chuẩn hoá</param>
        /// <param name="listFail">Danh sách dữ liệu không hợp lệ</param>
        /// <param name="list">Danh sách dữ liệu đúng kiểu</param>
        /// CreatedBy: Nguyễn Khắc Tiềm (5/10/2022)
        public override void CustomListTypeImportXlsx(string json, ref List<object> listFail, ref List<Employee> list)
        {
            void setFail(ref EmployeeImport item, ref List<object> listFail, ref bool check, string propertyName)
            {
                item.EmployeeID = Guid.NewGuid().ToString();
                item.ErrorDetail = $"validate.malformed MESSAGE.VALID.SPLIT {propertyName}";
                item.StatusImportExcel = "common.illegal";
                listFail.Add(item);
                check = true;
            }
            List<EmployeeImport> listData = JsonConvert.DeserializeObject<List<EmployeeImport>>(json.ToString());
            int count = 1;
            foreach (var temp in listData)
            {
                var item = temp;
                count++;
                item.LineExcel = count;
                var properties = typeof(EmployeeImport).GetProperties();
                bool check = false;
                foreach (var property in properties)
                {
                    var propertyValue = property.GetValue(item)?.ToString();
                    var validateString = (ValidateString?)Attribute.GetCustomAttribute(property, typeof(ValidateString));
                    if (validateString != null)
                    {
                        if (validateString.IsDate && !string.IsNullOrEmpty(propertyValue))
                        {
                            if (!Validate<EmployeeImport>.IsDateFormatValid(propertyValue))
                            {
                                setFail(ref item,ref listFail, ref check, property.Name);
                                break;
                            }
                        }
                        if (validateString.IsBoolean && !string.IsNullOrEmpty(propertyValue))
                        {
                            if (!Validate<EmployeeImport>.IsBoolean(propertyValue))
                            {
                                setFail(ref item, ref listFail, ref check, property.Name);
                                break;
                            }
                        }
                        if (validateString.IsNumber && !string.IsNullOrEmpty(propertyValue))
                        {
                            if (!Validate<EmployeeImport>.IsNumeric(propertyValue))
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
                    if (!Validate<EmployeeImport>.IsGender(item.Gender))
                    {
                        setFail(ref item, ref listFail, ref check, "Gender");
                        continue;
                    }
                }
                item.EmployeeID = null;
                item.BranchID = null;
                list.Add(JsonConvert.DeserializeObject<Employee>(JsonConvert.SerializeObject(item).ToString()));
            }
        }

        /// <summary>
        /// Hàm custom dữ liệu xuất file
        /// </summary>
        /// <param name="property">Cột dữ liệu cần custom</param>
        /// <returns>Trả ra false khi không custom gì</returns>
        ///  NK Tiềm 05/10/2022
        public override bool CustomValuePropertieExport(PropertyInfo property, ref ExcelWorksheet sheet, int indexRow, int indexBody, Employee employee)
        {
            if(property.Name == "Gender")
            {
                sheet.Cells[indexRow + 4, indexBody].Value = employee.Gender == Gender.Male ? Resource.GenderMale : employee.Gender == Gender.Female ? Resource.GenderFemale : Resource.GenderOther;
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
                FileName = "Danh sách nhân viên",
                Header = "DANH SÁCH NHÂN VIÊN"
            };
        }

        #endregion
    }
}
