﻿using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Transaction.Api.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }
        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            HttpStatusCode code = HttpStatusCode.BadRequest;
            string result = JsonConvert.SerializeObject(new { error = ex.Message });

            //if (ex is LoginFailedException)
            //    code = HttpStatusCode.Unauthorized;
            //else if (ex is CreateAccountFailed)
            //    code = HttpStatusCode.InternalServerError;
            //else if (ex is AccountNotFoundException)
            //    code = HttpStatusCode.NotFound;
            //// whice status code?
            //else if (ex is EmailVerificationException)
            //    code = HttpStatusCode.NotFound;

            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}