using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExemploGraph.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenIDChallengeDay.ViewModels;

namespace OpenIDChallengeDay.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGraphSdkHelper _graphSdkHelper;

        public HomeController(IGraphSdkHelper graphSdkHelper)
        {
            this._graphSdkHelper = graphSdkHelper;
        }

        // GET: Home
        [Authorize]
        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var identifier = User.FindFirst(Startup.ObjectIdentifierType).Value;

                var client = this._graphSdkHelper.GetAuthenticatedClient(identifier);

                var info = await client.Me.Request().GetAsync();

                return View(UserVm.ToViewModel(info));
            }

            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> SendMail(IFormCollection keyValuePairs)
        {
            if (User.Identity.IsAuthenticated)
            {
                var identifier = User.FindFirst(Startup.ObjectIdentifierType).Value;

                var client = this._graphSdkHelper.GetAuthenticatedClient(identifier);

                var info = await client.Me.Request().GetAsync();

                var body = new Microsoft.Graph.ItemBody
                {
                    Content = keyValuePairs["mailBody"].ToString()
                };

                var email = new Microsoft.Graph.Message
                {
                    Body = body,
                    From = new Microsoft.Graph.Recipient { EmailAddress = new Microsoft.Graph.EmailAddress { Name = info.DisplayName, Address = info.Mail } },
                    Importance = Microsoft.Graph.Importance.High,
                    Subject = "This is a Graph API Message",
                    ToRecipients = new List<Microsoft.Graph.Recipient> { new Microsoft.Graph.Recipient { EmailAddress = new Microsoft.Graph.EmailAddress { Name = info.DisplayName, Address = info.Mail } } }
                };

                await client.Me.SendMail(email, true).Request().PostAsync();

                return RedirectToAction("MailSent");
            }

            return RedirectToAction("Index");
        }

        public ActionResult MailSent()
        {
            return Ok("Mail sent successfully");
        }

        // GET: Home/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Home/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Home/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Home/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Home/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
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

        // GET: Home/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Home/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
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