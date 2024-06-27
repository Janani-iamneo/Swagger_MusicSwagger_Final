using System;
using System.ComponentModel.DataAnnotations;

namespace dotnetapp.Models
{
    public class PetAdoption
    {
        [Key]
        public int PetAdopterID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public int PetID { get; set; }
        public Pet Pet { get; set; }
    }
}
