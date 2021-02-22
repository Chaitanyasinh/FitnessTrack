using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FItnessTrack.Models
{
    public class PersonalTraining
    {
        public int TrainingId { get; set; } // PK
        public String Desciption { get; set; }
        public float Charge { get; set; }
        public Service Service { get; set; }
        public int ServiceId { get; set; }
    }
}
