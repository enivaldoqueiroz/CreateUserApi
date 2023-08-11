using Microsoft.AspNetCore.Authorization;

namespace UsuariosApi.Authorization
{
    /*
     O código que você forneceu define uma classe chamada `IdadeMinima` que implementa a interface `IAuthorizationRequirement`. Essa classe é usada para criar um requisito de autorização personalizado que pode ser aplicado em políticas de autorização para verificar a idade mínima de um usuário. Vou descrever o código em detalhes:
        Aqui está o que cada parte do código faz:
    1. `public class IdadeMinima : IAuthorizationRequirement`: Você está definindo uma classe chamada `IdadeMinima` que implementa a interface `IAuthorizationRequirement`. Essa interface é usada para definir os requisitos de autorização personalizados que serão aplicados nas políticas de autorização.
    2. `public IdadeMinima(int idade)`: Este é o construtor da classe `IdadeMinima`. Ele aceita um parâmetro `idade` do tipo `int`, que é usado para definir a idade mínima necessária para acessar um recurso protegido.
    3. `Idade = idade;`: Neste trecho, o valor do parâmetro `idade` passado para o construtor é atribuído à propriedade `Idade`. Isso permite que a classe mantenha internamente o valor da idade mínima necessária para a autorização.
    4. `public int Idade { get; set; }`: Aqui, você está declarando uma propriedade pública chamada `Idade` do tipo `int`. Essa propriedade permite que o valor da idade mínima seja lido e definido externamente.
    Essa classe `IdadeMinima` pode ser usada como um requisito em uma política de autorização, permitindo que você defina a idade mínima necessária para acessar um recurso. Quando uma solicitação de acesso a um recurso protegido é feita, a política de autorização pode verificar se o usuário atende ao requisito de idade mínima usando esse objeto `IdadeMinima` personalizado.
    */
    public class IdadeMinima : IAuthorizationRequirement
    {
        public IdadeMinima(int idade)
        {
            Idade = idade;
        }

        public int Idade { get; set; }
    }
}
