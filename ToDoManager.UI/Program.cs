using ToDoManager.Application.Extensions;
using ToDoManager.DataAccess.Extensions;
using ToDoManager.UI.Extensions;

var builder = WebApplication.CreateBuilder(args);


var configuration = builder.Configuration;
var connectionString = configuration["ConnectionStrings:ToDoConnectionString"];

builder.Services.AddSingleton(configuration);

builder.Services.AddControllers();
builder.Services.AddCorsPolicy();

builder.Services
    .AddApplication()
    .AddDataAccess(connectionString);

builder.Services.AddJwtAuthentication(configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();