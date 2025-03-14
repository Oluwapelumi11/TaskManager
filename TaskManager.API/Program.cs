using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TaskManager.API.Data;
using TaskManager.API.Interfaces;
using TaskManager.API.Middlewares;
using TaskManager.API.Model;
using TaskManager.API.Repository;
using TaskManager.API.Service.Auth;

Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;
var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(c =>
    c.SwaggerDoc("v1", new() { Title = "Task_Manager", Version = "v1" })
    );



builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
    ));

string jwtKey = builder.Configuration["Jwt:Key"]!;
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

builder.Services.AddAuthentication( options =>
    {
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false; 
        options.SaveToken = true;
        options.UseSecurityTokenValidators = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidAudience = builder.Configuration["Jwt:Audience"]!,
            ValidIssuer = builder.Configuration["Jwt:Issuer"]!,
            IssuerSigningKey = key,
            ClockSkew = TimeSpan.Zero
        };
    });
builder.Services.Configure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine($"Authentication failed: {context.Exception.Message}");
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            Console.WriteLine("Token successfully validated!");
            return Task.CompletedTask;
        },
        OnChallenge = context =>
        {
            Console.WriteLine($"Challenge issued: {context.ErrorDescription}");
            return Task.CompletedTask;
        }
    };
});



//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowAll",
//        builder => builder.AllowAnyOrigin()
//                          .AllowAnyMethod()
//                          .AllowAnyHeader());
//});

builder.Services.AddEndpointsApiExplorer();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI();
    app.UseSwagger();
}


//app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();

app.Run();
