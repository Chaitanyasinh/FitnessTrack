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
        public string FirstNameme { get; set; }
        public string LastNameme{ get; set; }
        public string ServiceName { get; set; }
        public List<PersonalTraining> Training { get; set; }

       
    }
}
