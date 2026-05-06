using System;

namespace Api.Extensions;

public static class CorsServiceExtensions
{
public static void ConfigureCors(this IServiceCollection services) =>
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
                builder.AllowAnyOrigin()    //WithOrigins("https://dominio.com")
                       .AllowAnyMethod()    //WithMethods("GET","POST")
                       .AllowAnyHeader());  //WithHeaders("accept","content-type")
        });
}
