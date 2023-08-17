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
    .AddDefaultTokenProviders();//Usado para Autenticação

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//Definições da palavra-passe predefinidas.
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
    AddScoped: Cria uma instância única para cada escopo de requisição dentro da mesma requisição HTTP, sendo reutilizada ao longo do processamento.
    AddTransient: Gera uma nova instância a cada solicitação, ideal para serviços leves e efêmeros, sem necessidade de compartilhamento.
    AddSingleton: Cria uma única instância durante todo o ciclo de vida da aplicação, compartilhada entre todas as requisições, adequada para serviços globais e constantes.
 */

builder.Services.AddScoped<UserServices>();
builder.Services.AddScoped<TokenServices>();
builder.Services.AddSingleton<IAuthorizationHandler,IdadeAuthorization>();

/*
 O trecho de código que você forneceu está configurando políticas de autorização para sua aplicação usando o serviço de autorização no ASP.NET Core. Essas políticas controlam o acesso a determinados recursos com base em requisitos específicos. Vou explicar em detalhes o que cada linha faz:
1. `builder.Services`: Aqui, você está acessando o contêiner de serviços (IServiceCollection) para adicionar configurações relacionadas à autorização.
2. `.AddAuthorization(options => ...)`: Isso adiciona a configuração para o serviço de autorização à coleção de serviços. A função de callback dentro deste método permite que você configure as políticas de autorização.
3. `options.AddPolicy("IdadeMinima", policy => policy.AddRequirements(new IdadeMinima(18)));`: Aqui você está adicionando uma política de autorização chamada "IdadeMinima". Isso significa que você está definindo uma política personalizada para verificar se o usuário tem pelo menos 18 anos de idade para acessar determinados recursos.
- `"IdadeMinima"`: É o nome da política de autorização que você está criando.
- `policy => policy.AddRequirements(new IdadeMinima(18))`: Aqui, você está adicionando um requisito à política. O requisito é definido por meio da classe `IdadeMinima` que provavelmente implementa a interface `IAuthorizationRequirement`. Essa classe é usada para definir os critérios necessários para satisfazer a política de autorização. Neste caso, o requisito `IdadeMinima` é instanciado com o valor mínimo de idade de 18 anos.
Com essa configuração em vigor, sempre que você desejar proteger um recurso e exigir que o usuário tenha pelo menos 18 anos de idade, você pode aplicar a política "IdadeMinima" ao recurso, fazendo com que o sistema verifique automaticamente se o usuário atende a esse requisito antes de permitir o acesso. Note que você também precisará implementar um manipulador de autorização para a classe `IdadeMinima` para que a lógica de verificação seja executada durante a autorização.
*/
builder.Services.AddAuthorization(options => 
{
    options.AddPolicy("IdadeMinima", policy => 
        policy.AddRequirements(new IdadeMinima(18))
    );
});

/*
 O código que você forneceu está configurando a autenticação por meio de tokens JWT (JSON Web Token) em uma aplicação ASP.NET Core. Isso permite que os usuários se autentiquem usando tokens JWT para acessar recursos protegidos. Vou descrever cada parte do código:

Aqui está o que cada parte do código faz:

1. `builder.Services.AddAuthentication(options => ...`: Isso adiciona a configuração para o serviço de autenticação à coleção de serviços. O método de callback dentro deste bloco permite que você configure opções de autenticação.
2. `options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;`: Aqui você está definindo o esquema de autenticação padrão como `JwtBearerDefaults.AuthenticationScheme`, o que significa que a autenticação será feita usando o esquema de token JWT (Bearer).
3. `.AddJwtBearer(options => ...`: Isso configura o esquema de autenticação `JwtBearer`. Ele define as opções de validação do token JWT.
4. `options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters`: Aqui você está configurando os parâmetros de validação para o token JWT.
- `ValidateIssuerSigningKey = true`: Define se a chave de assinatura do emissor será validada. No caso, está definida como verdadeira, o que significa que a chave de assinatura será validada.

- `IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("1qaz@WSX3edc$RFV5tgb"))`: Aqui você está especificando a chave de assinatura usada para validar os tokens. A chave simétrica é usada para verificar a autenticidade do token.

- `ValidateAudience = false`: Define se o público-alvo (audiência) será validado. Está definido como falso, o que significa que a validação da audiência será desativada.

- `ValidateIssuer = false`: Define se o emissor do token será validado. Está definido como falso, o que significa que a validação do emissor será desativada.

- `ClockSkew = TimeSpan.Zero`: Define o valor de "ClockSkew" como zero, o que significa que a tolerância para a validade do tempo do token é zero, ou seja, o token deve estar exatamente dentro do prazo especificado.

No geral, esse código configura o sistema de autenticação para usar tokens JWT, definindo opções de validação para verificar a autenticidade e validade dos tokens durante o processo de autenticação. Isso permite que os usuários se autentiquem usando tokens JWT para acessar recursos protegidos na aplicação. 
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
