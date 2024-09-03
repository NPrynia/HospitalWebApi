using HospitalWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HospitalWebApi.Controllers
{
    public class DoctorController : ApiController
    {
        // GET: api/Doctor
        public Doctor Get(int id)
        {
            try
            {
                Appdata.refreshChanges();
                return  Appdata.Context.Doctor.FirstOrDefault(p => p.ID == id);
                
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // GET: api/Doctor
        public dynamic Get(string orderBy, int numberPage)
        {

            try
            {
                Appdata.refreshChanges();
                List<Models.Doctor> doctors = new List<Models.Doctor>();
                doctors = Appdata.Context.Doctor.ToList();
                var result = doctors
               .Select(i => new { i.ID, i.Surname, i.FirstName, i.Patronymic, i.IDCabinet, SpecializationsName = i.Specializations.Name, i.IDArea })
               .Skip(20 * (numberPage - 1)).Take(20).ToList();

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
                        result = result.OrderBy(i => i.IDCabinet).ToList();
                        break;
                    case "DateOfBirthday":
                        result = result.OrderBy(i => i.SpecializationsName).ToList();
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

        // POST: api/Doctor
        public HttpResponseMessage Post([FromBody] Doctor doctor)
        {
            Appdata.refreshChanges();
            try
            {
                Appdata.Context.Doctor.Add(doctor);
                Appdata.Context.SaveChanges();
                var request = Request.CreateResponse(HttpStatusCode.Created, doctor);
                request.Headers.Location = new Uri(Request.RequestUri +
                    doctor.ID.ToString());
                return request;
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }


        // PUT: api/Doctor
        public HttpResponseMessage Put([FromBody] Doctor doctor)
        {
            try
            {
                var existingtDoctor = Appdata.Context.Doctor.Where(s => s.ID == doctor.ID).FirstOrDefault();
                if (existingtDoctor != null)
                {
                    existingtDoctor.Surname = doctor.Surname;
                    existingtDoctor.FirstName = doctor.FirstName;
                    existingtDoctor.Patronymic = doctor.Patronymic;
                    existingtDoctor.IDCabinet = doctor.IDCabinet;
                    existingtDoctor.IDSpecializations = doctor.IDSpecializations;
                    existingtDoctor.IDArea = doctor.IDArea;
                    Appdata.Context.SaveChanges();
                }

                var message = Request.CreateResponse(HttpStatusCode.Created, doctor);
                message.Headers.Location = new Uri(Request.RequestUri +
                    doctor.ID.ToString());
                return message;

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }




        // DELETE: api/Doctor
        public HttpResponseMessage Delete(int idDoctor)
        {

            try
            {
                var existingtDoctor = Appdata.Context.Doctor.Where(s => s.ID == idDoctor).FirstOrDefault();
                if (existingtDoctor != null)
                {
                    Appdata.Context.Doctor.Remove(existingtDoctor);

                    Appdata.Context.SaveChanges();
                    var message = Request.CreateResponse(HttpStatusCode.Created, idDoctor);
                    message.Headers.Location = new Uri(Request.RequestUri +
                        idDoctor.ToString());
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
