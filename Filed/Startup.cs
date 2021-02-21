using Filed.Models;
using Filed.Services;
using Filed.Services.Contracts;
using Filed.Services.Data;
using Filed.Services.Data.DataModels;
using Filed.Services.ExternalServices;
using Filed.Services.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Filed
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

            //Setting Up Automapper
            services.AddAutoMapper(a => a.CreateMap<PaymentVM, Payment>().ReverseMap());

            services.AddDbContext<PaymentContext>(options => options
                .UseSqlServer(Configuration.GetConnectionString("PaymentsConnString")));

            services.AddScoped<PaymentContext, PaymentContext>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IPaymentStateRepository, PaymentStateRepository>();
            services.AddScoped<IExpensivePaymentGateway, ExpensivePaymentGateway>();
            services.AddScoped<ICheapPaymentGateway, CheapPaymentGateway>();
            services.AddScoped<IExpensivePaymentGateway, PremiumPaymentService>();
            services.AddScoped<IPaymentService, PaymentService>();

            services.AddControllers().ConfigureApiBehaviorOptions(op => op.SuppressModelStateInvalidFilter = true);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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
