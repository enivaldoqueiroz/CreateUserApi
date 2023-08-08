using AutoMapper;
using Microsoft.AspNetCore.Identity;
using UsuariosApi.Data.DTOs;
using UsuariosApi.Models;

namespace UsuariosApi.Services
{
    public class RegisterServices
    {
        private IMapper _mapper;
        private UserManager<User> _userManager;

        public RegisterServices(IMapper mapper, UserManager<User> userManager)
        {
            _mapper = mapper;
            _userManager = userManager;
        }

        internal async Task RegisterAsync(CreateUserDTO createUserDTO)
        {
            User user = _mapper.Map<User>(createUserDTO);

            IdentityResult result = await _userManager.CreateAsync(user, createUserDTO.Password);

            if (!result.Succeeded)
                throw new ApplicationException("Falha ao cadastrar usuário!");
        }
    }
}
