using System;
using System.Collections.Generic;
using System.Text;

namespace Trustly.Ghder.Core.Downloader
{
    public class ProjectResult
    {
        public string FileExtension { get; set; }

        public long NumberOfLines { get; set; }

        public long Size { get; set; }
    }
}
