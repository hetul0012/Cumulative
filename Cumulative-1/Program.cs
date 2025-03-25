using Cumulative_1.Controllers;
using Cumulative_1.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//database
//builder.Services.AddSingleton<Cumulative_1.Models.SchoolDbContext>();
builder.Services.AddScoped<SchoolDbContext>();
builder.Services.AddScoped<TeacherAPIController>();

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
