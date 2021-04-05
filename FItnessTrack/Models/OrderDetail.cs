using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FItnessTrack.Models
{
    public class OrderDetail
    {
        public int OrderDetailId { get; set; } //PK
        public int ServiceId { get; set; } //FK
        public int OrderId { get; set; } //FK
        public int Quantity { get; set; }
        public double Charge { get; set; }

        public Order Order { get; set; }

        public Service Service { get; set; }

    }
}
