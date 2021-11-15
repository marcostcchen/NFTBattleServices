using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NFTBattleApi.Models;
using NFTBattleApi.Models.Settings;
using NFTBattleApi.Services;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "NFT Battle API",
        Description = "Trabalho de Sistemas Interativos da Escola Politécnica de São Paulo",
        Contact = new OpenApiContact
        {
            Name = "Github",
            Url = new Uri("https://github.com/marcostcchen/NFTBattleServices")
        },
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"Autorização Bearer. Insira 'Bearer' [espaço] em seguida o token de acesso. Exemplo: 'Bearer 12345abcdef'",
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

    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
    {
        openApiSecurityScheme,
        new List<string>()
    } });

});

builder.Services.Configure<MongoCollectionSettings>(builder.Configuration.GetSection(nameof(MongoCollectionSettings)));
builder.Services.AddSingleton<IMongoCollectionSettings>(sp => sp.GetRequiredService<IOptions<MongoCollectionSettings>>().Value);

builder.Services.Configure<TokenSettings>(builder.Configuration.GetSection(nameof(TokenSettings)));
builder.Services.AddSingleton<ITokenSettings>(sp => sp.GetRequiredService<IOptions<TokenSettings>>().Value);

builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<TokenService>();

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
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenSettings:JwtKey"]))
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();