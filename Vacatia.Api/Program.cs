using Microsoft.AspNetCore.Authentication.JwtBearer;

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
builder.Services.AddValidatorFromAssembly(typeof(CrearSolicitudValidator).Assembly);

// Pipeline de validaciónn automático en MediatR
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));

// ─── API ──────────────────────────────────────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builders.Services.AddSwaggerGen(c => { 

});
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
