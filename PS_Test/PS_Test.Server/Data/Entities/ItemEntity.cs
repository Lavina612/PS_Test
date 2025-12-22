namespace PS_Test.Server.Data.Entities
{
    public class ItemEntity
    {
        public Guid Id { get; set; }

        public Guid OrderId { get; set; }

        public OrderEntity Order { get; set; }

        public Guid ProductId { get; set; }

        public ProductEntity Product { get; set; }

        public int ItemsCount { get; set; }
    }
}
