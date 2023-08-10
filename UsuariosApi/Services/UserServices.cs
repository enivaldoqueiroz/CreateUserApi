using AutoMapper;
using Microsoft.AspNetCore.Identity;
using UsuariosApi.Data.DTOs;
using UsuariosApi.Models;

namespace UsuariosApi.Services
{
    public class UserServices
    {
        private IMapper _mapper;
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;//O SignInManager é uma classe oferecida pelo ASP.NET Identity, que é uma estrutura de autenticação e gerenciamento de identidade para aplicativos web. Ele gerencia o processo de autenticação do usuário, como login e logout, e fornece métodos para lidar com a identidade do usuário, como redefinição de senha e confirmação de email. O SignInManager é usado para facilitar a autenticação do usuário e a criação de cookies de autenticação para permitir que os usuários acessem partes protegidas do aplicativo.
        private TokenServices _tokenService;

        public UserServices(IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager, TokenServices tokenService)
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        public async Task<string> Login(LoginUserDTO loginUserDTO)
        {
            var result = await _signInManager.PasswordSignInAsync(loginUserDTO.Username, loginUserDTO.Password, isPersistent: false, lockoutOnFailure: false);
            
            if (!result.Succeeded) 
                throw new ApplicationException("Usuário não autenticado!");

            var user = _signInManager.UserManager.Users.FirstOrDefault(user => user.NormalizedUserName == loginUserDTO.Username.ToUpper());

            var token = _tokenService.GenerateToken(user);

            return token;
        }

        public async Task RegisterAsync(CreateUserDTO createUserDTO)
        {
            User user = _mapper.Map<User>(createUserDTO);

            IdentityResult result = await _userManager.CreateAsync(user, createUserDTO.Password);

            if (!result.Succeeded)
                throw new ApplicationException("Falha ao cadastrar usuário!");
        }
    }
}
