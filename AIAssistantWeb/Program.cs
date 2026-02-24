using AIAssistant.Core.Interfaces;
using AIAssistant.Core.Services;
using AIAssistant.Core.Plugins;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Services.AddSingleton<OllamaService>();
builder.Services.AddSingleton<IPlugin, OllamaPlugin>();
builder.Services.AddSingleton<ChatHistory>();
builder.Services.AddSingleton<Chatbot>();

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
