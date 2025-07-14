using MediatR;
using Microsoft.EntityFrameworkCore;
using SkopiaManager.API.Extensions;
using SkopiaManager.Application.Handlers.Commands;
using SkopiaManager.Infrastructure.Data;
using SkopiaManager.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddMediatR(typeof(CreateProjectCommandHandler).Assembly)
                .AddDbContext<SkopiaDbContext>(options =>options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")))
                .AddInfrastructure()
                .AddEndpointsApiExplorer()
                .AddSwaggerGen(c => { c.EnableAnnotations(); })
                .AddControllers();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();
app.MigrateDatabase();
app.MapControllers();

app.Run();
