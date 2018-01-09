using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dot.Kitchen.Ons.Application;
using Dot.Kitchen.Ons.Application.Commands;
using Dot.Kitchen.Ons.Application.Models;
using Microsoft.AspNetCore.Mvc;
using Dot.Kitchen.Ons.Application.Queries;
using Dot.Kitchen.Ons.Domain;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Dot.Kitchen.Ons.Presentation.Controllers
{
    public class ScrapesController : Controller
    {
        private IGetScrapeListQuery _listQuery;
        private IGetScrapeDetailQuery _detailQuery;
        private ITriggerScrapeCommand _scrapeCommand;

        public ScrapesController(IGetScrapeListQuery listQuery, IGetScrapeDetailQuery detailQuery, ITriggerScrapeCommand scrapeCommand)
        {
            if (listQuery == null)
                throw new ArgumentNullException(nameof(listQuery));
            if (detailQuery == null)
                throw new ArgumentNullException(nameof(detailQuery));
            if (scrapeCommand == null)
                throw new ArgumentNullException(nameof(scrapeCommand));

            _listQuery = listQuery;
            _detailQuery = detailQuery;
            _scrapeCommand = scrapeCommand;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            var scrapes = _listQuery.Execute();
            return View(scrapes);
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
                return NotFound();
            
            var scrape = _detailQuery.Execute(id.Value);

            if (scrape == null)
                return NotFound();
            
            return View(scrape);
        }

        // GET: Sources/Create
        public IActionResult Create()
        {
            return View(new ScrapeModel());
        }

        // POST: Sources/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(SourceModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //_createCommand.Execute(model);
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    // log error or put something in the view error??
                }
            }
            return View();
        }


    }
}
