using Api.BoilerPlate.Net6.Extensions;
using Api.BoilerPlate.Net6.Helpers;
using Ems.Api.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Custom Services
builder.Services.AddCorsConfigurations();
builder.Services.AddIISIntegration();
builder.Services.AddDbContextConfigurations(builder.Configuration);
builder.Services.AddRepositoryConfigurations();
builder.Services.AddDIServicesConfigurations();
builder.Services.AddAppSettingsConfigurations(builder.Configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.UseAuthentication();

// global error handler
app.UseMiddleware<ErrorHandlerMiddleware>();
// custom jwt auth middleware
app.UseMiddleware<JwtMiddleware>();

// global cors policy
app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.MapControllers();

app.Run();
