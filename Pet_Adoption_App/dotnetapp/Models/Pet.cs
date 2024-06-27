// Pet.cs
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace dotnetapp.Models
{
    public class Pet
    {
        public int PetID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int Age { get; set; }
        public bool Availability { get; set; }
        public List<PetAdopter> PetAdopters { get; set; }
    }
}
