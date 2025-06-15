using MISA.WEB08.AMIS.Common.Attributes;
using MISA.WEB08.AMIS.Common.Entities;
using MISA.WEB08.AMIS.Common.Enums;
using MISA.WEB08.AMIS.Common.Result;
using MISA.WEB08.AMIS.DL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace MISA.WEB08.AMIS.BL
{
    /// <summary>
    /// Dữ liệu thao tác với Database và trả về với bảng Depot từ tầng BL
    /// </summary>
    /// Create by: TVTHAI (21/09/2022)
    public class InwardBL : BaseBL<Inward>, IInwardBL
    {
        #region Field

        private IInwardDL _inwardDL;

        #endregion

        #region Contructor

        public InwardBL(IInwardDL inwardDL) : base(inwardDL)
        {
            _inwardDL = inwardDL;
        }

        #endregion

        #region Method
        /// <summary>
        /// Hàm lấy ra bản ghi theo ID
        /// </summary>
        /// <param name="recordID">ID bản ghi</param>
        /// <param name="stateForm">Trạng thái lấy (sửa hay nhân bản, ...)</param>
        /// <returns>Thông tin chi tiết một bản ghi</returns>
        /// Create by: TVTHAI (26/09/2022)
        public object GetDetailByID(string recordID, string? stateForm)
        {
            return _inwardDL.GetDetailByID(recordID, stateForm);
        }

        public bool Approve(Guid recordID)
        {
            return _inwardDL.Approve(recordID);
        }
        #endregion
    }
}
