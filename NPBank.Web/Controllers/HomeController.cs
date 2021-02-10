using NPBank.BusinessLogic;
using NPBank.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NPBank.Web.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        private IPersonService _iPersonService;
        ReturnMessageModel rtnMsg;
        public HomeController()
        {
            _iPersonService = new PersonService();
            rtnMsg = new ReturnMessageModel();
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetPersonInformation(FilterSortModel model)
        {
            var list = _iPersonService.GetPersonInformation(model);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddEditPerson(int ? PersonId)
        {
            PersonModel model = new PersonModel();
            if (PersonId > 0)
            {
                model = _iPersonService.GetPersonById((int)PersonId);
                return PartialView(model);
            }
            else
            {
                model.DateOfBirth = DateTime.Now;
                return View(model);
            }
            
        }

        [HttpGet]
        public ActionResult GetListItem(string categoryName)
        {
            var list = _iPersonService.GetListItemByCategory(categoryName)?.ToList();
            return Json(list,JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SavePersonInformation(PersonModel model)
        {
            rtnMsg = _iPersonService.SavePersonInformation(model);
            return Json(rtnMsg);
        }
        public ActionResult GetAcademicRecord(int personId)
        {
            var list = _iPersonService.GetAcademicRecord(personId);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetTrainingRecord(int personId)
        {
            var list = _iPersonService.GetTrainingRecord(personId);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteRecord(int personId)
        {
            var msg = _iPersonService.DeleteRecord(personId);
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
    }
}