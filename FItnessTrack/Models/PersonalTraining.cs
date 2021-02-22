using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FItnessTrack.Models
{
    public class PersonalTraining
    {
        [Key]
        public int TrainingId { get; set; } // PK
        public String Desciption { get; set; }
        public Double Charge { get; set; }
        public Service Service { get; set; }
        public int ServiceId { get; set; }
    }
}
