using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using FluentValidation.Results;
using System.Linq;
using AutoMapper;
using gidu.Domain.Helpers;

namespace gidu.WebAPI.Helpers
{
    public class HandlingMiddleware
    {
        private readonly RequestDelegate next;

        public HandlingMiddleware(RequestDelegate next) => this.next = next;

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await RequestLogAsync(context);
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
                ErrorLog(ex);
            }
        }

        private async Task RequestLogAsync(HttpContext context)
        {
            context.Request.EnableBuffering();
            var jsonBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
            var queryString = context.Request.QueryString.HasValue ? context.Request.QueryString.Value : "";
            context.Request.Body.Position = 0;
            Console.WriteLine($"Tipo: Request - " +
                              $"Verbo: {context.Request.Method} - " +
                              $"Api: {context.Request.Path} - " +
                              $"Json: {jsonBody} - " +
                              $"QueryString: {queryString}");
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;
            IList<ValidationFailure> errors;

            if (exception is DomainException)
            {
                var exDomain = exception as DomainException;
                code = HttpStatusCode.BadRequest;
                errors = exDomain.Errors;
            }
            else
            {
                errors = new List<ValidationFailure>();
                errors.Add(new ValidationFailure("", exception.Message));
            }

            var errorsModel = errors.Select(u => Mapper.Map<ErrorModel>(u)).ToList();
            var result = JsonConvert.SerializeObject(new { errorsModel });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }

        private static void ErrorLog(Exception ex)
        {
            Console.WriteLine($"Tipo: Erro - " +
                              $"Mensagem: {ex.Message} - " +
                              $"Classe: {ex.GetType().ToString()}");
        }
    }
}
