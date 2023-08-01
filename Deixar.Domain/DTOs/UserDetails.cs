using Deixar.Domain.Entities;

namespace Deixar.Domain.DTOs
{
    public class UserDetails
    {
        public User User { get; set; } = null!;
        public string Role { get; set; } = null!;
    }
}
