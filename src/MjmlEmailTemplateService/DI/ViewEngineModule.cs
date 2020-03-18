using Autofac;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.NodeServices;
using MjmlEmailTemplateService.ViewEngineServices;

namespace MjmlEmailTemplateService.DI
{
    public class ViewEngineModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(v => new ViewProcessor(v.Resolve<IRazorViewEngine>(), v.Resolve<ITempDataProvider>(), v.Resolve<INodeServices>())).As<IViewProcessor>();
        }
    }
}