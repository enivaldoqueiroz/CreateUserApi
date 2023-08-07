using Microsoft.AspNetCore.Identity;

namespace UsuariosApi.Models
{
    public class User : IdentityUser
    {
        public DateTime BirthDate { get; set; }

        public User() : base() { }
    }
}
