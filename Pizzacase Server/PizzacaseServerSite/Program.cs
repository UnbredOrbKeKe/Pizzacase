using PizzacaseServerSite.ServerListening;

var builder = WebApplication.CreateBuilder(args);
//comment out to switch between tcp and udp connection
var listener = new ListenerTCP();
Task.Run(() => listener.StartTcpServer());

//var listener = new ListenerUDP();
//Task.Run(() => listener.StartUdpServer());



// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
