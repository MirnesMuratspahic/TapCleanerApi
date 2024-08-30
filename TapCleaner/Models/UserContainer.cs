using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TapCleaner.Models
{
    public class UserContainer
    {
        [JsonIgnore][Key]public int Id { get; set; }
        [Required]
        public User User { get; set; }
        [Required]
        public Container Container { get; set; }
        [Required]
        public DateTime Date {  get; set; }

    }
}
