using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace HospitalWebApi
{
    public class Appdata
    {
        public static Models.HospitalDBEntities  Context { get; set; } = new Models.HospitalDBEntities();

        public static void refreshChanges()
        {
            Context = new Models.HospitalDBEntities();
        }
    }
}