using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using PictureLibrary.API;
using PictureLibrary.DataAccess.DatabaseAccess;
using PictureLibrary.DataAccess.Repositories;
using PictureLibrary.Tools;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services
    .AddSingleton<IAccessTokenService, AccessTokenService>()
    .AddSingleton(typeof(IDatabaseAccess<>), typeof(DatabaseAccess<>))
    .AddSingleton<ITokensRepository, TokensRepository>()
    .AddSingleton<IUserRepository, UserRepository>()
    .AddSingleton<IHashAndSalt, HashAndSalt>();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => options.TokenValidationParameters = new PictureLibraryTokenValidationParameters(builder.Configuration));

builder.Services.AddMediatR(typeof(PictureLibrary.DataAccess.MediatorEntrypoint));

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

app.Run();
