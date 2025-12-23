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

        public IEnumerable<ProductEntity> GetAll()
        {
            return _context.Products;
        }

        public ProductEntity? GetById(Guid id)
        {
            return _context.Products
                .Include(x => x.Items)
                .FirstOrDefault(x => x.Id == id);
        }

        public ProductEntity? GetByCode(string code)
        {
            return _context.Products
                .Include(x => x.Items)
                .AsNoTracking()
                .FirstOrDefault(x => x.Code == code);
        }

        public void Add(ProductEntity product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public void Update(ProductEntity product)
        {
            _context.Products.Update(product);
            _context.SaveChanges();
        }

        public void Delete(ProductEntity product)
        {
            _context.Products.Remove(product);
            _context.SaveChanges();
        }
    }
}
