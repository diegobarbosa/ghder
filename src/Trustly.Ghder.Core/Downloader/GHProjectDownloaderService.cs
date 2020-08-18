using HtmlAgilityPack;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Trustly.Ghder.Core.Downloader
{
    public class GHProjectDownloaderService
    {
        public static string GitHubUrl = "https://github.com/";


        /// <summary>
        /// Returns the total number of lines and the total number of bytes of all the files of a given public Github repository, grouped by file extension
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="projectName"></param>
        /// <returns></returns>
        public async Task<List<ProjectResult>> DownloadProjectInfoAsync(string userName, string respositoryName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new DomainException("userName not informed");
            }

            if (string.IsNullOrWhiteSpace(respositoryName))
            {
                throw new DomainException("projectName not informed");
            }


            var userAndRepository = $"{userName}/{respositoryName}";


            await RepositoryValidator.CheckIfRepositoryIsValid(userAndRepository);





            var dirObjects = new List<DirectoryItem>();
            await Task.WhenAll(DirectoryItemExtractor.GetDirItems(userAndRepository, dirObjects));



            var fileTasks = new List<Task>();
            foreach (var item in dirObjects.Where(x => x.Type == DirObjectType.FILE))
            {
                fileTasks.Add(FileInfoExtractor.GetFileInfo(item));
            }
            await Task.WhenAll(fileTasks);



            var result = dirObjects.Where(x => x.Type == DirObjectType.FILE)
                 .GroupBy(x => x.Extension)
                 .Select(x => new ProjectResult
                 {
                     FileExtension = x.Key,
                     Size = x.Sum(y => y.ConvertSizeToBytes()),
                     NumberOfLines = x.Sum(y => y.NumberOfLines)
                 });

            return result.OrderBy(x => x.FileExtension).ToList();


        }


    }


}
