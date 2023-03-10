global using Microsoft.EntityFrameworkCore;
global using ShopAPI.Models.Users;
global using ShopAPI.Models.Products;
global using ShopAPI.Models.Carts;
global using ShopAPI.Models.Checkouts;
global using ShopAPI.Data;
global using System.Text.Json.Serialization;
global using ShopAPI;
global using Microsoft.OpenApi.Models;
global using Microsoft.IdentityModel.Tokens;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Swashbuckle.AspNetCore.Filters;
global using System.Text;
global using ShopAPI.Services.Users;
global using ShopAPI.Services.Products;
global using ShopAPI.Services.Carts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddAutoMapper(typeof(Program).Assembly);

// Add services for Dependency Injection
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddHttpContextAccessor();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

// Add Database Service.
//builder.Services.AddDbContext<DataContext>();
builder.Services.AddDbContext<DataContext>(options =>
{
    // Configure SQL Server
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Add Stripe Infrastructure
builder.Services.AddStripeInfrastructure(builder.Configuration);

builder.Services.AddCors(options => options.AddPolicy(name: "NgOrigins",
    policy =>
    {
        policy.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader();
    }));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("NgOrigins");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
