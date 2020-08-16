using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Trustly.Ghder.Core.Downloader
{
    public class GHProjectDownloaderService
    {
        /// <summary>
        /// Returns the total number of lines and the total number of bytes of all the files of a given public Github repository, grouped by file extension
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="projectName"></param>
        /// <returns></returns>
        public List<ProjectResult> DownloadProjectInfo(string userName, string projectName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new DomainException("userName not informed");
            }

            if (string.IsNullOrWhiteSpace(projectName))
            {
                throw new DomainException("projectName not informed");
            }

            var projectUrl = $"https://github.com/{userName}/{projectName}";


            CheckIfProjectIsValid(projectUrl);



            var dirObjects = new List<DirObject>();

            ProcessDirObjects(projectUrl, dirObjects);


            Parallel.ForEach(dirObjects.Where(x => x.Type == DirObjectType.FILE), (item) =>
            {
                GetFileInfo(item);
            });


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



        /// <summary>
        /// Checks if the user/project informed exists
        /// </summary>
        /// <param name="projectUrl"></param>
        void CheckIfProjectIsValid(string projectUrl)
        {
            try
            {
                var dataStr = GetPageData(projectUrl);
            }
            catch (WebException ex)
            {
                var httpResponseCode = ((System.Net.HttpWebResponse)ex.Response).StatusCode;

                if (httpResponseCode == HttpStatusCode.NotFound)
                {
                    throw new DomainException($"GitHub Project not found. {projectUrl}");
                }

                throw ex;
            }

        }

        string GetPageData(string pageUrl)
        {
            var client = new WebClient();

            byte[] data;

            data = client.DownloadData(pageUrl);

            return client.Encoding.GetString(data);
        }

        /// <summary>
        /// Return all Dir or files in a project
        /// </summary>
        /// <param name="pageUrl"></param>
        /// <param name="dirObjects"></param>
        void ProcessDirObjects(string pageUrl, List<DirObject> dirObjects)
        {

            var dataStr = GetPageData(pageUrl);


            var doc = new HtmlDocument();
            doc.LoadHtml(dataStr);

            //List all rows in a directory.
            //Each row is a div and each cell is a div.
            var divs = doc.DocumentNode.SelectNodes("//div[contains(@class,'js-navigation-item')]");


            Parallel.ForEach(divs, div =>
            {
                //Inside a folder, the first item of the directory is a link for the upper directory.
                //If the first cell row doesnt have a svg, its a upper directory link.
                var svg = div.SelectSingleNode("div[1]/svg");

                if (svg == null)
                {
                    return;
                }

                var type = svg.Attributes["aria-label"].Value.ToLower();// directory or file
                var dirObjectType = type == "file" ? DirObjectType.FILE : DirObjectType.DIRECTORY;

                var url = div.SelectSingleNode("div[2]/span/a").Attributes["href"].Value;
                var name = div.SelectSingleNode("div[2]/span/a").InnerText;

                dirObjects.Add(new DirObject { Type = dirObjectType, Url = url, Name = name, Extension = GetExtention(name) });

                if (dirObjectType == DirObjectType.DIRECTORY)
                {
                    ProcessDirObjects("https://github.com" + url, dirObjects);
                }

            });

        }


        string GetExtention(string file)
        {
            var count = file.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);

            return count.Length == 1 ? string.Empty : count[1].ToLower();
        }


        void GetFileInfo(DirObject file)
        {
            var client = new WebClient();
            byte[] data = client.DownloadData("https://github.com" + file.Url);
            var dataStr = client.Encoding.GetString(data);


            var doc = new HtmlDocument();
            doc.LoadHtml(dataStr);

            var div = doc.DocumentNode.SelectSingleNode("//div[contains(@class,'Box mt-3 position-relative')]/div[contains(@class,'Box-header')]/div");

            var divider = div.SelectSingleNode("span");

            //The Html in Github informs the Number of lines and Size of a File.
            //Here these values are extracted.

            //If divider is null the page only display de size of the file.
            //Its a Binary file.
            //Else its a Text file and the number of lines and Size of the file are shown.

            if (divider == null)// only size
            {
                var fileSize = div.InnerText.Trim().Split(' ');
                file.Size = Convert.ToDecimal(fileSize[0]);
                file.SizeUnit = fileSize[1].ToLower();
            }
            else
            {
                LinesAndSizeExtractor.LinesAndSize linesAndSize;

                try
                {
                    linesAndSize = LinesAndSizeExtractor.Extract(div.InnerText);
                }
                catch (Exception ex)
                {

                    throw;
                }

               

                file.NumberOfLines = linesAndSize.NumberOfLines;
                file.Size = linesAndSize.Size;
                file.SizeUnit = linesAndSize.Unit;

            }

        }



    }


}
