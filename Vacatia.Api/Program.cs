using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.OpenApi;
using Vacatia.Api.Middleware;
using Vacatia.Application.Features.Solicitudes.Commands.CrearSolicitud;
using Vacatia.Infrastructure;

var builder = WebApplication.CreateBuilder(args);


// ─── Autenticación con Azure AD / Microsoft Identity Platform ──────────────
// IMPORTANTE: Valida issuer, audience y firma criptográfica del JWT automáticamente.
// Microsoft.Identity.Web gestiona la rotación de claves (JWKS) de Azure AD.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

// ─── Autorización con políticas ───────────-────────────────────────────────
builder.Services.AddAuthorization(opt =>
{
    // Política para aprobadores (rol en Azure AD App Registration)
    opt.AddPolicy("EsAprobador", policy => policy.RequireRole("Aprobador", "Manager", "Admin", "RRHH"));

    // Política basada en scope (delega permisos de la app SPFx)
    opt.AddPolicy("TieneScopeAcceso", policy => policy.RequireRole("scp", "vacatia.acceso", "vacatia.admin"));
});

// ─── Infraestructura de la aplicación ──────────────────────────────────────
builder.Services.AddInfraestructure(builder.Configuration);

// ─── Application ──────────────────────────────────────────────────────────
// MediatR - CQRS
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CrearSolicitudHandler).Assembly));

// FluentValidation
builder.Services.AddValidatorsFromAssembly(typeof(CrearSolicitudValidator).Assembly);

// Pipeline de validaciónn automático en MediatR
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

// ─── API ──────────────────────────────────────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => { 
    c.SwaggerDoc("v1", new() { Title="Vacatia API", Version="v1" });

    // Configurar autenticacion Bearer en Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("bearer", document)] = []
    });
});

// CORS para SPFx/Sharepoint
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("SPFxPolicy", policy =>
    {
        policy.WithOrigins(builder.Configuration.GetSection("AllowedOrigins").Get<string[]>() ?? ["https://*.sharepoint.com"])
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Logging estructurado (base para Application Insights)
builder.Logging.AddConsole();

var app = builder.Build();


// Pipeline de middleware personalizados
app.UseMiddleware<ExceptionMiddleware>(); //Manejo global de excepciones con formato consistente

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("SPFxPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
