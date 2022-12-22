using GRPT.Helper;
using GRPT.Repository;
using GRPT.Repository.Database;
using GRPT.Service;
using GRPT.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace GRPT.WebApi.Common
{
    internal static class ConfigurationExtension
    {

        internal static void ServiceConfigurations(IServiceCollection services, IConfiguration configuration)
        {
            GeneralConfigurations(services);
            RepositoryConfigurations(services);
            ServiceConfigurations(services);
            SqlContextConfigurations(services, configuration);
            ConfigureAutoMapper(services);
        }

        /// <summary>
        /// Register general configurations like swagger, endpoint, controllers etc.
        /// </summary>
        /// <param name="services"></param>
        internal static void GeneralConfigurations(this IServiceCollection services)
        {
            services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Generic Repository Pattern Template", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id="Bearer",
                            }
                        },
                        new string[]{}
                    }
                });
            });
        }


        /// <summary>
        /// Register repository services
        /// </summary>
        /// <param name="services"></param>
        internal static void RepositoryConfigurations(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository), typeof(GenericRepository));
        }


        /// <summary>
        /// SQL Context configuration
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        internal static void SqlContextConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<GRPT_DbContext>(options =>
            {
                options.UseSqlServer(
                    configuration.GetConnectionString("GRPT"),
                    sqlServerOptions => sqlServerOptions.CommandTimeout((int)TimeSpan.FromMinutes(30).TotalSeconds));
            });
        }

        public static void ServiceConfigurations(this IServiceCollection services)
        {
            services.AddScoped<IEmployeeService, EmployeeService>();
        }



        /// <summary>
        /// Configuring Middlewares
        /// </summary>
        /// <param name="app"></param>
        internal static void ConfigureMiddleWares(WebApplication app)
        {
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        /// <summary>
        /// Register AutoMapper Profile
        /// </summary>
        /// <param name="services"></param>
        internal static void ConfigureAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapProfile));
        }
    }
}
