using Microsoft.AspNetCore.Mvc;
using UsuariosApi.Data.DTOs;

namespace UsuariosApi.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class UsuarioController : Controller
    {
        [HttpPost]
        public IActionResult CadastroUsuario(CreateUserDTO createUserDTO)
        {
            throw new NotImplementedException();
        }
    }
}
