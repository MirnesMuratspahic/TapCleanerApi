using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TapCleaner.Models.DTO
{
    public class dtoContainer
    {
        [Required]
        public string Adress { get; set; }
        [Required]
        public string Coordinates { get; set; }
        [Required]
        public string Type { get; set; }
    }
}
