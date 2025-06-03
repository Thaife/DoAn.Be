using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using MISA.WEB08.AMIS.Common.Attributes;
using MISA.WEB08.AMIS.Common.Resources;
using MISA.WEB08.AMIS.DL;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace MISA.WEB08.AMIS.BL
{
    public class SaveFileImage
    {
        /// <summary>
        /// Code để tạo filestream từ stream và ghi vào ổ đĩa
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="fileName"></param>
        /// CreatedBy: Nguyễn Khắc Tiềm (6/10/2022)
        public static string SaveExcelFileToDisk(Stream stream, string fileName)
        {
            // Đường dẫn đến thư mục trong source code backend
            var path = Path.Combine(Directory.GetCurrentDirectory(), DataContext.Path_root + "/Excel/Export/");

            // Nếu thư mục không tồn tại, tạo mới thư mục
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            // Tạo đường dẫn đầy đủ đến tập tin Excel
            var filePath = Path.Combine(path, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                stream.CopyTo(fileStream);
            }
            return "/Excel/Export/" + fileName;
        }

        /// <summary>
        /// Hàm lấy ra column name theo số cột muốn lấy
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        /// CreatedBy: Nguyễn Khắc Tiềm (6/10/2022)
        public static string GetColumnName(int index)
        {
            string columnName = "";
            while (index > 0)
            {
                int remainder = (index - 1) % 26;
                columnName = (char)(65 + remainder) + columnName;
                index = (index - 1) / 26;
            }
            return columnName;
        }

        /// <summary>
        /// Hàm xử lý đọc file excel
        /// </summary>
        /// <param name="path"></param>
        /// <param name="headerRow"></param>
        /// <param name="messageError"></param>
        /// <returns></returns>
        /// CreatedBy: Nguyễn Khắc Tiềm (6/10/2022)
        public static DataTable ReadFromExcelFile(string path, int headerRow, out string messageError)
        {
            DataTable result = new DataTable();
            try
            {
                if (string.IsNullOrEmpty(path) || !File.Exists(path))
                {
                    messageError = "FILE_NOT_EXISTS";
                    return null;
                }

                if (!string.IsNullOrEmpty(path) && Path.HasExtension(path) && Path.GetExtension(path).ToLower() != ".xlsx")
                {
                    messageError = "WRONG_FORMAT_FILE";
                    return null;
                }

                using (ExcelPackage package = new ExcelPackage(new FileInfo(path)))
                {
                    var workBook = package.Workbook;
                    if (workBook == null || workBook.Worksheets.Count == 0)
                    {
                        messageError = "EMPTY_DATA";
                        return null;
                    }
                    var workSheet = workBook.Worksheets.First();
                    // Đọc từng header column
                    int columnCount = 0;
                    foreach (var firstRowCell in workSheet.Cells[headerRow, 1, headerRow, workSheet.Dimension.End.Column])
                    {
                        string columnName = "" + firstRowCell.Text.Trim();
                        if (string.IsNullOrEmpty(columnName))
                            break;
                        columnCount++;
                        result.Columns.Add(firstRowCell.Text.Trim());
                    }
                    //Đọc dữ liệu từ từng header row
                    for (var rowNumber = headerRow + 1; rowNumber <= workSheet.Dimension.End.Row; rowNumber++)
                    {
                        var row = workSheet.Cells[rowNumber, 1, rowNumber, columnCount];
                        var newRow = result.NewRow();
                        bool isRowData = false;
                        string str = "";
                        //Đọc và check rỗng
                        foreach (var cell in row)
                        {
                            if (cell != null)
                                isRowData = true;

                            newRow[cell.Start.Column - 1] = cell.Value;
                            str += cell.Value != null ? cell.Value : "";
                        }
                        //Đưa dữ liệu vào datatable
                        if (isRowData && !string.IsNullOrEmpty(str.Trim()))
                            result.Rows.Add(newRow);
                        if (string.IsNullOrEmpty(str.Trim())) break;
                    }
                }
                messageError = "";
            }
            catch (Exception ex) { messageError = "ERROR:" + ex.Message; }

            return result;
        }

        /// <summary>
        /// Hàm thực hiện xoá file
        /// </summary>
        /// <param name="pathFileName">Đường dẫn file và tên file</param>
        /// CreatedBy: Nguyễn Khắc Tiềm (6/10/2022)
        public static void DeleteFile(string pathFileName)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), pathFileName);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        /// <summary>
        /// Hàm xử lý css file excel
        /// </summary>
        /// <param name="sheet">excel</param>
        /// <param name="column">tên cột excel cuối cùng</param>
        /// <param name="Header">tên cột đầu tiên</param>
        /// <param name="count">số lượng bản ghi xuất</param>
        /// <param name="properties">các properties khi xuất</param>
        /// <param name="listField">Các cột được yêu cầu xuất từ FontEnd</param>
        /// CreatedBy: Nguyễn Khắc Tiềm (6/10/2022)
        public static void SetUpExportHeaderData(ref ExcelWorksheet sheet, string column, string Header, int count, PropertyInfo[] properties, string listField)
        {
            // style header
            sheet.Cells[$"A1:{column}1"].Merge = true;
            sheet.Cells[$"A1:{column}1"].Value = Header;
            sheet.Cells[$"A1:{column}1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Cells[$"A1:{column}1"].Style.Font.Bold = true;
            sheet.Cells[$"A1:{column}1"].Style.Font.Size = 16;
            sheet.Cells[$"A1:{column}1"].Style.Font.Name = Resource.ExcelFontHeader;
            sheet.Cells[$"A2:{column}2"].Merge = true;
            sheet.Row(3).Style.Font.Name = Resource.ExcelFontHeader;
            sheet.Row(3).Style.Font.Bold = true;
            sheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Row(3).Style.Font.Size = 10;
            sheet.Cells[$"A3:{column}3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            sheet.Cells[$"A3:{column}3"].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(Resource.BackGroundColorHeaderExport));
            sheet.Cells[3, 1].Value = "STT";
            sheet.Column(1).Width = 10;
            sheet.Cells[$"A3:{column}{count + 3}"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            sheet.Cells[$"A3:{column}{count + 3}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            sheet.Cells[$"A3:{column}{count + 3}"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            sheet.Cells[$"A3:{column}{count + 3}"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            sheet.Cells[$"A3:{column}{count + 3}"].Style.Font.Name = Resource.ExcelFontContent;
            sheet.Cells[$"A3:{column}{count + 3}"].Style.Font.Size = 11;

            // customize tên header của file excel
            int indexHeader = 2;
            // Duyệt từng property
            foreach (var property in properties)
            {
                // lấy tên hiển thị đầu tiên của thuộc tính
                var displayNameAttributes = property.GetCustomAttributes(typeof(ColumnName), true);
                if (listField.Contains(property.Name) && displayNameAttributes != null && displayNameAttributes.Length > 0)
                {
                    // add vào header của file excel
                    sheet.Column(indexHeader).Width = (displayNameAttributes[0] as ColumnName).Width;
                    sheet.Cells[3, indexHeader].Value = (displayNameAttributes[0] as ColumnName).Name;
                    indexHeader++;
                }
            }
        }

        /// <summary>
        /// Hàm xử lý xuất excel cần custom dữ liệu đối với những trường đặc biệt hay gặp ví dụ: ngày tháng
        /// </summary>
        /// <param name="sheet">Excel</param>
        /// <param name="convertDate">Kiểu convert date</param>
        /// <param name="property">properties khi xuấ</param>
        /// <param name="indexRow">index row trong file excel</param>
        /// <param name="indexBody">index body trong file excel</param>
        /// <param name="displayNameAttributes">Tên cột arrt</param>
        /// <param name="recordItem">Bản ghi đang xuất</param>
        /// <returns>True: Có setup, false: Không setup</returns>
        /// CreatedBy: Nguyễn Khắc Tiềm (6/10/2022)
        public static bool SetUpDataExportDefault(ref ExcelWorksheet sheet, ref string convertDate, PropertyInfo property, int indexRow, int indexBody, object[] displayNameAttributes,object recordItem)
        {
            bool check = false;
            // xử lí các datetime
            if ((displayNameAttributes[0] as ColumnName).IsDate)
            {
                try
                {
                    sheet.Cells[indexRow + 4, indexBody].Value = property.GetValue(recordItem) != null ? DateTime.ParseExact(property.GetValue(recordItem).ToString(), convertDate, CultureInfo.InvariantCulture).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "";
                }
                catch
                {
                    if (convertDate == "M/d/yyyy hh:mm:ss tt")
                    {
                        convertDate = "d/M/yyyy hh:mm:ss tt";
                    }
                    else
                    {
                        convertDate = "M/d/yyyy hh:mm:ss tt";
                    }
                    sheet.Cells[indexRow + 4, indexBody].Value = property.GetValue(recordItem) != null ? DateTime.ParseExact(property.GetValue(recordItem).ToString(), convertDate, CultureInfo.InvariantCulture).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "";
                }
                sheet.Cells[indexRow + 4, indexBody].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                check = true;
            }
            // xử lí các kiểu boolen
            else if ((displayNameAttributes[0] as ColumnName).IsBollen)
            {
                sheet.Cells[indexRow + 4, indexBody].Value = (bool)property.GetValue(recordItem) == true ? "Có" : "Không";
                sheet.Cells[indexRow + 4, indexBody].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                check = true;
            }
            // Xử lý kiểu hoạt động
            else if (property.Name == "IsActive")
            {
                sheet.Cells[indexRow + 4, indexBody].Value = (bool)property.GetValue(recordItem) == true ? Resource.ActiveTrue : Resource.ActiveFalse;
                check = true;
            }
            return check;
        }

        /// <summary>
        /// Hàm tính ra số lượng cột trong excel dựa vào propties và listField cần xuất
        /// </summary>
        /// <param name="properties">các properties khi xuất</param>
        /// <param name="listField">Các cột được yêu cầu xuất từ FontEnd</param>
        /// <returns>Số lượng cột trong excel</returns>
        /// CreatedBy: Nguyễn Khắc Tiềm (6/10/2022)
        public static int CalcIndexCol(PropertyInfo[] properties, string listField)
        {
            int indexCol = 0;
            // Duyệt từng property để tính column
            foreach (var property in properties)
            {
                // lấy tên hiển thị đầu tiên của thuộc tính
                var displayNameAttributes = property.GetCustomAttributes(typeof(ColumnName), true);
                if (listField.Contains(property.Name) && displayNameAttributes != null && displayNameAttributes.Length > 0)
                {
                    // Nếu là cột cần xuất thì sẽ tăng index lên 1
                    indexCol += 1;
                }
            }
            return indexCol;
        }
    }
}
