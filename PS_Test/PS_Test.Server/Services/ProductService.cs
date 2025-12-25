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

        public async Task<IEnumerable<ProductModel>> GetAll()
        {
            var productEntities = await _productRepository.GetAll();
            return productEntities.Select(x => x.ToProductModel());
        }

        public async Task<ProductModel?> GetById(Guid id)
        {
            var productEntity = await _productRepository.GetById(id);
            return productEntity?.ToProductModel();
        }

        public async Task<ProductModel?> GetByCode(string code)
        {
            var productEntity = await _productRepository.GetByCode(code);
            return productEntity?.ToProductModel();
        }

        public async Task<ProductModel?> Add(ProductModel addingProduct)
        {
            if (!CheckValidity(addingProduct) || !(await CheckCodeUnique(addingProduct)))
            {
                return null;
            }

            var addingEntity = addingProduct.ToProductEntity();
            await _productRepository.Add(addingEntity);
            return addingEntity.ToProductModel();
        }

        public async Task<bool> Update(ProductModel updatingProduct)
        {
            if (!CheckValidity(updatingProduct) || !(await CheckCodeUnique(updatingProduct)))
            {
                return false;
            }

            var updatingEntity = updatingProduct.ToProductEntity();
            await _productRepository.Update(updatingEntity);
            return true;
        }

        public async Task Delete(Guid id)
        {
            var productEntity = await _productRepository.GetById(id);

            if (productEntity != null)
            {
                await _productRepository.Delete(productEntity);
            }
        }

        private bool CheckValidity(ProductModel product)
        {
            return product != null
                && !string.IsNullOrWhiteSpace(product.Code)
                && _codeFormat.IsMatch(product.Code)
                && product.Price >= 0;
        }

        private async Task<bool> CheckCodeUnique(ProductModel product)
        {
            var productWithSameCode = await _productRepository.GetByCode(product.Code);
            return productWithSameCode == null || productWithSameCode.Id == product.Id;
        }
    }
}
