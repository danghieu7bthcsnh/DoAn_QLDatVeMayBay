using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using QLDatVeMayBay.Data;
using QLDatVeMayBay.Services;

var builder = WebApplication.CreateBuilder(args);

// ✅ Cấu hình xác thực Cookie
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/TaiKhoan/DangNhap";
        options.LogoutPath = "/TaiKhoan/DangXuat";
        options.AccessDeniedPath = "/TaiKhoan/AccessDenied";
    });

// ✅ Cấu hình EF DbContext
builder.Services.AddDbContext<QLDatVeMayBayContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("QLDatVeMayBayContext")));

// ✅ Cấu hình gửi email từ appsettings.json
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<EmailService>(); // dùng trực tiếp

// ✅ Cho phép dùng session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Hết hạn sau 30 phút không hoạt động
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// ✅ Cho phép inject HttpContextAccessor
builder.Services.AddHttpContextAccessor();

// ✅ Thêm MVC + Razor View
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<EmailService>();
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<EmailService>(); // ← Dịch vụ gửi email


var app = builder.Build();

// ✅ Middleware lỗi
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// ✅ Middleware xác thực và phân quyền
app.UseAuthentication();
app.UseAuthorization();

// ✅ Bắt buộc: Middleware Session (sau UseRouting, trước MapRoutes)
app.UseSession();

// ✅ Định tuyến mặc định
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// ✅ Tự động migrate CSDL (nếu cần)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<QLDatVeMayBayContext>();
    context.Database.Migrate();
}

app.Run();