using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ReddNet.API.Authorization.Handlers;
using ReddNet.API.Authorization.Requirements;
//using ReddNet.API.Authorization.Requiremenets;
using ReddNet.Core.Services.Abstract;
using ReddNet.Core.Services.Concrete;
using ReddNet.Domain;
using ReddNet.Infrastructure;
using ReddNet.Infrastructure.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//TODO:
// Add Caching service (Redis? Memcache?)
// Logger should be added and configured.
// Ability to host it in Docker.
// Exception Filters should be added.
// Should add JWT
// Possible IdentityServer?
// Need to think wheter we continue supporting fully InMemoryDb or will move to any of the db.
// Swagger configuration should be extended.
// Add healthcheck

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{

});

// Identity related
builder.Services.AddDbContext<ReddNetDbContext>(options => options.UseInMemoryDatabase("ReddNetDb"));
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 3;
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;

}).AddEntityFrameworkStores<ReddNetDbContext>().AddDefaultTokenProviders();


// Repository related
builder.Services.AddScoped<IRepositoryAsync<Comment>, CommentRepository>();
builder.Services.AddScoped<IRepositoryAsync<Post>, PostRepository>();
builder.Services.AddScoped<IRepositoryAsync<Community>, CommunityRepository>();

// Services related
builder.Services.AddScoped<ICommunityService, CommunityService>();
builder.Services.AddScoped<IPostService, PostService>();

// Authentication related
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            RoleClaimType = JwtClaimTypes.Role,
            ValidateIssuerSigningKey = true,
            ValidAudience = "http://localhost:5031",
            ValidIssuer = "http://localhost:5031",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Super Secret Key"))
        };
        options.SaveToken = true;
    });

builder.Services.AddAuthorization(options =>
{
    JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
    options.AddPolicy("IsEligibleForCommunityDelete", policy =>
    {
        policy.Requirements.Add(new IsEligibleForCommunityDeleteRequirement());
    });

    options.AddPolicy("IsEligibleForCommunityModeratorAddition", policy =>
    {
        policy.Requirements.Add(new IsEligibleForCommunityModeratorAdditionRequirement());
    });
});

builder.Services.AddScoped<IAuthorizationHandler, IsEligibleForCommunityDeleteHandler>();
builder.Services.AddScoped<IAuthorizationHandler, IsEligibleForCommunityModeratorAdditionHandler>();
//builder.Services.AddScoped<IAuthorizationHandler, IsEligibleForCommunityDeleteHandler>();

var app = builder.Build();
SeedDatabase(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();

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
                    CommunityId = communityGuid,
                    Comments = new List<Comment>
                    {
                        new Comment
                        {
                            Content = "After hours of trying, I have failed to migrate successfully. Is there anyone that could help me with it?",
                        }
                    }
                },
                new()
                {
                    Title = "How to add Entity Framework as NuGet package.",
                    Content = "For more tutorial please follow the link : https://example.com",
                    CommunityId = communityGuid,
                    Comments = new List<Comment>
                    {
                        new Comment
                        {
                            Content = "by following your tutorials, I was able to successfully use EF!",
                        }
                    }
                }
            }
        }
    });

    service.SaveChanges();


}