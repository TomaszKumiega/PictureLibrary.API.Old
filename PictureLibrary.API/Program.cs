using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using PictureLibrary.API;
using PictureLibrary.API.Filters;
using PictureLibrary.DataAccess.DatabaseAccess;
using PictureLibrary.DataAccess.Repositories;
using PictureLibrary.DataAccess.Services;
using PictureLibrary.Tools;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddMvc(options =>
{
    options.EnableEndpointRouting = false;
    options.Filters.Add<ValidationFilter>();
});

builder.Services
    .AddSingleton<IAccessTokenService, AccessTokenService>()
    .AddSingleton(typeof(IDatabaseAccess<>), typeof(DatabaseAccess<>))
    .AddSingleton<ITokensRepository, TokensRepository>()
    .AddSingleton<IUserRepository, UserRepository>()
    .AddSingleton<ILibraryRepository, LibraryRepository>()
    .AddSingleton<ITagRepository, TagRepository>()
    .AddSingleton<IHashAndSalt, HashAndSalt>()
    .AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => options.TokenValidationParameters = new PictureLibraryTokenValidationParameters(builder.Configuration));

builder.Services.AddMediatR(typeof(PictureLibrary.DataAccess.DataAccessEntrypoint));
builder.Services.AddValidatorsFromAssembly(Assembly.GetAssembly(typeof(PictureLibrary.DataAccess.DataAccessEntrypoint)));

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
app.UseAuthentication();

app.MapControllers();

app.UseExceptionHandler(appError => appError.Run(ExceptionHandler.Handle));

app.Run();
