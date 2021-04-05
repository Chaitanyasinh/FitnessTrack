using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FItnessTrack.Models
{
    public class Cart
    {
        public int CartId { get; set; } //PK

        public int ServiceId { get; set; }

        public DateTime DateCreated { get; set; }

        public int Quantity { get; set; }

        public double Charge { get; set; }
        public string CustomerId { get; set; }
        public Service Service { get; set; }
    }
}
