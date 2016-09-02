using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using Teclyn.AspNetMvc.Mvc.Models;
using Teclyn.Core;
using Teclyn.Core.Commands;
using Teclyn.Core.Errors.Models;
using Teclyn.Core.Events;
using Teclyn.Core.Ioc;
using Teclyn.Core.Storage;

namespace Teclyn.AspNetMvc.Mvc.Controllers
{
    public class CommandController : Controller
    {
        [Inject]
        public CommandService CommandService { get; set; }

        [Inject]
        public RepositoryService RepositoryService { get; set; }

        [Inject]
        public IRepository<IError> ErrorRepository { get; set; }

        [Inject]
        public IRepository<IEventInformation> EventInformationRepository { get; set; }

        public ActionResult Index()
        {
            return this.Content("Index OK");
        }

        [HttpPost]
        public ActionResult Execute(IBaseCommand command)
        {
            var result = this.CommandService.ExecuteGeneric(command);

            return this.Structured(result);
        }

        [HttpPost]
        public ActionResult ExecutePost(IBaseCommand command, string returnUrl)
        {
            this.CommandService.ExecuteGeneric(command);
            
            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                returnUrl = this.Request.UrlReferrer?.ToString();
            }

            return Redirect(returnUrl);
        }
        
        public ActionResult Info()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;

            var model = new HomeInfoModel
            {
                Aggregates = this.RepositoryService.Aggregates.Select(agg => new AggregateInfoModel
                {
                    AggregateType = agg.AggregateType.ToString(),
                    ImplementationType = agg.ImplementationType.ToString(),
                }).ToArray(),
                TeclynVersion = version,
                Commands = this.CommandService.GetAllInfo(),
            };

            return this.Structured(model);
        }

        public ActionResult Errors()
        {
            var errors = this.ErrorRepository
                .OrderByDescending(error => error.Date)
                .Take(10)
                .ToList();

            return this.Structured(errors);
        }

        public ActionResult Events()
        {
            var events = this.EventInformationRepository
                .OrderByDescending(e => e.Date)
                .Take(100)
                .ToList();

            return this.Structured(events);
        }
    }
}