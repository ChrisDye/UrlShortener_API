using Entities.Context;
using Entities.Models.DTO;
using Microsoft.EntityFrameworkCore;
using Services;
using System;
using System.Runtime.InteropServices.ComTypes;
using Xunit;

namespace UnitTests.Services
{
    public class UrlServiceTest
    {
        protected UrlService UrlServiceToTest { get; }
        protected UrlContext UrlContextMock { get; }

        public UrlServiceTest()
        {
            // Giving each DB a different guid name to prevent conflicts
            var options = new DbContextOptionsBuilder<UrlContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            UrlContextMock = new UrlContext(options);
            UrlServiceToTest = new UrlService(UrlContextMock);
            
            UrlContextMock.Database.EnsureCreated();
            UrlContextMock.Urls.AddRange(
                new Entities.Models.Entity.Url
                {
                    Id = 1,
                    AccessCount = 0,
                    ActualUrl = "https://google.co.uk",
                    ShortenedUrl = "asTG36",
                    Created = DateTime.Now
                },
                new Entities.Models.Entity.Url
                {
                    Id = 2,
                    AccessCount = 0,
                    ActualUrl = "https://youtube.co.uk",
                    ShortenedUrl = "fr7Y2J",
                    Created = DateTime.Now
                },
                new Entities.Models.Entity.Url
                {
                    Id = 3,
                    AccessCount = 0,
                    ActualUrl = "https://github.co.uk",
                    ShortenedUrl = "TT66hh",
                    Created = DateTime.Now
                }
            );
            UrlContextMock.SaveChanges();
        }

        public class GetAll : UrlServiceTest
        {
            [Fact]
            public async void ShouldReturnItems()
            {
                // Arrange

                // Act
                var result = await UrlServiceToTest.GetAll(1, 10);

                // Assert
                Assert.True(3 == result.Items[0].Id);
            }
        }

        public class GetUrl : UrlServiceTest
        {
            [Fact]
            public async void ShouldReturnItem()
            {

                // Act
                var result = await UrlServiceToTest.GetUrl("fr7Y2J");

                // Assert
                Assert.True(result.ActualUrl == "https://youtube.co.uk");
                Assert.True(result.AccessCount == 1);
            }
        }

        public class DeleteUrl : UrlServiceTest
        {
            [Fact]
            public async void ShouldRemove()
            {
                // This test is rather brittle and I'm not truly happy with it. Kept it in for coverage
                // Brittle because if GetUrl fails then this test also fails

                // Act
                await UrlServiceToTest.DeleteUrl(2);

                // Assert 
                try
                {
                    await UrlServiceToTest.GetUrl("fr7Y2J");
                }
                catch (Exception ex)
                {
                    Assert.True(ex.Message == "Not found");
                }
            }
        }

        public class AddUrl : UrlServiceTest
        {
            [Fact]
            public async void ShouldAdd()
            {
                // Arrange
                UrlCreate urlCreate = new UrlCreate { ActualUrl = "https://bbc.co.uk" };

                // Act
                var result = await UrlServiceToTest.AddUrl(urlCreate);

                // Assert
                Assert.True(result.ActualUrl == urlCreate.ActualUrl);
            }
        }

        public class UpdateUrl : UrlServiceTest
        {
            [Fact]
            public async void ShouldUpdate()
            {
                // Arrange
                UrlDto newUrl = new UrlDto 
                { 
                    Id = 2,
                    ActualUrl = "https://www.bbc.co.uk/news" ,
                    ShortenedUrl = "FG5tYi"
                };

                // Act
                var result = await UrlServiceToTest.UpdateUrl(newUrl);

                // Assert
                Assert.True(result.Id == newUrl.Id);
                Assert.True(result.ActualUrl == newUrl.ActualUrl);
            }

        }
    }
}
