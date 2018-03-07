using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Teclyn.Core.Commands;
using Teclyn.Core.Errors.Models;
using Teclyn.Core.Events;
using Teclyn.Core.Security.Context;
using Teclyn.Core.Storage;

namespace Teclyn.AspNetMvc.Mvc.Controllers
{
    public class CommandController : Controller
    {
        public CommandService CommandService { get; set; }
        
        public IRepository<IError> ErrorRepository { get; set; }
        
        public IRepository<IEventInformation> EventInformationRepository { get; set; }
        
        public ITeclynContext Context { get; set; }
        
        [HttpPost]
        public async Task<ActionResult> Execute(ICommand command)
        {
            var result = await this.CommandService.Execute(command);

            return this.Structured(result);
        }
        
        [HttpPost]
        public async Task<ActionResult> ExecutePost(ICommand command, string returnUrl)
        {
            await this.CommandService.Execute(command);
            
            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                returnUrl = this.Request.UrlReferrer?.ToString();
            }

            return Redirect(returnUrl);
        }
        
        //[OnlyAdminFilter]
        public ActionResult Info()
        {
            return null;
            //System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            //var fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            //string version = fvi.FileVersion;

            //var model = new HomeInfoModel
            //{
            //    Aggregates = this.RepositoryService.Aggregates.Select(agg => new AggregateInfoModel
            //    {
            //        AggregateType = agg.AggregateType.ToString(),
            //        ImplementationType = agg.ImplementationType.ToString(),
            //    }).ToArray(),
            //    TeclynVersion = version,
            //    Commands = this.MetadataRepository.Commands,
            //    Events = this.MetadataRepository.Events,
            //};

            //return this.Structured(model);
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