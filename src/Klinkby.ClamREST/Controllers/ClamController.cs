using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Klinkby.Clam;
using System.Net;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Sockets;
using System.Diagnostics;

namespace Klinkby.ClamREST.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public partial class ClamController : ControllerBase
    {
        private readonly ILogger<ClamController> _logger;
        private readonly Func<IClamClient> clamClientFactory;
        private readonly long maxFileSize;

        public ClamController(ILogger<ClamController> logger, IAppConfiguration appConfiguration, Func<IClamClient> clamClientFactory)
        {
            _logger = logger;
            this.clamClientFactory = clamClientFactory;
            maxFileSize = appConfiguration.MaxFileSize;
        }

        public class FileFormData
        {
            public IFormFile File { get; set; }
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        [SwaggerResponse((int)HttpStatusCode.NoContent, Description = "Scan pass")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "File is empty", typeof(ProblemDetails))]
        [SwaggerResponse((int)HttpStatusCode.Forbidden, "Scan fail", typeof(ProblemDetails))]
        [SwaggerResponse((int)HttpStatusCode.RequestEntityTooLarge, "File is too large to scan", typeof(ProblemDetails))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, "Unexpected", typeof(ProblemDetails))]
        [SwaggerResponse((int)HttpStatusCode.ServiceUnavailable, "Clam socket fail", typeof(ProblemDetails))]
        public async Task<object> Post([FromForm] FileFormData formData)
        {
            IFormFile file = formData?.File;
            if (null == file)
            {
                _logger.LogWarning("File is empty");
                return Problem(
                    type: "err-empty",
                    title: "File is empty",
                    statusCode: (int)HttpStatusCode.BadRequest);
            }
            if (file.Length > maxFileSize)
            {
                _logger.LogWarning($"File too big {file.Length}");
                return Problem(
                    type: "err-file-too-large",
                    title: $"Request file should be no more than {maxFileSize / 1024 / 1024} Mbytes",
                    statusCode: (int)HttpStatusCode.RequestEntityTooLarge);
            }
            var sw = new Stopwatch();
            sw.Start();
            try
            {
                await using (var fi = file.OpenReadStream())
                {
                    using (var clamClient = clamClientFactory())
                    {
                        await clamClient.InstreamAsync(fi);
                    }
                }
            }
            catch (ClamException e)
            {
                _logger.LogWarning(e.Message);
                return Problem(
                    type: "err-scan",
                    title: e.Message,
                    statusCode: (int)HttpStatusCode.Forbidden);
            }
            catch (SocketException e)
            {
                _logger.LogError(e, e.Message);
                return Problem(
                    type: "err-socket",
                    title: e.Message,
                    statusCode: (int)HttpStatusCode.ServiceUnavailable);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return Problem(
                    type: "err-unexpected",
                    title: e.Message,
                    statusCode: (int)HttpStatusCode.InternalServerError);
            }
            finally
            {
                _logger.LogInformation($"Scanned {file.Length} bytes in {sw.ElapsedMilliseconds}mS ({(long)((double)file.Length*1000 / sw.ElapsedMilliseconds)} bps)");
            }
            _logger.LogInformation("Scan pass");
            return NoContent(); // a good thing
        }
    }
}
