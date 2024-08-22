using ServiceManagementApp.Data.Models.Wherehouse;
using System.ComponentModel.DataAnnotations;

namespace ServiceManagementApp.Data.Models.RepairModels
{
    public class RepairPart
    {
        public int Id { get; set; }

        [Required]
        public int RepairId { get; set; }
        public Repair Repair { get; set; } = null!;

        [Required]
        public int PartId { get; set; }
        public Part Part { get; set; } = null!;

        [Required]
        public int Quantity { get; set; }
    }
}
