using MISA.WEB08.AMIS.Common.Entities;
using System;

namespace MISA.WEB08.AMIS.DL
{
    /// <summary>
    /// Dữ liệu thao tác với Database và trả về với bảng depot từ tầng DL
    /// </summary>
    /// Create by: TVTHAI (21/09/2022)
    public interface IInwardDL : IBaseDL<Inward>
    {
        public object GetDetailByID(string recordID, string? stateForm);
        public bool Approve(Guid recordID);
    }
}
