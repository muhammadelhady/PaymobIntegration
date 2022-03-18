namespace TestPaymob.Dtos
{
    public class CreateOrderRequestDto
    {
        public string? auth_token { get; set; }
        public double amount_cents { get; set; }
        public string? currency { get; set; }
        public string? terminal_id { get; set; }
        public int merchant_order_id { get; set; }
        public List<ItemDto>?items { get; set; }

    }
    public class ItemDto
    {
     

        public string? name { get; set; }
        public double amount_cents { get; set; }
        public string? description { get; set; }
        public int quantity { get; set; }

    }
}
