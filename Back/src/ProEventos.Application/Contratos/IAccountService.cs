using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ProEventos.Application.Dtos;

namespace ProEventos.Application.Contratos
{
    public interface IAccountService
    {
        Task<bool> UserExistAsync(string username);
        Task<UserUpdateDto> GetUserByUserNameAsync(string username);
        Task<SignInResult> CheckUserPasswordAsync(UserUpdateDto userUpdateDto, string password);
        Task<UserUpdateDto> CreateAccountAsync(UserUpdateDto userDto);
        Task<UserUpdateDto> UpdateAccountAsync(UserUpdateDto userUpdateDto);
    }
}