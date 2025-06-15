using MISA.WEB08.AMIS.Common.Entities;
using System;

namespace MISA.WEB08.AMIS.BL
{
    /// <summary>
    /// Dữ liệu thao tác với Database và trả về với bảng Depot từ tầng BL
    /// </summary>
    /// Create by: TVTHAI (21/09/2022)
    public interface IInwardBL : IBaseBL<Inward>
    {
        public object GetDetailByID(string recordID, string? stateForm);
        public bool Approve(Guid recordID);
    }
}
