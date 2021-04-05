using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FItnessTrack.Models
{
    public class Service 
    {
        [Key]
        public int ServiceId { get; set; } //Primary Key
        [Required]
        public string FirstName { get; set; }
        public string LastName{ get; set; }
        public string ServiceName { get; set; }
        public double Charge { get; set; }
        public List<Cart> Carts { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
        public List<PersonalTraining> Training { get; set; }

       
    }
}
