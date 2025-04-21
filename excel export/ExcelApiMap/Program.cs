using excelexport.Maps;
using excelexport.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var _barn_connection = builder.Configuration.GetConnectionString("BarnStorageConnection") ?? "Server=192.168.30.11:51400;User ID=root;Password=Orash7127640@;Database=TaxBarnStorage";

builder.Services.AddDbContext<BarnStorageDBContext>(options =>
{
    options.UseMySql(_barn_connection, ServerVersion.AutoDetect(_barn_connection));

});

builder.Services.AddControllers();

builder.Services.AddScoped<ExcelExporterServices>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

app.MapExcelAPI();

app.Run();


