using DOINHE;
using DOINHE.Db; // Đảm bảo đúng namespace của DbContext
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Net.payOS; // Đảm bảo đúng namespace của PayOS

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Add DbContext with SQL Server connection string
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DOINHE"))
);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
    .AddCookie(options =>
    {
        options.LoginPath = "/Login"; // Đường dẫn đến trang đăng nhập
        options.AccessDeniedPath = "/Account/AccessDenied"; // Đường dẫn nếu không đủ quyền truy cập
    });

IConfiguration configuration = builder.Configuration;

// Configure EmailSender
//builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
//builder.Services.AddScoped<IEmailSender, EmailSender>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Cấu hình PayOS
builder.Services.AddSingleton<PayOS>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();

    return new PayOS(
        config["Environment:PAYOS_CLIENT_ID"] ?? throw new Exception("PAYOS_CLIENT_ID not found in configuration"),
        config["Environment:PAYOS_API_KEY"] ?? throw new Exception("PAYOS_API_KEY not found in configuration"),
        config["Environment:PAYOS_CHECKSUM_KEY"] ?? throw new Exception("PAYOS_CHECKSUM_KEY not found in configuration"),
        config["Environment:PAYOS_PARTNER_CODE"] // Partner code là tùy chọn, không cần throw Exception
    );
});
builder.Services.AddHttpClient();

// Cấu hình Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Session timeout
    options.Cookie.HttpOnly = true; // Đảm bảo tính bảo mật của cookie
    options.Cookie.IsEssential = true; // Đảm bảo session hoạt động dù người dùng không chấp nhận cookie
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Sử dụng Session middleware
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapRazorPages();

app.Run();
