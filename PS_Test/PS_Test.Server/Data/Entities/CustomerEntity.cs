namespace PS_Test.Server.Data.Entities
{
    public class CustomerEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public string? Address { get; set; }

        public int? Discount { get; set; }

        public List<OrderEntity> Orders { get; set; }
    }
}