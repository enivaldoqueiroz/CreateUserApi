using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UsuariosApi.Authorization;
using UsuariosApi.Data;
using UsuariosApi.Models;
using UsuariosApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

#region DbContext - Acessando o banco de dados

var connString = builder.Configuration.GetConnectionString("DefaultConnection");

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

app.UseAuthorization();

app.MapControllers();

app.Run();
