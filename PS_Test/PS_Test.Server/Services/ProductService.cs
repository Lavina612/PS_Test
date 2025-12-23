using PS_Test.Server.Interfaces;
using PS_Test.Server.Models;
using System.Data;
using System.Text.RegularExpressions;

namespace PS_Test.Server.Services
{
    public class ProductService : IProductService
    {
        private static readonly Regex _codeFormat = new Regex(@"^\d{2}-\d{4}-[A-Z]{2}\d{2}$", RegexOptions.Compiled);

        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public IEnumerable<ProductModel> GetAll()
        {
            var productEntities = _productRepository.GetAll();
            return productEntities.Select(x => x.ToProductModel());
        }

        public ProductModel? GetById(Guid id)
        {
            var productEntity = _productRepository.GetById(id);
            return productEntity?.ToProductModel();
        }

        public ProductModel? GetByCode(string code)
        {
            var productEntity = _productRepository.GetByCode(code);
            return productEntity?.ToProductModel();
        }

        public ProductModel? Add(ProductModel addingProduct)
        {
            if (!CheckValidity(addingProduct) || !CheckCodeUnique(addingProduct))
            {
                return null;
            }

            var addingEntity = addingProduct.ToProductEntity();
            _productRepository.Add(addingEntity);
            return addingEntity.ToProductModel();
        }

        public bool Update(ProductModel updatingProduct)
        {
            if (!CheckValidity(updatingProduct) || !CheckCodeUnique(updatingProduct))
            {
                return false;
            }

            var updatingEntity = updatingProduct.ToProductEntity();
            _productRepository.Update(updatingEntity);
            return true;
        }

        public void Delete(Guid id)
        {
            var productEntity = _productRepository.GetById(id);

            if (productEntity != null)
            {
                _productRepository.Delete(productEntity);
            }
        }

        private bool CheckValidity(ProductModel product)
        {
            return product != null
                && !string.IsNullOrWhiteSpace(product.Code)
                && _codeFormat.IsMatch(product.Code)
                && product.Price >= 0;
        }

        private bool CheckCodeUnique(ProductModel product)
        {
            var productWithSameCode = _productRepository.GetByCode(product.Code);
            return productWithSameCode == null || productWithSameCode.Id == product.Id;
        }
    }
}
