using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MapcoSolutionTrackings.Controllers
{
    public class MetricItem_Mapco
    {
        public int IdSolicitud { get; set; }
        public string Fecha { get; set; }
        public string Nombres { get; set; }
        public string APELLIDOPATERNO { get; set; }
        public string APELLIDOMATERNO { get; set; }
        public string DESCRIPCION { get; set; }
        public string Promotor { get; set; }
        public string Tienda { get; set; }
        public string Region { get; set; }
        public string Subregion { get; set; }
    }
}