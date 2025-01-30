using FluentValidation.AspNetCore;
using Hostel_Management.Interface;
using Hostel_Management.Models;
using Hostel_Management.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddFluentValidation(
    v => v.RegisterValidatorsFromAssemblyContaining<Room>());

// Add services to the container.
builder.Services.AddDbContext<HostelContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnStd")));
builder.Services.AddScoped<IRoomService, RoomService>();

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

app.UseCors(policy =>
{
    policy.AllowAnyOrigin()  
          .AllowAnyHeader()  
          .AllowAnyMethod(); 
});

app.MapControllers();

app.Run();
