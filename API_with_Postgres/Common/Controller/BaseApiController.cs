//using API_with_Postgres.Common.Contract;
//using Microsoft.AspNetCore.Mvc;
//using System.Net;
//using System.Text;

//namespace API_with_Postgres.Common.Controller
//{
//    public abstract class BaseApiController : ControllerBase
//    {
//        #region Protected Properties

//        protected readonly RequestHelper RequestHelper;
//        protected readonly ILog Logger;
//        protected string RequestHost => RequestHelper.RequestHost;

//        #endregion


//        #region Constructor

//        protected BaseApiController(Type concreteType)
//        {
//            Logger = LogManager.GetLogger(concreteType);
//            RequestHelper = MsDiService.Instance.GetService<RequestHelper>();
//        }

//        #endregion


//        #region Protected Methods

//        protected bool IsMultipartContentType(string contentType)
//        {
//            return !string.IsNullOrEmpty(contentType)
//                   && contentType.Contains("multipart/", StringComparison.OrdinalIgnoreCase);
//        }

//        protected async Task<IActionResult> Handle<T>(IDataProvider<T> dataProvider) where T : class
//        {
//            try
//            {
//                if (!dataProvider.IsAppVerify())
//                {
//                    return Unauthorized();
//                }

//                await dataProvider.Execute();

//                return Ok(dataProvider.Data);
//            }
//            catch (ArgumentException ex)
//            {
//                Logger.Error(ex.Message, ex);
//                return BadRequest(ex.Message);
//            }
//            catch (UnauthorizedAccessException ex)
//            {
//                Logger.Error(ex.Message, ex);
//                return Unauthorized();
//            }
//            catch (AccessViolationException avEx)
//            {
//                Logger.Error(avEx.Message, avEx);
//                return StatusCode((int)HttpStatusCode.Forbidden);
//            }
//            catch (Exception ex)
//            {
//                Logger.Error(ex.Message, ex);
//                return StatusCode(500, ex);
//            }
//        }

//        protected async Task<bool> VerifyUploadFile(List<Stream> fileStreams)
//        {
//            if (!ClamAVScanner.EnableClamAV)
//                return await Task.Run(() => true);

//            foreach (var file in fileStreams)
//            {
//                var fileBytes = ((MemoryStream)file).ToArray();

//                var result = await ClamAVScanner.ScanFile(fileBytes);

//                if (result.Result != ClamScanResults.Clean)
//                    return false;
//            }

//            return true;
//        }

//        protected async Task<bool> VerifyUploadFile(IFormFileCollection files)
//        {
//            if (!ClamAVScanner.EnableClamAV)
//                return await Task.Run(() => true);

//            foreach (var file in files)
//            {
//                await using var ms = new MemoryStream();
//                await file.CopyToAsync(ms);
//                var fileBytes = ms.ToArray();

//                var result = await ClamAVScanner.ScanFile(fileBytes);

//                if (result.Result != ClamScanResults.Clean)
//                    return false;
//            }

//            return true;
//        }

//        protected async Task<string> ReadBodyAsString()
//        {
//            if (Request.Body.CanRead)
//            {
//                using StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8);
//                return await reader.ReadToEndAsync();
//            }

//            return await Task.Run(() => string.Empty);
//        }

//        protected void ConfigureStreamingResponse()
//        {
//            Response.ContentType = "text/event-stream";
//            Response.Headers.Add("Cache-Control", "no-store, no-cache, must-revalidate, proxy-revalidate");
//            Response.Headers.Add("Pragma", "no-cache");
//            Response.Headers.Add("Expires", "0");
//            Response.Headers.Add("Surrogate-Control", "no-store");
//        }

//        #endregion
//    }
//}
