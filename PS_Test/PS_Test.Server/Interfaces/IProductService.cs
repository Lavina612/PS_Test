using PS_Test.Server.Models;

namespace PS_Test.Server.Interfaces
{
    public interface IProductService
    {
        public Task<IEnumerable<ProductModel>> GetAll();

        public Task<ProductModel?> GetById(Guid id);

        public Task<ProductModel?> GetByCode(string code);

        public Task<ProductModel?> Add(ProductModel addingProduct);

        public Task<bool> Update(ProductModel updatingProduct);

        public Task Delete(Guid id);
    }
}
