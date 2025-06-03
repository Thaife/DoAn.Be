using Dapper;
using MISA.WEB08.AMIS.Common.Entities;
using MISA.WEB08.AMIS.Common.Result;
using MySqlConnector;
using System.Data;
using System.Linq;

namespace MISA.WEB08.AMIS.DL
{
    /// <summary>
    /// Dữ liệu thao tác với Database và trả về với bảng Product từ tầng DL
    /// </summary>
    /// Create by: Nguyễn Khắc Tiềm (21/09/2022)
    public class ProductDL : BaseDL<Product>, IProductDL
    {
        #region Field

        private IDatabaseHelper<Product> _dbHelper;

        #endregion

        #region Contructor

        public ProductDL(IDatabaseHelper<Product> dbHelper) : base(dbHelper)
        {
            _dbHelper = dbHelper;
        }

        #endregion

        #region Method

        /// <summary>
        /// Hàm xử lý custom các tham số parameter truyền vào proc create ngoài những tham số mặc định
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="record"></param>
        /// create by: nguyễn khắc tiềm (21/10/2022)
        public override void CustomParameterForCreate(ref DynamicParameters? parameters, Product record)
        {
            string prefix = "";
            string number = "";
            string last = "";
            _dbHelper.SaveCode(record.ProductCode, ref prefix, ref number, ref last);
            parameters.Add($"v_prefix", prefix);
            parameters.Add($"v_number", number);
            parameters.Add($"v_last", last);
            parameters.Add($"v_lengthNumber", number.Length);
        }

        /// <summary>
        /// API lấy ra 10 sản phẩm mới nhất
        /// </summary>
        /// <param name="formData">Từ khoá tìm kiếm</param>
        /// <returns>Danh sách record và tổng số bản ghi</returns>
        /// Create by: Nguyễn Khắc Tiềm (26/09/2022)
        public object GetProductHome(string keyWord)
        {
            // Khởi tạo các parameter để chèn vào trong Proc
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add($"v_KeyWord", keyWord);
            // Khai báo stored procedure
            string storeProcedureName = "Proc_product_GetProductHome";
            return _dbHelper.RunProcWithQuery(storeProcedureName, parameters);
        }

        /// <summary>
        /// API lấy ra 10 sản phẩm mới nhất
        /// </summary>
        /// <param name="formData">Từ khoá tìm kiếm</param>
        /// <returns>Danh sách record và tổng số bản ghi</returns>
        /// Create by: Nguyễn Khắc Tiềm (26/09/2022)
        public object GetProductHot()
        {
            string storeProcedureName = "Proc_Product_GetProductHot";
            return _dbHelper.RunProcWithQuery(storeProcedureName, null);
        }

        /// <summary>
        /// API lấy ra 10 sản phẩm mới nhất
        /// </summary>
        /// <param name="formData">Từ khoá tìm kiếm</param>
        /// <returns>Danh sách record và tổng số bản ghi</returns>
        /// Create by: Nguyễn Khắc Tiềm (26/09/2022)
        public object GetProductPrice()
        {
            string storeProcedureName = "Proc_Product_GetProductPrice";
            return _dbHelper.RunProcWithQuery(storeProcedureName, null);
        }

        /// <summary>
        /// Hàm lấy ra danh sách record có lọc và phân trang
        /// </summary>
        /// <returns>Danh sách record và tổng số bản ghi</returns>
        /// Create by: Nguyễn Khắc Tiềm (26/09/2022)
        public Paging GetFitterShops(string v_KeyWord, double v_PriceStart, double v_PriceEnd, string v_CategoryID, string v_TrademarkID, string v_OriginID, string v_DepotID, int v_Page)
        {
            Paging result;
            // Khởi tạo các parameter để chèn vào trong Proc
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("v_KeyWord", v_KeyWord);
            parameters.Add("v_PriceStart", v_PriceStart);
            parameters.Add("v_PriceEnd", v_PriceEnd);
            parameters.Add("v_CategoryID", v_CategoryID);
            parameters.Add("v_TrademarkID", v_TrademarkID);
            parameters.Add("v_OriginID", v_OriginID);
            parameters.Add("v_DepotID", v_DepotID);
            parameters.Add("v_Page", v_Page);

            // chuẩn bị câu lệnh MySQL
            string storeProcedureName = "Proc_product_GetProductShop";

            using (var mysqlConnection = new MySqlConnection(DataContext.MySqlConnectionString))
            {
                //nếu như kết nối đang đóng thì tiến hành mở lại
                if (mysqlConnection.State != ConnectionState.Open)
                {
                    mysqlConnection.Open();
                }
                // thực hiện gọi vào DB
                var records = mysqlConnection.QueryMultiple(
                    storeProcedureName,
                    parameters,
                    commandType: CommandType.StoredProcedure
                    );
                result = new Paging
                {
                    RecordList = records.Read<Product>().ToList(),
                    TotalCount = records.ReadSingle().totalCount
                };
                if (mysqlConnection.State == ConnectionState.Open)
                {
                    mysqlConnection.Close();
                }
            }
            return result;
        }

        #endregion
    }
}
