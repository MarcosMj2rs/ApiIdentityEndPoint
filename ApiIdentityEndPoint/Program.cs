using ApiIdentityEndPoint.Data;
using ApiIdentityEndPoint.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);
var provider = builder.Services.BuildServiceProvider();
var _configuration = provider.GetRequiredService<IConfiguration>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//dotnet user-secrets
var connectionString = _configuration.GetSection("ConnectionStrings").GetValue<string>("SqlServerConnection");

//builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services
	.AddIdentityApiEndpoints<User>()
	.AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddDbContext<AppDbContext>(
	options => options.UseSqlServer(connectionString));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapSwagger();

app.MapGet(pattern: "/", (ClaimsPrincipal user) => user.Identity!.Name)
	.RequireAuthorization();

app.MapIdentityApi<User>();

app.MapPost("/logout",
	async (SignInManager<User> signInManager, [FromBody] object empty) =>
	{
		await signInManager.SignOutAsync();
		return Results.Ok();
	});

app.Run();
