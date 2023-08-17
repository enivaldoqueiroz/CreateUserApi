using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UsuariosApi.Authorization;
using UsuariosApi.Data;
using UsuariosApi.Models;
using UsuariosApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

#region DbContext - Acessando o banco de dados

var connString = builder.Configuration["ConnectionStrings:DefaultConnection"];

//DbContext - Acessando o banco de dados MySql
//builder.Services.AddDbContext<UserDbContext> (dbContextOptions => 
//{
//    dbContextOptions.UseMySql 
//        (connectionString: builder.Configuration.GetConnectionString("UserConnection"),
//         ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("UserConnection")));
//});

// DbContext - Acessando o banco de dados Sql Server
builder.Services.AddDbContext<UserDbContext>(dbContextOptions =>
{
    dbContextOptions.UseSqlServer(
        connectionString: connString);
});

builder.Services
    .AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<UserDbContext>()
    .AddDefaultTokenProviders();//Usado para Autentica��o

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//Defini��es da palavra-passe predefinidas.
builder.Services.Configure<IdentityOptions>(options =>
{
    // Default Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 10;
    options.Password.RequiredUniqueChars = 1;
});

/*  Obs.:
    AddScoped: Cria uma inst�ncia �nica para cada escopo de requisi��o dentro da mesma requisi��o HTTP, sendo reutilizada ao longo do processamento.
    AddTransient: Gera uma nova inst�ncia a cada solicita��o, ideal para servi�os leves e ef�meros, sem necessidade de compartilhamento.
    AddSingleton: Cria uma �nica inst�ncia durante todo o ciclo de vida da aplica��o, compartilhada entre todas as requisi��es, adequada para servi�os globais e constantes.
 */

builder.Services.AddScoped<UserServices>();
builder.Services.AddScoped<TokenServices>();
builder.Services.AddSingleton<IAuthorizationHandler,IdadeAuthorization>();

/*
 O trecho de c�digo que voc� forneceu est� configurando pol�ticas de autoriza��o para sua aplica��o usando o servi�o de autoriza��o no ASP.NET Core. Essas pol�ticas controlam o acesso a determinados recursos com base em requisitos espec�ficos. Vou explicar em detalhes o que cada linha faz:
1. `builder.Services`: Aqui, voc� est� acessando o cont�iner de servi�os (IServiceCollection) para adicionar configura��es relacionadas � autoriza��o.
2. `.AddAuthorization(options => ...)`: Isso adiciona a configura��o para o servi�o de autoriza��o � cole��o de servi�os. A fun��o de callback dentro deste m�todo permite que voc� configure as pol�ticas de autoriza��o.
3. `options.AddPolicy("IdadeMinima", policy => policy.AddRequirements(new IdadeMinima(18)));`: Aqui voc� est� adicionando uma pol�tica de autoriza��o chamada "IdadeMinima". Isso significa que voc� est� definindo uma pol�tica personalizada para verificar se o usu�rio tem pelo menos 18 anos de idade para acessar determinados recursos.
- `"IdadeMinima"`: � o nome da pol�tica de autoriza��o que voc� est� criando.
- `policy => policy.AddRequirements(new IdadeMinima(18))`: Aqui, voc� est� adicionando um requisito � pol�tica. O requisito � definido por meio da classe `IdadeMinima` que provavelmente implementa a interface `IAuthorizationRequirement`. Essa classe � usada para definir os crit�rios necess�rios para satisfazer a pol�tica de autoriza��o. Neste caso, o requisito `IdadeMinima` � instanciado com o valor m�nimo de idade de 18 anos.
Com essa configura��o em vigor, sempre que voc� desejar proteger um recurso e exigir que o usu�rio tenha pelo menos 18 anos de idade, voc� pode aplicar a pol�tica "IdadeMinima" ao recurso, fazendo com que o sistema verifique automaticamente se o usu�rio atende a esse requisito antes de permitir o acesso. Note que voc� tamb�m precisar� implementar um manipulador de autoriza��o para a classe `IdadeMinima` para que a l�gica de verifica��o seja executada durante a autoriza��o.
*/
builder.Services.AddAuthorization(options => 
{
    options.AddPolicy("IdadeMinima", policy => 
        policy.AddRequirements(new IdadeMinima(18))
    );
});

/*
 O c�digo que voc� forneceu est� configurando a autentica��o por meio de tokens JWT (JSON Web Token) em uma aplica��o ASP.NET Core. Isso permite que os usu�rios se autentiquem usando tokens JWT para acessar recursos protegidos. Vou descrever cada parte do c�digo:

Aqui est� o que cada parte do c�digo faz:

1. `builder.Services.AddAuthentication(options => ...`: Isso adiciona a configura��o para o servi�o de autentica��o � cole��o de servi�os. O m�todo de callback dentro deste bloco permite que voc� configure op��es de autentica��o.
2. `options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;`: Aqui voc� est� definindo o esquema de autentica��o padr�o como `JwtBearerDefaults.AuthenticationScheme`, o que significa que a autentica��o ser� feita usando o esquema de token JWT (Bearer).
3. `.AddJwtBearer(options => ...`: Isso configura o esquema de autentica��o `JwtBearer`. Ele define as op��es de valida��o do token JWT.
4. `options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters`: Aqui voc� est� configurando os par�metros de valida��o para o token JWT.
- `ValidateIssuerSigningKey = true`: Define se a chave de assinatura do emissor ser� validada. No caso, est� definida como verdadeira, o que significa que a chave de assinatura ser� validada.

- `IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("1qaz@WSX3edc$RFV5tgb"))`: Aqui voc� est� especificando a chave de assinatura usada para validar os tokens. A chave sim�trica � usada para verificar a autenticidade do token.

- `ValidateAudience = false`: Define se o p�blico-alvo (audi�ncia) ser� validado. Est� definido como falso, o que significa que a valida��o da audi�ncia ser� desativada.

- `ValidateIssuer = false`: Define se o emissor do token ser� validado. Est� definido como falso, o que significa que a valida��o do emissor ser� desativada.

- `ClockSkew = TimeSpan.Zero`: Define o valor de "ClockSkew" como zero, o que significa que a toler�ncia para a validade do tempo do token � zero, ou seja, o token deve estar exatamente dentro do prazo especificado.

No geral, esse c�digo configura o sistema de autentica��o para usar tokens JWT, definindo op��es de valida��o para verificar a autenticidade e validade dos tokens durante o processo de autentica��o. Isso permite que os usu�rios se autentiquem usando tokens JWT para acessar recursos protegidos na aplica��o. 
 */
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["SymmetricSecurityKey"])),
        ValidateAudience = false,
        ValidateIssuer = false,
        ClockSkew = TimeSpan.Zero,
    };
});
#endregion

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
