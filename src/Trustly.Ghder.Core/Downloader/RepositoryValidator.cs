using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Trustly.Ghder.Core.Downloader
{
    public class RepositoryValidator
    {
        /// <summary>
        /// Checks if the user/project informed exists
        /// </summary>
        /// <param name="userAndRepository"></param>
        public static async Task CheckIfRepositoryIsValid(string userAndRepository)
        {
            var fullUrl = GHProjectDownloaderService.GitHubUrl + userAndRepository;

            try
            {
                var dataStr = await GhderHttpClient.Instance.GetStringAsync(fullUrl);

                if (dataStr.Contains("This repository is empty."))
                {
                    throw new DomainException($"GitHub Repository is empty: {fullUrl}");
                }
            }
            
            catch (HttpRequestException ex)
            {
                if (ex.Message.Contains("404"))
                {
                    throw new DomainException($"GitHub Repository not found: {fullUrl}");
                }

                throw;
            }
           

        }

    }
}
