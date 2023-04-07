using API.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// DbContext 등록
//string connectionString = builder.Configuration.GetConnectionString("SqlServer");
//builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("hello-database"));
builder.Services.AddScoped<ApplicationDbContextSeed>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
// JWT
builder.Services
	//.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddAuthentication()
	.AddJwtBearer(x =>
	{
		x.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["JwtKey"])),
			ValidateIssuer = false,
			ValidateAudience = false,
			ValidateLifetime = true
		};
		x.RequireHttpsMetadata= false;
	});

// SWAGGER
var securitySchemeName = "Authorization";
builder.Services.AddSwaggerGen(options =>
{
	options.SwaggerDoc("v1", new OpenApiInfo
	{
		Version = "v1",
		Title = "HelloWorld API"
	});

	options.AddSecurityDefinition(securitySchemeName, new OpenApiSecurityScheme
	{
		Name = securitySchemeName,
		Type = SecuritySchemeType.ApiKey,
		In = ParameterLocation.Header
	});

	options.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type = ReferenceType.SecurityScheme,
					Id = securitySchemeName
				}
			},
			Array.Empty<string>()
		}
	});
});

// TEST 필요
//builder.Services.AddEndpointsApiExplorer();

builder.Services.AddControllers().AddJsonOptions(options =>
{
	options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
	options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
	//options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

var app = builder.Build();

// DB 생성
using (var scope = app.Services.CreateScope())
{
	using var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
	//// DB가 생성되어 있지 않을 경우 자동 생성
	//if (!((RelationalDatabaseCreator)dbContext.GetService<IDatabaseCreator>()).Exists())
	//	dbContext.Database.EnsureCreated();
	// DATA 생성
	var dbContextSeed = scope.ServiceProvider.GetRequiredService<ApplicationDbContextSeed>();
	await dbContextSeed.SeedAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI(options =>
	{
		options.SwaggerEndpoint("/swagger/v1/swagger.json", "HelloWorld API v1");
	});
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
