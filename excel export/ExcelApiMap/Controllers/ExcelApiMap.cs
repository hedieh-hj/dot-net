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
                    var _stream = await _excel_service.GetExcellResultAsync(model);                                                            

                    var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                    var t = DateTimeOffset.Now.ToUnixTimeMilliseconds();

                    var fileName = $"{t}.xlsx";                   

                    return Results.File(_stream, contentType,fileName);
                }
                catch (Exception e)
                {                    
                    return Results.UnprocessableEntity(e);
                }
            }).AllowAnonymous();
        }
    }
}
