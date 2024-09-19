using Core_IExceptionHandler.Infrastructure;
using Core_IExceptionHandler.Models;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.

builder.Services.AddDbContext<CompanyContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddExceptionHandler<AppExceptionHandler>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddHealthChecks();

//https://localhost:7134/api/CatAPI
builder.Services.AddHealthChecks()
		.AddSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),tags: new[] { "sqldb" }).
		AddUrlGroup(new Uri("https://localhost:7134/api/CatAPI"), name: "webhook1", failureStatus: HealthStatus.Unhealthy, tags: new[] { "webhook" }); 

builder.Services.AddHealthChecksUI()
    .AddInMemoryStorage();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseExceptionHandler(opt => { });
 

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

 
app.MapHealthChecksUI();


app.MapPost("/dept", async(CompanyContext ctx, Department dept) =>
{
	try
	{
		var result = await ctx.Departments.AddAsync(dept);
		await ctx.SaveChangesAsync();
		return Results.Created($"/dept/{result.Entity.DeptNo}", result.Entity);
	}
	catch (DbUpdateException ex)
	{
		throw ex;
	}
});

app.Run();

 
