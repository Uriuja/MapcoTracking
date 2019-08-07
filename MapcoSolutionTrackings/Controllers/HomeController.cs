﻿using MapcoSolution.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace MapcoSolutionTrackings.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View("~/Views/Home/_LoginMapco.cshtml");
        }

      

        [HttpPost]
        public ActionResult LoginLog(ModelUserLogin model)
        {

            //return Content("<script language='javascript' type='text/javascript'>alert('wololo');</script>");
            //ScriptManager.RegisterStartupScript(this.page, this.GetType(), "script", "test();", true);

            if (model.Usuario.Trim() == "")

            {
                ScriptManager.RegisterStartupScript(null, this.GetType(), "script", "test();", true);
                // ScriptManager.RegisterClientScriptBlock(this, GetType(), "Popup32", "alert('Ingrese el Usuario!')", true);
                //Page..RegisterStartupScript(this.GetType(), "Prueba", "ConfirmacionFechasAnteriores();", true);
            }
            else
                if (model.Contraseña.Trim() == "")
            {
                //ScriptManager.RegisterClientScriptBlock(this, GetType(), "Popup32", "alert('Ingrese su Contraseña!')", true);
            }
            else
            {
                try
                {

                    SqlCommand cmd = new SqlCommand();
                    DataTable dt = new DataTable();
                    SqlCommand cmd2 = new SqlCommand();
                    DataTable dt2 = new DataTable();
                    string connString = ConfigurationManager.ConnectionStrings["Global"].ToString();
                    SqlConnection con = new SqlConnection(connString);
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@sUsuario", SqlDbType.NVarChar).Value = model.Usuario;
                    cmd.Parameters.Add("@sPass", SqlDbType.NVarChar).Value = model.Contraseña;
                    cmd.Connection = con;
                    cmd.CommandText = "Validar_Usuario_Mapco";
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0 && dt.Rows[0]["Client"].ToString().Trim() == "mp")
                    {
                        string nivel = "";
                        if (dt.Rows[0]["iNivel"].ToString() == "20")
                        {
                            nivel = "Administrador";
                        }
                        Session["Nivel"] = nivel;
                        string connString2 = ConfigurationManager.ConnectionStrings["Mapco"].ToString();
                        SqlConnection con2 = new SqlConnection(connString2);

                        cmd2.CommandType = CommandType.StoredProcedure;
                        cmd2.Parameters.Add("@sUser", SqlDbType.NVarChar).Value = model.Usuario;
                        cmd2.Connection = con2;
                        cmd2.CommandText = "IMspr_GetStore";
                        SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                        da2.Fill(dt2);

                        Session["Usuario_Mapco"] = "Valido";
                        Session["Tienda"] = dt2.Rows[0]["sTienda"].ToString();

                        Response.Redirect("Reports.cshtml", false);
                    }
                    else
                    {
                        //ScriptManager.RegisterClientScriptBlock(this, GetType(), "Popup32", "alert('Datos Erroneos, Por Favor Verifique')", true);
                    }
                    con.Close();

                }
                catch (Exception ex)
                {
                    //ScriptManager.RegisterClientScriptBlock(this, GetType(), "Popup32", "alert('Ocurrio Un Error, Por Favor Contacte Al Area De IT\\n" + ex.Message.Replace("'", "\"") + " ')", true);

                }

            }
            return null;
        }



        protected void Update_Promotor()
        {
            try
            {

                string connString = ConfigurationManager.ConnectionStrings["Mapco"].ToString();
                SqlConnection con = new SqlConnection(connString);
                con.Open();

                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable t = new DataTable();



                cmd.CommandType = CommandType.StoredProcedure;

                cmd.CommandText = "Update_Promotor";
                cmd.Connection = con;
                cmd.ExecuteNonQuery();
                con.Close();
            }

            catch (Exception ex)
            {
                //ScriptManager.RegisterClientScriptBlock(this, GetType(), "Popup32", "alert('Ocurrio Un Error, Por Favor Contacte Al Area De IT\\n" + ex.Message.Replace("'", "\"") + " ')", true);
            }
        }

    }
}