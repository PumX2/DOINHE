using Microsoft.AspNetCore.OData;
using Microsoft.OData.ModelBuilder;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.OData.Edm;
using DOINHE_BusinessObject;
using DOINHE_Repository;

var builder = WebApplication.CreateBuilder(args);

// Thiết lập mô hình OData
static IEdmModel GetEdmModel()
{
    ODataConventionModelBuilder builder = new();
    builder.EntitySet<Category>("Categories");
    builder.EntitySet<Order>("Orders");
    builder.EntitySet<Product>("Products");
    builder.EntitySet<User>("Users");
    builder.EntitySet<Wallet>("Wallets");
    builder.EntitySet<Report>("Reports");
    return builder.GetEdmModel();
}

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("https://localhost:7040")  // Cho phép Razor Pages giao tiếp với API
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Cấu hình chuỗi kết nối
var connectionString = builder.Configuration.GetConnectionString("DOINHE");
builder.Services.AddDbContext<ApplicationDbContext>(); // Đảm bảo kết nối đúng với SQL Server

// Đăng ký các Repository
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IWalletRepository, WalletRepository>();

// Cấu hình OData và các dịch vụ API
builder.Services.AddControllers()
    .AddOData(opt => opt.AddRouteComponents("odata", GetEdmModel())
    .Filter().Select().Expand().OrderBy().Count().SetMaxTop(100));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "DOINHE API", Version = "v1" });
});

var app = builder.Build();

// Middleware cho HTTPS và xác thực
app.UseHttpsRedirection();
app.UseCors();  // Đảm bảo CORS được gọi sau khi cấu hình HTTPS
app.UseAuthorization();

// Định tuyến các controller
app.MapControllers();

app.UseODataBatching();  // Sử dụng OData Batching

// Swagger UI trong môi trường phát triển
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "DOINHE API V1");
    });
}

app.Run();
