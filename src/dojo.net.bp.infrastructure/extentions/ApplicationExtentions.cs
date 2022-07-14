using dojo.net.bp.application.models.dtos;
using dojo.net.bp.application.models.exeptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Prometheus;
using System.Text.Json;

namespace dojo.net.bp.infrastructure.extentions
{
    public static class ApplicationExtentions
    {

        public static IApplicationBuilder ConfigureMetricServer(this IApplicationBuilder app)
        {
            app.UseMetricServer();
            app.UseHttpMetrics();

            return app;
        }

        public static IApplicationBuilder ConfigureExceptionHandler(this IApplicationBuilder app)
        {

            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    
                    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();

                    string internalCode = "";
                    int _httpCode = context.Response.StatusCode;
                    Exception ex = exceptionHandlerPathFeature.Error;
                    string _message = ex.Message;
                    try
                    {
                        internalCode = ((BaseCustomException)((ExceptionHandlerFeature)exceptionHandlerPathFeature).Error).Code.ToString();
                        _message = ((BaseCustomException)((ExceptionHandlerFeature)exceptionHandlerPathFeature).Error).Message;

                        //if (Convert.ToInt32(_code) != 0) context.Response.StatusCode = _code;

                    }
                    catch (InvalidCastException) { }


                    if (ex.GetType() == typeof(APIException))
                    {
                        _httpCode = 200;
                        internalCode = ((APIException)ex).Code;
                        _message = _message;
                    }
                    else
                    {
                        _httpCode = _httpCode;
                        internalCode = "Hubo un error general en la aplicacion";
                        _message = _message;
                    }


                    context.Response.StatusCode = _httpCode;
                    context.Response.ContentType = "application/json";
                    MsDtoResponseError _response = new MsDtoResponseError
                    {
                        code = _httpCode,
                        message = "Error en: " + exceptionHandlerPathFeature.Path,
                        errors = new List<MsDtoError> {
                         new MsDtoError
                        {
                            code = internalCode,
                            message = _message
                        }
                        },
                        traceid = context?.TraceIdentifier == null ? "no-traceid" : context?.TraceIdentifier.Split(":")[0].ToLower()
                    };


                    string sjson = JsonSerializer.Serialize(_response);

                    await context.Response.WriteAsync(sjson);
                    await context.Response.Body.FlushAsync();
                });
            });

            return app;
        }
    }
}
