using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class hhController : Controller
    {
        public IActionResult Index()
        {
            var nazvaniaProfesiiyBolshe120000 = new List<string>();
            var nazvaniaProfesiiyDo15000 = new List<string>();
            var nazvaniaNavikovBolshe120000 = new List<string>();
            var nazvaniaNavikovDo15000 = new List<string>();

            var webClient = new WebClient();
            
            for (var page = 1; page < 5; page++)
            {
                webClient.Headers.Add("User-Agent", "HH-User-Agent");
                webClient.Headers.Add("Content-Type", "application/json");
                var poisk = Newtonsoft.Json.JsonConvert.DeserializeObject<ResultatPoiska>(webClient.DownloadString("https://api.hh.ru/vacancies?currency=RUR&only_with_salary=true&order_by=publication_time&page=" + page + "&per_page=100"));

                foreach (var vacansiya in poisk.items)
                {
                    webClient.Headers.Add("User-Agent", "HH-User-Agent");
                    webClient.Headers.Add("Content-Type", "application/json");
                    var podrobno = Newtonsoft.Json.JsonConvert.DeserializeObject<hh>(webClient.DownloadString("https://api.hh.ru/vacancies/" + vacansiya.id));
                    if (podrobno.salary.from >= 120000)
                    {
                        foreach (var professiay in podrobno.specializations)
                        {
                            if (!nazvaniaProfesiiyBolshe120000.Contains(professiay.name))
                            {
                                nazvaniaProfesiiyBolshe120000.Add(professiay.name);
                            }
                        }
                        foreach (var navik in podrobno.key_skills)
                        {
                            if (!nazvaniaNavikovBolshe120000.Contains(navik.name))
                            {
                                nazvaniaNavikovBolshe120000.Add(navik.name);
                            }
                        }
                    }
                    if (podrobno.salary.to < 15000)
                    {
                        foreach (var professiay in podrobno.specializations)
                        {
                            if (!nazvaniaProfesiiyDo15000.Contains(professiay.name))
                            {
                                nazvaniaProfesiiyDo15000.Add(professiay.name);
                            }
                        }
                        foreach (var navik in podrobno.key_skills)
                        {
                            if (!nazvaniaNavikovDo15000.Contains(navik.name))
                            {
                                nazvaniaNavikovDo15000.Add(navik.name);
                            }
                        }
                    }
                }
            }

            ViewBag.nazvaniaProfesiiyBolshe120000 = nazvaniaProfesiiyBolshe120000;
            ViewBag.nazvaniaProfesiiyDo15000 = nazvaniaProfesiiyDo15000;
            ViewBag.nazvaniaNavikovBolshe120000 = nazvaniaNavikovBolshe120000;
            ViewBag.nazvaniaNavikovDo15000 = nazvaniaNavikovDo15000;

            return View();
        }
    }
}