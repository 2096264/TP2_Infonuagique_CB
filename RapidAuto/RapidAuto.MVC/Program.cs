using RapidAuto.MVC.Interfaces;
using RapidAuto.MVC.ServicesProxy;
using Microsoft.Extensions.Azure;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAzureClients
    (configure =>
    {
        // Add a Storage account client
        configure.AddBlobServiceClient(builder.Configuration.GetConnectionString("StorageConnectionString"));

    });

//Ajout des liens vers les APIs
builder.Services.AddHttpClient<IGestionFichiersService, GestionFichiersServiceProxy>(client => client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("UrlAPIGestionFichiers")));
builder.Services.AddHttpClient<IGestionVehiculesService, GestionVehiculesServiceProxy>(client => client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("UrlAPIGestionVehicules")));
builder.Services.AddHttpClient<IGestionUtilisateursService, GestionUtilisateursServiceProxy>(client => client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("UrlAPIGestionUtilisateurs")));
builder.Services.AddHttpClient<IGestionCommandesService, GestionCommandesServiceProxy>(client => client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("UrlAPIGestionCommandes")));
builder.Services.AddHttpClient<IGestionFavorisService, GestionFavorisServiceProxy>(client => client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("UrlAPIGestionFavoris")));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithRedirects("/Home/CodeStatus?code={0}");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
