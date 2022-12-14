using microservice.core;
using microservice.infrastructure;
using Microsoft.AspNetCore.HttpOverrides;
using product.api.Extensions.App;
using product.api.Extensions.Service;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Infraestructura
builder.Services.AddDbContexts();
builder.Services.AddRepository();

//Core
builder.Services.AddCoreLayer();

//paginacion
builder.Services.AddPaginationExtension(builder.Configuration);

//Configuracion Swagger
builder.Services.AddSwaggerExtension();

//Configuracion acceso controlador
builder.Services.AddControllerExtension();


//Seguridad yprocteecion de datos
builder.Services.AddAuthenticationExtension(builder.Configuration);
builder.Services.AddCorsExtension();

//Salud de los servicios
builder.Services.AddHealthChecks();

//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwaggerExtension();
}

app.MapHealthChecks("/health");

//HTTPS Redirection
app.UseHttpsRedirection();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

//Routing
app.UseRouting();

//CORS
app.UseCors();

//Authorization
app.UseAuthentication();
app.UseAuthorization();

//Personalizadas
app.UseErrorHandlingMiddleware();

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
