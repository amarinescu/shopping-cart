using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ShoppingCart.Application.Baskets.Commands;
using ShoppingCart.Application.Baskets.Models;
using ShoppingCart.Application.Baskets.Queries;
using ShoppingCart.Application.ErrorHandling;
using ShoppingCart.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace ShoppingCart.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSwaggerGen();

            services.AddMediatR(typeof(Startup).GetTypeInfo().Assembly);

            #region Register app services
            services.AddSingleton<IBasketRepository, BasketRepository>();
            services.AddSingleton<IRequestHandler<GetBasketByIdQuery, Basket>, GetBasketByIdQuery.Handler>();
            services.AddSingleton<IRequestHandler<AddBasketCommand, Basket>, AddBasketCommand.Handler>();
            services.AddSingleton<IRequestHandler<AddArticleToBasketCommand, Basket>, AddArticleToBasketCommand.Handler>();
            services.AddSingleton<IRequestHandler<CloseBasketCommand, Unit>, CloseBasketCommand.Handler>();
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseExceptionHandler(new ExceptionHandlerOptions()
            {
                AllowStatusCode404Response = true,
                ExceptionHandler = new ExceptionHandler().RequestHandler
            });

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ShoppingCartAPI"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
