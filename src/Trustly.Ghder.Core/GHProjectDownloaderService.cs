using System;
using System.Collections.Generic;
using System.Text;

namespace Trustly.Ghder.Core
{
    public class GHProjectDownloaderService
    {
        public GHProjectInfo DownloadGHInfo(string userName, string projectName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new DomainException("userName not informed");
            }

            if (string.IsNullOrWhiteSpace(projectName))
            {
                throw new DomainException("projectName not informed");
            }



            var pageInfo = new GHProjectInfo();

            return pageInfo;
        }





    }
}
