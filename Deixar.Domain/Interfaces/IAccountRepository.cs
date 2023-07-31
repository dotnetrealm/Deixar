using Deixar.Domain.DTOs;

namespace Deixar.Domain.Interfaces
{
    public interface IAccountRepository
    {
        /// <summary>
        /// Get user information by email address and password
        /// </summary>
        /// <param name="email">The user email address.</param>
        /// <param name="password">The email Password</param>
        /// <returns>User object</returns>
        Task<UserDetails?> GetUserByEmailPasswordAsync(string email, string password);
    }
}
