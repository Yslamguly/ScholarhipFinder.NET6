using ScholarhipFinderAPI.Data;
using ScholarhipFinderAPI.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Get connection string from appsettings.json and connect to database
builder.Services.AddEntityFrameworkNpgsql()
    .AddDbContext<ApiDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("SampleDbConnection")));

//Configure JWT secret key
builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));


//Let the request know that there is an authentication
builder.Services.AddAuthentication(options=>{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(jwt=>{
    var key = Encoding.ASCII.GetBytes(builder.Configuration.GetSection("JwtConfig:Secret").Value);

    //keep the token in the header
    jwt.SaveToken = true;
    jwt.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false, //for dev
        ValidateAudience = false, //for dev
        RequireExpirationTime = false, //for dev -- needs to be updated when refresh token is added
        ValidateLifetime = true
    };
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
