using EasyHousingSolutionProjectAPI.Interface;
using EasyHousingSolutionProjectAPI.Models;
using EasyHousingSolutionProjectAPI.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using System.Text;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Build configuration
        var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

        // Set up the database connection string
        string cnstring = config.GetConnectionString("EasyHousingConStr");
        builder.Services.AddDbContext<EasyHousingSolutionProjectDbContext>(options =>
            options.UseSqlServer(cnstring));

        // Add services to the container
        builder.Services.Configure<JWTSettings>(config.GetSection("Jwt")); // Bind JWT settings from config
        builder.Services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<JWTSettings>>().Value); // Inject JWTSettings

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddHttpClient();

        // Register repositories
        builder.Services.AddTransient<IBuyerRepository, BuyerRepository>();
        builder.Services.AddScoped<IPropertyRepository, PropertyRepository>();
        builder.Services.AddTransient<ISellerRepository, SellerRepository>();
        builder.Services.AddScoped<IImageRepository, ImageRepository>();
        builder.Services.AddTransient<IWishlistRepository, WishlistRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IStateRepository, StateRepository>();
        builder.Services.AddScoped<ICityRepository, CityRepository>();
        builder.Services.AddSingleton<JwtHelper>();

        // Authentication with JWT
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                // Access the JWT settings from DI
                var jwtSettings = builder.Services.BuildServiceProvider().GetRequiredService<IOptions<JWTSettings>>().Value;

                // Check if the SecretKey is missing or empty
                if (string.IsNullOrEmpty(jwtSettings.SecretKey))
                {
                    throw new ArgumentNullException(nameof(jwtSettings.SecretKey), "Secret key cannot be null or empty.");
                }

                // Set token validation parameters
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience
                };
            });

        // var allowOrigins =builder.Configuration.GetValue<string>("allowedOrigins")!.Split(",");

        //builder.Services.AddCors(Options =>
        //{
        //    Options.AddDefaultPolicy(policy =>
        //    {
        //        policy.WithOrigins("").AllowAnyHeader().AllowAnyMethod();
        //    });
        //});
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAngularApp",
                builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            //.AllowCredentials());
        });
        var app = builder.Build();
      


        // Configure the HTTP request pipeline
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

    //    app.UseCors(policy =>
    //policy.AllowAnyOrigin()
    //      .AllowAnyMethod()
    //      .AllowAnyHeader());

        app.UseHttpsRedirection();
        app.UseCors("AllowAngularApp");

        // Ensure Authentication and Authorization middleware are in place
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
