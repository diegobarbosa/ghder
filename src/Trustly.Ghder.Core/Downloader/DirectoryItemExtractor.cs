using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trustly.Ghder.Core.Downloader
{
    public class DirectoryItemExtractor
    {
        /// <summary>
        /// Return all Dir and files in a Repository
        /// </summary>
        /// <param name="pageUrl"></param>
        /// <param name="directoryItemObjects"></param>
        public static async Task GetDirItems(string pageUrl, List<DirectoryItem> directoryItemObjects)
        {
            //Dirs at the current folder/page
            var localDirs = new List<DirectoryItem>();

            var tasks = new List<Task<Page>>();

            //Start with the root Repository folder
            localDirs.Add(new DirectoryItem { Url = pageUrl, Type = DirObjectType.DIRECTORY });


            while (localDirs.Count > 0)
            {

                //Downloads all folder pages in parallel
                foreach (var item in localDirs.Where(x => x.Type == DirObjectType.DIRECTORY))
                {
                    tasks.Add(GetPage(GHProjectDownloaderService.GitHubUrl + item.Url));
                }

                localDirs = new List<DirectoryItem>();


                //Waits all folder/pages to be downloaded
                await Task.WhenAll(tasks);


                //For each downloaded dir/page
                foreach (var page in tasks)
                {
                    var doc = new HtmlDocument();
                    doc.LoadHtml(page.Result.Html);

                    //List all rows in a directory.
                    //Each row is a div and each cell is a div.
                    var divs = doc.DocumentNode.SelectNodes("//div[contains(@class,'js-navigation-item')]");

                    if (divs == null)
                    {
                        throw new DomainException($"No Rows found in: {page.Result.Url}");
                    }

                    foreach (var div in divs)
                    {

                        //Inside a folder, the first item of the directory is a link for the upper directory.
                        //If the first cell row doesnt have a svg, its a upper directory link.
                        var svg = div.SelectSingleNode("div[1]/svg");

                        if (svg == null)
                        {
                            continue;
                        }

                        var type = svg.Attributes["aria-label"].Value.ToLower();// directory or file
                        var dirObjectType = type == "file" ? DirObjectType.FILE : DirObjectType.DIRECTORY;

                        var url = div.SelectSingleNode("div[2]/span/a").Attributes["href"].Value;
                        var name = div.SelectSingleNode("div[2]/span/a").InnerText;

                        var newItem = new DirectoryItem { Type = dirObjectType, Url = url, Name = name, Extension = GetExtention(name) };

                        //all folder objects
                        directoryItemObjects.Add(newItem);


                        //Only folders.
                        //Folders can have subfolders!
                        if (newItem.Type == DirObjectType.DIRECTORY)
                        {
                            localDirs.Add(newItem);
                        }


                    }

                }//foreach

                tasks = new List<Task<Page>>();


            }//While


        }// Method


        static string GetExtention(string file)
        {
            var count = file.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);

            return count.Length == 1 ? string.Empty : count[1].ToLower();
        }


        static async Task<Page> GetPage(string url)
        {
            return new Page { Html = await GhderHttpClient.Instance.GetStringAsync(url), Url = url };
        }

        public class Page
        {
            public string Html { get; set; }

            public string Url { get; set; }
        }


    }
}
