using NPBank.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace NPBank.BusinessLogic
{
    public interface IPersonService
    {
        List<ListItemModel> GetListItemByCategory(string categoryName);
        ArrayList GetPersonInformation(FilterSortModel model);
        PersonModel GetPersonById(int PersonId);
        List<EducationModel> GetAcademicRecord(int PersonId);
        List<TrainingModel> GetTrainingRecord(int PersonId);
        ReturnMessageModel SavePersonInformation(PersonModel model);
        ReturnMessageModel DeleteRecord(int PersonId);
    }
}
