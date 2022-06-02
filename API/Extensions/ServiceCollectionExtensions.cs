using API.Authorization;
using API.DTOs.ListTasks;
using API.Services.Assignments;
using API.Services.ListTasks;
using API.Services.Projects;
using API.Services.TaskItems;
using API.Services.TodoItems;
using API.Services.Todos;
using API.Services.Users;
using Domain.Interfaces;
using Domain.Interfaces.Authorization;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        // <summary>
        /// Add needed instances for database
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddDatabase(this IServiceCollection services)
        {
            // Configure DbContext with Scoped lifetime   
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(AppSettings.ConnectionString,
                    sqlOptions => sqlOptions.CommandTimeout(12000));
                options.UseLazyLoadingProxies();
            }
            );

            services.AddScoped<Func<ApplicationDbContext>>((provider) => () => provider.GetService<ApplicationDbContext>());
            services.AddScoped<DbFactory>();
            services.AddHttpContextAccessor();

            return services;
        }

        /// <summary>
        /// Add jwt utils
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddJwtUtils(this IServiceCollection services)
        {
            return services.AddScoped<IJwtUtils, JwtUtils>(); ;
        }

        /// <summary>
        /// Add Uniofwork service
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddUnitOfWork(this IServiceCollection services)
        {
            return services
                .AddScoped<IUnitOfWork, UnitOfWork>();
        }

        /// <summary>
        /// Add bussiness services
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddBusinessServices(this IServiceCollection services
        )
        {
            return services
                .AddScoped<UserService>()
                .AddScoped<ProjectService>()
                .AddScoped<ListTaskService>()
                .AddScoped<TaskItemService>()
                .AddScoped<AssignmentService>()
                .AddScoped<TodoService>()
                .AddScoped<TodoItemService>();
        }

        /// <summary>
        /// Add CORS policy to allow external accesses
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCORS(this IServiceCollection services)
        {
            return // CORS
                services.AddCors(options => {
                    options.AddPolicy("CorsPolicy",
                        builder => {
                            builder.AllowAnyOrigin()
                                .AllowAnyMethod()
                                .AllowAnyHeader()
                                .AllowCredentials();
                        });
                });
        }

        /// <summary>
        /// Add Swagger configuration
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Task Management", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description =
                        "JWT Authorization header using the Bearer scheme. \r\n\r\n " +
                        "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\n" +
                        "Example: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }
            });

            });

            return services;
        }
    }
}
