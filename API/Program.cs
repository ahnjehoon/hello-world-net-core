using API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// DbContext 등록
string connectionString = builder.Configuration.GetConnectionString("SqlServer");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddScoped<ApplicationDbContextSeed>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
	// DB가 생성되어 있지 않을 경우 자동 생성
	if (!((RelationalDatabaseCreator)dbContext.GetService<IDatabaseCreator>()).Exists())
		dbContext.Database.EnsureCreated();
	// DATA 생성
	var dbContextSeed = scope.ServiceProvider.GetRequiredService<ApplicationDbContextSeed>();
	await dbContextSeed.SeedAsync();
}

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
