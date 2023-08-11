using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UsuariosApi.Controllers
{
    /*
     O código que você forneceu define um controlador chamado `AccessController` em uma aplicação ASP.NET Core. Esse controlador possui um endpoint HTTP GET que é protegido por uma política de autorização chamada "IdadeMinima". Vou descrever cada parte do código:
    Aqui está o que cada parte do código faz:
    1. `[ApiController]`: Isso é um atributo que indica que a classe `AccessController` é um controlador da Web API. Ele fornece comportamentos específicos para controladores de API, como a automática validação de modelos e a formatação automática de respostas.
    2. `[Route("[Controller]")]`: Isso é um atributo de rota que define o padrão de rota para o controlador. Nesse caso, o padrão de rota será baseado no nome do controlador, ou seja, os endpoints terão a rota base definida por "[Controller]".
    3. `public class AccessController : ControllerBase`: Aqui você está definindo uma classe chamada `AccessController` que herda de `ControllerBase`, a qual é a classe base para os controladores em ASP.NET Core.
    4. `[HttpGet]`: Este é um atributo de rota que define que o método `Get()` responderá a solicitações HTTP GET.
    5. `[Authorize(Policy = "IdadeMinima")]`: Este é um atributo de autorização que protege o endpoint. Ele especifica que a política de autorização a ser aplicada é a política "IdadeMinima". Isso significa que antes de permitir o acesso ao método `Get()`, o sistema verificará se o usuário atende ao requisito de idade mínima definido pela política.
    6. `public ActionResult Get()`: Este é o método de ação que será executado quando uma solicitação HTTP GET for feita para o endpoint. Ele retorna um `ActionResult`, que é a base para retornar respostas HTTP. Neste caso, está retornando uma resposta "Ok" com a mensagem "Acesso permitido!".
    No geral, este código define um controlador de API chamado `AccessController` com um endpoint GET protegido por uma política de autorização que verifica a idade mínima do usuário antes de permitir o acesso. Se o usuário atender ao requisito, receberá a mensagem "Acesso permitido!" como resposta. Caso contrário, a autorização falhará e o acesso será negado.
     */
    [ApiController]
    [Route("[Controller]")]
    public class AccessController : ControllerBase
    {
        [HttpGet]
        [Authorize(Policy = "IdadeMinima")]
        public ActionResult Get() 
        { 
            return Ok("Acesso permitido!");
        }
    }
}
