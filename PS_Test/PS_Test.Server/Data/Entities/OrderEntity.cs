using System.ComponentModel.DataAnnotations.Schema;

namespace PS_Test.Server.Data.Entities
{
    public class OrderEntity
    {
        public Guid Id { get; set; }

        public Guid CustomerId { get; set; }

        public CustomerEntity Customer { get; set; }

        public DateTime OrderDate { get; set; }
        
        public DateTime? ShipmentDate { get; set; }

        public int OrderNumber { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.New;

        public List<ItemEntity> Items { get; set; }
    }
}
