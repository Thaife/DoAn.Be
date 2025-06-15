using Dapper;
using MISA.WEB08.AMIS.Common.Attributes;
using MISA.WEB08.AMIS.Common.Entities;
using MISA.WEB08.AMIS.Common.Resources;
using MISA.WEB08.AMIS.Common.Result;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;

namespace MISA.WEB08.AMIS.DL
{
    /// <summary>
    /// Dữ liệu thao tác với Database và trả về với bảng depot từ tầng DL
    /// </summary>
    /// Create by: TVTHAI (21/09/2022)
    public class InwardDL : BaseDL<Inward>, IInwardDL
    {
        #region Field

        private IDatabaseHelper<Inward> _dbHelper;

        #endregion

        #region Contructor

        public InwardDL(IDatabaseHelper<Inward> dbHelper) : base(dbHelper)
        {
            _dbHelper = dbHelper;
        }

        #endregion

        #region Method
        public object GetDetailByID (string recordID, string? stateForm)
        {
            // Khởi tạo các parameter để chèn vào trong Proc
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add($"v_InwardID", recordID);
            // Khai báo stored procedure
            string storeProcedureName = "Proc_InwardDetail_GetDetailOne";
            var res = _dbHelper.RunProcWithQueryFirstOrDefaultInwardDetail(storeProcedureName, parameters);
            return res;
        }

        public bool Approve(Guid recordID)
        {
            var sqlUpdate = @$"update inward set IsApprove = 1 where InwardID = '{recordID.ToString()}';
                update product a join inwarddetail b on a.ProductID = b.ProductID set a.Quantity = IFNULL(a.Quantity, 0) + IFNULL(b.Quantity, 0) where b.InwardID = '{recordID.ToString()}'";
            var resUpdate = _dbHelper.RunsqlWithExecute(sqlUpdate, new DynamicParameters());
            return true;
        }
        public override ServiceResponse InsertRecord(Inward record, string userID)
        {
            var v_MessOut = "";
            // tạo recordID
            Guid recordID = Guid.NewGuid();
            if(record.InwardID != null)
            {
                recordID = record.InwardID.Value;
            }
            // Khởi tạo các parameter để chèn vào trong Proc
            DynamicParameters parameters = new DynamicParameters();
            CustomParameterForCreate(ref parameters, record);
            parameters.Add("v_UserID", userID);
            var properties = typeof(Inward).GetProperties();
            foreach (var property in properties)
            {
                string propertyName = property.Name;
                // Kiểm tra trường nào là khoá chính thì thêm param là id tự sinh Guid
                var primaryKeyAttribute = (ValidateAttribute?)Attribute.GetCustomAttribute(property, typeof(ValidateAttribute));
                if (primaryKeyAttribute != null && primaryKeyAttribute.PrimaryKey)
                {
                    parameters.Add($"v_{propertyName}", recordID);
                }
                else if (primaryKeyAttribute != null && primaryKeyAttribute.NotMapParameterProc == true)
                {
                    continue;
                }
                else
                {
                    var propertyValue = property.GetValue(record, null);
                    parameters.Add($"v_{propertyName}", propertyValue);
                }
            }
            // chuẩn bị câu lệnh MySQL
            string storeProcedureName = string.Format(Resource.Proc_InsertOne, typeof(Inward).Name);
            int numberOfAffectdRows = _dbHelper.RunProcWithExecute(storeProcedureName, parameters, ref v_MessOut);
            if (numberOfAffectdRows > 0)
            {
                List<InwardDetail> inwardDetail = record.InwardDetail;
                var sqlInsert = "insert into inwarddetail(InwardDetailID, InwardID, ProductID, Quantity) value ";
                var subSqlInsert = "";
                DynamicParameters parametersDetail = new DynamicParameters();
                foreach (var item in inwardDetail)
                {
                    Guid detailID = Guid.NewGuid();
                    subSqlInsert += $"('{detailID}', '{recordID}', '{item.ProductID}', {item.Quantity}),";
                }
                subSqlInsert = subSqlInsert.Substring(0, subSqlInsert.Length - 1);
                sqlInsert = sqlInsert + subSqlInsert;
                var resDetail = _dbHelper.RunsqlWithExecute(sqlInsert, parametersDetail);
                return new ServiceResponse
                {
                    Success = true,
                    Data = recordID
                };
            }
            return new ServiceResponse
            {
                Success = false,
                Data = !string.IsNullOrEmpty(v_MessOut) ? v_MessOut : Guid.Empty.ToString()
            };
        }

        public virtual ServiceResponse UpdateRecord(Guid recordID, Inward record, string userID)
        {
            var sqldelete = $"delete from inwarddetail where InwardID = '{recordID}'; delete from inward where InwardID = '{recordID}';";
            var resdelete = _dbHelper.RunsqlWithExecute(sqldelete, new DynamicParameters());
            InsertRecord(record, userID);
            
            return new ServiceResponse
            {
                Success = true,
                Data = ""
            };
        }
        #endregion
    }
}
