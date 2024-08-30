using System.ComponentModel.DataAnnotations.Schema;

namespace TapCleaner.Models
{
    [NotMapped]
    public class ErrorProvider
    {
        public string Name { get; set; }
        public bool Status { get; set; } = false;
    }
}
