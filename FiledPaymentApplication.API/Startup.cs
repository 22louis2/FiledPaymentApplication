using FiledPaymentApplication.Common;
using FiledPaymentApplication.Core;
using FiledPaymentApplication.Data;
using FiledPaymentApplication.Model.AutoMaps;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;

namespace FiledPaymentApplication.API
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
            services.AddControllers()
                .AddNewtonsoftJson(options =>
                  options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore); ;

            // register dbContext
            services.AddDbContext<AppDbContext>(opt => opt.UseSqlite(Configuration.GetConnectionString("DbConnection")));

            // register fluent validation
            services.AddMvc(options =>
            {
                options.Filters.Add(new ValidationFilter());
            })
             .AddFluentValidation(options =>
             {
                 options.RegisterValidatorsFromAssemblyContaining<Startup>();
             });

            // register automapper
            services.AddAutoMapper(typeof(AutoMapperProfile));

            // register swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FiledPaymentApplication.API", Version = "v1" });
            });

            // dependency injection
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<ICheapPaymentGateway, CheapPaymentService>();
            services.AddScoped<IExpensivePaymentGateway, ExpensivePaymentService>();
            services.AddScoped<IPremiumPaymentGateway, PremiumPaymentService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FiledPaymentApplication.API v1"));

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
