using System.ComponentModel.DataAnnotations;

namespace ServiceManagementApp.Data.Models.Core
{
    public class Address
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(100)]
        public string? City { get; set; } = string.Empty;
        [MaxLength(255)]

        public string? Street { get; set; } = string.Empty;
        [MaxLength(5)]
        public string? Number { get; set; } = string.Empty;
        [MaxLength(5)]
        public string? Block { get; set; } = string.Empty;

    }
}
