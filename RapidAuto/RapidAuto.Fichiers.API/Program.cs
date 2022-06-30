using Microsoft.OpenApi.Models;
using RapidAuto.Fichiers.API.Interfaces;
using RapidAuto.Fichiers.API.ServicesProxy;
using System.Reflection;
using Microsoft.Extensions.Azure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAzureClients
    (configure =>
    {
        // Add a Storage account client
        configure.AddBlobServiceClient(builder.Configuration.GetConnectionString("StorageConnectionString"));
        
        
    });
builder.Services.AddScoped<IGestionFichiersService, GestionFichierServiceProxy>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API de gestion de fichiers pour le projet RapidAuto",
        Version = "v1",
        Description = "Système permettant de stocker et de retourner des fichiers.",
        Contact = new OpenApiContact
        {
            Name = "Kevin, Olivier, Simon",
            Email = "dreamteam@gmail.com",
            Url = new Uri("https://www.youtube.com/watch?v=dQw4w9WgXcQ")
        }
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Fichiers.API v1"));
    }
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
