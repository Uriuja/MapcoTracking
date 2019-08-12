using MapcoSolutionTrackings.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace MapcoSolutionTrackings.Controllers
{
    public class PrecalificacionesController : Controller
    {

        // GET: Home
        public ActionResult Index()
        {
            return View("~/Views/Home/_LoginMapco.cshtml");
        }

        protected Object Generar_Reporte(ModelReports modelPreca, object sender, EventArgs e)
        {
            UtilController util = new UtilController();
            try
            {
                string Query = "";
                Query = "select IDprecalificador,Precalificacion,Confirmado,Nombres,Ap_Paterno,Ap_Materno,Fecha_de_Nacimiento,Ciudad,Municipio,Estado,Tienda,promotor,Fecha   FROM [IMSOL_mp].[dbo].[TblPrecalificaciones] where Fecha>= '" + Convert.ToDateTime(modelPreca.desde).ToString("yyyy-dd-MM") + " 00:00:00' and Fecha<= '" + Convert.ToDateTime(modelPreca.hasta).ToString("yyyy-dd-MM") + " 23:59:59' order by Fecha";
                string query = (string)Query.Clone();
                string constr = ConfigurationManager.ConnectionStrings["Mapco"].ToString(); // connection string
                SqlConnection con = new SqlConnection(constr);
                con.Open();
                System.Data.DataTable dt = new System.Data.DataTable();
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandTimeout = 0;
                SqlDataReader reader1 = cmd.ExecuteReader();
                List<ModelResults> resultList = new List<ModelResults>();
                while (reader1.Read())
                {
                    ModelResults result = new ModelResults();
                    //result.noConsulta = reader1.GetValue(0).ToString();
                    ////result.preCalificación = reader1.GetValue(1).ToString();
                    ////result.confirmado = reader1.GetValue(2).ToString();
                    //result.fecha = reader1.GetValue(1).ToString();
                    //result.nombre = reader1.GetValue(2).ToString();
                    //result.apePat = reader1.GetValue(3).ToString();
                    //result.apeMat = reader1.GetValue(4).ToString();
                    //result.estatus = reader1.GetValue(5).ToString();
                    //result.motivoStatus = reader1.GetValue(6).ToString();
                    //result.estatusRecepcion = reader1.GetValue(7).ToString();
                    //result.promotor = reader1.GetValue(8).ToString();
                    //result.tienda = reader1.GetValue(10).ToString();
                    //resultList.Add(result);
                }
                con.Close();
                return util.GetResponse(resultList, "Consulta Exitosa", true);
             
               

            }
            catch (Exception ex)
            {
                return util.GetResponse(ex, "Error de sistema", false);
            }

        }
    }
}