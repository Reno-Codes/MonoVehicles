using System;
using System.Collections.Generic;

namespace MonoVehicles.Models
{
    public partial class VehicleMake
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Abrv { get; set; } = null!;
    }
}
