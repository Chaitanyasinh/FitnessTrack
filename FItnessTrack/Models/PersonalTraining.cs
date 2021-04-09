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

        [DisplayFormat(DataFormatString ="{0:c}")]
        [Range(0.01,999999)]
        public double Charge { get; set; }

        [Display(Name = "Time (Months)")]

        public int ServiceId { get; set; }
        [Required]

        public Service Service { get; set; }
    }
}
