using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UsuariosApi.Models;

namespace UsuariosApi.Services
{
    public class TokenServices
    {
        public void GenerateToken(User user)
        {
            /*
             * Nessa parte, um array de objetos Claim está sendo criado.
             * Cada objeto Claim representa um pedaço de informação sobre o usuário que será incluído no token JWT. 
             * No caso, estão sendo adicionados três claims: um para o nome de usuário ("username"), outro para o ID do usuário ("id"), 
             * e o último para a data de nascimento (ClaimTypes.DateOfBirth), que é convertida para string.
             ***/
            Claim[] claims = new Claim[]
            {
                new Claim("username", user.UserName),
                new Claim("id", user.Id),
                new Claim(ClaimTypes.DateOfBirth, user.BirthDate.ToString())
            };

            /*
             * Aqui, uma chave de segurança simétrica está sendo criada a partir de uma sequência de bytes obtida a partir da string fornecida. 
             * Essa chave será usada para assinar o token JWT, garantindo sua integridade e autenticidade.
             ***/
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("1qaz@WSX3edc$RFV5tgb"));

            /*
             * Uma instância de SigningCredentials está sendo criada, combinando a chave de segurança criada anteriormente com o algoritmo de 
             * assinatura HMAC-SHA256. 
             * Isso define como o token será assinado para que ele possa ser verificado posteriormente.
             ***/
            var signingCledentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            /*         
             * Aqui, o token JWT está sendo construído. São fornecidos três parâmetros:
             *
             *  expires: Define o momento em que o token expirará. Nesse caso, está definido para expirar 10 minutos após a criação (tempo atual + 10 minutos).
             *  claims: Os claims (informações) sobre o usuário que foram criados anteriormente.
             *  signingCredentials: As credenciais de assinatura, contendo a chave de segurança e o algoritmo de assinatura.
             ***/
            var token = new JwtSecurityToken(expires: DateTime.Now.AddMinutes(10),
                                             claims: claims,
                                             signingCredentials: signingCledentials);
        }
    }
}
