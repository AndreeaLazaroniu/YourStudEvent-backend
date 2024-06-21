using BEYourStudEvents.ChatConfig;
using BEYourStudEvents.Data;
using BEYourStudEvents.Entities;
using BEYourStudEvents.Interfaces;
using BEYourStudEvents.Repositories;
using BEYourStudEvents.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
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

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

// Add services to the container.
builder.Services.AddDbContext<YSEDBContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("YourStudEventsContext");
    options.UseSqlServer(connectionString);
});

builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequiredLength = 10;
    }
).AddEntityFrameworkStores<YSEDBContext>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme =
            options.DefaultChallengeScheme =
                options.DefaultForbidScheme =
                    options.DefaultScheme =
                        options.DefaultSignInScheme =
                            options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
    }
).AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JWT:Audience"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"])
            )
        };
    }
);

// builder.Services.AddCors(options =>  
// {  
//       
//     options.AddDefaultPolicy(  
//         policy =>  
//         {  
//             policy.WithOrigins("http://localhost:3000")  
//                 .AllowAnyHeader()  
//                 .AllowAnyMethod();  
//         });  
// });

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<IEventService, EventService>();
builder.Services.AddTransient<IFileService, FileService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IRepository<Category>, CategoryRepository>();
builder.Services.AddTransient<IRepository<Event>, EventRepository>();
builder.Services.AddSignalR();

var app = builder.Build();
// app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "content")),
    RequestPath = "/content"
});

app.UseHttpsRedirection();

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials()
    //.WithOrigins("https://localhost:44351))
    .SetIsOriginAllowed(origin => true));

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseRouting();

app.UseEndpoints(endpoints =>
    {
        endpoints.MapHub<ChatHub>("/chathub");
    }
);

app.Run();
