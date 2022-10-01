using System;
using System.Collections.Generic;

namespace MonoVehicles.Models
{
    public partial class VehicleModel
    {
        public int Id { get; set; }
        public string MakeId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Abrv { get; set; } = null!;
    }
}
