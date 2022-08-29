using Catalog.API.Entities;
using Catalog.API.Repoistories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly IProductRepoistory _productRepoistory;
        private readonly ILogger<CatalogController> _logger;

        public CatalogController(IProductRepoistory productRepoistory,ILogger<CatalogController> logger)
        {
            _productRepoistory = productRepoistory ?? throw new ArgumentNullException(nameof(productRepoistory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>),(int)HttpStatusCode.OK]
        public ActionResult<Task<IEnumerable<Product>>> GetProducts()
        {
            var products = _productRepoistory.GetProducts();
            return Ok(products);
        }

        [HttpGet("{id:length(24)}",Name ="GetProduct")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> GetProductsById(string id)
        {
            var products = await _productRepoistory.GetProductById(id);

            if(products == null)
            {
                _logger.LogError($"Product with id: {id}, not found.");
                return NotFound();
            }
            return Ok(products);
        }

        [Route("[action]/{category}")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>),(int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByCategory(string category)
        {
            var products = await _productRepoistory.GetProductByCategory(category);
            return Ok(products);
        }
    }
}
