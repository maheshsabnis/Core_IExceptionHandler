using Core_IExceptionHandler.Infrastructure;
using Core_IExceptionHandler.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.

builder.Services.AddDbContext<CompanyContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddExceptionHandler<AppExceptionHandler>();
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
app.UseExceptionHandler(opt => { });



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

 
