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

        [Fact]
        public void Extract_0Bytes()
        {
            var input = @"

      0 lines(0 sloc)
      
    0 Bytes
  ";

            var result = LinesAndSizeExtractor.Extract(input);


            Assert.Equal(0, result.NumberOfLines);

            Assert.Equal(0, result.Size);

            Assert.Equal("bytes", result.Unit);
        }


        [Fact]
        public void Extract_KB()
        {
            var input = @"

      40 lines(24 sloc)
      
    7.07 KB
  ";

            var result = LinesAndSizeExtractor.Extract(input);


            Assert.Equal(40, result.NumberOfLines);

            Assert.Equal(7.07m, result.Size);

            Assert.Equal("kb", result.Unit);
        }


        [Fact]
        public void Extract_MB()
        {
            var input = @"

      400 lines(24 sloc)
      
    23.5 MB
  ";

            var result = LinesAndSizeExtractor.Extract(input);


            Assert.Equal(400, result.NumberOfLines);

            Assert.Equal(23.5m, result.Size);

            Assert.Equal("mb", result.Unit);
        }



    }
}