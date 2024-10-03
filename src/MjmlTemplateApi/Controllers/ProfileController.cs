using Microsoft.AspNetCore.Mvc;
using MjmlEmailTemplate.Shared.Models.Profile;
using MjmlTemplateApi.ViewEngineServices;

namespace MjmlTemplateApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : Controller
    {
        private readonly IViewProcessor _viewProcessor;

        public ProfileController(IViewProcessor viewProcessor)
        {
            _viewProcessor = viewProcessor;
        }

        [HttpPost("profileRegistrationSuccess")]
        public async Task<ActionResult> ProfileRegistrationSuccess([FromBody] ProfileRegistrationSuccessModel model, [FromHeader] bool isDebug = false)
        {
            if (isDebug)
                return await _viewProcessor.DebugView(model, ControllerContext);

            var body = await _viewProcessor.RenderHtml(model, ControllerContext);

            return Created("Ok", body);
        }
    }
}
