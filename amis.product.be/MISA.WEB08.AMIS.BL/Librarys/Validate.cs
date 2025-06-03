using MISA.WEB08.AMIS.Common.Attributes;
using MISA.WEB08.AMIS.Common.Enums;
using MISA.WEB08.AMIS.Common.Result;
using System;
using System.Globalization;

namespace MISA.WEB08.AMIS.BL
{
    /// <summary>
    /// Validate kiểm tra dữ liệu các trường bắt buộc
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Validate<T>
    {
        #region Method

        /// <summary>
        /// Validate dữ liệu truyền lên từ tầng controller
        /// </summary>
        /// <param name="record">Đối tượng cần validate</param>
        /// <returns></returns>
        /// NK Tiềm 05/10/2022
        public static ServiceResponse ValidateData(T record)
        {
            // Validate dữ liệu đầu vào 
            var properties = typeof(T).GetProperties();
            var validateFailures = "";
            foreach (var property in properties)
            {
                // Kiểm tra không được rỗng
                var propertyValue = property.GetValue(record)?.ToString();
                var validateAttribute = (ValidateAttribute?)Attribute.GetCustomAttribute(property, typeof(ValidateAttribute));
                if (validateAttribute != null)
                {
                    if (validateAttribute.IsNotNullOrEmpty && string.IsNullOrEmpty(propertyValue))
                    {
                        validateFailures = $"{validateAttribute.ErrorMessage} MESSAGE.VALID.SPLIT {property.Name}";
                        break;
                    }
                    // Kiểm tra độ dài
                    else if (validateAttribute.MaxLength > 0 && !string.IsNullOrEmpty(propertyValue))
                    {
                        if(propertyValue.Length > validateAttribute.MaxLength)
                        {
                            validateFailures = $"validate.max_length MESSAGE.VALID.SPLIT {property.Name} MESSAGE.VALID.SPLIT {validateAttribute.MaxLength}";
                            break;
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(validateFailures))
            {
                return new ServiceResponse
                {
                    Success = false,
                    ErrorCode = MisaAmisErrorCode.InvalidInput,
                    Data = validateFailures
                };
            }
            return new ServiceResponse
            {
                Success = true
            };
        }

        /// <summary>
        /// Hàm xử lý fomat số đúng định dạng
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        /// NK Tiềm 05/10/2022
        public static string FormatNumber(double number)
        {
            return String.Format("{0:0,0.0}", number);
        }

        /// <summary>
        /// Hàm xử lý build truy vấn
        /// </summary>
        /// <param name="key">Column trong data base cần so sánh</param>
        /// <param name="value">Giá trị cần so sánh</param>
        /// <param name="typeSearch">Kiểu so sánh là chữ hay dạng số</param>
        /// <param name="comparisonType">toán tử so sánh</param>
        /// <returns>truy vấn sau khi build</returns>
        ///  NK Tiềm 05/10/2022
        public static string FormatQuery(string key, string value, string typeSearch, string comparisonType)
        {
            string v_Query = "";
            if ((comparisonType == "=" || comparisonType == ">" || comparisonType == ">=" || comparisonType == "<" || comparisonType == "<=") && (typeSearch == "number" || typeSearch == "date"))
            {
                if (typeSearch == "number")
                {
                    v_Query += $" AND {key} {comparisonType} {value}";
                }
                else
                {
                    v_Query += $" AND {key} {comparisonType} STR_TO_DATE('{DateTime.Parse(value).ToString("dd/MM/yyyy")}', '%d/%m/%Y')";
                }
            }
            else if ((comparisonType == "=Null" || comparisonType == "!=Null" || comparisonType == "!=") && (typeSearch == "number" || typeSearch == "date"))
            {
                switch (comparisonType)
                {
                    case "=Null":
                        if (typeSearch == "number")
                        {
                            v_Query += $" AND ({key} IS NULL OR {key} = '' AND {key} != 0)";
                        }
                        else
                        {
                            v_Query += $" AND {key} IS NULL";
                        }
                        break;
                    case "!=Null":
                        if (typeSearch == "number")
                        {
                            v_Query += $" AND ({key} != NULL OR {key} != '' OR {key} = 0)";
                        }
                        else
                        {
                            v_Query += $" AND {key} IS NOT NULL";
                        }
                        break;
                    case "!=":
                        if (typeSearch == "number")
                        {
                            v_Query += $" AND ({key} != {value} OR {key} = '' OR {key} IS NULL)";
                        }
                        else
                        {
                            v_Query += $" AND {key} != STR_TO_DATE('{DateTime.Parse(value).ToString("dd/MM/yyyy")}', '%d/%m/%Y')";
                        }
                        break;
                }
            }
            else
            {
                switch (comparisonType)
                {
                    //Chứa
                    case "%%":
                        v_Query += $" AND {key} LIKE '%{value}%'";
                        break;
                    //Rỗng
                    case "=Null":
                        v_Query += $" AND ({key} IS NULL OR {key} = '')";
                        break;
                    //Không rỗng
                    case "!=Null":
                        v_Query += $" AND ({key} != NULL OR {key} != '')";
                        break;
                    //Bằng
                    case "=":
                        v_Query += $" AND {key} = '{value}'";
                        break;
                    //Khác
                    case "!=":
                        v_Query += $" AND ({key} != '{value}' OR {key} = '' OR {key} IS NULL)";
                        break;
                    //Không chứa
                    case "!%%":
                        v_Query += $" AND ({key} NOT LIKE '%{value}%' OR {key} = '' OR {key} IS NULL)";
                        break;
                    //Bắt đầu bởi
                    case "_%":
                        v_Query += $" AND {key} LIKE '{value}%'";
                        break;
                    //Kết thúc bởi
                    case "%_":
                        v_Query += $" AND {key} LIKE '%{value}'";
                        break;
                }
            }
            return v_Query;
        }

        /// <summary>
        /// Hàm kiểm tra chuỗi có đúng định dạng ngày tháng
        /// </summary>
        /// <param name="dateString">Dữ liệu cần kiểm tra</param>
        /// <param name="format">Kiểu kiểm tra, mặc định là yyyy-MM-dd</param>
        /// <returns>bool</returns>
        ///  NK Tiềm 05/10/2022
        public static bool IsDateFormatValid(string dateString, string format = "yyyy-MM-dd")
        {
            DateTime result;
            return DateTime.TryParseExact(dateString, format, CultureInfo.InvariantCulture, DateTimeStyles.None | DateTimeStyles.AllowTrailingWhite, out result);
        }

        /// <summary>
        /// Hàm kiểm tra chuỗi có phải kiểu boolen
        /// </summary>
        /// <param name="input">Chuỗi cần kiểm tra</param>
        /// <returns>bool</returns>
        ///  NK Tiềm 05/10/2022
        public static bool IsBoolean(string input)
        {
            bool result;
            return bool.TryParse(input, out result);
        }

        /// <summary>
        /// Hàm kiểm tra chuỗi có phải kiểu số
        /// </summary>
        /// <param name="input">Chuỗi cần kiểm tra</param>
        /// <returns>bool</returns>
        ///  NK Tiềm 05/10/2022
        public static bool IsNumeric(string input)
        {
            double result;
            return double.TryParse(input, out result);
        }

        /// <summary>
        /// Hàm kiểm tra chuỗi có phải kiểu Gender
        /// </summary>
        /// <param name="input">Chuỗi cần kiểm tra</param>
        /// <returns>bool</returns>
        ///  NK Tiềm 05/10/2022
        public static bool IsGender(string input)
        {
            if (IsNumeric(input))
            {
                return Enum.IsDefined(typeof(Gender), int.Parse(input));
            }
            return false;
        }

        #endregion
    }
}
