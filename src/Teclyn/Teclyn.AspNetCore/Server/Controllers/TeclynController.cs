using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Teclyn.Core.Api;

namespace Teclyn.AspNetCore.Server.Controllers
{
    public class TeclynController : Controller
    {
        private readonly ITeclynApi _teclynApi;
        
        public TeclynController(ITeclynApi teclynApi)
        {
            this._teclynApi = teclynApi;
        }

        public IActionResult Index()
        {
            var model = this._teclynApi.Domains.Select(d => new
            {
                Id = d.Id,
                Name = d.Name,
                Commands = d.Commands.Select(c => new
                {
                    Id = c.Id,
                    Name = c.Name,
                }),
                Aggregates = d.Aggregates.Select(a => new
                {
                    Id = a.Id,
                    Name = a.Name,
                })
            });

            return this.Json(model);
        }
    }
}