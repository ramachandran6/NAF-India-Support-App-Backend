using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NISA.DataAccessLayer;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DBContext>(options => options.UseSqlServer(
       builder.Configuration.GetConnectionString("DbConnection")));
builder.Services.AddCors(option =>
        option.AddDefaultPolicy(policy =>
        {
            policy.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();

        }
        ));

builder.Services.AddAuthentication("Bearer").AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateAudience = false,
        ValidateIssuer = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF32.GetBytes(
        builder.Configuration.GetSection("AppSettings:Token").Value!))
    };
});

var app = builder.Build();
app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors();
app.MapControllers();

app.Run();
