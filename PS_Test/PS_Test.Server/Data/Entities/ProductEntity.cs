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
    }
}
