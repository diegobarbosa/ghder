using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trustly.Ghder.Core;
using Trustly.Ghder.Core.Downloader;
using Xunit;

namespace Trustly.Ghder.Tests
{
    public class GHProjectDownloaderServiceFacts
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async void DownloadProjectInfo_NoUserName_Fails(string input)
        {
            var service = new GHProjectDownloaderService();

            var ex = await Assert.ThrowsAsync<DomainException>(() => service.DownloadProjectInfoAsync(input, "project"));

            Assert.Equal("userName not informed", ex.Message);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async void DownloadProjectInfo_NoProjectName_Fails(string input)
        {
            var service = new GHProjectDownloaderService();

            var ex = await Assert.ThrowsAsync<DomainException>(() => service.DownloadProjectInfoAsync("user", input));

            Assert.Equal("projectName not informed", ex.Message);
        }

        [Fact]
        public async void DownloadProjectInfo_NoExistantProject_Fails()
        {
            var service = new GHProjectDownloaderService();

            var ex = await Assert.ThrowsAsync<DomainException>(() => service.DownloadProjectInfoAsync("diegobarbosa", "SomeRandomName"));

            Assert.StartsWith("GitHub Repository not found:", ex.Message);
        }

        [Fact]
        public async void DownloadProjectInfo_EmptyFolder_Fails()
        {
            var service = new GHProjectDownloaderService();

            var ex = await Assert.ThrowsAsync<DomainException>(() => service.DownloadProjectInfoAsync("diegobarbosa", "ghdertestempty"));

            Assert.StartsWith("No Rows found in:", ex.Message);
        }

        


        [Fact]
        public async void DownloadProjectInfo_Sucess()
        {
            //Executes the service against a real github repository with a expected result.

            var service = new GHProjectDownloaderService();

            var result = await service.DownloadProjectInfoAsync("diegobarbosa", "ghdertest");

            Assert.Equal(10, result.Count);

            AssertResult(result, "txt", 340, 27565);
            AssertResult(result, "", 40, 1402);
            AssertResult(result, "bmp", 0, 0);
            AssertResult(result, "cs", 100, 3493);
            AssertResult(result, "doc", 0, 28647096);
            AssertResult(result, "java", 4, 50);
            AssertResult(result, "js", 10, 307);
            AssertResult(result, "md", 2, 48);
            AssertResult(result, "pdf", 0, 7239);
            AssertResult(result, "rtf", 103, 10342);


        }


        void AssertResult(List<ProjectResult> results, string extension, int numberOfLines, long size)
        {
            var item = results.SingleOrDefault(x => x.FileExtension == extension);

            Assert.Equal(numberOfLines, item.NumberOfLines);

            Assert.Equal(size, item.Size);
        }

    }
}
