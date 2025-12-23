using PS_Test.Server.Data.Entities;

namespace PS_Test.Server.Interfaces
{
    public interface IProductRepository
    {
        public IEnumerable<ProductEntity> GetAll();

        public ProductEntity? GetById(Guid id);

        public ProductEntity? GetByCode(string code);

        public void Add(ProductEntity addingProduct);

        public void Update(ProductEntity updatingProduct);

        public void Delete(ProductEntity product);
    }
}
