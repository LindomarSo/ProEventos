using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;
using ProEventos.Domain.Identity;
using ProEventos.Persistence.Contratos;

namespace ProEventos.Application
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _useManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;
        private readonly IUserPersistence _userPersistence;

        public AccountService(UserManager<User> useManager,
                              SignInManager<User> signInManager,
                              IMapper mapper,
                              IUserPersistence userPersistence)
        {
            _mapper = mapper;
            _userPersistence = userPersistence;
            _signInManager = signInManager;
            _useManager = useManager;
        }

        public async Task<SignInResult> CheckUserPasswordAsync(UserUpdateDto userUpdateDto, string password)
        {
            try
            {
                var user = await  _useManager.Users
                                            .SingleOrDefaultAsync(user => user.UserName == userUpdateDto.UserName.ToLower());

                // O parâmetro booleano é passado como falso para que ele não bloquei o usuário caso não bata a senha.
                return await _signInManager.CheckPasswordSignInAsync(user, password, false);
            }
            catch(Exception ex)
            {
                throw new Exception($"Erro ao tentar a senha do usuário. Erro: {ex.Message}");
            }
        }

        public async Task<UserUpdateDto> CreateAccountAsync(UserUpdateDto userDto)
        {
            try
            {
                var user = _mapper.Map<User>(userDto);
                var result = await _useManager.CreateAsync(user, userDto.Password);

                if(result.Succeeded)
                {
                    var returnDto = _mapper.Map<UserUpdateDto>(user);
                    return returnDto;
                }

                return null;
            }
            catch(Exception ex)
            {
                throw new Exception($"Erro ao tentar criar conta. Erro: {ex.Message}");
            }
        }

        public async Task<UserUpdateDto> GetUserByUserNameAsync(string userName)
        {
            try
            {
                var user = await _userPersistence.GetUserByUserNameAsync(userName);

                if(user == null) return null;

                var userUpdateDto = _mapper.Map<UserUpdateDto>(user);
                return userUpdateDto;
            }
            catch(Exception ex)
            {
                throw new Exception($"Erro ao tentar buscar username. Erro: {ex.Message}");
            }
        }

        public async Task<UserUpdateDto> UpdateAccountAsync(UserUpdateDto userUpdateDto)
        {
            try
            {
                var user = await _userPersistence.GetUserByUserNameAsync(userUpdateDto.UserName);

                if(user is null) return null;

                userUpdateDto.Id = user.Id;

                _mapper.Map(userUpdateDto, user);

                if(userUpdateDto.Password != null)
                {
                    var token = await _useManager.GeneratePasswordResetTokenAsync(user);
                    var result = await _useManager.ResetPasswordAsync(user, token, userUpdateDto.Password);
                }

                _userPersistence.Update<User>(user);

                if(await _userPersistence.SaveChangesAsync())
                {
                    return _mapper.Map<UserUpdateDto>(await _userPersistence.GetUserByUserNameAsync(user.UserName));
                }

                return null;
            }
            catch(Exception ex)
            {
                throw new Exception($"Erro ao tentar atualizar o usuário. Erro: {ex.Message}");
            }
        }

        public async Task<bool> UserExistAsync(string username)
        {
            try
            {
                return await _useManager.Users
                                            .AnyAsync(user => user.UserName == username.ToLower());
            }
            catch(Exception ex)
            {
                throw new Exception($"Erro ao veficar existência do usuário. Erro: {ex.Message}");
            }
        }
    }
}