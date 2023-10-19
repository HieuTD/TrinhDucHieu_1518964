namespace WebApi.DTOs.Carts
{
    public class CartDeleteRequest
    {
        public string UserId { get; set; }
        public int ProdVariantId { get; set; }
    }
}
