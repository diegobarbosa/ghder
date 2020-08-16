using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Trustly.Ghder.Core.Downloader
{
    public class LinesAndSizeExtractor
    {

        static Regex regex = new Regex(@"(\d* lines).*\(.*\).* ([0-9]*[\.\d*].*[a-zA-Z]*)");

        public static LinesAndSize Extract(string htmlInput)
        {
            var result = new LinesAndSize();

            var match = regex.Match(htmlInput.Trim().Replace('\n', ' '));

            result.NumberOfLines = Convert.ToInt32(match.Groups[1].Value.Replace("lines", ""));

            var fileSize = match.Groups[2].Value.Split(' ');

            result.Size = Convert.ToDecimal(fileSize[0]);
            result.Unit = fileSize[1].Trim().ToLower();

            return result;
        }

        public class LinesAndSize
        {
            public int NumberOfLines { get; set; }

            public decimal Size { get; set; }

            /// <summary>
            /// Possible values: Bytes, Byte, kb, mb
            /// </summary>
            public string Unit { get; set; }
        }

    }
}
