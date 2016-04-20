using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Tasks;
using LightInject;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc.Cors;
using Microsoft.Data.Entity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using QuizPromo.Infrastructure.DDD;
using QuizPromo.Infrastructure.EF;
using QuizPromo.Infrastructure.EF.Repositories;
using QuizPromo.ModelCore;
using WebApi.Hal;
using ServiceContainer = LightInject.ServiceContainer;

namespace Quiz
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
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

            var container = new ServiceContainer();
            container.Register<IFoo, Foo>(new PerScopeLifetime());

            //return container.CreateServiceProvider(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //this is the magic line
            loggerFactory.AddProvider(new MyLoggerProvider());

            app.UseIISPlatformHandler();

            app.UseMvc();
        }

        // Entry point for the application.
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

    public interface IFoo
    {
        int Bar();
    }

    public class Foo : IFoo
    {
        private int r = new Random().Next();

        public Foo()
        {

        }
        public int Bar()
        {
            return r;
        }
    }
}
