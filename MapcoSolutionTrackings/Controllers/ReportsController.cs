using MapcoSolution.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

using System.Collections;
using System.Globalization;
using System.Security.Principal;
using System.Web.Caching;
using System.Web.Profile;
using System.Web.SessionState;
using MapcoSolutionTrackings.Models;

namespace MapcoSolutionTrackings.Controllers
{
    public class ReportsController : Controller
    {

        private const string PARAMETER = "@PARAS";

        #region Update promotor
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
        #endregion

        #region Fill 
        public void Fill_Estatus()
        {
            try
            {
                string constr = ConfigurationManager.ConnectionStrings["Mapco"].ToString(); // connection string
                SqlConnection con = new SqlConnection(constr);
                con.Open();

                //Ddl_Estatus.Items.Clear();


                SqlCommand com = new SqlCommand("select distinct B.descripcion as DESCRIPCION from tblEstatus B" +
                " inner join tblSolicitudes A on (B.IDEstatus COLLATE DATABASE_DEFAULT = A.EstatusActual COLLATE DATABASE_DEFAULT) order by B.descripcion asc", con); // table name 
                SqlDataAdapter da = new SqlDataAdapter(com);
                DataSet ds = new DataSet();
                da.Fill(ds);  // fill dataset
                //Ddl_Estatus.DataTextField = ds.Tables[0].Columns["DESCRIPCION"].ToString(); // text field name of table dispalyed in dropdown
                //Ddl_Estatus.DataValueField = ds.Tables[0].Columns["DESCRIPCION"].ToString();             // to retrive specific  textfield name 
                //Ddl_Estatus.DataSource = ds.Tables[0];      //assigning datasource to the dropdownlist
                //Ddl_Estatus.DataBind();  //binding dropdownlist
                con.Close();

                //Ddl_Estatus.Items.Insert(0, new ListItem("Seleccione", "0"));
            }
            catch (Exception ex)
            {
                //ScriptManager.RegisterClientScriptBlock(this, GetType(), "Popup32", "alert('Ocurrio Un Error, Por Favor Contacte Al Area De IT\\n" + ex.Message.Replace("'", "\"") + " ')", true);
            }

        }

        protected void Fill_Tienda()
        {
            try
            {
                string constr = ConfigurationManager.ConnectionStrings["Mapco"].ToString(); // connection string
                SqlConnection con = new SqlConnection(constr);
                con.Open();

                //LB_Tienda.Items.Clear();


                SqlCommand com = new SqlCommand("select sTienda from tblUsuariosTiendas order by sTienda desc", con); // table name 
                SqlDataAdapter da = new SqlDataAdapter(com);
                DataSet ds = new DataSet();
                da.Fill(ds);  // fill dataset
                //LB_Tienda.DataTextField = ds.Tables[0].Columns["sTienda"].ToString(); // text field name of table dispalyed in dropdown
                //LB_Tienda.DataValueField = ds.Tables[0].Columns["sTienda"].ToString();             // to retrive specific  textfield name 
                //LB_Tienda.DataSource = ds.Tables[0];      //assigning datasource to the dropdownlist
                //LB_Tienda.DataBind();  //binding dropdownlist
                con.Close();

                //Ddl_Tienda.Items.Insert(0, new ListItem("Seleccione", "0"));
            }
            catch (Exception ex)
            {
                //ScriptManager.RegisterClientScriptBlock(this, GetType(), "Popup32", "alert('Ocurrio Un Error, Por Favor Contacte Al Area De IT\\n" + ex.Message.Replace("'", "\"") + " ')", true);
            }

        }

        public Object Fill_Data()
        {

            return Get_Metrics_Data();

         
        }

        #endregion

        #region Generar Reportes
        protected void Generar_Reporte_Administrador(ModelReports modRepo)
        {
            //Fill_Data();
            //ScriptManager.RegisterClientScriptBlock(this, GetType(), "Popup32", "alert('Reporte admin')", true);

            string Query = "";
            string Query2 = "";
            string Query3 = "";
            string Query4 = "";

            if (modRepo.Ddl_Estatus != "Seleccione")
            {
                Query = "SELECT  A.IdSolicitud ,Replace(CONVERT(VARCHAR,A.fecha,106), ' ', '-') as [Fecha], A.Nombres ,A.APELLIDOPATERNO, A.APELLIDOMATERNO  , B.DESCRIPCION ," +
                    "'Motivo Estatus' = (case MotivoEstatus " +
                    "when '01' then 'NORMAL' " +
                    "when '02' then 'PROCESO' " +
                    "when '03' then 'APROBADO' " +
                    "when '04' then 'RECHAZADO' " +
                    "when '05' then 'PENDIENTE' " +
                    "when '06' then 'CANCELADO' " +
                    "when '07' then 'RECHAZADO POR BURO DE CREDITO' " +
                    "when '08' then 'RECHAZADO POR VERIFICACION TELEFONICA' " +
                    "when '09' then 'RECHAZADO POR POLITICA DE CREDITO' " +
                    "when '10' then 'CANCELADO POR SOLICITUD DUPLICADA' " +
                    "when '11' then 'CANCELADO POR DOCUMENTACION INCOMPLETA' " +
                    "when '12' then 'PENDIENTE POR SOLICITUD INCOMPLETA' " +
                    "end), " +
                    "'Estatus Recepción' = (case A.EstatusRecepcion " +
                      "when 'CO' then 'Completo' " +
                      "when 'IA' then 'Falta ID' " +
                      "when 'IB' then 'Falta Solicitud' " +
                      "when 'IC' then 'Falta anexo de buro' " +
                      "when 'ID' then 'Firmas distintas al ID en todos los  documentos' " +
                      "when 'IE' then 'Firmas distintas al ID en anexo de buro' " +
                      "when 'IF' then 'Solicitud con datos incompletos' " +
                      "when 'IG' then 'Solicitud con nombre incorrecto' " +
                      "when 'IH' then 'Anexo de buro con nombre incorrecto' " +
                      "when 'II' then 'Solicitud con tachaduras' " +
                      "when 'IJ' then 'Anexo de buro con tachaduras' " +
                      "when 'IK' then 'Solicitud Ilegible' " +
                      "when 'IL' then 'ID Ilegible' " +
                      "when 'IM' then 'Anexo de buro Ilegible' " +
                      "when 'IN' then 'Documentación no pertenece al solicitante' " +
                      "when 'RE' then 'Revisión' " +
                      "when 'IO' then 'Solicitud con fecha de nacimiento incorrecta' " +
                      "when 'IP' then 'Anexo de buro con RFC incorrecto' " +
                      "when 'IQ' then 'Anexo de buro con datos incompletos' " +
                      "when 'IR' then 'Solicitud duplicada'	 " +
                      "when 'IS' then 'Falta comprobante de domicilio' " +
                      "when 'IT' then 'Checklist' " +
                      "when 'NO' then 'NO Asignado Aún'" +
                      "end), " +
                    " A.Promotor,  N.Nota,A.Tienda  FROM dbo.tblSolicitudes  AS A JOIN dbo.tblEstatus AS B ON A.EstatusActual COLLATE DATABASE_DEFAULT = B.IDEstatus COLLATE DATABASE_DEFAULT left  join dbo.tblNotas as  N on (A.IdSolicitud = N.IDSolicitud and N.IDNota = 0) " +
                      "  inner join dbo.tblUsuariosTiendas U on (A.Tienda COLLATE DATABASE_DEFAULT = U.sTienda COLLATE DATABASE_DEFAULT) " +
                     /*  " inner join [faxes].[dbo].tbl_faxes C" +
                       " on (A.IdSolicitud = C.IDSolicitud  and C.IDSolicitud is not null and c.Cliente='ml')" +*/
                     "  WHERE " +
                " A.FECHA >= '" + Convert.ToDateTime(modRepo.desde).ToString("yyyy-dd-MM") + " 00:00:00'   and A.FECHA <= '" + Convert.ToDateTime(modRepo.hasta).ToString("yyyy-dd-MM") + " 23:59:59'" +
                " and( (A.NOMBRES like '%" + modRepo.name + "%') and (A.APELLIDOPATERNO like '%" + modRepo.APaterno + "%')" +
                " and (A.APELLIDOMATERNO like '%" + modRepo.Amaterno + "%')" +
                " and (B.Descripcion = '" + modRepo.Ddl_Estatus + "')" +
                " and (A.IDSOLICITUD like '%" + modRepo.solicitud+ "%')" +
                " and (A.Promotor like '%" + modRepo.promotor+ "%')) " + PARAMETER + " ORDER BY A.IdSolicitud"; ;

                Query2 = "select" +
                " SUM(CASE WHEN B.descripcion IN ('Solicitud Aprobada','Solicitud Rechazada'," +
                " 'Solicitud en proceso de Captura','Solicitud Ingresada'" +
                " ,'Solicitud Pendiente/En Proceso'," +
                " 'Solicitud Cancelada','Solicitud Cancelada en Pre-Captura')THEN 1 ELSE 0 END) [Ingresadas]," +
                " SUM(CASE WHEN B.descripcion IN ('Solicitud Aprobada')THEN 1 ELSE 0 END) [Aprobadas]," +
                " SUM(CASE WHEN B.descripcion IN ('Solicitud Rechazada')THEN 1 ELSE 0 END) [Rechazadas]," +
                " SUM(CASE WHEN B.descripcion IN ('Solicitud en proceso de Captura','Solicitud Ingresada'" +
                " ,'Solicitud Pendiente/En Proceso')THEN 1 ELSE 0 END) [Pendientes]," +
                " SUM(CASE WHEN B.descripcion IN ('Solicitud Cancelada','Solicitud Cancelada en Pre-Captura')THEN 1 " +
                " ELSE 0 END) [Canceladas]" +
                 " FROM dbo.tblSolicitudes " +
                    " AS A JOIN dbo.tblEstatus AS B ON A.EstatusActual COLLATE DATABASE_DEFAULT=B.IDEstatus COLLATE DATABASE_DEFAULT" +
                    " left  join dbo.tblNotas as  N on (A.IdSolicitud = N.IDSolicitud and N.IDNota = 0)" +
                      " inner join dbo.tblUsuariosTiendas U on (A.Tienda COLLATE DATABASE_DEFAULT = U.sTienda COLLATE DATABASE_DEFAULT) " +
                /*  " inner join [faxes].[dbo].tbl_faxes C" +
                  " on (A.IdSolicitud = C.IDSolicitud  and C.IDSolicitud is not null and c.Cliente='ml')" +*/
                " WHERE" +
                "  A.FECHA >= '" + Convert.ToDateTime(modRepo.desde).ToString("yyyy-dd-MM") + " 00:00:00'   and A.FECHA <= '" + Convert.ToDateTime(modRepo.hasta).ToString("yyyy-dd-MM") + " 23:59:59'" +
                " and( (A.NOMBRES like '%" + modRepo.name+ "%') and (A.APELLIDOPATERNO like '%" + modRepo.APaterno+ "%')" +
                " and (A.APELLIDOMATERNO like '%" + modRepo.Amaterno+ "%')" +
                " and (B.Descripcion = '" + modRepo.Ddl_Estatus + "')" +
                " and (A.IDSOLICITUD like '%" + modRepo.solicitud + "%')" +
                " and (A.Promotor like '%" + modRepo.promotor + "%')) " + PARAMETER;

                Query3 = "select" +
                " B.Descripcion as Estatus, " +
                " sum( case when A.MotivoEstatus in('07') then 1 else 0 end) [Buró de crédito]," +
                " sum( case when A.MotivoEstatus in('08') then 1 else 0 end) [Verificación telefónica]," +
                " sum( case when A.MotivoEstatus in('09') then 1 else 0 end) [Política de crédito]," +
                " sum( case when A.MotivoEstatus in('10') then 1 else 0 end) [Solicitud duplicada]," +
                " sum( case when A.MotivoEstatus in('11') then 1 else 0 end) [Documentación incompleta]," +
                " sum( case when A.MotivoEstatus in('12') then 1 else 0 end) [Pendiente Por Documentación incompleta]," +
                " sum( case when A.MotivoEstatus in('02') then 1 else 0 end) [En proceso]" +
                " from" +
                    " tblSolicitudes A inner join  tblEstatus B" +
                    " on (A.EstatusActual COLLATE DATABASE_DEFAULT=B.IDEstatus COLLATE DATABASE_DEFAULT)" +
                    /*  " inner join [faxes].[dbo].tbl_faxes C" +
                      " on (A.IdSolicitud = C.IDSolicitud  and C.IDSolicitud is not null and c.Cliente='ml')" +*/
                    " inner join dbo.tblUsuariosTiendas U on (A.Tienda COLLATE DATABASE_DEFAULT = U.sTienda COLLATE DATABASE_DEFAULT) " +
                " where b.descripcion in ('Solicitud Rechazada','Solicitud en proceso de Captura'" +
                " ,'Solicitud Ingresada','Solicitud Pendiente/En Proceso','Solicitud Cancelada'" +
                " ,'Solicitud Cancelada en Pre-Captura')" +
                //  " and A.Fecha >= '2014-16-07 14:00:00'" +
                " and A.FECHA >= '" + Convert.ToDateTime(modRepo.desde).ToString("yyyy-dd-MM") + " 00:00:00'   and A.FECHA <= '" + Convert.ToDateTime(modRepo.hasta).ToString("yyyy-dd-MM") + " 23:59:59'" +
                " and( (A.NOMBRES like '%" + modRepo.name+ "%') and (A.APELLIDOPATERNO like '%" + modRepo.APaterno + "%')" +
                " and (A.APELLIDOMATERNO like '%" + modRepo.Amaterno+ "%')" +
                " and (B.Descripcion = '" + modRepo.Ddl_Estatus + "')" +
                " and (A.IDSOLICITUD like '%" + modRepo.solicitud + "%')" +
                " and (A.Promotor like '%" + modRepo.promotor + "%')) " + PARAMETER +
                " group by B.Descripcion";

                Query4 = "select " +
                " A.Tienda, count(A.idsolicitud) as Ingresadas " +
                " from" +
                " tblSolicitudes A inner join  tblEstatus B" +
                " on (A.EstatusActual COLLATE DATABASE_DEFAULT = B.IDEstatus COLLATE DATABASE_DEFAULT)" +
                " inner join dbo.tblUsuariosTiendas U on (A.Tienda COLLATE DATABASE_DEFAULT = U.sTienda COLLATE DATABASE_DEFAULT) " +
                " where " +
                " A.FECHA >= '" + Convert.ToDateTime(modRepo.desde).ToString("yyyy-dd-MM") + " 00:00:00'   and A.FECHA <= '" + Convert.ToDateTime(modRepo.hasta).ToString("yyyy-dd-MM") + " 23:59:59'" +
                " and( (A.NOMBRES like '%" + modRepo.name + "%') and (A.APELLIDOPATERNO like '%" + modRepo.APaterno + "%')" +
                " and (A.APELLIDOMATERNO like '%" + modRepo.Amaterno + "%')" +
                " and (B.Descripcion = '" + modRepo.Ddl_Estatus + "')" +
                " and (A.IDSOLICITUD like '%" + modRepo.solicitud+ "%')" +
                " and (A.Promotor like '%" + modRepo.promotor + "%')) " + PARAMETER +
                " group by Tienda order by count(A.idsolicitud) desc ";

            }
            else
            {
                Query = "SELECT  A.IdSolicitud ,Replace(CONVERT(VARCHAR,A.fecha,106), ' ', '-') as [Fecha], A.Nombres ,A.APELLIDOPATERNO, A.APELLIDOMATERNO  , B.DESCRIPCION, " +
                    "'Motivo Estatus' = (case MotivoEstatus " +
                    "when '01' then 'NORMAL' " +
                    "when '02' then 'PROCESO' " +
                    "when '03' then 'APROBADO' " +
                    "when '04' then 'RECHAZADO' " +
                    "when '05' then 'PENDIENTE' " +
                    "when '06' then 'CANCELADO' " +
                    "when '07' then 'RECHAZADO POR BURO DE CREDITO' " +
                    "when '08' then 'RECHAZADO POR VERIFICACION TELEFONICA' " +
                    "when '09' then 'RECHAZADO POR POLITICA DE CREDITO' " +
                    "when '10' then 'CANCELADO POR SOLICITUD DUPLICADA' " +
                    "when '11' then 'CANCELADO POR DOCUMENTACION INCOMPLETA' " +
                    "when '12' then 'PENDIENTE POR SOLICITUD INCOMPLETA' " +
                    "end), " +
                "'Estatus Recepción' = (case A.EstatusRecepcion " +
                      "when 'CO' then 'Completo' " +
                      "when 'IA' then 'Falta ID' " +
                      "when 'IB' then 'Falta Solicitud' " +
                      "when 'IC' then 'Falta anexo de buro' " +
                      "when 'ID' then 'Firmas distintas al ID en todos los  documentos' " +
                      "when 'IE' then 'Firmas distintas al ID en anexo de buro' " +
                      "when 'IF' then 'Solicitud con datos incompletos' " +
                      "when 'IG' then 'Solicitud con nombre incorrecto' " +
                      "when 'IH' then 'Anexo de buro con nombre incorrecto' " +
                      "when 'II' then 'Solicitud con tachaduras' " +
                      "when 'IJ' then 'Anexo de buro con tachaduras' " +
                      "when 'IK' then 'Solicitud Ilegible' " +
                      "when 'IL' then 'ID Ilegible' " +
                      "when 'IM' then 'Anexo de buro Ilegible' " +
                      "when 'IN' then 'Documentación no pertenece al solicitante' " +
                      "when 'RE' then 'Revisión' " +
                      "when 'IO' then 'Solicitud con fecha de nacimiento incorrecta' " +
                      "when 'IP' then 'Anexo de buro con RFC incorrecto' " +
                      "when 'IQ' then 'Anexo de buro con datos incompletos' " +
                      "when 'IR' then 'Solicitud duplicada'	 " +
                      "when 'IS' then 'Falta comprobante de domicilio' " +
                      "when 'IT' then 'Checklist' " +
                      "when 'NO' then 'NO Asignado Aún'" +
                      "end), " +
                      "A.Promotor,  N.Nota,A.Tienda  FROM dbo.tblSolicitudes  AS A JOIN dbo.tblEstatus AS B ON A.EstatusActual COLLATE DATABASE_DEFAULT=B.IDEstatus COLLATE DATABASE_DEFAULT left  join dbo.tblNotas as  N on (A.IdSolicitud  = N.IDSolicitud  and N.IDNota = 0) " +
                       "  inner join dbo.tblUsuariosTiendas U on (A.Tienda COLLATE DATABASE_DEFAULT = U.sTienda COLLATE DATABASE_DEFAULT) " +
                /*  " inner join [faxes].[dbo].tbl_faxes C" +
                  " on (A.IdSolicitud = C.IDSolicitud  and C.IDSolicitud is not null and c.Cliente='ml')" +*/
                "  WHERE " +
                " A.FECHA >= '" + Convert.ToDateTime(modRepo.desde).ToString("yyyy-dd-MM") + " 00:00:00'   and A.FECHA <= '" + Convert.ToDateTime(modRepo.hasta).ToString("yyyy-dd-MM") + " 23:59:59'" +
                " and( (A.NOMBRES like '%" + modRepo.name + "%') and (A.APELLIDOPATERNO like '%" + modRepo.APaterno + "%')" +
                " and (A.APELLIDOMATERNO like '%" + modRepo.Amaterno + "%')" +
                " and (A.IDSOLICITUD like '%" + modRepo.solicitud + "%')" +
                " and (A.Promotor like '%" + modRepo.promotor + "%')) " + PARAMETER + " ORDER BY A.IdSolicitud";

                Query2 = "select " +
                " SUM(CASE WHEN B.descripcion IN ('Solicitud Aprobada','Solicitud Rechazada'," +
                " 'Solicitud en proceso de Captura','Solicitud Ingresada'" +
                " ,'Solicitud Pendiente/En Proceso'," +
                " 'Solicitud Cancelada','Solicitud Cancelada en Pre-Captura')THEN 1 ELSE 0 END) [Ingresadas]," +
                " SUM(CASE WHEN B.descripcion IN ('Solicitud Aprobada')THEN 1 ELSE 0 END) [Aprobadas]," +
                " SUM(CASE WHEN B.descripcion IN ('Solicitud Rechazada')THEN 1 ELSE 0 END) [Rechazadas]," +
                " SUM(CASE WHEN B.descripcion IN ('Solicitud en proceso de Captura','Solicitud Ingresada'" +
                " ,'Solicitud Pendiente/En Proceso')THEN 1 ELSE 0 END) [Pendientes]," +
                " SUM(CASE WHEN B.descripcion IN ('Solicitud Cancelada','Solicitud Cancelada en Pre-Captura')THEN 1 " +
                " ELSE 0 END) [Canceladas]" +
                " FROM dbo.tblSolicitudes " +
                " AS A JOIN dbo.tblEstatus AS B ON A.EstatusActual COLLATE DATABASE_DEFAULT =B.IDEstatus COLLATE DATABASE_DEFAULT" +
                " left  join dbo.tblNotas as  N on (A.IdSolicitud = N.IDSolicitud and N.IDNota = 0)" +
                     " inner join dbo.tblUsuariosTiendas U on (A.Tienda COLLATE DATABASE_DEFAULT = U.sTienda COLLATE DATABASE_DEFAULT) " +
                /*   " inner join [faxes].[dbo].tbl_faxes C" +
                  " on (A.IdSolicitud = C.IDSolicitud  and C.IDSolicitud is not null and c.Cliente='ml')" +*/
                " WHERE" +
                " A.FECHA >= '" + Convert.ToDateTime(modRepo.desde).ToString("yyyy-dd-MM") + " 00:00:00'   and A.FECHA <= '" + Convert.ToDateTime(modRepo.hasta).ToString("yyyy-dd-MM") + " 23:59:59'" +
                " and( (A.NOMBRES like '%" + modRepo.name + "%') and (A.APELLIDOPATERNO like '%" +modRepo.APaterno + "%')" +
                " and (A.APELLIDOMATERNO like '%" + modRepo.Amaterno + "%')" +
                " and (A.IDSOLICITUD like '%" + modRepo.solicitud + "%')" +
                " and (A.Promotor like '%" + modRepo.promotor + "%')) " + PARAMETER;

                Query3 = "select " +
                " B.Descripcion as Estatus, " +
                " sum( case when A.MotivoEstatus in('07') then 1 else 0 end) [Buró de crédito]," +
                " sum( case when A.MotivoEstatus in('08') then 1 else 0 end) [Verificación telefónica]," +
                " sum( case when A.MotivoEstatus in('09') then 1 else 0 end) [Política de crédito]," +
                " sum( case when A.MotivoEstatus in('10') then 1 else 0 end) [Solicitud duplicada]," +
                " sum( case when A.MotivoEstatus in('11') then 1 else 0 end) [Documentación incompleta]," +
                " sum( case when A.MotivoEstatus in('12') then 1 else 0 end) [Pendiente Por Documentación incompleta]," +
                " sum( case when A.MotivoEstatus in('02') then 1 else 0 end) [En proceso]" +
                " from" +
                " tblSolicitudes A inner join  tblEstatus B" +
                " on (A.EstatusActual COLLATE DATABASE_DEFAULT = B.IDEstatus COLLATE DATABASE_DEFAULT)" +
                /* " inner join [faxes].[dbo].tbl_faxes C" +
                 " on (A.IdSolicitud = C.IDSolicitud  and C.IDSolicitud is not null and c.Cliente='ml')" +*/
                " inner join dbo.tblUsuariosTiendas U on (A.Tienda COLLATE DATABASE_DEFAULT = U.sTienda COLLATE DATABASE_DEFAULT) " +
                " where b.descripcion in ('Solicitud Rechazada','Solicitud en proceso de Captura'" +
                " ,'Solicitud Ingresada','Solicitud Pendiente/En Proceso','Solicitud Cancelada'" +
                " ,'Solicitud Cancelada en Pre-Captura')" +
                // " and A.Fecha >= '2014-16-07 14:00:00'" +
                " and A.FECHA >= '" + Convert.ToDateTime(modRepo.desde).ToString("yyyy-dd-MM") + " 00:00:00'   and A.FECHA <= '" + Convert.ToDateTime(modRepo.hasta).ToString("yyyy-dd-MM") + " 23:59:59'" +
                " and( (A.NOMBRES like '%" + modRepo.name + "%') and (A.APELLIDOPATERNO like '%" + modRepo.APaterno + "%')" +
                " and (A.APELLIDOMATERNO like '%" + modRepo.Amaterno + "%')" +
                " and (A.IDSOLICITUD like '%" + modRepo.solicitud + "%')" +
                " and (A.Promotor like '%" + modRepo.promotor + "%')) " + PARAMETER +
                " group by B.Descripcion";


                Query4 = "select " +
                " A.Tienda, count(A.idsolicitud) as Ingresadas " +
                " from" +
                " tblSolicitudes A inner join  tblEstatus B" +
                " on (A.EstatusActual COLLATE DATABASE_DEFAULT =B.IDEstatus COLLATE DATABASE_DEFAULT)" +
                " inner join dbo.tblUsuariosTiendas U on (A.Tienda COLLATE DATABASE_DEFAULT = U.sTienda COLLATE DATABASE_DEFAULT) " +
                " where " +
                " A.FECHA >= '" + Convert.ToDateTime(modRepo.desde).ToString("yyyy-dd-MM") + " 00:00:00'   and A.FECHA <= '" + Convert.ToDateTime(modRepo.hasta).ToString("yyyy-dd-MM") + " 23:59:59'" +
                " and( (A.NOMBRES like '%" + modRepo.name + "%') and (A.APELLIDOPATERNO like '%" + modRepo.APaterno + "%')" +
                " and (A.APELLIDOMATERNO like '%" + modRepo.APaterno + "%')" +
                " and (A.IDSOLICITUD like '%" + modRepo.solicitud + "%')" +
                " and (A.Promotor like '%" + modRepo.promotor + "%')) " + PARAMETER +
                " group by Tienda order by count(A.idsolicitud) desc ";

            }



            string query = (string)Query.Clone();
            string query2 = (string)Query2.Clone();
            string query3 = (string)Query3.Clone();
            string query4 = (string)Query4.Clone();
            //string parameters = Get_Query_Filters();
            //query = query.Replace(PARAMETER, parameters);
            //query2 = query2.Replace(PARAMETER, parameters);
            //query3 = query3.Replace(PARAMETER, parameters);
            //query4 = query4.Replace(PARAMETER, parameters);

            try
            {

                string constr = ConfigurationManager.ConnectionStrings["Mapco"].ToString(); // connection string
                SqlConnection con = new SqlConnection(constr);
                con.Open();


                System.Data.DataTable dt = new System.Data.DataTable();
                System.Data.DataTable dt2 = new System.Data.DataTable();
                System.Data.DataTable dt3 = new System.Data.DataTable();
                System.Data.DataTable dt4 = new System.Data.DataTable();

                SqlCommand cmd = new SqlCommand(query, con);
                SqlCommand cmd2 = new SqlCommand(query2, con);
                SqlCommand cmd3 = new SqlCommand(query3, con);
                SqlCommand cmd4 = new SqlCommand(query4, con);

                cmd.CommandTimeout = 0;
                cmd2.CommandTimeout = 0;
                cmd3.CommandTimeout = 0;
                cmd4.CommandTimeout = 0;

                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                SqlDataAdapter adp2 = new SqlDataAdapter(cmd2);
                SqlDataAdapter adp3 = new SqlDataAdapter(cmd3);
                SqlDataAdapter adp4 = new SqlDataAdapter(cmd4);

                adp.Fill(dt);
                adp2.Fill(dt2);
                adp3.Fill(dt3);
                adp4.Fill(dt4);

                //GridView1.DataSource = dt;
                //GridView1.Columns[0].Visible = true;

                //GridView2.DataSource = dt;
                //GridView3.DataSource = dt2;
                //GridView4.DataSource = dt3;
                //GridView5.DataSource = dt4;
                //GridView6.DataSource = dt4;

                if (dt.Rows.Count > 0)
                {
                    //LBRegistros.Visible = true;
                    //LBRegistros.Font.Bold = true;
                    //LBRegistros.Text = "Registros encontrados: " + dt.Rows.Count.ToString();
                    //GridView1.Visible = true;
                    //GridView3.Visible = true;
                    //GridView4.Visible = true;
                    //GridView5.Visible = true;
                    //GridView6.DataBind();
                    //GridView5.DataBind();
                    //GridView4.DataBind();
                    //GridView3.DataBind();
                    //GridView1.DataBind();
                    //GridView2.DataBind();



                    //Btn_Export_To_Excel.Visible = true;
                    //Btn_Excel2.Visible = true;


                }

                else
                {

                    //Btn_Export_To_Excel.Visible = false;
                    //Btn_Excel2.Visible = false;
                    //LBRegistros.Visible = false;
                    //GridView1.Visible = false;
                    //GridView3.Visible = false;
                    //GridView4.Visible = false;
                    //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Scroll", "alert('No hay coincidencias!')", true);

                }
                con.Close();
                //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Scroll", "scroll();", true);

            }

            catch (Exception ex)
            {
                //ScriptManager.RegisterClientScriptBlock(this, GetType(), "Popup32", "alert('Ocurrio Un Error, Por Favor Contacte Al Area De IT\\n" + ex.Message.Replace("'", "\"") + " ')", true);
            }
        }

        //protected void Generar_Reporte(ModelReports modRepo,object sender, EventArgs e)
        //{
        //    //Reiniciar_Tablas();
        //    Update_Promotor();

        //    if (ViewState["Nivel"].ToString() == "Administrador")
        //    {
        //        Generar_Reporte_Administrador(modRepo);
        //    }
        //    else
        //    {

        //        int dia_mes = Convert.ToInt32(DateTime.Now.ToString("dd"));
        //        DateTime inicio_mes = DateTime.Now;
        //        inicio_mes = inicio_mes.AddDays(Convert.ToInt32((-dia_mes) + 1));
        //        DateTime minimo = inicio_mes.AddMonths(-2);
        //        string Minimo = Convert.ToString(minimo.ToString("yyyy-MM-dd"));



        //        if (Convert.ToDateTime(Minimo) > Convert.ToDateTime(modRepo.desde))
        //        {

        //            ScriptManager.RegisterClientScriptBlock(this, GetType(), "Popup32", "alert('Sólo puedes visualizar desde el primer día de 2 meses atrás hasta hoy!')", true);
        //        }
        //        else
        //        {
        //            try
        //            {

        //                SqlCommand cmd = new SqlCommand();
        //                SqlDataAdapter da = new SqlDataAdapter(cmd);
        //                DataTable t = new DataTable();

        //                SqlCommand cmd2 = new SqlCommand();
        //                SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
        //                DataTable t2 = new DataTable();

        //                SqlCommand cmd3 = new SqlCommand();
        //                SqlDataAdapter da3 = new SqlDataAdapter(cmd3);
        //                DataTable t3 = new DataTable();

        //                SqlCommand cmd4 = new SqlCommand();
        //                SqlDataAdapter da4 = new SqlDataAdapter(cmd4);
        //                DataTable t4 = new DataTable();

        //                string connString = ConfigurationManager.ConnectionStrings["Mapco"].ToString();
        //                SqlConnection con = new SqlConnection(connString);
        //                con.Open();
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd2.CommandType = CommandType.StoredProcedure;
        //                cmd3.CommandType = CommandType.StoredProcedure;
        //                cmd4.CommandType = CommandType.StoredProcedure;


        //                cmd.Parameters.Clear();
        //                cmd.Parameters.Add("@desde", SqlDbType.VarChar).Value = Convert.ToDateTime(modRepo.desde).ToString("yyyy-dd-MM") + " 00:00:00";
        //                cmd.Parameters.Add("@hasta", SqlDbType.VarChar).Value = Convert.ToDateTime(modRepo.hasta).ToString("yyyy-dd-MM") + " 23:59:59";
        //                cmd.Parameters.Add("@nombre", SqlDbType.VarChar).Value = modRepo.name;
        //                cmd.Parameters.Add("@apaterno", SqlDbType.VarChar).Value = modRepo.APaterno;
        //                cmd.Parameters.Add("@amaterno", SqlDbType.VarChar).Value = modRepo.Amaterno;
        //                cmd.Parameters.Add("@promotor", SqlDbType.VarChar).Value = modRepo.promotor;
        //                cmd.Parameters.Add("@descripcion", SqlDbType.VarChar).Value = modRepo.Ddl_Estatus;
        //                cmd.Parameters.Add("@tienda", SqlDbType.VarChar).Value = modRepo.tienda.Trim().Replace("Tienda ", "");
        //                cmd.Parameters.Add("@idsolicitud", SqlDbType.VarChar).Value = modRepo.solicitud;
        //                cmd.CommandText = "IMspr_GetReportes";


        //                cmd.Connection = con;
        //                da.SelectCommand = cmd;
        //                da.Fill(t);
        //                //GridView1.DataSource = t;
        //                //GridView1.Columns[0].Visible = true;

        //                //GridView2.DataSource = t;

        //                cmd2.Parameters.Clear();
        //                cmd2.Parameters.Add("@desde", SqlDbType.VarChar).Value = Convert.ToDateTime(modRepo.desde).ToString("yyyy-dd-MM") + " 00:00:00";
        //                cmd2.Parameters.Add("@hasta", SqlDbType.VarChar).Value = Convert.ToDateTime(modRepo.hasta).ToString("yyyy-dd-MM") + " 23:59:59";
        //                cmd2.Parameters.Add("@nombre", SqlDbType.VarChar).Value = modRepo.name;
        //                cmd2.Parameters.Add("@apaterno", SqlDbType.VarChar).Value =modRepo.APaterno;
        //                cmd2.Parameters.Add("@amaterno", SqlDbType.VarChar).Value = modRepo.Amaterno;
        //                cmd2.Parameters.Add("@promotor", SqlDbType.VarChar).Value = modRepo.promotor;
        //                cmd2.Parameters.Add("@descripcion", SqlDbType.VarChar).Value = modRepo.Ddl_Estatus;
        //                cmd2.Parameters.Add("@tienda", SqlDbType.VarChar).Value = modRepo.tienda.Trim().Replace("Tienda ", "");
        //                cmd2.Parameters.Add("@idsolicitud", SqlDbType.VarChar).Value = modRepo.solicitud;
        //                cmd2.CommandText = "IMspr_GetReportes_Estatus";


        //                cmd2.Connection = con;
        //                da2.SelectCommand = cmd2;
        //                da2.Fill(t2);
        //                //GridView3.DataSource = t2;


        //                cmd3.Parameters.Clear();
        //                cmd3.Parameters.Add("@desde", SqlDbType.VarChar).Value = Convert.ToDateTime(modRepo.desde).ToString("yyyy-dd-MM") + " 00:00:00";
        //                cmd3.Parameters.Add("@hasta", SqlDbType.VarChar).Value = Convert.ToDateTime(modRepo.hasta).ToString("yyyy-dd-MM") + " 23:59:59";
        //                cmd3.Parameters.Add("@nombre", SqlDbType.VarChar).Value = modRepo.name;
        //                cmd3.Parameters.Add("@apaterno", SqlDbType.VarChar).Value = modRepo.APaterno;
        //                cmd3.Parameters.Add("@amaterno", SqlDbType.VarChar).Value = modRepo.Amaterno;
        //                cmd3.Parameters.Add("@promotor", SqlDbType.VarChar).Value = modRepo.promotor;
        //                cmd3.Parameters.Add("@descripcion", SqlDbType.VarChar).Value = modRepo.Ddl_Estatus;
        //                cmd3.Parameters.Add("@tienda", SqlDbType.VarChar).Value = modRepo.tienda.Trim().Replace("Tienda ", "");
        //                cmd3.Parameters.Add("@idsolicitud", SqlDbType.VarChar).Value = modRepo.solicitud;
        //                cmd3.CommandText = "IMspr_GetReportes_Estatus_Descripcion";


        //                cmd3.Connection = con;
        //                da3.SelectCommand = cmd3;
        //                da3.Fill(t3);
        //                //GridView4.DataSource = t3;

        //                cmd4.Parameters.Clear();
        //                cmd4.Parameters.Add("@desde", SqlDbType.VarChar).Value = Convert.ToDateTime(modRepo.desde).ToString("yyyy-dd-MM") + " 00:00:00";
        //                cmd4.Parameters.Add("@hasta", SqlDbType.VarChar).Value = Convert.ToDateTime(modRepo.hasta).ToString("yyyy-dd-MM") + " 23:59:59";
        //                cmd4.Parameters.Add("@nombre", SqlDbType.VarChar).Value = modRepo.name;
        //                cmd4.Parameters.Add("@apaterno", SqlDbType.VarChar).Value = modRepo.APaterno;
        //                cmd4.Parameters.Add("@amaterno", SqlDbType.VarChar).Value = modRepo.Amaterno;
        //                cmd4.Parameters.Add("@promotor", SqlDbType.VarChar).Value = modRepo.promotor;
        //                cmd4.Parameters.Add("@descripcion", SqlDbType.VarChar).Value =modRepo.Ddl_Estatus;
        //                cmd4.Parameters.Add("@tienda", SqlDbType.VarChar).Value = modRepo.tienda.Replace("Tienda ", "");
        //                cmd4.Parameters.Add("@idsolicitud", SqlDbType.VarChar).Value = modRepo.solicitud;
        //                cmd4.CommandText = "IMspr_GetReportes_Total_Tienda";


        //                cmd4.Connection = con;
        //                da4.SelectCommand = cmd4;
        //                da4.Fill(t4);
        //                //GridView5.DataSource = t4;
        //                //GridView6.DataSource = t4;


        //                if (t.Rows.Count > 0)
        //                {
        //                    //LBRegistros.Visible = true;
        //                    //LBRegistros.Font.Bold = true;
        //                    //LBRegistros.Text = "Registros encontrados: " + t.Rows.Count.ToString();
        //                    //GridView1.Visible = true;
        //                    //GridView3.Visible = true;
        //                    //GridView4.Visible = true;
        //                    //GridView5.Visible = true;
        //                    //GridView6.DataBind();
        //                    //GridView5.DataBind();
        //                    //GridView4.DataBind();
        //                    //GridView3.DataBind();
        //                    //GridView1.DataBind();
        //                    //GridView2.DataBind();





        //                }

        //                else
        //                {
        //                    //Btn_Export_To_Excel.Visible = false;
        //                    //Btn_Excel2.Visible = false;
        //                    //LBRegistros.Visible = false;
        //                    //GridView1.Visible = false;
        //                    //GridView3.Visible = false;
        //                    //GridView4.Visible = false;
        //                    //GridView5.Visible = false;
        //                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Scroll", "alert('No hay coincidencias!')", true);
        //                }
        //                con.Close();

        //                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Scroll", "scroll();", true);


        //            }

        //            catch (Exception ex)
        //            {
        //                ScriptManager.RegisterClientScriptBlock(this, GetType(), "Popup32", "alert('Ocurrio Un Error, Por Favor Contacte Al Area De IT\\n" + ex.Message.Replace("'", "\"") + " ')", true);
        //            }
        //        }
        //    }
        //}
        #endregion

        #region Get Query and Metrics
        //private static string Get_Query_Filters()
        //{
        //    string filters = "";
        //    List<string> selectedRegion = (List<string>)HttpContext.Current.Session["selectedRegion"];
        //    if (selectedRegion != null && selectedRegion.Count > 0)
        //    {
        //        filters += " AND U.Region IN ( ";
        //        foreach (string region in selectedRegion)
        //        {
        //            filters += "'" + region.Trim() + "', ";
        //        }
        //        filters += ") ";
        //        filters = filters.Replace(", )", ")");
        //    }

        //    List<string> selectedSubregion = (List<string>)HttpContext.Current.Session["selectedSubregion"];
        //    if (selectedSubregion != null && selectedSubregion.Count > 0)
        //    {
        //        filters += " AND U.Subregion IN ( ";
        //        foreach (string subregion in selectedSubregion)
        //        {
        //            filters += "'" + subregion.Trim() + "', ";
        //        }
        //        filters += ") ";
        //        filters = filters.Replace(", )", ")");
        //    }


        //    List<string> selectedTienda = (List<string>)HttpContext.Current.Session["selectedTienda"];
        //    if (selectedTienda != null && selectedTienda.Count > 0)
        //    {
        //        filters += " AND A.tienda IN ( ";
        //        foreach (string tienda in selectedTienda)
        //        {
        //            filters += "'" + tienda.Trim() + "', ";
        //        }
        //        filters += ") ";
        //        filters = filters.Replace(", )", ")");
        //    }

        //    filters += " ";
        //    return filters;

        //}

        private List<MetricItem_Mapco> Get_Metrics_Data()
        {
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
            return items_Mapco;
        }
        #endregion
    }
}