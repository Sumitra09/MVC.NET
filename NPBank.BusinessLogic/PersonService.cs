using NPBank.DataAccess;
using NPBank.Model;
using System;
using System.Activities.Statements;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace NPBank.BusinessLogic
{
    public class PersonService: IPersonService
    {
        private NPBankEntities nPBankEntities;
        public PersonService()
        {
            nPBankEntities = new NPBankEntities();
        }

        public List<ListItemModel> GetListItemByCategory(string categoryName)
        {
            var list = nPBankEntities.ListItems.Where(x => x.ListItemCategory.CategoryName == categoryName).OrderBy(m => m.ListItemId).Select(x => new ListItemModel
            {
                ListItemId = x.ListItemId,
                ListItemDisplayName = x.ListItemDisplayName
            }).ToList();
            return list;
        }

        public PersonModel GetPersonById(int PersonId)
        {
            PersonModel model = new PersonModel();
            model = nPBankEntities.People.Where(x => x.PersonId == PersonId).Select(x => new PersonModel
            {
                PersonId= x.PersonId,
                FirstName = x.FirstName,
                MiddleName = x.MiddleName,
                LastName = x.LastName,
                DateOfBirth = (DateTime)x.DateOfBirth,
                MobileNo = x.MobileNo,
                PhoneNo = x.PhoneNo,
                Email= x.Email,
                Address = x.Address,
                GenderListItemId = (int)x.GenderListItemId
            }).FirstOrDefault();
            return model;
        }
        public ReturnMessageModel SavePersonInformation(PersonModel model)
        {
            ReturnMessageModel returnMessageModel = new ReturnMessageModel();
            try
            {
                var data = nPBankEntities.People.Where(x => x.PersonId == model.PersonId).FirstOrDefault();
                if(data==null)
                {
                    data = new Person();
                }
                data.FirstName = model.FirstName;
                data.MiddleName = model.MiddleName;
                data.LastName = model.LastName;
                data.Email = model.Email;
                data.Address = model.Address;
                data.DateOfBirth = model.DateOfBirth;
                data.PhoneNo = model.PhoneNo;
                data.MobileNo = model.MobileNo;
                data.GenderListItemId = model.GenderListItemId;
                if (data.PersonId == 0)
                {
                    nPBankEntities.People.Add(data);
                    returnMessageModel.ReturnMessage = "Save successfully.!!";
                }
                else
                {
                    nPBankEntities.Entry(data).State = System.Data.Entity.EntityState.Modified;
                    returnMessageModel.ReturnMessage = "Update successfully.!!";
                }
                
                foreach (var item in model.Education)
                {
                    var edu = nPBankEntities.AcademicRecords.Where(x => x.AcademicRecordId == item.AcademicRecordId).FirstOrDefault();
                    if (edu== null)
                    {
                        edu = new AcademicRecord();
                    }
                    edu.EducationLevelListItemId = item.EducationLevelListItemId;
                    edu.InstituteName = item.InstituteName;
                    edu.Major = item.Major;
                    edu.Board = item.Board;
                    edu.GPAObtained = item.GPAObtained;
                    edu.CompletionYear = item.CompletionYear;

                    if (edu.AcademicRecordId == 0)
                    {
                        data.AcademicRecords.Add(edu);
                    }
                    else
                    {
                        nPBankEntities.Entry(edu).State = System.Data.Entity.EntityState.Modified;
                    }
                }
                foreach (var item in model.Training)
                {
                    var trai = nPBankEntities.Trainings.Where(x => x.TrainingId == item.TrainingId).FirstOrDefault();
                    if (trai == null)
                    {
                        trai = new Training();
                    }
                    trai.InstituteName = item.InstituteName;
                    trai.Course = item.Course;
                    trai.Location = item.Location;
                    trai.StartDate = (DateTime)item.StartDate;
                    trai.EndDate = (DateTime)item.EndDate;
                    if (trai.TrainingId == 0)
                    {
                        data.Trainings.Add(trai);
                    }
                    else
                    {
                        nPBankEntities.Entry(trai).State = System.Data.Entity.EntityState.Modified;
                    }
                }

                nPBankEntities.SaveChanges();
                returnMessageModel.IsSuccess = true;
                returnMessageModel.ReturnMessage = "save successfully.";

            }
            catch (Exception ex)
            {
                returnMessageModel.IsSuccess = false;
                returnMessageModel.ReturnMessage = ex.Message;
            }

            return returnMessageModel;
        }
        public ArrayList GetPersonInformation(FilterSortModel param)
        {
            try
            {
                ArrayList list = new ArrayList();
                SortModel sort = new SortModel();
                if (param.sort.Count() > 0)
                    sort = param.sort.FirstOrDefault();
                else
                {
                    sort.Dir = "";
                    sort.Field = "";
                }
                var outParam1 = new SqlParameter("@TotalRecordCount", SqlDbType.Int) { Direction = ParameterDirection.Output };

                var resultData = nPBankEntities.Database.SqlQuery<PersonModel>(@"
                   EXEC dbo.SpPersonInformationSel 
                   @OffsetRows,
                   @FetchRows,
                   @WhereClause,
                   @SortOrder,
                   @SortField,
                   @TotalRecordCount out",

                       new SqlParameter("@OffsetRows", param.skip),
                       new SqlParameter("@FetchRows", param.take),
                       new SqlParameter("@WhereClause", FilterSortHelper.BuildWhereClause(param.Filter)),
                       new SqlParameter("@SortOrder", sort.Dir),
                       new SqlParameter("@SortField", sort.Field),
                      outParam1
                    ).ToList();
                list.Add(resultData);
                list.Add(outParam1.Value);
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
        public List<EducationModel> GetAcademicRecord(int PersonId)
        {
           var model = nPBankEntities.AcademicRecords.Where(x => x.PersonId == PersonId).Select(x => new EducationModel
            {
                AcademicRecordId = x.AcademicRecordId,
                Board = x.Board,
                Major = x.Major,
                InstituteName = x.InstituteName,
                CompletionYear = (DateTime)x.CompletionYear,
                GPAObtained = (float)x.GPAObtained,
                EducationLevelListItemId = x.EducationLevelListItemId
            }).ToList();
            return model;
        }
        public List<TrainingModel> GetTrainingRecord(int PersonId)
        {
            var model = nPBankEntities.Trainings.Where(x => x.PersonId == PersonId).Select(x => new TrainingModel
            {
                TrainingId = x.TrainingId,
                InstituteName = x.InstituteName,
                Course = x.Course,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                Location = x.Location
            }).ToList();
            return model;
        }
        public ReturnMessageModel DeleteRecord(int PersonId)
        {
            ReturnMessageModel returnMessageModel = new ReturnMessageModel();
            try
            {
                var person = nPBankEntities.People.Where(x => x.PersonId == PersonId).FirstOrDefault();
                var rows = nPBankEntities.AcademicRecords.Where(x => x.PersonId == PersonId).ToList();
                foreach (var item in rows)
                {
                    var educationRow = nPBankEntities.AcademicRecords.Where(x => x.AcademicRecordId == item.AcademicRecordId).FirstOrDefault();
                    nPBankEntities.Entry(educationRow).State = System.Data.Entity.EntityState.Deleted;
                }
                var training = nPBankEntities.Trainings.Where(x => x.PersonId == PersonId).ToList();
                foreach (var item in training)
                {
                    var trainingRow = nPBankEntities.Trainings.Where(x => x.TrainingId == item.TrainingId).FirstOrDefault();
                    nPBankEntities.Entry(trainingRow).State = System.Data.Entity.EntityState.Deleted;
                }
                nPBankEntities.Entry(person).State = System.Data.Entity.EntityState.Deleted;
                nPBankEntities.SaveChanges();
                returnMessageModel.IsSuccess = true;

            }
            catch (Exception ex)
            {
                returnMessageModel.IsSuccess = false;
            }
            return returnMessageModel;
            }
        
    }
}
