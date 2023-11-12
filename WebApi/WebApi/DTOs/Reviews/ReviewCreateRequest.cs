using System;

namespace WebApi.DTOs.Reviews
{
    public class ReviewCreateRequest
    {
        public string UserId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int ProdId { get; set; }
        public string Content { get; set; }
        public int Rating { get; set; }
    }
}
