using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeledocTest.Models;
using System.Data.Entity.Validation;

namespace TeledocTest.Controllers
{
    public class HomeController : Controller
    {
        TeledocTestDBEntities db = new TeledocTestDBEntities();
        int _countOfRelations = 0;
        public ActionResult Index()
        {
            ViewData["Firms"]= db.Firms.ToList();
            ViewData["FirmsFounders"] = db.FirmsFounders.ToList();
            ViewData["Founders"] =  db.Founders.ToList();
            return View();
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetForm(Firm firm, List<Founder> founders)
        {
            try
            {
                if( string.IsNullOrWhiteSpace(firm.firm_name) || string.IsNullOrWhiteSpace(firm.INN))
                {
                    ViewData["result"] = "Поля \"Наименование организации\" и \"ИНН\" обязательны для заполнения.";
                    return View("Add");
                }

                var b1 = Int32.TryParse(firm.INN.Trim(), out int result1);
                var b2 = (firm.INN.Trim().Length != 10 && firm.INN.Trim().Length != 12);

                if (!Int32.TryParse(firm.INN.Trim(), out int result) || (firm.INN.Trim().Length != 10 && firm.INN.Trim().Length != 12))
                {
                    ViewData["result"] = "Поле \"ИНН\" должно состоять из 10 или 12 цифр.";
                    return View("Add");
                }

                _countOfRelations = db.FirmsFounders.Count();

                if(db.Firms.Where(f => (f.INN == firm.INN && f.firm_name == firm.firm_name)).ToList().Count > 0)
                {
                    firm = db.Firms.Where(f => f.INN == firm.INN).First();
                }
                else
                {
                    firm.firm_id = db.Firms.OrderByDescending(firm_id => firm_id.firm_id)
                                           .Select(firm_id => firm_id.firm_id)
                                           .First() + 1;
                    db.Firms.Add(firm);
                }
                for(int i = 0; i < founders.Count; i++)
                {
                    Founder founder = founders[i]; 
                    if (!string.IsNullOrWhiteSpace(founder.FirstName) && !string.IsNullOrWhiteSpace(founder.FirstName) && !string.IsNullOrWhiteSpace(founder.FirstName))
                    {
                        if (db.Founders.Where(f => (f.FirstName == founder.FirstName && f.SurName == founder.SurName && f.PatronymicName == founder.PatronymicName)).ToList().Count > 0)
                        {
                            founders[i] = db.Founders.Where(f => (f.FirstName == founder.FirstName && f.SurName == founder.SurName && f.PatronymicName == founder.PatronymicName)).First();
                        }
                        else
                        {
                            founder.FounderId = db.Founders.OrderByDescending(founder_id => founder_id.FounderId)
                                                           .Select(founder_id => founder_id.FounderId)
                                                           .First() + 1 + i;
                            db.Founders.Add(founder);
                        }

                        if (db.FirmsFounders.Where(ff => (ff.FirmId == firm.firm_id && ff.FounderId == founder.FounderId)).Count() < 1)
                        {
                            FirmsFounder firmsFounder = new FirmsFounder();
                            firmsFounder.FirmId = firm.firm_id;
                            firmsFounder.FounderId = founder.FounderId;
                            firmsFounder.Id = db.FirmsFounders.OrderByDescending(FirmsFounderId => FirmsFounderId.Id)
                                                              .Select(FirmsFounderId => FirmsFounderId.Id)
                                                              .First() + 1 + i;
                            db.FirmsFounders.Add(firmsFounder);
                        }
                    }
                }

                db.SaveChanges();

                ViewData["result"] = "Организация успешно добавлена.";
            }
            catch(Exception ex)
            {
                ViewData["result"] = "Произошла ошибка." + ex.Message;
                
            }
            return View("Add");
        }

    }
}