using System;
using System.Threading.Tasks;
using Entities.Interfaces;
using Entities.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UrlController : ControllerBase
    {
        private readonly ILogger<UrlController> _logger;
        private readonly IUrlService _urlService;

        public UrlController(IUrlService urlService, ILogger<UrlController> logger)
        {
            _urlService = urlService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int? page, int? pageSize)
        {
            try
            {
                var urls = await _urlService.GetAll(page, pageSize);
                return Ok(urls);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Get", new Object[] { page, pageSize } );
                throw; 
            }
        }

        [HttpGet("{shortened}")]
        public async Task<IActionResult> Get(string shortened)
        {
            try
            {
                var url = await _urlService.GetUrl(shortened);
                if (url == null)
                {
                    return NotFound();
                }

                return Ok(url);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Get 1", new Object[] { shortened });
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(UrlCreate url)
        {
            try
            {
                var result = await _urlService.AddUrl(url);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Post", new Object[] { url });
                throw;
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put(UrlDto url)
        {
            try
            {
                var result = await _urlService.UpdateUrl(url);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Put", new Object[] { url });
                throw;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _urlService.DeleteUrl(id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Delete", new Object[] { id });
                throw;
            }
        }
    }
}
