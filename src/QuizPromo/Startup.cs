using System;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Mvc.Cors;
using Microsoft.Data.Entity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using QuizPromo.Infrastructure.DDD;
using QuizPromo.Infrastructure.EF;
using QuizPromo.Infrastructure.EF.Repositories;

using QuizPromo.ModelCore.BoundedContext;
using WebApi.Hal;

namespace Quiz
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(opts =>
            {
                var jsonHalOutputFormatter = new JsonHalMediaTypeOutputFormatter();


                opts.OutputFormatters.Insert(0, jsonHalOutputFormatter);

                opts.Filters.Add(new CorsAuthorizationFilterFactory("any"));
            });

            services.AddCors(opts =>
            {
                opts.AddPolicy("any", builder =>
                {
                    builder.AllowAnyMethod().AllowAnyOrigin().AllowCredentials();
                });
            });


            services.AddEntityFramework()
                .AddSqlServer();

            services.AddScoped<DbContext, QuizPromoContext>();
            services.AddScoped<IDbSession, EFDbSession>();
            services.AddScoped<IRepositoryWithTypedId<Question, int>, EFRepository<Question, int>>();

            //return container.CreateServiceProvider(services);
        }

        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddProvider(new MyLoggerProvider());

            app.UseIISPlatformHandler();

            app.UseMvc();
        }
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }

    public class MyLoggerProvider : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName)
        {
            return new MyLogger();
        }

        public void Dispose()
        { }

        private class MyLogger : ILogger
        {
            public bool IsEnabled(LogLevel logLevel)
            {
                return true;
            }

            public void Log(LogLevel logLevel, int eventId, object state, Exception exception, Func<object, Exception, string> formatter)
            {
                System.Diagnostics.Trace.WriteLine(formatter(state, exception));
            }

            public IDisposable BeginScopeImpl(object state)
            {
                return null;
            }
        }
    }
}
