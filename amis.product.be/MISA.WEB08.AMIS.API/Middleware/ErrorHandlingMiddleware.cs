using Microsoft.AspNetCore.Http;
using MISA.WEB08.AMIS.Common.Enums;
using System;
using MISA.WEB08.AMIS.Common.Resources;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MISA.WEB08.AMIS.API.Middleware
{
    public class MisaAmisErrorResultCustom
    {
        #region Field

        /// <summary>
        /// Mã lỗi enum
        /// </summary>
        /// Created by : Nguyễn Khắc Tiềm 21.09.2022
        public MisaAmisErrorCode errorCode { get; set; }

        /// <summary>
        /// Mã lỗi cho dev
        /// </summary>
        /// Created by : Nguyễn Khắc Tiềm 21.09.2022
        public string devMsg { get; set; }

        /// <summary>
        /// Mã lỗi cho người dùng
        /// </summary>
        /// Created by : Nguyễn Khắc Tiềm 21.09.2022
        public dynamic? userMsg { get; set; }

        /// <summary>
        /// Thông tin thêm
        /// </summary>
        /// Created by : Nguyễn Khắc Tiềm 21.09.2022
        public string moreInfo { get; set; }

        /// <summary>
        /// ID kết nối để trace sau này để dò lỗi
        /// </summary>
        /// Created by : Nguyễn Khắc Tiềm 21.09.2022
        public string traceId { get; set; }

        #endregion

        #region Contructor

        /// <summary>
        /// Hàm khởi tạo
        /// </summary>
        /// <param name="errorCode"></param>
        /// <param name="devMsg"></param>
        /// <param name="userMsg"></param>
        /// <param name="moreInfo"></param>
        /// <param name="traceId">ID kết nối</param>
        /// Created by : Nguyễn Khắc Tiềm 21.09.2022
        public MisaAmisErrorResultCustom(MisaAmisErrorCode errorCode, string devMsg, dynamic? userMsg, string moreInfo, string traceId)
        {
            this.userMsg = userMsg;
            this.errorCode = errorCode;
            this.devMsg = devMsg;
            this.moreInfo = moreInfo;
            this.traceId = traceId;
        }

        #endregion
    }
    public class ServiceResponseCustom
    {
        #region Field

        /// <summary>
        /// trả về true hoặc false (thành công hoặc thất bại)
        /// </summary>
        /// Create by: Nguyễn Khắc Tiềm (21/09/2022)
        public bool success { get; set; }

        /// <summary>
        /// Mã lỗi đi kèm
        /// </summary>
        /// Create by: Nguyễn Khắc Tiềm (21/09/2022)
        public MisaAmisErrorCode? errorCode { get; set; }

        /// <summary>
        /// Dữ liệu đi kèm
        /// </summary>
        /// Create by: Nguyễn Khắc Tiềm (21/09/2022)
        public dynamic? data { get; set; }

        #endregion
    }
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            Console.WriteLine(exception.Message);
            var result = new ServiceResponseCustom
            {
                success = false,
                errorCode = MisaAmisErrorCode.Exception,
                data = new MisaAmisErrorResultCustom(
                    MisaAmisErrorCode.Exception,
                    exception.Message.ToString(),
                    Resource.UserMsg_Exception,
                    Resource.MoreInfo_Exception,
                    context.TraceIdentifier
                )
            };
            var resultJson = JsonConvert.SerializeObject(result);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.OK;
            await context.Response.WriteAsync(resultJson);
        }
    }
}
