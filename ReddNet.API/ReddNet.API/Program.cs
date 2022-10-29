using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
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
builder.Services.AddIdentityCore<User>().AddRoles<IdentityRole>().AddEntityFrameworkStores<ReddNetDbContext>();

// Repository related
builder.Services.AddScoped<IRepositoryAsync<Comment>, CommentRepository>();
builder.Services.AddScoped<IRepositoryAsync<Post>, PostRepository>();
builder.Services.AddScoped<IRepositoryAsync<Community>, CommunityRepository>();


// Services related
builder.Services.AddScoped<ICommunityService, CommunityService>();
builder.Services.AddScoped<IPostService, PostService>();

var app = builder.Build();
SeedDatabase(app);

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

void SeedDatabase(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var sp = scope.ServiceProvider;
    var service = sp.GetRequiredService<ReddNetDbContext>();

    if (service.Communities.Any())
        return;

    var communityGuid = Guid.NewGuid();
    service.Communities.AddRange(new[]
    {
        new Community
        {
            Id = communityGuid,
            Name = ".NET Developers Community",
            Posts = new List<Post>
            {
                new()
                {
                    Title = "Migration to .NET 6",
                    Content = "This is a demo tutorial representing on how to migrate from .NET Core to .NET 6. Follow the steps!",
                },
                new()
                {
                    Title = "How to add Entity Framework as NuGet package.",
                    Content = "For more tutorial please follow the link : https://example.com",
                }
            }
        }
    });

    service.SaveChanges();

    
}