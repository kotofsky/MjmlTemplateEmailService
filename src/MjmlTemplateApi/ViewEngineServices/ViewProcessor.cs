using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using MjmlEmailTemplate.Shared.Models;
using MjmlTemplateApi.Models;
using NeuroSpeech.AspNet.NodeServices;

namespace MjmlTemplateApi.ViewEngineServices;

public sealed class ViewProcessor : Controller, IViewProcessor
{
    private readonly IRazorViewEngine _viewEngine;
    private readonly ITempDataProvider _tempDataProvider;
    private readonly INodeServices _nodeServices;

    public ViewProcessor(IRazorViewEngine viewEngine, ITempDataProvider tempDataProvider, IServiceProvider sp)
    {
        var options = new NodeServicesOptions(sp)
        {
            ProjectPath = Path.Combine(Directory.GetCurrentDirectory(), "NodejsConfiguration")
        };


        _nodeServices = NodeServicesFactory.CreateNodeServices(options);


        _viewEngine = viewEngine;
        _tempDataProvider = tempDataProvider;
    }

    public async Task<ViewResult> DebugView(NotifyBaseModel model, ControllerContext controllerContext)
    {
        var htmlView = await GetHtmlView(model, controllerContext);
        return View("Html", new DebugHtmlViewModel { HtmlRaw = htmlView });
    }

    public async Task<string> RenderHtml(NotifyBaseModel model, ControllerContext controllerContext)
    {
        var htmlView = await GetHtmlView(model, controllerContext);
        return htmlView;
    }

    private async Task<string> GetHtmlView(NotifyBaseModel model, ControllerContext controllerContext)
    {
        var viewName = model.GetRoute();
        var language = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;

        var templatePath = $"{viewName}.{language}";

        var sourceView = await RenderViewToString(templatePath, model, controllerContext);

        var htmlView = "";

        try
        {
            htmlView = await _nodeServices.InvokeAsync<string>("mjml-init.js", sourceView);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return htmlView;
    }

    private async Task<string> RenderViewToString<TModel>(string viewName, TModel model, ControllerContext controllerContext)
    {
        if (string.IsNullOrEmpty(viewName))
            viewName = controllerContext.ActionDescriptor.ActionName;

        ViewData.Model = model;

        try
        {
            using (var writer = new StringWriter())
            {
                var view = FindView(controllerContext, viewName);

                ViewContext viewContext = new ViewContext(
                    controllerContext,
                    view,
                    ViewData,
                    new TempDataDictionary(controllerContext.HttpContext, _tempDataProvider),
                    writer,
                    new HtmlHelperOptions()
                );

                await view.RenderAsync(viewContext);

                return writer.GetStringBuilder().ToString();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private IView FindView(ActionContext actionContext, string viewName)
    {
        var getViewResult = _viewEngine.GetView(executingFilePath: null, viewPath: viewName, isMainPage: true);
        if (getViewResult.Success)
        {
            return getViewResult.View;
        }

        var findViewResult = _viewEngine.FindView(actionContext, viewName, isMainPage: true);
        if (findViewResult.Success)
        {
            return findViewResult.View;
        }

        var searchedLocations = getViewResult.SearchedLocations.Concat(findViewResult.SearchedLocations);
        var errorMessage = string.Join(
            Environment.NewLine,
            new[] { $"Unable to find view '{viewName}'. The following locations were searched:" }.Concat(searchedLocations)); ;

        throw new InvalidOperationException(errorMessage);
    }
}

public interface IViewProcessor
{
    Task<ViewResult> DebugView(NotifyBaseModel model, ControllerContext controllerContext);

    Task<string> RenderHtml(NotifyBaseModel model, ControllerContext controllerContext);
}