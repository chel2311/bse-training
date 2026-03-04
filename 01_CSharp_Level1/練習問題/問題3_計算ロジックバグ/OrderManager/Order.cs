namespace OrderManager
{
    public class Order
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal TaxRate { get; set; }
        public int Subtotal { get; set; }

        public Order(int id, string productName, int unitPrice, int quantity, decimal taxRate, int subtotal)
        {
            Id = id;
            ProductName = productName;
            UnitPrice = unitPrice;
            Quantity = quantity;
            TaxRate = taxRate;
            Subtotal = subtotal;
        }
    }
}
