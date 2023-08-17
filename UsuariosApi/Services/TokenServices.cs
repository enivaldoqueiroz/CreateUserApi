using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UsuariosApi.Models;

namespace UsuariosApi.Services
{
    public class TokenServices
    {
        private IConfiguration _configuration;

        public TokenServices(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(User user)
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
                new Claim(ClaimTypes.DateOfBirth, user.BirthDate.ToString()),
                new Claim("loginTimeStamp", DateTime.UtcNow.ToString())
            };

            /*
             * Aqui, uma chave de segurança simétrica está sendo criada a partir de uma sequência de bytes obtida a partir da string fornecida. 
             * Essa chave será usada para assinar o token JWT, garantindo sua integridade e autenticidade.
             ***/
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SymmetricSecurityKey"]));

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

            /*
             A linha de código return new JwtSecurityTokenHandler().WriteToken(token); é responsável por transformar o objeto JwtSecurityToken em uma string JWT válida, que pode ser enviada como parte de uma resposta de autenticação. 
            Vou explicar em detalhes o que essa linha faz:
            1.  new JwtSecurityTokenHandler(): Aqui, é criada uma instância do 
                JwtSecurityTokenHandler, que é uma classe fornecida pelo namespace 
                System.IdentityModel.Tokens.Jwt. 
            Essa classe é usada para lidar com a criação e manipulação de tokens JWT.
            
            2.  WriteToken(token): O método WriteToken é invocado na instância do 
                JwtSecurityTokenHandler, passando o objeto JwtSecurityToken criado anteriormente como argumento. 
            Esse método tem como objetivo serializar o token JWT em sua representação de string.

            3. return: A string JWT resultante é então retornada a partir da função que encapsula esse trecho de código, 
            tornando-a disponível para ser utilizada na aplicação.

            Portanto, a linha completa está convertendo o objeto JwtSecurityToken em uma string no formato JWT, permitindo que ela seja incorporada em uma resposta, por exemplo, para autenticação de usuários em um sistema.
             */
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
