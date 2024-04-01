using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PT1_API.Extensions;
using PT1_API.Utilities.OutputFormatter;
using Repository;
using Service;
using Service.Telegram;
using Shared.DTO.Mail;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(config =>
{
    config.RespectBrowserAcceptHeader = true;
    config.ReturnHttpNotAcceptable = true;

}).AddXmlDataContractSerializerFormatters()
    .AddMvcOptions(c => c.OutputFormatters.Add(new CsvOutputFormatters()));

//builder.Services.AddControllers(config =>
//{
//    config.RespectBrowserAcceptHeader = true;
//    config.ReturnHttpNotAcceptable = true; // 406
//})
//    .AddXmlDataContractSerializerFormatters()
//    .AddJsonOptions(options =>
//    {
//        options.JsonSerializerOptions.PropertyNamingPolicy = null;
//    })
//    .AddCustomCSVFormatter();



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(opts =>
{
    opts.AddPolicy("CORSPolicy", builder => builder.AllowAnyHeader().AllowAnyMethod().AllowCredentials().SetIsOriginAllowed((host) => true));
});
builder.Services.AddUserService();
builder.Services.AddDepartmentService();
builder.Services.AddJobPostionService();
builder.Services.AddProjectService();
builder.Services.AddPhaseService();
builder.Services.AddLevelService();
builder.Services.AddLogWorkService();
//builder.Services.AddTelegramService();
//load thông tin cấu hình và lưu vào đối tượng MailSetting
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
//add dependency inject cho MailService
builder.Services.AddTransient<IMailService, MailService>();

var key = Encoding.ASCII.GetBytes("$2a$12$atfA/R0/eug64md1RHG.ROPhIUBlQrZy2Ags9Dx1O6xm9nAm/LRcG");
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = true;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };
}); ;
builder.Services.AddAutoMapper(typeof(Program));


var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();
builder.Services.AddDbContext<RepositoryContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

builder.Services.BuildServiceProvider().GetService<RepositoryContext>().Database.Migrate();
builder.Services.AddHttpContextAccessor();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CORSPolicy");

app.Use(async (context, next) =>
{
    string authHeader = context.Request.Headers["Authorization"];
    if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
    {
        var token = authHeader.Substring("Bearer ".Length).Trim();
        context.Items["Token"] = token;
    }
    await next();
});


app.UseAuthentication();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
