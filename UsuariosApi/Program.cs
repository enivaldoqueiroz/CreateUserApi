using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UsuariosApi.Data;
using UsuariosApi.Models;

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
