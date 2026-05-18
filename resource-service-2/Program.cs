using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// JWT AUTH
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
            )
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

// PUBLIC
app.MapGet("/public", () =>
{
    return Results.Ok("Resource Service 2 - Public Data");
});

// SECURE
app.MapGet("/secure", (HttpContext ctx) =>
{
    var userId = ctx.User.Claims.FirstOrDefault(c =>
        c.Type.Contains("nameidentifier"))?.Value ?? "unknown";

    var email = ctx.User.Claims.FirstOrDefault(c =>
        c.Type.Contains("emailaddress"))?.Value ?? "unknown";

    return Results.Ok(new
    {
        service = "resource-service-2",
        message = "Secure access granted",
        userId,
        email
    });
}).RequireAuthorization();

app.Run();
