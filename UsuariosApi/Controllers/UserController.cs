using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using UsuariosApi.Data.DTOs;
using UsuariosApi.Models;
using UsuariosApi.Services;

namespace UsuariosApi.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class UserController : Controller
    {
        private UserServices _userServices;

        public UserController(UserServices registerServices)
        {
            _userServices = registerServices;
        }

        [HttpPost("Cadastro")]
        public async Task<IActionResult> CadastroUsuario(CreateUserDTO createUserDTO)
        {
            await _userServices.RegisterAsync(createUserDTO);

            return Ok("Usuário Cadastrado!");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync(LoginUserDTO loginUserDTO)
        {
            await _userServices.Login(loginUserDTO);

            return Ok("Usuário autenticado!");
        }
    }
}
