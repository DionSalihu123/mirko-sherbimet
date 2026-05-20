using dashboard.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<SessionState>();

builder.Services.AddHttpClient("auth", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["AuthService:BaseUrl"] ?? "http://localhost:5038");
});

builder.Services.AddHttpClient("resource1", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ResourceService1:BaseUrl"] ?? "http://localhost:5226");
});

builder.Services.AddHttpClient("resource2", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ResourceService2:BaseUrl"] ?? "http://localhost:5295");
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

if (!app.Environment.IsDevelopment())
    app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseRouting();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.Run();
