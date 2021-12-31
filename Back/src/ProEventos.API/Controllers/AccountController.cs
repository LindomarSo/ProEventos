using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProEventos.API.Extensions;
using ProEventos.API.Helpers;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;

namespace ProEventos.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ITokenService _tokenService;
        private readonly IUtils _utils;

        public AccountController(IAccountService accountService, ITokenService tokenService, IUtils utils)
        {
            _accountService = accountService;
            _tokenService = tokenService;
            _utils = utils;
        } 

        [HttpGet("getUser")] 
        public async Task<IActionResult> GetUser()
        {
            try
            {
                var userName = User.GetUserName();
                var user =  await _accountService.GetUserByUserNameAsync(userName);

                return Ok(user);
            }
            catch(Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, 
                    $"Erro ao tentar pegar o conta. Erro: {ex.Message}"
                );
            }
        } 

        [HttpPost("Register")] 
        [AllowAnonymous]
        public async Task<IActionResult> Rgister(UserUpdateDto userDto)
        {
            try
            {
                if(await _accountService.UserExistAsync(userDto.UserName))
                {
                    return BadRequest("O usuário já existe!");
                }

                var user = await _accountService.CreateAccountAsync(userDto);

                return (user != null) 
                                    ? Ok(new 
                                    {
                                        UserName = user.UserName,
                                        PrimeiroNome = user.PrimeiroNome,
                                        token = _tokenService.CreateToken(user).Result
                                    })
                                    : BadRequest("Usuário não criado tente novamente mais tarde");
            }
            catch(Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, 
                    $"Erro ao tentar cadastrar o usuário. Erro: {ex.Message}"
                );
            }
        } 

        [HttpPost("login")] 
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            try
            {
                var user =  await _accountService.GetUserByUserNameAsync(loginDto.Username);
                if(user == null)
                    return Unauthorized("Usuário ou senha inválido!");
                
                var result = await _accountService.CheckUserPasswordAsync(user, loginDto.Password);
                if(!result.Succeeded)
                {
                    return Unauthorized();
                }

                return Ok(new 
                {
                    UserName = user.UserName,
                    PrimeiroNome = user.PrimeiroNome,
                    token = _tokenService.CreateToken(user).Result
                });
            }
            catch(Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, 
                    $"Erro ao tentar realizar o login. Erro: {ex.Message}"
                );
            }
        } 

        [HttpPut("UpdateUser")] 
        public async Task<IActionResult> UpdateUser(UserUpdateDto userUpdateDto)
        {
            try
            {
                if(userUpdateDto.UserName != User.GetUserName())
                    return Unauthorized("Usuário inválido");

                var user = await _accountService.GetUserByUserNameAsync(User.GetUserName());

                if(user == null) return Unauthorized("Usuário inválido");

                var userRetorno = await _accountService.UpdateAccountAsync(userUpdateDto);

                return (userRetorno != null) 
                                    ? Ok(new {
                                        UserName = userRetorno.UserName,
                                        PrimeiroNome = userRetorno.PrimeiroNome,
                                        token = _tokenService.CreateToken(userRetorno).Result
                                    })
                                    : NoContent();
            }
            catch(Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, 
                    $"Erro ao tentar atualizar o usuário. Erro: {ex.Message}"
                );
            }
        } 

        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadImage()
        {
            try
            {
                var user = await _accountService.GetUserByUserNameAsync(User.GetUserName());

                if(user == null) return Unauthorized();

                var file = Request.Form.Files[0];
                if(file.Length > 0)
                {
                    if(user.ImageURL != null)
                    {
                        _utils.DeleteImage(user.ImageURL, "Perfil");
                    }

                    user.ImageURL = await _utils.SaveImage(file, "Perfil");

                    user = await _accountService.UpdateAccountAsync(user);
                }

                return Ok(user);
            }
            catch(Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro {ex.Message}");
            }
        }
    }
}