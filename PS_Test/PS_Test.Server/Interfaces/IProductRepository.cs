using PS_Test.Server.Data.Entities;

namespace PS_Test.Server.Interfaces
{
    public interface IProductRepository
    {
        public Task<List<ProductEntity>> GetAll();

        public Task<ProductEntity?> GetById(Guid id);

        public Task<ProductEntity?> GetByCode(string code);

        public Task Add(ProductEntity addingProduct);

        public Task Update(ProductEntity updatingProduct);

        public Task Delete(ProductEntity product);
    }
}
