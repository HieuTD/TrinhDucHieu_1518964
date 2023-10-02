namespace WebApi.DTOs.Colors
{
    public class ColorViewModel
    {
        public int Id { get; set; }
        public string ColorName { get; set; }
        public int? CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
