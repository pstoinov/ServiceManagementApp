using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceManagementApp.Data.Models.Wherehouse
{
    public class Part
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
    }
}
