using Autofac;
using Mediate.Core;
using Mediate.Core.Abstractions;
using Mediate.Extensions.AspNetCore;
using Mediate.Extensions.AspNetCore.HostedService;
using Mediate.Extensions.AspNetCore.Queue;
using Mediate.Samples.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Mediate.Samples.AspNetCore.Autofac
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

            services.AddControllersWithViews();
        }

        // ConfigureContainer is where you can register things directly
        // with Autofac. This runs after ConfigureServices so the things
        // here will override registrations made in ConfigureServices.
        // Don't build the container; that gets done for you by the factory.
        public void ConfigureContainer(ContainerBuilder builder)
        {
            // Register your own things directly with Autofac here. Don't
            // call builder.Populate(), that happens in AutofacServiceProviderFactory
            // for you.

            builder.RegisterType<Mediator>().As<IMediator>().InstancePerDependency();
            builder.RegisterType<ServiceProviderHandlerProvider>().As<IMessageHandlerProvider>().InstancePerDependency();
            builder.RegisterType<ServiceProviderHandlerProvider>().As<IEventHandlerProvider>().InstancePerDependency();
            
            builder.RegisterType<EventQueue>().As<IEventQueue>().SingleInstance();
            builder.RegisterType<EventDispatcherService>().As<IHostedService>().InstancePerDependency();
            builder.RegisterType<EventQueueDispatchStrategy>().As<IEventDispatchStrategy>().InstancePerDependency();

            //this register the OnHomeInvokedEventHandler for OnHomeInvoked event
            builder.RegisterType<OnHomeInvokedEventHandler>().As<IEventHandler<OnHomeInvoked>>().InstancePerDependency();
            builder.RegisterType<OnHomeInvokedEventHandler2>().As<IEventHandler<OnHomeInvoked>>().InstancePerDependency();

            //for TestMsg message type with TestMsgReply response type this registers the TestMsgHandler
            builder.RegisterType<TestMsgHandler>().As<IMessageHandler<TestMsg,TestMsgReply>>().InstancePerDependency();

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
