using HospitalWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Linq;
using System.Data.Entity;

namespace HospitalWebApi.Controllers
{
    public class PatientController : ApiController
    {


        // GET: api/Patient
        public Patient Get(int id)
        {
            try 
            {
                Appdata.refreshChanges();
                var listPatient = Appdata.Context.Patient.FirstOrDefault(p => p.ID == id);
                return listPatient;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // GET: api/Patient
        public dynamic Get(string orderBy, int numberPage)
        {

            try
            {
                Appdata.refreshChanges();
                string order = "i.IDArea";
                List<Models.Patient> patients = new List<Models.Patient>();
                patients = Appdata.Context.Patient.ToList();
                var result = patients
               .Select(i => new { i.ID, i.Surname, i.FirstName, i.Patronymic, i.Address, i.DateOfBirthday, GenderName = i.Gender.Name  , i.IDArea })
               .Skip(20 * (numberPage-1) ).Take(20).ToList();

                switch (orderBy)
                {

                    case "Surname":
                        result = result.OrderBy(i => i.Surname).ToList();
                        break;
                    case "FirstName":
                        result = result.OrderBy(i => i.FirstName).ToList();
                        break;
                    case "Patronymic":
                        result = result.OrderBy(i => i.Patronymic).ToList();
                        break;
                    case "Address":
                        result = result.OrderBy(i => i.Address).ToList();
                        break;
                    case "DateOfBirthday":
                        result = result.OrderBy(i => i.DateOfBirthday).ToList();
                        break;
                    case "IDGender":
                        result = result.OrderBy(i => i.GenderName).ToList();
                        break;
                    case "IDArea":
                        result = result.OrderBy(i => i.IDArea).ToList();
                        break;
                    default:
                        result = result.OrderBy(i => i.ID).ToList();
                        break;
                }
                return result;
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
           
        }

        // POST: api/Patient
        public HttpResponseMessage Post([FromBody] Patient patient)
        {
            Appdata.refreshChanges();
            try
            {
                Appdata.Context.Patient.Add(patient);
                Appdata.Context.SaveChanges();
                var request = Request.CreateResponse(HttpStatusCode.Created, patient);
                request.Headers.Location = new Uri(Request.RequestUri +
                    patient.ID.ToString());
                return request;
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }


        // PUT: api/Patient
        public HttpResponseMessage Put([FromBody] Patient patient)
        {
            try
            {
                var existingtPatient = Appdata.Context.Patient.Where(s => s.ID == patient.ID).FirstOrDefault();
                if (existingtPatient != null)
                {
                    existingtPatient.Surname = patient.Surname;
                    existingtPatient.FirstName = patient.FirstName;
                    existingtPatient.Patronymic = patient.Patronymic;
                    existingtPatient.Address = patient.Address;
                    existingtPatient.DateOfBirthday = patient.DateOfBirthday;
                    existingtPatient.IDGender = patient.IDGender;
                    existingtPatient.IDArea = patient.IDArea;
                    Appdata.Context.SaveChanges();
                }

                var message = Request.CreateResponse(HttpStatusCode.Created, patient);
                message.Headers.Location = new Uri(Request.RequestUri +
                    patient.ID.ToString());
                return message;

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }




        // DELETE: api/Patient
        public HttpResponseMessage Delete(int idPatient)
        {

            try
            {
                var existingPatient = Appdata.Context.Patient.Where(s => s.ID == idPatient).FirstOrDefault();
                if (existingPatient != null)
                {
                    Appdata.Context.Patient.Remove(existingPatient);

                    Appdata.Context.SaveChanges();
                    var message = Request.CreateResponse(HttpStatusCode.Created, idPatient);
                    message.Headers.Location = new Uri(Request.RequestUri +
                        idPatient.ToString());
                    return message;
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "not found");
                }


            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }


    }
}
