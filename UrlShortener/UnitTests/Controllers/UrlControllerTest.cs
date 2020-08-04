using Castle.Core.Logging;
using Controllers;
using Entities.Interfaces;
using Entities.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace UnitTests.Controllers
{
    public class UrlControllerTest
    {
        protected Mock<IUrlService> UrlServiceMock { get; }
        protected UrlController UrlControllerToTest { get; }

        public UrlControllerTest()
        {
            UrlServiceMock = new Mock<IUrlService>();
            UrlControllerToTest = new UrlController(UrlServiceMock.Object, new NullLogger<UrlController>() );
        }

        public class Get : UrlControllerTest
        {
            [Fact]
            public async void ShouldReturnOkWithResults()
            {
                // Arrange
                var urls = new List<UrlDto>
                {
                    new UrlDto
                    {
                        Id = 2,
                        AccessCount = 1,
                        ActualUrl = "https://google.com",
                        ShortenedUrl = "GH65ff"
                    },
                    new UrlDto
                    {
                        Id = 1,
                        AccessCount = 5,
                        ActualUrl = "https://youtube.com",
                        ShortenedUrl = "3H6Dwe"
                    }
                };
                Paginated<UrlDto> paginatedResults = new Paginated<UrlDto>(urls, 2, 1, 10);
                
                UrlServiceMock.Setup(x => x.GetAll(1, 10)).ReturnsAsync(paginatedResults);

                // Act
                var result = await UrlControllerToTest.Get(1, 10);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                Assert.Same(paginatedResults, okResult.Value);
            }
        }

        public class GetOne : UrlControllerTest
        {
            [Fact]
            public async void ShouldReturnOkWithResults()
            {
                // Arrange
                var url = new UrlDto
                {
                    Id = 2,
                    AccessCount = 1,
                    ActualUrl = "https://google.com",
                    ShortenedUrl = "GH65ff"
                };

                UrlServiceMock.Setup(x => x.GetUrl("GH65ff")).ReturnsAsync(url);

                // Act
                var result = await UrlControllerToTest.Get("GH65ff");

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                Assert.Same(url, okResult.Value);
            }

            [Fact]
            public async void ShouldReturnNotFound()
            {
                // Arrange
                var url = new UrlDto
                {
                    Id = 2,
                    AccessCount = 1,
                    ActualUrl = "https://google.com",
                    ShortenedUrl = "GH65ff"
                };

                UrlServiceMock.Setup(x => x.GetUrl("GH65fd")).ReturnsAsync(url);

                // Act
                var result = await UrlControllerToTest.Get("GH65gg");

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        public class Post : UrlControllerTest
        {
            [Fact]
            public async void ShouldReturnOkResult()
            {
                // Arrange
                var url = new UrlCreate
                {
                    ActualUrl = "https://google.com"
                };

                var urlDto = new UrlDto
                {
                    Id = 1,
                    ActualUrl = "https://google.com",
                    ShortenedUrl = "fGwY73",
                    AccessCount = 0
                };

                UrlServiceMock.Setup(x => x.AddUrl(url)).ReturnsAsync(urlDto);

                // Act
                var result = await UrlControllerToTest.Post(url);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                Assert.Same(urlDto, okResult.Value);
            }
        }

        public class Put : UrlControllerTest
        {
            [Fact]
            public async void ShouldReturnOkResult()
            {
                // Arrange
                var urlDto = new UrlDto
                {
                    Id = 1,
                    ActualUrl = "https://google.com",
                    ShortenedUrl = "fGwY73",
                    AccessCount = 0
                };

                UrlServiceMock.Setup(x => x.UpdateUrl(urlDto)).ReturnsAsync(urlDto);

                // Act
                var result = await UrlControllerToTest.Put(urlDto);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                Assert.Same(urlDto, okResult.Value);
            }

            [Fact]
            public async void ShouldReturnNotFound()
            {
                // Arrange
                var url = new UrlDto
                {
                    Id = 2,
                    AccessCount = 1,
                    ActualUrl = "https://google.com",
                    ShortenedUrl = "GH65ff"
                };

                var urlDto = new UrlDto
                {
                    Id = 8,
                    AccessCount = 1,
                    ActualUrl = "https://google.com",
                    ShortenedUrl = "GH65ff"
                };

                UrlServiceMock.Setup(x => x.UpdateUrl(url)).ReturnsAsync(url);

                // Act
                var result = await UrlControllerToTest.Put(urlDto);

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        public class Delete : UrlControllerTest
        {
            [Fact]
            public async void ShouldReturnOk()
            {
                // Arrange
                int id = 1;

                // Act
                var result = await UrlControllerToTest.Delete(id);

                // Assert
                Assert.IsType<OkResult>(result);
            }
        }
    }
}
