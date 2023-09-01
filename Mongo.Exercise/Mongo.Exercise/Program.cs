using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Mongo.Exercise.Models;
using Mongo.Exercise.Services;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

// JWT Configuration
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = Encoding.ASCII.GetBytes(jwtSettings["Secret"]);

builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
	options.RequireHttpsMetadata = false;
	options.SaveToken = true;
	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuerSigningKey = true,
		IssuerSigningKey = new SymmetricSecurityKey(secretKey),
		ValidateIssuer = false,
		ValidateAudience = false
	};
});  //BURAYA KADAR JWT AUTH


builder.Services.AddScoped<StudentService>();
builder.Services.AddSingleton<UserService>(); // Kullanýcý servisini ekleyin


var mongoConnectionString = builder.Configuration.GetConnectionString("MongoDB");
builder.Services.AddSingleton<IMongoClient>(new MongoClient(mongoConnectionString));

var mongoDBSettings = builder.Configuration.GetSection("MongoDBSettings").Get<Settings>();
builder.Services.AddSingleton(mongoDBSettings);

builder.Services.AddScoped<StudentService>();
builder.Services.AddSingleton<UserService>(); // Kullanýcý servisini ekleyin
											  // Add services to the container.
builder.Services.Configure<Settings>(opt =>
{
	builder.Configuration.GetSection("MongoDBSettings").Bind(opt);
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure MongoDB settings from appsettings.json
var configuration = builder.Configuration;
var ConnectionString = configuration.GetConnectionString("MongoDB");
builder.Services.AddSingleton<IMongoClient>(new MongoClient(ConnectionString));

var Settings = configuration.GetSection("MongoDBSettings").Get<Settings>();
builder.Services.AddSingleton(mongoDBSettings);


//BURADA JWT ÝLE ÝLGÝLÝ KISIMLAR YOK ÖÐRENÝLDÝKTEN SONRA BURAYA EKLENMESÝ GEREKLÝ.

// Add other services and controllers
builder.Services.AddScoped<StudentService>();
builder.Services.AddControllers();




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
