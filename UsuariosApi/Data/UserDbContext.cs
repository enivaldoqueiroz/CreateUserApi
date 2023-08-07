using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UsuariosApi.Models;

namespace UsuariosApi.Data
{
    public class UserDbContext : IdentityDbContext<User>
    {
        public UserDbContext(DbContextOptions<UserDbContext> dbContextOptions) : base(dbContextOptions)
        {

        }
    }
}
