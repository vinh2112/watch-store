using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Do_An.Models
{
    public class UserModel
    {
        [Required]
        public string SDT { get; set; }
        public string Passowrd { get; set; }
        public string ReenterPassword { get; set; }
        public string HoTen { get; set; }
        public string DiaChi { get; set; }
        public string Email { get; set; }
        
    }
}