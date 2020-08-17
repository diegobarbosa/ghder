using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace Trustly.Ghder.Core.Downloader
{
    [XmlRoot("Projects")]
    public class ProjectResult
    {
        public string FileExtension { get; set; }

        public long NumberOfLines { get; set; }

        public long Size { get; set; }
    }
}
