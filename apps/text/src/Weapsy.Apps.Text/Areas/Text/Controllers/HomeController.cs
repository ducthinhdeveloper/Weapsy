using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Weapsy.Apps.Text.Domain;
using Weapsy.Apps.Text.Domain.Commands;
using Weapsy.Apps.Text.Reporting;
using Weapsy.Infrastructure.Dispatcher;
using Weapsy.Mvc.Context;
using Weapsy.Mvc.Controllers;

namespace Weapsy.Apps.Text.Areas.Text.Controllers
{
    public class HomeController : BaseAdminController
    {
        private readonly ITextModuleFacade _textFacade;
        private readonly ICommandSender _commandSender;

        public HomeController(ITextModuleFacade textFacade,
            ICommandSender commandSender,
            IContextService contextService)
            : base(contextService)
        {
            _textFacade = textFacade;
            _commandSender = commandSender;
        }

        public async Task<IActionResult> Index(Guid moduleId)
        {
            var model = await Task.Run(() => _textFacade.GetAdminModel(SiteId, moduleId));
            return View(model);
        }

        public async Task<IActionResult> Save(AddVersion model)
        {
            model.SiteId = SiteId;
            model.VersionId = Guid.NewGuid();
            await Task.Run(() => _commandSender.Send<AddVersion, TextModule>(model));
            return Ok(string.Empty);
        }
    }
}
