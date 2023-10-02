﻿using Microsoft.AspNetCore.Identity;
using System.Collections;
using System.Collections.Generic;

namespace WebApi.Models
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public  ICollection<AuthHistory> AuthHistorys{ get; set; }
        public  ICollection<Blog> Blogs{ get; set; }
        public  ICollection<Cart> Carts { get; set; }
        public  ICollection<Order> Orders { get; set; }
        public  ICollection<Receipt> Receipts{ get; set; }
        public  ICollection<ProductLike> ProductLikes{ get; set; }
    }
}
