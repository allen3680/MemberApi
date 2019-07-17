using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gotcha1.Models
{
    public class Member
    {
        //[Required(ErrorMessage = "Id is required.")]
        public string Id { get; set; }
        [MaxLength(10, ErrorMessage = "over10")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Birthday is required.")]
        public string Birthday { get; set; }
        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; }
        public string LastUpdateTime { get; set; }
    }
}
