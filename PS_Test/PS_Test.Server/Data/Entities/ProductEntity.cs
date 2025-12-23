using PS_Test.Server.Models;

namespace PS_Test.Server.Data.Entities
{
    public class ProductEntity
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public int Price { get; set; }

        public string Category { get; set; }

        public List<ItemEntity> Items { get; set; }

        protected ProductEntity() { }

        public ProductEntity(Guid id, string code, string name, int price, string category, List<ItemEntity> items)
        {
            Id = id;
            Code = code;
            Name = name;
            Price = price;
            Category = category;
            Items = items;
        }

        public ProductModel ToProductModel()
        {
            return new ProductModel(Id, Code, Name, Price, Category, Items);
        }
    }
}
