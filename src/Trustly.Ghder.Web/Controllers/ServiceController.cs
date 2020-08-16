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
    [ApiController]
    public class ServiceController : ControllerBase
    {

        // GET api/service/userName/projectName
        [HttpGet("{user}/{project}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        public List<ProjectResult> Get(string user, string project)
        {
            return new GHProjectDownloaderService().DownloadProjectInfo(user, project);
        }

        
    }
}
