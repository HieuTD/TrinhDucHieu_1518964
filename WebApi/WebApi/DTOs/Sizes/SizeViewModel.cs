namespace WebApi.DTOs.Sizes
{
    public class SizeViewModel
    {
        public int Id { get; set; }
        public string SizeName { get; set; }
        public int? CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
