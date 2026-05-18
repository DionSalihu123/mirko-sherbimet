using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// OpenAPI (optional)
builder.Services.AddOpenApi();

// JWT Authentication
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
            ),

            // 👇 THIS is what fixes "unknown"
            NameClaimType = ClaimTypes.NameIdentifier,
            RoleClaimType = ClaimTypes.Role
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// PUBLIC endpoint
app.MapGet("/public", () =>
{
    return Results.Ok("This is public data");
});

// SECURE endpoint
app.MapGet("/secure", (HttpContext ctx) =>
{
    var userId = ctx.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "unknown";
    var email = ctx.User.FindFirst(ClaimTypes.Email)?.Value ?? "no-email";

    return Results.Ok(new
    {
        message = "You accessed a secure endpoint",
        userId,
        email
    });
})
.RequireAuthorization();

app.Run();
