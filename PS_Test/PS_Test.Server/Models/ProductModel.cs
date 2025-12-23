using PS_Test.Server.Data.Entities;

namespace PS_Test.Server.Models
{
    public class ProductModel
    {
        public Guid? Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public int Price { get; set; }

        public string Category { get; set; }

        public List<ItemModel>? Items { get; set; }

        public ProductModel() { }

        public ProductModel(Guid id, string code, string name, int price, string category, List<ItemEntity> items)
        {
            Id = id;
            Code = code;
            Name = name;
            Price = price;
            Category = category;
            Items = new List<ItemModel>();
            //Items = itemEntities.Select(x => x.ToItemModel()).ToList();
        }

        public ProductEntity ToProductEntity()
        {
            var itemEntities = new List<ItemEntity>();
            //var itemEntities = Items.Select(x => x.ToItemEntity());
            return new ProductEntity(Id ?? Guid.Empty, Code, Name, Price, Category, itemEntities);
        }
    }
}
