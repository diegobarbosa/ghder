using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Trustly.Ghder.Core.Downloader
{
    public class FileInfoExtractor
    {
        /// <summary>
        /// Extracts the Informations about a file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static async Task GetFileInfo(DirectoryItem file)
        {
            var dataStr = await GhderHttpClient.Instance.GetStringAsync("https://github.com" + file.Url);

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
