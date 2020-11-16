using Autofac;
using Mediate.Abstractions;
using Mediate.DispatchStrategies;
using Mediate.HostedService;
using Mediate.Queue;
using Mediate.Samples.Shared.Event;
using Mediate.Samples.Shared.EventWithMiddleware;
using Mediate.Samples.Shared.Query;
using Mediate.Samples.Shared.QueryWithMiddleware;
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

            //register default mediator services
            builder.RegisterType<Mediator>().As<IMediator>().InstancePerDependency();
            builder.RegisterType<ServiceProviderHandlerProvider>().As<IHandlerProvider>().InstancePerDependency();
            builder.RegisterType<ServiceProviderMiddlewareProvider>().As<IMiddlewareProvider>().InstancePerDependency();

            //this registers the required services for the event queue dispatch strategy
            builder.RegisterType<EventQueue>().As<EventQueue>().SingleInstance();
            builder.RegisterType<EventDispatcherService>().As<IHostedService>().InstancePerDependency();
            builder.RegisterType<EventQueueDispatchStrategy>().As<IEventDispatchStrategy>().InstancePerDependency();

            //this register the SampleEventHandler for SampleEvent event
            builder.RegisterType<SampleEventHandler>().As<IEventHandler<SampleEvent>>().InstancePerDependency();
            //this register the SampleComplexEventHandler for SampleComplexEvent event
            builder.RegisterType<SampleComplexEventHandler>().As<IEventHandler<SampleComplexEvent>>().InstancePerDependency();

            //for SampleComplexQuery query type with SampleComplexQueryResponse response type this registers the SampleComplexQueryHandler
            builder.RegisterType<SampleComplexQueryHandler>().As<IQueryHandler<SampleComplexQuery, SampleComplexQueryResponse>>().InstancePerDependency();
            //for SampleQuery query type with SampleQueryResponse response type this registers the SampleQueryHandler
            builder.RegisterType<SampleQueryHandler>().As<IQueryHandler<SampleQuery, SampleQueryResponse>>().InstancePerDependency();


            //this registers a generic event handler that catchs all events
            builder.RegisterGeneric(typeof(GenericEventHandler<>)).As(typeof(IEventHandler<>)).InstancePerDependency();
            //this registers a generic event handler that catchs all events derived from BaseEvent class
            builder.RegisterGeneric(typeof(BaseEventGenericHandler<>)).As(typeof(IEventHandler<>)).InstancePerDependency();

            //this registers a generic middleware for all querys derived from BaseQuery class
            builder.RegisterGeneric(typeof(BaseQueryGenericMiddleware<,>)).As(typeof(IQueryMiddleware<,>)).InstancePerDependency();
            //this registers a generic middleware for all events derived from BaseQuery class
            builder.RegisterGeneric(typeof(BaseEventGenericMiddleware<>)).As(typeof(IEventMiddleware<>)).InstancePerDependency();

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

                endpoints.MapHub<Samples.Shared.Hubs.SignalRSampleHub>("/hub/test");
            });
        }
    }
}
