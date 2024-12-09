using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TapCleaner.Models
{
    public class UserQuery
    {
        [JsonIgnore][Key] public int Id { get; set; }
        [Required]
        public User User { get; set; }
        [Required]
        public string Query { get; set; }
    }
}
