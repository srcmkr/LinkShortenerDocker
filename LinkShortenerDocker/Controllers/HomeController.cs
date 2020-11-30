using HashidsNet;
using LinkShortenerDocker.Lib;
using LinkShortenerDocker.Models;
using LinkShortenerDocker.ViewModels;
using LiteDB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace LinkShortenerDocker.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private Hashids _hashId;
        private string UrlPrefix; // "https://d3v.to/l"
        private string AdminPassword;

        public HomeController(ILogger<HomeController> logger, IConfiguration config)
        {
            _logger = logger;
            _hashId = new Hashids();
            UrlPrefix = EnvironmentHelpers.GetEnvironmentVariable("URL_PREFIX");
            AdminPassword = EnvironmentHelpers.GetEnvironmentVariable("ADMIN_PASSWORD");
        }

        [HttpGet("{id}")]
        public IActionResult Index(string id)
        {
            var linkId = 1;
            var url = UrlPrefix;

            try
            {
                var linkIds = _hashId.Decode(id);
                if (linkIds.Length > 0) linkId = linkIds[0];

                using var db = new LiteDatabase("linkshortener.db");
                var col = db.GetCollection<LinkModel>("links");
                var foundLink = col.FindOne(c => c.Id == linkId);
                if (foundLink != null) 
                    url = foundLink.RedirectTargetUrl;

            } catch (Exception ex)
            {
                _logger.LogWarning("Could not resolve hashid to integer", ex);
            }

            return RedirectPermanent(url);
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            var vm = new CreateLinkViewModel
            {
                NewLink = string.Empty,
                Password = string.Empty
            };

            return View(vm);
        }

        [HttpPost("create")]
        public IActionResult PostCreate([FromForm]CreateLinkViewModel vm)
        {
            if (!ModelState.IsValid) return RedirectToAction(nameof(Create));
            if (!vm.Password.Equals(AdminPassword)) return RedirectToAction(nameof(Create));

            try
            {
                using var db = new LiteDatabase("linkshortener.db");
                var col = db.GetCollection<LinkModel>("links");
                var newIdInt = col.Insert(new LinkModel { RedirectTargetUrl = vm.NewLink });
                var newIdHashed = _hashId.Encode(newIdInt);

                var vm2 = new CreatedLinkViewModel
                {
                    NewLink = vm.NewLink,
                    LinkHash = $"{UrlPrefix}/{newIdHashed}"
                };

                return View(vm2);
            } catch (Exception ex)
            {
                _logger.LogError("Error while DB insert", ex);
            }
            
            return RedirectToAction(nameof(Create));
        }
    }
}
