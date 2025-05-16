using System.Reflection;

using Feedback.ExternalServices.Telegram;
using Feedback.ExternalServices.ToneService;
using Feedback.Infrastructure;

using MediatR;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Feedback.Presentation;

public class Startup
{
    private const string DomainServiceAssembly = "Feedback.DomainServices";
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins",
                policy =>
                {
                    policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
        });

        services.AddControllers();

        services.AddSwaggerGen();

        services
            .AddInfrastructure(Configuration);

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.Load(DomainServiceAssembly)));

        var provider = services.BuildServiceProvider();

        var mediator = provider.GetRequiredService<IMediator>();

        services.AddTelegramClient(mediator, Configuration);
        services.AddToneService(Configuration);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseRouting();

        app.UseCors("AllowAllOrigins");

        app.UseEndpoints(
            endpoints =>
            {
                endpoints.MapControllers();
            });
    }
}