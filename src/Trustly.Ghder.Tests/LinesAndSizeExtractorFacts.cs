using Trustly.Ghder.Core.Downloader;
using Xunit;

namespace Trustly.Ghder.Tests
{
    public class LinesAndSizeExtractorFacts
    {
       

        [Fact]
        public void Extract_Bytes()
        {
            var input = @"

      40 lines(24 sloc)
      
    915 Bytes
  ";

            var result = LinesAndSizeExtractor.Extract(input);


            Assert.Equal(40, result.NumberOfLines);

            Assert.Equal(915m, result.Size);

            Assert.Equal("bytes", result.Unit);
        }
    }
}