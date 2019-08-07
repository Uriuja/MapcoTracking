using MapcoSolutionTrackings.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace MapcoSolutionTrackings.Controllers
{
    public class PrecalificacionesController : System.Web.UI.Page
    {
        protected void Generar_Reporte(ModelReports modelPreca, object sender, EventArgs e)
        {
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
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
                //GridView1.DataSource = dt;
                //GridView1.Columns[0].Visible = true;
                if (dt.Rows.Count > 0)
                {
                    //LBRegistros.Visible = true;
                    //LBRegistros.Font.Bold = true;
                    //LBRegistros.Text = "Registros encontrados: " + dt.Rows.Count.ToString();
                    //GridView1.Visible = true;
                    //GridView1.DataBind();
                    //Btn_Export_To_Excel.Visible = true;
                    //Btn_Excel2.Visible = true;
                }
                else
                {
                    //Btn_Export_To_Excel.Visible = false;
                    //Btn_Excel2.Visible = false;
                    //LBRegistros.Visible = false;
                    //GridView1.Visible = false;

                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Scroll", "alert('No hay coincidencias!')", true);
                }
                con.Close();
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Scroll", "scroll();", true);

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "Popup32", "alert('Ocurrio Un Error, Por Favor Contacte Al Area De IT\\n" + ex.Message.Replace("'", "\"") + " ')", true);
            }

        }
    }
}