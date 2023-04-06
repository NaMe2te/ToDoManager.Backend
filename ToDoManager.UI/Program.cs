using ToDoManager.Application.Extensions;
using ToDoManager.DataAccess.Extensions;
using ToDoManager.UI.Extensions;

var builder = WebApplication.CreateBuilder(args);


var configuration = builder.Configuration;

builder.Services.AddSingleton(configuration);

builder.Services.AddControllers();
builder.Services.AddCorsPolicy();

builder.Services
    .AddApplication()
    .AddDataAccess(configuration);

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