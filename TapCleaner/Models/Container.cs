using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TapCleaner.Models
{
    public class Container
    {
        [JsonIgnore][Key] public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Adress { get; set; }
        [Required]
        public string Coordinates { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public string Condition { get; set; }
        [Required]
        public int NumberOfReports { get; set; }
    }
}
