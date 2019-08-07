using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using MapcoSolutionTrackings.Models;

namespace MapcoSolutionTrackings.Controllers
{
    public class SearchController : Controller
    {
        // GET: Search
        public ActionResult Index()
        {
            return View();
        }

        public Object GetData()
        {
            UtilController util = new UtilController();
            String Connection_String = ConfigurationManager.ConnectionStrings["Mapco"].ConnectionString;
            List<MetricItem_Mapco> items_Mapco = new List<MetricItem_Mapco>();
            using (SqlConnection conn = new SqlConnection(Connection_String))
            {
                // using (SqlCommand cmd = new SqlCommand("SELECT distinct A.IdSolicitud ,Replace(CONVERT(VARCHAR,A.fecha,106), ' ', '-') as [Fecha], A.Nombres ,A.APELLIDOPATERNO, A.APELLIDOMATERNO  , B.DESCRIPCION ,A.Promotor,A.Tienda  FROM dbo.tblSolicitudes  AS A JOIN dbo.tblEstatus AS B ON A.EstatusActual=B.IDEstatus left  join dbo.tblNotas as  N on (A.IdSolicitud = N.IDSolicitud and N.IDNota = 0)" /* where A.FECHA >= '" + Convert.ToDateTime(Txt_Desde.Text).ToString("yyyy-dd-MM") + "'   and A.FECHA <= '" + Convert.ToDateTime(Txt_Hasta.Text).ToString("yyyy-dd-MM") + "'"*/, conn))
                using (SqlCommand cmd = new SqlCommand("select distinct region,subregion,sTienda from tblUsuariosTiendas where administrador <> 1 order by sTienda asc", conn))
                {

                    try
                    {
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            MetricItem_Mapco item_Mapco = new MetricItem_Mapco();

                            item_Mapco.Region = reader.GetString(0).Trim();
                            item_Mapco.Subregion = reader.GetString(1).Trim();
                            item_Mapco.Tienda = reader.GetString(2).Trim();
                            items_Mapco.Add(item_Mapco);
                        }
                    }
                    catch (Exception ex)
                    {
                        //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "GetDataError",
                        //    "alert('" + ex.Message.Replace("'", "\"") + "');", true);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
            return util.GetResponse(items_Mapco, "Consulta Exitosa", true);
        }
    }
}