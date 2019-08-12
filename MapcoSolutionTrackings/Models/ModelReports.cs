using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MapcoSolutionTrackings.Models
{
    public class ModelReports
    {
       public DateTime desde { get; set; }
        public DateTime hasta { get; set; }
       public string name { get; set;  }

       public string Amaterno { get; set; }
        public string APaterno { get; set; }
        public string Ddl_Estatus { get; set; }
        public string solicitud { get; set; }

        public string promotor { get; set; }

        public string tienda { get; set; }
        public string aprobado { get; set; }


    }
}