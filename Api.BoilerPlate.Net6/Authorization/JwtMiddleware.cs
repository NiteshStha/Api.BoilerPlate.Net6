﻿using Api.BoilerPlate.Net6.Helpers;
using Api.BoilerPlate.Net6.Services;
using Microsoft.Extensions.Options;

namespace Api.BoilerPlate.Net6.Authorization
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AppSettings _appSettings;

        public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
        {
            _next = next;
            _appSettings = appSettings.Value;
        }

        public async Task Invoke(HttpContext context, IUserService userService, IJwtUtils jwtUtils)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = jwtUtils.ValidateJwtToken(token);
            if (userId != null)
            {
                // attach user to context on successful jwt validation
                context.Items["User"] = await userService.GetById(userId.Value);
            }

            await _next(context);
        }
    }
}