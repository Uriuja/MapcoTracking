﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Web.Services;
using MapcoSolutionTrackings.Models;

namespace MapcoSolutionTrackings.Controllers
{
    public class SearchController : Controller
    {
        private const string PARAMETER = "@PARAS";
        // GET: Search
        public ActionResult Index()
        {
            return View();
        }

        public Object Search(ModelReports modRepo)
        {
            UtilController util = new UtilController();
            String Connection_String = ConfigurationManager.ConnectionStrings["Mapco"].ConnectionString;
            List<ModelReports> items_Mapco = new List<ModelReports>();
            string Query = "";
            string Query2 = "";
            string Query3 = "";
            string Query4 = "";
            if (modRepo.Ddl_Estatus != "0")
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
                " and (A.IDSOLICITUD like '%" + modRepo.solicitud + "%')" +
                " and (A.Promotor like '%" + modRepo.promotor + "%')) " + 
                " ORDER BY A.IdSolicitud"; ;

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
                " and( (A.NOMBRES like '%" + modRepo.name + "%') and (A.APELLIDOPATERNO like '%" + modRepo.APaterno + "%')" +
                " and (A.APELLIDOMATERNO like '%" + modRepo.Amaterno + "%')" +
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
                " and( (A.NOMBRES like '%" + modRepo.name + "%') and (A.APELLIDOPATERNO like '%" + modRepo.APaterno + "%')" +
                " and (A.APELLIDOMATERNO like '%" + modRepo.Amaterno + "%')" +
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
                " and (A.IDSOLICITUD like '%" + modRepo.solicitud + "%')" +
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
                " and (A.Promotor like '%" + modRepo.promotor + "%')) " + PARAMETER + 
                " ORDER BY A.IdSolicitud";

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
                " and( (A.NOMBRES like '%" + modRepo.name + "%') and (A.APELLIDOPATERNO like '%" + modRepo.APaterno + "%')" +
                " and (A.APELLIDOMATERNO like '%" + modRepo.Amaterno + "%')" +
               // " and (B.Descripcion = '" + modRepo.Ddl_Estatus + "')" +
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
            string parameters = Get_Query_Filters(modRepo);
            query = query.Replace(PARAMETER, parameters);
            query2 = query2.Replace(PARAMETER, parameters);
            query3 = query3.Replace(PARAMETER, parameters);
            query4 = query4.Replace(PARAMETER, parameters);
            string constr = ConfigurationManager.ConnectionStrings["Mapco"].ToString(); // connection string
            SqlConnection con = new SqlConnection(constr);
            SqlConnection con2 = new SqlConnection(constr);
            SqlConnection con3 = new SqlConnection(constr);
            SqlConnection con4 = new SqlConnection(constr);
            try
            {
                              
                //System.Data.DataTable dt = new System.Data.DataTable();
                //System.Data.DataTable dt2 = new System.Data.DataTable();
                //System.Data.DataTable dt3 = new System.Data.DataTable();
                //System.Data.DataTable dt4 = new System.Data.DataTable();

                SqlCommand cmd = new SqlCommand(query, con);
                SqlCommand cmd2 = new SqlCommand(query2, con2);
                SqlCommand cmd3 = new SqlCommand(query3, con3);
                SqlCommand cmd4 = new SqlCommand(query4, con4);
                con.Open();
                SqlDataReader reader1 = cmd.ExecuteReader();
                List<ModelResults> resultList = new List<ModelResults>();
                while (reader1.Read())
                {
                    ModelResults result = new ModelResults();
                    result.noConsulta = reader1.GetValue(0).ToString();
                    //result.preCalificación = reader1.GetValue(1).ToString();
                    //result.confirmado = reader1.GetValue(2).ToString();
                    result.fecha = reader1.GetValue(1).ToString();
                    result.nombre = reader1.GetValue(2).ToString();
                    result.apePat = reader1.GetValue(3).ToString();
                    result.apeMat = reader1.GetValue(4).ToString();
                    result.estatus = reader1.GetValue(5).ToString();
                    result.motivoStatus = reader1.GetValue(6).ToString();
                    result.estatusRecepcion = reader1.GetValue(7).ToString();
                     result.promotor = reader1.GetValue(8).ToString();
                    result.tienda = reader1.GetValue(10).ToString();
                    resultList.Add(result);        
                }
                con.Close();
                con2.Open();
                SqlDataReader reader2 = cmd2.ExecuteReader();
                List<ModeltotalesA> resultListA = new List<ModeltotalesA>();
                while (reader2.Read())
                {
                    ModeltotalesA resultA = new ModeltotalesA();
                    resultA.ingresadas = reader2.GetValue(0).ToString();
                    resultA.aprobadas = reader2.GetValue(1).ToString();
                    resultA.rechazadas = reader2.GetValue(2).ToString();
                    resultA.pendientes = reader2.GetValue(3).ToString();
                    resultA.canceladas = reader2.GetValue(4).ToString();
                   resultListA.Add(resultA);
                }
                con2.Close();
                con3.Open();
                SqlDataReader reader3 = cmd3.ExecuteReader();
                List<ModeltotalesB> resultListB = new List<ModeltotalesB>();
                while (reader3.Read())
                {
                    ModeltotalesB resultB = new ModeltotalesB();
                    resultB.estatus = reader3.GetValue(0).ToString();
                    resultB.buroCredito = reader3.GetValue(1).ToString();
                    resultB.verificacionTelefonica = reader3.GetValue(2).ToString();
                    resultB.politicaDeCredito = reader3.GetValue(3).ToString();
                    resultB.solicitudDuplicada = reader3.GetValue(4).ToString();
                    resultB.documentacionIncompleta = reader3.GetValue(5).ToString();
                    resultB.pendienteDocumentacion = reader3.GetValue(6).ToString();
                    resultB.enProceso = reader3.GetValue(7).ToString();
                    resultListB.Add(resultB);
                }
                con3.Close();
                con4.Open();
                SqlDataReader reader4 = cmd4.ExecuteReader();
                List<ModeltotalesC> resultListC = new List<ModeltotalesC>();
                while (reader4.Read())
                {
                    ModeltotalesC resultC = new ModeltotalesC();
                    resultC.tienda = reader4.GetValue(0).ToString();
                    resultC.ingresadas = reader4.GetValue(1).ToString();
                    resultListC.Add(resultC);
                }
                con4.Close();
                ModelFull full = new ModelFull();
                full.modelResult = resultList;
                full.modelA = resultListA;
                full.modelB = resultListB;
                full.modelC = resultListC;


                return util.GetResponse(full, "Consulta Exitosa", true);
            }

            catch (Exception ex)
            {
                return util.GetResponse(ex, "Error de sistema", false);
            }
            //finally
            //{
            //    con.Close();
            //}
          
        }

        public Object GetStatus()
        {

            UtilController util = new UtilController();
            String Connection_String = ConfigurationManager.ConnectionStrings["Mapco"].ConnectionString;
            List<Status> statusList = new List<Status>();
            using (SqlConnection conn = new SqlConnection(Connection_String))
            {
                // using (SqlCommand cmd = new SqlCommand("SELECT distinct A.IdSolicitud ,Replace(CONVERT(VARCHAR,A.fecha,106), ' ', '-') as [Fecha], A.Nombres ,A.APELLIDOPATERNO, A.APELLIDOMATERNO  , B.DESCRIPCION ,A.Promotor,A.Tienda  FROM dbo.tblSolicitudes  AS A JOIN dbo.tblEstatus AS B ON A.EstatusActual=B.IDEstatus left  join dbo.tblNotas as  N on (A.IdSolicitud = N.IDSolicitud and N.IDNota = 0)" /* where A.FECHA >= '" + Convert.ToDateTime(Txt_Desde.Text).ToString("yyyy-dd-MM") + "'   and A.FECHA <= '" + Convert.ToDateTime(Txt_Hasta.Text).ToString("yyyy-dd-MM") + "'"*/, conn))
                using (SqlCommand cmd = new SqlCommand("select distinct B.descripcion as DESCRIPCION from tblEstatus B" +
                " inner join tblSolicitudes A on (B.IDEstatus COLLATE DATABASE_DEFAULT = A.EstatusActual COLLATE DATABASE_DEFAULT) order by B.descripcion asc", conn))
                {
                    try
                    {
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            Status status = new Status();
                            status.status = reader.GetString(0).Trim();
                            statusList.Add(status);
                         }
                        return util.GetResponse(statusList, "Consulta Exitosa", true);
                    }
                    catch (Exception ex)
                    {
                        return util.GetResponse(ex, "Error de sistema", false);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
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
                        return util.GetResponse(ex, "Error de sistema", false);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
            return util.GetResponse(items_Mapco, "Consulta Exitosa", true);
        }

        public Object Generar_Precalificacion(ModelReports modelPreca)
        {
            UtilController util = new UtilController();
            try
            {
                string Query = "";
                switch (modelPreca.aprobado)
                {
                    case "0":
                        Query = "select IDprecalificador,Precalificacion,Confirmado,Nombres,Ap_Paterno,Ap_Materno,Fecha_de_Nacimiento,Ciudad,Municipio,Estado,Tienda,promotor,Fecha   FROM [IMSOL_mp].[dbo].[TblPrecalificaciones] where Fecha>= '" + Convert.ToDateTime(modelPreca.desde).ToString("yyyy-dd-MM") + " 00:00:00' and Fecha<= '" + Convert.ToDateTime(modelPreca.hasta).ToString("yyyy-dd-MM") + " 23:59:59' order by Fecha";
                        break;
                    default:
                        Query = "select IDprecalificador,Precalificacion,Confirmado,Nombres,Ap_Paterno,Ap_Materno,Fecha_de_Nacimiento,Ciudad,Municipio,Estado,Tienda,promotor,Fecha   FROM [IMSOL_mp].[dbo].[TblPrecalificaciones] where Fecha>= '" + Convert.ToDateTime(modelPreca.desde).ToString("yyyy-dd-MM") + " 00:00:00' and Fecha<= '" + Convert.ToDateTime(modelPreca.hasta).ToString("yyyy-dd-MM") + " 23:59:59' and Precalificacion = '" + modelPreca.aprobado + "' order by Fecha";
                        break;
                }
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
                    result.noConsulta = reader1.GetValue(0).ToString();
                    result.preCalificación = reader1.GetValue(1).ToString();
                    result.confirmado = reader1.GetValue(2).ToString();
                    result.fecha = reader1.GetValue(12).ToString();
                    result.nombre = reader1.GetValue(3).ToString();
                    result.apePat = reader1.GetValue(4).ToString();
                    result.apeMat = reader1.GetValue(5).ToString();
                    result.fechaNacimiento = reader1.GetValue(6).ToString();
                    result.ciudad = reader1.GetValue(7).ToString();
                    result.municipio = reader1.GetValue(8).ToString();
                    result.estado = reader1.GetValue(9).ToString();
                    result.promotor = reader1.GetValue(11).ToString();
                    result.tienda = reader1.GetValue(10).ToString();
                    resultList.Add(result);
                }
                con.Close();
                return util.GetResponse(resultList, "Consulta Exitosa", true);



            }
            catch (Exception ex)
            {
                return util.GetResponse(ex, "Error de sistema", false);
            }

        }

        public Object Generar_ReporteNormal(ModelReports model)
        {
            UtilController util = new UtilController();
            int dia_mes = Convert.ToInt32(DateTime.Now.ToString("dd"));
            DateTime inicio_mes = DateTime.Now;
            inicio_mes = inicio_mes.AddDays(Convert.ToInt32((-dia_mes) + 1));
            DateTime minimo = inicio_mes.AddMonths(-2);
            string Minimo = Convert.ToString(minimo.ToString("yyyy-MM-dd"));
            if (Convert.ToDateTime(Minimo) > Convert.ToDateTime(model.desde))
            {

            }
            else
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable t = new DataTable();
                    SqlCommand cmd2 = new SqlCommand();
                    SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                    DataTable t2 = new DataTable();
                    SqlCommand cmd3 = new SqlCommand();
                    SqlDataAdapter da3 = new SqlDataAdapter(cmd3);
                    DataTable t3 = new DataTable();
                    SqlCommand cmd4 = new SqlCommand();
                    SqlDataAdapter da4 = new SqlDataAdapter(cmd4);
                    DataTable t4 = new DataTable();
                    string connString = ConfigurationManager.ConnectionStrings["Mapco"].ToString();
                    SqlConnection con = new SqlConnection(connString);
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd3.CommandType = CommandType.StoredProcedure;
                    cmd4.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add("@desde", SqlDbType.VarChar).Value = Convert.ToDateTime(model.desde).ToString("yyyy-dd-MM") + " 00:00:00";
                    cmd.Parameters.Add("@hasta", SqlDbType.VarChar).Value = Convert.ToDateTime(model.hasta).ToString("yyyy-dd-MM") + " 23:59:59";
                    cmd.Parameters.Add("@nombre", SqlDbType.VarChar).Value = model.name;
                    cmd.Parameters.Add("@apaterno", SqlDbType.VarChar).Value = model.APaterno;
                    cmd.Parameters.Add("@amaterno", SqlDbType.VarChar).Value = model.Amaterno;
                    cmd.Parameters.Add("@promotor", SqlDbType.VarChar).Value = model.promotor;
                    cmd.Parameters.Add("@descripcion", SqlDbType.VarChar).Value = model.Ddl_Estatus;
                    cmd.Parameters.Add("@tienda", SqlDbType.VarChar).Value = model.tienda.Replace("Tienda ", "");
                    cmd.Parameters.Add("@idsolicitud", SqlDbType.VarChar).Value = model.solicitud;
                    cmd.CommandText = "IMspr_GetReportes";
                    cmd.Connection = con;
                    da.SelectCommand = cmd;
                    da.Fill(t);
                    //TODO: Ciclar los resultados y meterlos a una lsita para que se regrese eso en un json
                    // De los 4 store procedure
                   
                    //GridView1.DataSource = t;
                    //GridView1.Columns[0].Visible = true;
                    //GridView2.DataSource = t;
                    cmd2.Parameters.Clear();
                    cmd2.Parameters.Add("@desde", SqlDbType.VarChar).Value = Convert.ToDateTime(model.desde).ToString("yyyy-dd-MM") + " 00:00:00";
                    cmd2.Parameters.Add("@hasta", SqlDbType.VarChar).Value = Convert.ToDateTime(model.hasta).ToString("yyyy-dd-MM") + " 23:59:59";
                    cmd2.Parameters.Add("@nombre", SqlDbType.VarChar).Value = model.name;
                    cmd2.Parameters.Add("@apaterno", SqlDbType.VarChar).Value = model.APaterno;
                    cmd2.Parameters.Add("@amaterno", SqlDbType.VarChar).Value = model.Amaterno;
                    cmd2.Parameters.Add("@promotor", SqlDbType.VarChar).Value = model.promotor;
                    cmd2.Parameters.Add("@descripcion", SqlDbType.VarChar).Value = model.Ddl_Estatus;
                    cmd2.Parameters.Add("@tienda", SqlDbType.VarChar).Value = model.tienda.Replace("Tienda ", "");
                    cmd2.Parameters.Add("@idsolicitud", SqlDbType.VarChar).Value = model.solicitud;
                    cmd2.CommandText = "IMspr_GetReportes_Estatus";
                    cmd2.Connection = con;
                    da2.SelectCommand = cmd2;
                    da2.Fill(t2);
                    //GridView3.DataSource = t2;
                    cmd3.Parameters.Clear();
                    cmd3.Parameters.Add("@desde", SqlDbType.VarChar).Value = Convert.ToDateTime(model.desde).ToString("yyyy-dd-MM") + " 00:00:00";
                    cmd3.Parameters.Add("@hasta", SqlDbType.VarChar).Value = Convert.ToDateTime(model.hasta).ToString("yyyy-dd-MM") + " 23:59:59";
                    cmd3.Parameters.Add("@nombre", SqlDbType.VarChar).Value = model.name;
                    cmd3.Parameters.Add("@apaterno", SqlDbType.VarChar).Value = model.APaterno;
                    cmd3.Parameters.Add("@amaterno", SqlDbType.VarChar).Value = model.Amaterno;
                    cmd3.Parameters.Add("@promotor", SqlDbType.VarChar).Value = model.promotor;
                    cmd3.Parameters.Add("@descripcion", SqlDbType.VarChar).Value = model.Ddl_Estatus;
                    cmd3.Parameters.Add("@tienda", SqlDbType.VarChar).Value = model.tienda.Replace("Tienda ", "");
                    cmd3.Parameters.Add("@idsolicitud", SqlDbType.VarChar).Value = model.solicitud;
                    cmd3.CommandText = "IMspr_GetReportes_Estatus_Descripcion";
                    cmd3.Connection = con;
                    da3.SelectCommand = cmd3;
                    da3.Fill(t3);
                    //GridView4.DataSource = t3;
                    cmd4.Parameters.Clear();
                    cmd4.Parameters.Add("@desde", SqlDbType.VarChar).Value = Convert.ToDateTime(model.desde).ToString("yyyy-dd-MM") + " 00:00:00";
                    cmd4.Parameters.Add("@hasta", SqlDbType.VarChar).Value = Convert.ToDateTime(model.hasta).ToString("yyyy-dd-MM") + " 23:59:59";
                    cmd4.Parameters.Add("@nombre", SqlDbType.VarChar).Value = model.name;
                    cmd4.Parameters.Add("@apaterno", SqlDbType.VarChar).Value = model.APaterno;
                    cmd4.Parameters.Add("@amaterno", SqlDbType.VarChar).Value = model.Amaterno;
                    cmd4.Parameters.Add("@promotor", SqlDbType.VarChar).Value = model.promotor;
                    cmd4.Parameters.Add("@descripcion", SqlDbType.VarChar).Value = model.Ddl_Estatus;
                    cmd4.Parameters.Add("@tienda", SqlDbType.VarChar).Value = model.tienda.Replace("Tienda ", "");
                    cmd4.Parameters.Add("@idsolicitud", SqlDbType.VarChar).Value = model.solicitud;
                    cmd4.CommandText = "IMspr_GetReportes_Total_Tienda";
                    cmd4.Connection = con;
                    da4.SelectCommand = cmd4;
                    da4.Fill(t4);
                    con.Close();
                    return util.GetResponse(t, "Consulta Exitosa", false);
                }
                catch (Exception ex)
                {
                    return util.GetResponse(ex, "Error de sistema", false);
                }
            }
            return null;
        }

        public Object Fill_Data(ModelReports model)
        {
            UtilController util = new UtilController();
            List<MetricItem_Mapco> items = Get_Metrics_Data();
            //List<string> selectedRegion = new List<string>();
            //foreach (ListItem item in model.region)
            //{
            //    if (item.Selected)
            //    {
            //        selectedRegion.Add(item.Text);
            //    }
            //}
            //List<string> selectedSubregion = new List<string>();
            //foreach (ListItem item in model.subregion)
            //{
            //    if (item.Selected)
            //    {
            //        selectedSubregion.Add(item.Text);
            //    }
            //}
            //List<string> selectedTienda = new List<string>();
            //foreach (ListItem item in LB_Tienda.Items)
            //{
            //    if (item.Selected)
            //    {
            //        selectedTienda.Add(item.Text);
            //    }
            //}
            List<string> selectedRegion = model.region;
            List<string> selectedSubregion = model.subregion;
            List<string> selectedTienda = model.tiendaSelect;
            if (selectedRegion.Count > 0)
            {
                items = (from item_Mapco in items
                         where selectedRegion.Contains(item_Mapco.Region)
                         select item_Mapco).ToList();
            }

            if (selectedSubregion.Count > 0)
            {
                items = (from item_Mapco in items
                         where selectedSubregion.Contains(item_Mapco.Subregion)
                         select item_Mapco).ToList();
            }

            if (selectedTienda.Count > 0)
            {
                items = (from item_Mapco in items
                         where selectedTienda.Contains(item_Mapco.Tienda)
                         select item_Mapco).ToList();
            }
            ModelReports response = new ModelReports();
            response = Fill_Filters(false, items,model);
           
            //response.region = selectedRegion;
            //response.subregion = selectedSubregion;
            //response.tiendaSelect = selectedTienda;
            return util.GetResponse(response, "Consulta Exitosa", true);
        }

        private ModelReports Fill_Filters(bool willClearFilters, List<MetricItem_Mapco> items, ModelReports model)
        {
            ModelReports response = new ModelReports();
            int count = model.region.Count();
            if (willClearFilters || count == 0)
            {
                response.region = (from item in items
                                        orderby item.Region
                                        select item.Region).Distinct().ToList();
            }
            count = model.subregion.Count();
            if (willClearFilters || count == 0)
            {
                response.subregion = (from item in items
                                           orderby item.Subregion
                                           select item.Subregion).Distinct().ToList();
     
            }
            count = model.tiendaSelect.Count();
            if (willClearFilters || count == 0)
            {
                response.tiendaSelect = (from item in items
                                        orderby item.Tienda
                                        select item.Tienda).Distinct().ToList();
            }
            return response;

            //int count = LB_Region.GetSelectedIndices().Count();
            //count = LB_Region.GetSelectedIndices().Count();
            //if (willClearFilters || count == 0)
            //{
            //    LB_Region.DataSource = (from item in items
            //                            orderby item.Region
            //                            select item.Region).Distinct().ToList();
            //    LB_Region.DataBind();
            //}

            //count = LB_Subregion.GetSelectedIndices().Count();
            //if (willClearFilters || count == 0)
            //{
            //    LB_Subregion.DataSource = (from item in items
            //                               orderby item.Subregion
            //                               select item.Subregion).Distinct().ToList();
            //    LB_Subregion.DataBind();
            //}


            //count = LB_Tienda.GetSelectedIndices().Count();
            //if (willClearFilters || count == 0)
            //{
            //    LB_Tienda.DataSource = (from item in items
            //                            orderby item.Tienda
            //                            select item.Tienda).Distinct().ToList();
            //    LB_Tienda.DataBind();
            //}
        }

        public List<MetricItem_Mapco> Get_Metrics_Data()
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
                            /* item_Dportenis.IdSolicitud = reader.GetInt32(0);
                             item_Dportenis.Fecha = reader.GetString(1).Trim();
                             item_Dportenis.Nombres = reader.GetString(2).Trim();
                             item_Dportenis.APELLIDOPATERNO = reader.GetString(3).Trim();
                             item_Dportenis.APELLIDOMATERNO = reader.GetString(4).Trim();
                             item_Dportenis.DESCRIPCION = reader.GetString(5).Trim();
                             item_Dportenis.Promotor = reader.GetString(6).Trim();
                             item_Dportenis.Tienda = reader.GetString(7).Trim();*/
                            item_Mapco.Region = reader.GetString(0).Trim();
                            item_Mapco.Subregion = reader.GetString(1).Trim();
                            item_Mapco.Tienda = reader.GetString(2).Trim();
                            items_Mapco.Add(item_Mapco);
                        }
                    }
                    catch (Exception ex)
                    {
                        return null;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
            return items_Mapco;
        }

        public string Get_Query_Filters(ModelReports model)
        {
            string filters = "";
            List<string> selectedRegion = model.region;
            if (selectedRegion != null && selectedRegion.Count > 0)
            {
                filters += " AND U.Region IN ( ";
                foreach (string region in selectedRegion)
                {
                    filters += "'" + region.Trim() + "', ";
                }
                filters += ") ";
                filters = filters.Replace(", )", ")");
            }
            List<string> selectedSubregion = model.subregion;
            if (selectedSubregion != null && selectedSubregion.Count > 0)
            {
                filters += " AND U.Subregion IN ( ";
                foreach (string subregion in selectedSubregion)
                {
                    filters += "'" + subregion.Trim() + "', ";
                }
                filters += ") ";
                filters = filters.Replace(", )", ")");
            }
            List<string> selectedTienda = model.tiendaSelect;
            if (selectedTienda != null && selectedTienda.Count > 0)
            {
                filters += " AND A.tienda IN ( ";
                foreach (string tienda in selectedTienda)
                {
                    filters += "'" + tienda.Trim() + "', ";
                }
                filters += ") ";
                filters = filters.Replace(", )", ")");
            }
            filters += " ";
            return filters;

        }


       


    }
}