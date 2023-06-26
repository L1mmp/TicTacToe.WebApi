using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using TicTacToe.Application.Services.Interfaces;
using TicTacToe.DataAccess.Contexts;
using TicTacToe.DataAccess.Repos;
using TicTacToe.DataAccess.Repos.Interfaces;
using TicTacToe.Domain.Mappings;
using TicTacToe.Infrastructure.Providers;
using TicTacToe.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<TokenProvider>();
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IGamesService, GamesService>();
builder.Services.AddTransient<IGamesHistoryService, GamesHistoryService>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddTransient<IGamesRepository, GamesRepository>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IGamesHistoryRepository, GamesHistoryRepository>();

builder.Services.AddDbContext<ApplicationDbContext>(opt =>
{
	opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddSingleton<IGameSessionService, GameSessionService>();



builder.Services.AddAuthentication(
	JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(opt =>
	{
		opt.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
				.GetBytes(builder.Configuration.GetSection("TokenSource:Token").Value!)),
			ValidateIssuer = false,
			ValidateAudience = false
		};
	});


builder.Services.AddAutoMapper(typeof(DtoToEntitesProfile), typeof(DtoToModelsProfile));

builder.Host.UseSerilog((context, config) =>
{
	config.ReadFrom.Configuration(context.Configuration);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
	endpoints.MapControllers();
});

app.Run();
