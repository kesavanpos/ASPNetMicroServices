using Catalog.API.Data;
using Catalog.API.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.Repoistories
{
    public class ProductRepoistory : IProductRepoistory
    {
        private readonly ICatalogContext _context;

        public ProductRepoistory(ICatalogContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _context.
                            products
                            .Find(p => true)
                            .ToListAsync();
        }

        public async Task<Product> GetProductById(string id)
        {
            return await _context.
                            products
                            .Find(p => p.ProductId == id)
                            .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByName(string name)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Name, name);

            return await _context.
                            products
                            .Find(filter)
                            .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByCategory(string categoryName)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Category, categoryName);

            return await _context.
                            products
                            .Find(filter)
                            .ToListAsync();
        }

        public async Task Create(Product product)
        {
            await _context.products.InsertOneAsync(product);
        }

        public async Task<bool> Update(Product product)
        {
            var updateResult = await _context
                            .products
                            .ReplaceOneAsync(filter: g => g.ProductId == product.ProductId, replacement: product);

            return updateResult.IsAcknowledged
                    && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> Delete(string Id)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.ProductId, Id);

            DeleteResult deleteResult = await _context
                                                .products
                                                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged
                    && deleteResult.DeletedCount > 0;
        }
    }
}
