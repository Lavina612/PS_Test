using Microsoft.EntityFrameworkCore;
using PS_Test.Server.Data.Entities;
using PS_Test.Server.Interfaces;

namespace PS_Test.Server.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IApplicationDbContext _context;

        public ProductRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductEntity>> GetAll()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<ProductEntity?> GetById(Guid id)
        {
            return await _context.Products
                .Include(x => x.Items)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<ProductEntity?> GetByCode(string code)
        {
            return await _context.Products
                .Include(x => x.Items)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Code == code);
        }

        public async Task Add(ProductEntity product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task Update(ProductEntity product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(ProductEntity product)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
}
