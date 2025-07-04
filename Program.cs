using Microsoft.EntityFrameworkCore;
using HISAKA.Web.Data; // Đảm bảo đã import namespace của DbContext
using Microsoft.AspNetCore.Identity; // Cần thiết cho sau này nếu dùng Identity

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// 1. Cấu hình DbContext với SQL Server
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// 2. Thêm hỗ trợ Razor Pages (thường được dùng cho Identity UI), hoặc dùng AddControllersWithViews
builder.Services.AddRazorPages();

// 3. Thêm hỗ trợ Controller với Views cho kiến trúc MVC
builder.Services.AddControllersWithViews();

// 4. (Tùy chọn) Thêm HttpContextAccessor để có thể truy cập HttpContext từ các service khác
builder.Services.AddHttpContextAccessor();

// 5. Cấu hình Identity (sẽ cần cho xác thực/đăng nhập nâng cao sau này, tạm thời có thể bỏ qua nếu chưa dùng)
// builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
//     .AddEntityFrameworkStores<ApplicationDbContext>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// RẤT QUAN TRỌNG: Đảm bảo các dòng này có mặt và đúng vị trí
//app.UseHttpsRedirection(); // Chuyển hướng HTTP sang HTTPS (nếu đã cấu hình)
app.UseStaticFiles();     // Cho phép phục vụ các tệp tĩnh từ wwwroot

app.UseStaticFiles(); // Cho phép phục vụ các tệp tĩnh từ wwwroot

app.UseRouting();         // Định tuyến các yêu cầu

// app.UseAuthentication(); // Hiện tại chưa cần, nhưng nếu dùng Identity thì sẽ kích hoạt
app.UseAuthorization();   // Cần thiết cho ủy quyền

// Đây là định nghĩa route mặc định cho MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages(); // Nếu có Razor Pages

app.Run();