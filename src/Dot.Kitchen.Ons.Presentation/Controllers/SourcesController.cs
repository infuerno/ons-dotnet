using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dot.Kitchen.Ons.Application.Commands;
using Dot.Kitchen.Ons.Application.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Dot.Kitchen.Ons.Application.Queries;

namespace Dot.Kitchen.Ons.Presentation.Controllers
{
    public class SourcesController : Controller
    {
        private IGetSourceListQuery _listQuery;
        private ICreateSourceCommand _createCommand;

        public SourcesController(IGetSourceListQuery listQuery, ICreateSourceCommand createCommand)
        {
            if (listQuery == null)
                throw new ArgumentNullException(nameof(listQuery));
            if (createCommand == null)
                throw new ArgumentNullException(nameof(createCommand));

            _listQuery = listQuery;
            _createCommand = createCommand;
        }

        // GET: Sources
        public IActionResult Index()
        {
            var sources = _listQuery.Execute();
            return View(sources);
        }

        // GET: Sources/Details/5
        public IActionResult Details(int id)
        {
            return View();
        }

        // GET: Sources/Create
        public IActionResult Create()
        {
            return View(new SourceModel());
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
                    _createCommand.Execute(model);
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    // log error or put something in the view error??
                }
            }
            return View();
        }

        // GET: Sources/Edit/5
        public IActionResult Edit(int id)
        {
            return View();
        }

        // POST: Sources/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Sources/Delete/5
        public IActionResult Delete(int id)
        {
            return View();
        }

        // POST: Sources/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}