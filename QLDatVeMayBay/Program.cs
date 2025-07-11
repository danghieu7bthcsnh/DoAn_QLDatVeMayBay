using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using QLDatVeMayBay.Data;
using QLDatVeMayBay.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/TaiKhoan/DangNhap";    // Đường dẫn đến trang đăng nhập
        options.LogoutPath = "/TaiKhoan/DangXuat";   // Trang đăng xuất
        options.AccessDeniedPath = "/TaiKhoan/AccessDenied"; // Nếu cần
    });

// Đọc cấu hình ConnectionStrings từ appsettings.json
builder.Services.AddDbContext<QLDatVeMayBayContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("QLDatVeMayBayContext")));

// Cấu hình dịch vụ gửi email (EmailSettings từ appsettings.json)
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<IEmailSender, EmailSender>();

// Thêm MVC (Controller + Razor View)
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Middleware xử lý lỗi (production)
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();        // Chuyển sang HTTPS nếu cần
app.UseStaticFiles();            // Cho phép dùng wwwroot, css, js

app.UseRouting();                // Kích hoạt routing
app.UseAuthentication();
app.UseAuthorization();          // (sau này dùng xác thực có thể cần UseAuthentication)

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Tự động tạo/migrate database nếu cần (tuỳ chọn)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<QLDatVeMayBayContext>();
    context.Database.Migrate();
}

app.Run();
