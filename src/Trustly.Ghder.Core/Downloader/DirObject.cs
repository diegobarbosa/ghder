using System;
using System.Collections.Generic;
using System.Text;

namespace Trustly.Ghder.Core.Downloader
{
    public class DirObject
    {
        public DirObjectType Type { get; set; }

        public string Url { get; set; }

        public string Name { get; set; }

        public string Extension { get; set; }

        public decimal Size { get; set; }

        /// <summary>
        /// Possible values: Bytes, Byte, kb, mb
        /// </summary>
        public string SizeUnit { get; set; }

        public long NumberOfLines { get; set; }


        public long ConvertSizeToBytes()
        {
            if (SizeUnit == "byte" || SizeUnit == "bytes")
            {
                return (long)Size;
            }

            if (SizeUnit == "kb")
            {
                return (long)(Size * 1024);
            }

            if (SizeUnit == "mb")
            {
                return (long)(Size * 1024 * 1024);
            }

            throw new DomainException($"Invalid {nameof(SizeUnit)}: {SizeUnit}. Object: {Type} {Name}.{Extension}. Url:{Url}");

        }

    }

    public enum DirObjectType
    {
        FILE,
        DIRECTORY
    }
}
