using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Trustly.Ghder.Core;
using Trustly.Ghder.Core.Downloader;
using Trustly.Ghder.Web.Utils;

namespace Trustly.Ghder.Web.Controllers
{
    [Route("api/[controller]")]
    [Consumes("text/json", otherContentTypes: "text/xml")]
    [Produces("text/json", additionalContentTypes: "text/xml")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
    [ProducesResponseType(StatusCodes.Status406NotAcceptable, Type = typeof(ErrorResponse))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
    [ApiController]
    public class ServiceController : ControllerBase
    {

        // GET api/service/userName/projectName
        [HttpGet("{user}/{project}")]
        [ResponseCache(Location =ResponseCacheLocation.Any, Duration = 60, VaryByQueryKeys = new string[] { "*" })]
        public async Task<List<ProjectResult>> Get(string user, string project)
        {
            return await new GHProjectDownloaderService().DownloadProjectInfoAsync(user, project);
        }


    }
}
