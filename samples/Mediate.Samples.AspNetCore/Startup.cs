using Mediate.Extensions.AspNetCore.Microsoft.DependencyInjection;
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
                .AddDefaultHandlerProvider()
                .AddQueuedEventDispatchStrategy();

            //this register the OnHomeInvokedEventHandler for OnHomeInvoked event
            services.AddMediateEventHandler<OnHomeInvoked, OnHomeInvokedEventHandler>();

            //this not registers because we can't have the same handler registered multiple times for a specific event
            services.AddMediateEventHandler<OnHomeInvoked, OnHomeInvokedEventHandler>();

            //this register the OnHomeInvokedEventHandler2 for OnHomeInvoked event, we can have multiple handlers for the same event
            services.AddMediateEventHandler<OnHomeInvoked, OnHomeInvokedEventHandler2>();

            //for TestMsg message type with TestMsgReply response type this registers the TestMsgHandler
            services.AddMediateMessageHandler<TestMsg,TestMsgReply, TestMsgHandler>();

            // this not registers because we can't have more than one handler for a message type
            services.AddMediateMessageHandler<TestMsg,TestMsgReply, TestMsgHandler2>();

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
