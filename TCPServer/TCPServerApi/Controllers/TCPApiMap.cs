using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCPServerApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Services.Models;

namespace TCPServerApi.Maps
{
    public static class TCPApiMap
    {
        public static void MapTCPAPI(this WebApplication app)
        {
            app.MapPost("api/tcp/request", async (
                [FromBody] ValidationViewModel model,
                [FromServices] TCPService _tcp_service) =>
            {
                try
                {
                    var _response = await _tcp_service.GetData(model);

                    return Results.Ok(_response);
                }
                catch (Exception e)
                {
                    return Results.UnprocessableEntity(e);
                }
            }).AllowAnonymous();
        }
    }
}
