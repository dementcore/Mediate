using Mediate.Samples.Shared.Event;
using Mediate.Samples.Shared.EventWithMiddleware;
using Mediate.Samples.Shared.Query;
using Mediate.Samples.Shared.QueryWithMiddleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;

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

            services.AddMediate();

            services.AddMediateEventQueueDispatchStrategy();

            //services.AddMediateGenericEventMiddleware(typeof(BaseEventGenericMiddleware<>)); //this middleware will be used with all events derived from BaseEvent class
            //services.AddMediateGenericQueryMiddleware(typeof(BaseQueryGenericMiddleware<,>)); //this middleware will be used with all querys derived from BaseQuery class 

            services.AddMediateClassesFromAssembly(typeof(SampleEvent).Assembly);

            //services.AddMediateGenericEventHandler(typeof(GenericEventHandler<>)); //this event handler will be used with all events
            //services.AddMediateGenericEventHandler(typeof(BaseEventGenericHandler<>)); //this event handler will be used with all events derived from BaseEvent class

            //services.ForMediateEvent<SampleEvent>()
            //    .AddHandler<SampleEventHandler>(); //concrete handler

            //services.ForMediateEvent<SampleComplexEvent>()
            //    .AddHandler<SampleComplexEventHandler>() //concrete handler
            //    .AddMiddleware<SampleComplexEventMiddleware>(); //this middleware will be used only with this concrete event

            //services.ForMediateQuery<SampleQuery, SampleQueryResponse>()
            //    .AddHandler<SampleQueryHandler>(); //concrete handler

            //services.ForMediateQuery<SampleComplexQuery, SampleComplexQueryResponse>()
            //    .AddHandler<SampleComplexQueryHandler>() //concrete handler
            //    .AddMiddleware<SampleComplexQueryMiddleware>(); //this middleware will be used only with this concrete query

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

                endpoints.MapHub<Samples.Shared.Hubs.SignalRSampleHub>("/hub/test");
            });
        }
    }
}
