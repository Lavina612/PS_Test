using PS_Test.Server.Models;

namespace PS_Test.Server.Interfaces
{
    public interface IProductService
    {
        public IEnumerable<ProductModel> GetAll();

        public ProductModel? GetById(Guid id);

        public ProductModel? GetByCode(string code);

        public ProductModel? Add(ProductModel addingProduct);

        public bool Update(ProductModel updatingProduct);

        public void Delete(Guid id);
    }
}
