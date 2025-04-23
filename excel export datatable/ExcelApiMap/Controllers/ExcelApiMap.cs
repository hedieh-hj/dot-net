using excelexport.Models;
using excelexport.Services;
using Microsoft.AspNetCore.Mvc;

namespace excelexport.Maps
{
    public static class ExcelApiMap
    {
        public static void MapExcelAPI(this WebApplication app)
        {
            app.MapPost("api/excel/export", async (
                [FromBody] InvoiceModel model,
                [FromServices] ExcelExporterServices _excel_service) =>
            {
                try
                {
                    var _return = await _excel_service.GetExcellResultAsync(model);                 

                    return Results.Ok();
                }
                catch (Exception e)
                {                    
                    return Results.UnprocessableEntity(e);
                }
            }).AllowAnonymous();

            app.MapPost("api/excel/export/datatable", async (
               [FromBody] InvoiceModel model,
               [FromServices] ExcelExporterServices _excel_service) =>
            {
                try
                {
                    var _return = await _excel_service.CreateDataTableAsync(model);

                    return Results.Ok();
                }
                catch (Exception e)
                {
                    return Results.UnprocessableEntity(e);
                }
            }).AllowAnonymous();
        }
    }
}
