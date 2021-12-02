using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NFTBattleApi.Models;
using NFTBattleApi.Models.Settings;
using NFTBattleApi.Services;
using System.Text;
using NFTBattleApi.Settings;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins, builder => { builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin(); });
});

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.IgnoreNullValues = true;
});
builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "NFT Battle API",
        Description = "Trabalho de Sistemas Interativos da Escola Politecnica de Sao Paulo",
        Contact = new OpenApiContact
        {
            Name = "Github",
            Url = new Uri("https://github.com/marcostcchen/NFTBattleServices")
        },
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description =
            @"Exemplo: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    var openApiSecurityScheme = new OpenApiSecurityScheme
    {
        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" },
        Scheme = "oauth2",
        Name = "Bearer",
        In = ParameterLocation.Header
    };

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            openApiSecurityScheme,
            new List<string>()
        }
    });
});

builder.Services.Configure<MongoCollectionSettings>(builder.Configuration.GetSection(nameof(MongoCollectionSettings)));
builder.Services.AddSingleton<IMongoCollectionSettings>(sp => sp.GetRequiredService<IOptions<MongoCollectionSettings>>().Value);

builder.Services.Configure<TokenSettings>(builder.Configuration.GetSection(nameof(TokenSettings)));
builder.Services.AddSingleton<ITokenSettings>(sp => sp.GetRequiredService<IOptions<TokenSettings>>().Value);

builder.Services.Configure<OpenSeaSettings>(builder.Configuration.GetSection(nameof(OpenSeaSettings)));
builder.Services.AddSingleton<IOpenSeaSettings>(sp => sp.GetRequiredService<IOptions<OpenSeaSettings>>().Value);

builder.Services.AddSingleton<LogService>();
builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<TokenService>();
builder.Services.AddSingleton<NftService>();
builder.Services.AddSingleton<OpenSeaService>();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
{
    opt.TokenValidationParameters = new()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["TokenSettings:JwtIssuer"],
        ValidAudience = builder.Configuration["TokenSettings:JwtIssuer"],
        IssuerSigningKey =
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenSettings:JwtKey"]))
    };
});

var app = builder.Build();
app.UseCors(MyAllowSpecificOrigins);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();