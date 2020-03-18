using System;
using System.IO;
using Autofac;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.Extensions.Logging;

namespace MjmlEmailTemplateService.DI
{
    public class NodeJsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c =>
            {
                var loggerFactory = c.Resolve<ILoggerFactory>();

                var logger = loggerFactory.CreateLogger("NodeJs");

                var options = new NodeServicesOptions(c.Resolve<IServiceProvider>())
                {
                    ProjectPath = Path.Combine(Directory.GetCurrentDirectory(), "NodejsConfiguration"),
                    NodeInstanceOutputLogger = logger
                };

                return NodeServicesFactory.CreateNodeServices(options);
            }).As<INodeServices>().SingleInstance();
        }
    }
}