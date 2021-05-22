using System.Threading.Tasks;
using Clients.DataTransfer;

namespace Clients.Services
{
    public interface IAuthService
    {
        /// <summary>
        /// Аутентифицирует гостя Новороссийска
        /// </summary>
        Task<FullClientDto> AuthenticateGuest(string phoneNumber, string password);

        /// <summary>
        /// Аутентифицирует жителя Новороссийска (наивный мок ЕСИА)
        /// </summary>
        Task<FullClientDto> AuthenticateCitizen(string email, string password);
    }
}