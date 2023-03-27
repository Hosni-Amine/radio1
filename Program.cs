using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddApplicationInsightsTelemetry();


builder.Services.AddAuthentication(Options =>
{
	Options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	Options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
	Options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
	.AddJwtBearer(option =>
		{
			option.SaveToken = true;
			option.TokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuerSigningKey=true,
				ValidateIssuer = false,
	   		    ValidateAudience = false,
			    ValidateLifetime = false,
		        RequireExpirationTime= false,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection(key: "Jwt:Key").Value))
			};
	   });



var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}



app.UseHttpsRedirection();
app.UseStaticFiles();


app.UseRouting();


app.Use(async (context, next) =>
{
	var token = context.Request.Cookies["poupa_donuts"];
	if (!string.IsNullOrEmpty(token)) context.Request.Headers.Add("Authorization", "Bearer " + token);
	await next();
});



app.UseAuthentication();
app.UseAuthorization();






app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
