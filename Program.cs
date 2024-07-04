using Microsoft.OpenApi.Models;
using op.Services.Interfaces;
using op.Services;
using op.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "OP API",
        Version = "v1",
        Description = "This API services converts XML data into JSON format."
    });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

// IoC - Register custom services
builder.Services.AddSingleton<IJsonConvertService, JsonConvertService>();
builder.Services.AddSingleton<IFetchDataService, FetchDataService>();
builder.Services.AddSingleton<IXmlService, XmlService>();
builder.Services.AddHttpClient();
builder.Services.AddOptions<ApiSettings>().Configure<IConfiguration>((settings, configuration) =>
{
    configuration.GetSection("ApiSettings").Bind(settings);
});

var app = builder.Build();

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
