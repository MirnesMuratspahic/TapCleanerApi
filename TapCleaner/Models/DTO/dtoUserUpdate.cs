namespace TapCleaner.Models.DTO
{
    public class dtoUserUpdate
    {
        public string FirstName { get; set; } =string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
    }
}
