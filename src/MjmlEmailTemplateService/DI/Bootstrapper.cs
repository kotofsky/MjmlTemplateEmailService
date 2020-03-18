using Autofac;
using Microsoft.Extensions.Configuration;

namespace MjmlEmailTemplateService.DI
{
    public class Bootstrapper
    {
        public static void ConfigureIoC(ContainerBuilder builder,IConfiguration configuration)
        {
            builder.RegisterModule<ViewEngineModule>();
            builder.RegisterModule<NodeJsModule>();
        }
    }
}