using AIAssistant.Core.Services;

var builder = WebApplication.CreateBuilder(args);

// HttpClient pentru OllamaService
builder.Services.AddHttpClient();

// Service pentru comunicare cu Ollama
builder.Services.AddSingleton<OllamaService>();

// Istoric conversație
builder.Services.AddSingleton<ChatHistory>();

// MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Chat}/{action=Index}/{id?}");

app.Run();