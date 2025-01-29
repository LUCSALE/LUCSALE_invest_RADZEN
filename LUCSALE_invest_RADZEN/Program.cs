using Radzen;
using LUCSALEInvestRADZEN.Components;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents().AddHubOptions(options => options.MaximumReceiveMessageSize = 10 * 1024 * 1024);
builder.Services.AddControllers();
builder.Services.AddRadzenComponents();
builder.Services.AddRadzenCookieThemeService(options =>
{
    options.Name = "LUCSALEInvestRADZENTheme";
    options.Duration = TimeSpan.FromDays(365);
});
builder.Services.AddHttpClient();
builder.Services.AddScoped<LUCSALEInvestRADZEN.CadastroDBService>();
builder.Services.AddDbContext<LUCSALEInvestRADZEN.Data.CadastroDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("CadastroDBConnection"));
});
builder.Services.AddScoped<LUCSALEInvestRADZEN.LUCSALE_ExemplosService>();
builder.Services.AddDbContext<LUCSALEInvestRADZEN.Data.LUCSALE_ExemplosContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("LUCSALE_ExemplosConnection"));
});
var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.MapControllers();
app.UseStaticFiles();
app.UseAntiforgery();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();
app.Run();