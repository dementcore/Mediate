using Mediate.Samples.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Mediate.Samples.AspNetCore
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
            services.AddSignalR();

            services.AddMediate()
                .AddDefaultMediator()
                .AddServiceProviderHandlerProvider()
                .AddEventQueueDispatchStrategy();

            //this registers a generic event handler that catchs all events
            services.AddMediateGenericEventHandler(typeof(GenericEventHandler<>));

            //this registers a generic event handler that catchs all events that it's base class is BaseEvent
            services.AddMediateGenericEventHandler(typeof(GenericBaseEventHandler<>));

            services.ForMediateEvent<OnHomeInvoked>()
                .AddHandler<OnHomeInvokedEventHandler>()
                .AddHandler<OnHomeInvokedEventHandler2>();

            services.ForMediateQuery<TestMsg, TestMsgReply>()
                .AddHandler<TestMsgHandler>();

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapHub<Hubs.TestHub>("/hub/test");
            });
        }
    }
}
