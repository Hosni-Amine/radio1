using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddApplicationInsightsTelemetry();

  
builder.Services.AddCors(options =>
{
	options.AddDefaultPolicy(
		policy =>
		{
			policy.WithOrigins("http://example.com",
								"http://www.contoso.com");
		});
});

builder.Services.Configure<FormOptions>(options =>
{
	options.MultipartBodyLengthLimit = 104837600; // 50 MB
});

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
				ValidateIssuerSigningKey = true,
				ValidateIssuer = false,
				ValidateAudience = false,
				ValidateLifetime = true,
				RequireExpirationTime = true,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection(key: "Jwt:Key").Value))
			};
	   });



var app = builder.Build();

 
//Middleware pour personalisé les erreurs d'authentification et d'autorisation
app.Use(async (context, next) =>
{
	await next();
	var statusCode = context.Response.StatusCode;
	if (statusCode == 401)
	{
		context.Response.Headers["Content-Type"] = "text/html; charset=utf-8";
		await context.Response.WriteAsync("<h1>Session Expiré</h1><p>Votre session est expiré tu dois s'identifier une autre foix .</p><a href=\"/Account/Logout\">Se connecter</a>");
	}
	else if (statusCode == 403)
	{
		context.Response.Headers["Content-Type"] = "text/html; charset=utf-8";
		await context.Response.WriteAsync("<h1>Accès refusé</h1><p>Vous n'êtes pas autorisé à accéder à cette ressource.</p>");
	}
});



if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();

app.UseCors();

app.UseRouting();



//Middleware pour l'insertion de token dans tous les requetes s'il exist
app.Use(async (context, next) =>
{
	var token = context.Request.Cookies["poupa_donuts"];
    if (!string.IsNullOrEmpty(token))
	{
		context.Request.Headers.Add("Authorization", "Bearer " + token);
    }
    await next();
});

app.UseAuthentication();
app.UseAuthorization();





app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



app.Run();
