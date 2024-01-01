using System.Text;
using ClassNegarService.Db;
using ClassNegarService.Repos;
using ClassNegarService.Repos.Notification;
using ClassNegarService.Repos.Report;
using ClassNegarService.Repos.Session;
using ClassNegarService.Services;
using ClassNegarService.Services.Notification;
using ClassNegarService.Services.Report;
using ClassNegarService.Services.Session;
using ClassNegarService.Services.WebSocket;
using ClassNegarService.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

builder.Services.AddSingleton<WebSocketService>();
builder.Services.AddScoped<IAuthRepo, AuthRepo>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IClassRepo, ClassRepo>();
builder.Services.AddScoped<IClassService, ClassService>();
builder.Services.AddScoped<INotificationRepo, NotificationRepo>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddScoped<ISessionRepo, SessionRepo>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IReportRepo, ReportRepo>();

builder.Services.AddDbContext<ClassNegarDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("ConnStr")));



builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})

.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,

        ValidAudience = configuration["JWT:ValidAudience"],
        ValidIssuer = configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
    };
});



builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "Deadline Service", Version = "v1" });
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
{
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
});
});



builder.Services.AddControllers();
//builder.Services.AddDistributedMemoryCache();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseWebSockets();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


app.Use(async (context, next) =>
{
    if (context.Request.Path == "/ws")
    {
        if (context.WebSockets.IsWebSocketRequest)
        {
            try
            {
                var token = context.Request.Headers["Authorization"];
                var validateObj = new TokenValidateUtils(configuration);
                var claims = validateObj.ValidateToken(token)!.Claims;
                var claim = context.User.Claims.Where(x => x.Type == "user_id").FirstOrDefault();
                if (claim == null) throw new UnauthorizedAccessException();
                var userId = int.Parse(claim.Value);
                var wsService = context.RequestServices.GetRequiredService<WebSocketService>();
                await wsService.HandleWebSocketAsync(context, userId);
            }
            catch (Exception)
            {
                context.Response.StatusCode = 403;
            }
        }
        else
        {
            context.Response.StatusCode = 400;
        }
    }
    else
    {
        await next();
    }
});



app.MapControllers();

app.Run();
