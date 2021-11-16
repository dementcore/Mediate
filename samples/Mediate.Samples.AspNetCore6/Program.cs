using Mediate.Samples.Shared.Event;
using Mediate.Samples.Shared.Hubs;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddSignalR();

//add mediate default services
builder.Services.AddMediate();

//configure the mediate dispatch strategy
builder.Services.AddMediateEventQueueDispatchStrategyCore().AddDefaultExceptionHandler();

//auto register the messages, handlers and middlewares from an assembly
builder.Services.AddMediateClassesFromAssembly(typeof(SampleEvent).Assembly);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<SignalRSampleHub>("/hub/test");
app.Run();
