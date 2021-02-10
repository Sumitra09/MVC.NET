using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace NPBank.Model
{
    public class PersonModel
    {
        public PersonModel()
        {
            Education = new List<EducationModel>();
            Training = new List<TrainingModel>();
        }
        public int PersonId { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public int GenderListItemId { get; set; }
        [Required]
        public Nullable<DateTime> DateOfBirth { get; set; }
        [Required]
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        [Required]
        public string MobileNo { get; set; }
        [Required]
        public string Address { get; set; }
        public List<EducationModel> Education { get; set; }
        public List<TrainingModel> Training { get; set; }
        public IEnumerable<SelectListItem> ListItem { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }

    }
    public class EducationModel
    {
        public int AcademicRecordId { get; set; }
        public int PersonId { get; set; }
        public int EducationLevelListItemId { get; set; }
        public string InstituteName { get; set; }
        public double GPAObtained { get; set; }
        public Nullable<DateTime> CompletionYear { get; set; }
        public string Major { get; set; }
        public string Board { get; set; }
   
    }
    public class TrainingModel
    {
        public int TrainingId { get; set; }
        public int PersonId { get; set; }
        public string InstituteName { get; set; }
        public string Course { get; set; }
        public string Location { get; set; }
        public Nullable<DateTime> StartDate { get; set; }
        public Nullable<DateTime> EndDate { get; set; }
    }
}
