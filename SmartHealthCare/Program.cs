using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SmartHealthCare.Data;
using SmartHealthCare.Model;
using SmartHealthCare.Repositories;
using SmartHealthCare.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. Read JWT Settings from appsettings.json
var secretKey = builder.Configuration["JWT:Secret"]!;
var issuer = builder.Configuration["JWT:ValidIssuer"];
var audience = builder.Configuration["JWT:ValidAudience"];

// 2. Register Database (DbContext)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 3. Register Repositories (The Chefs)
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();

// 4. Register Security Services
builder.Services.AddScoped<PasswordHasher<User>>();
builder.Services.AddScoped<ITokenService, TokenService>();

// 5. Register JWT Authentication Handler
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = issuer,
            ValidAudience = audience,

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(secretKey)
            )
        };
    });
builder.Services.AddAuthorization(options =>
{
    // Policy 1: Sirf Doctor ke liye jiske paas DoctorId claim ho:
    options.AddPolicy("DoctorOnly", policy =>
        policy.RequireRole("Doctor").RequireClaim("DoctorId"));

    // Policy 2: Sirf Patient ke liye:
    options.AddPolicy("PatientOnly", policy =>
        policy.RequireRole("Patient"));
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 6. PIPELINE ORDER: Pehle Authentication (Identity), Phir Authorization (Permissions)
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();