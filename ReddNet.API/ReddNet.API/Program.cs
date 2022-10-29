using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ReddNet.Core.Services.Abstract;
using ReddNet.Core.Services.Concrete;
using ReddNet.Domain;
using ReddNet.Infrastructure;
using ReddNet.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Identity related
builder.Services.AddDbContext<ReddNetDbContext>(options => options.UseInMemoryDatabase("ReddNetDb"));
builder.Services.AddIdentityCore<User>().AddRoles<IdentityRole>();


// Repository related
builder.Services.AddScoped<IRepositoryAsync<Comment>, CommentRepository>();
builder.Services.AddScoped<IRepositoryAsync<Post>, PostRepository>();
builder.Services.AddScoped<IRepositoryAsync<Community>, CommunityRepository>();


// Services related
builder.Services.AddScoped<ICommunityService, CommunityService>();

var app = builder.Build();

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
