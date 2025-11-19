using SoftInvBusiness;
using SoftInvBusiness.SoftInvWSReportes;
using System;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace MGBeautySpaWebAplication.Admin
{
    public partial class Reportes : System.Web.UI.Page
    {

        private ReporteBO reporteBO;
        private filtroReporte filtro;

        protected void Page_Load(object sender, EventArgs e)
        {
            reporteBO = new ReporteBO();
            filtroReporte filtro = new filtroReporte();

            if (!IsPostBack)
            {
                inicializarFiltros("productos");
            }
            else
            {
                string tipoReporteActual = hdnTipoReporte.Value;
                inicializarFiltros(tipoReporteActual);
            }

            if (!string.IsNullOrEmpty(Request.QueryString["status"]))
            {
                string status = Request.QueryString["status"];
                string message = Server.UrlDecode(Request.QueryString["msg"]);

                if (status == "empty")
                {
                    // Mensaje para cuando no hay datos
                    litMensaje.Text = $"<div class='alert alert-warning'>{message}</div>";
                }
                else if (status == "error")
                {
                    // Mensaje para errores del sistema
                    litMensaje.Text = $"<div class='alert alert-danger'>❌ {message}</div>";
                }
            }
        }

        private void inicializarFiltros(string tipo)
        {
            if (tipo == "servicios")
            {
                // Activar Pestaña Servicios
                tabServicios.Attributes["class"] = "tab-active";
                tabProductos.Attributes["class"] = "tab-inactive";
                pnlFiltrosServicios.Style["display"] = "flex";
                pnlFiltrosProductos.Style["display"] = "none";
                hdnTipoReporte.Value = "servicios"; 
            }
            else
            {
                // Activar Pestaña Productos
                tabProductos.Attributes["class"] = "tab-active";
                tabServicios.Attributes["class"] = "tab-inactive";
                pnlFiltrosProductos.Style["display"] = "flex";
                pnlFiltrosServicios.Style["display"] = "none";
                hdnTipoReporte.Value = "productos"; 
            }
        }

        protected void btnGenerarReporte_Click(object sender, EventArgs e)
        {
            string tipoReporte = hdnTipoReporte.Value;

            filtro = new filtroReporte();

            DateTime? fechaInicio = null;
            DateTime? fechaFin = null;

            string periodo = ddlPeriodoTiempo.SelectedValue;

            byte[] reporteData = null;
            string nombreArchivo = "";

            if (periodo == "especifico")
            {
                if (DateTime.TryParse(txtFechaInicio.Text, out DateTime start) &&
                    DateTime.TryParse(txtFechaFin.Text, out DateTime end))
                {
                    fechaInicio = start;
                    fechaFin = end;
                }
                else
                {
                    return;
                }
            }
            else if (periodo == "mes")
            {
                fechaFin = DateTime.Today;
                fechaInicio = DateTime.Today.AddMonths(-1);
            }
            else if (periodo == "anual")
            {
                fechaFin = DateTime.Today;
                fechaInicio = DateTime.Today.AddYears(-1);
            }

            if (fechaInicio!=null) filtro.fechaInicio = fechaInicio.Value.Date;
            if (fechaFin!=null) filtro.fechaFin = fechaFin.Value.Date;

            if (tipoReporte == "productos")
            {
                //Filtros para productos
                string nombreProducto = (txtNombreProducto.Text.Trim().Length > 0) ? txtNombreProducto.Text.Trim() : null;
                string tipoProducto = (ddlTipoProducto.SelectedValue.Length > 0) ? ddlTipoProducto.SelectedValue : null;
                string estadoPedido = (ddlEstadoPedido.SelectedValue.Length > 0) ? ddlEstadoPedido.SelectedValue : null;


                filtro.nombreProducto = nombreProducto;
                filtro.tipoProducto = tipoProducto;
                filtro.estadoPedido = estadoPedido;

                reporteData = reporteBO.generarReporteVentas(filtro);
                nombreArchivo = $"Reporte_Ventas_{DateTime.Now:yyyyMMdd_HHmm}.pdf";

                string mensaje = $"Generando Reporte de PRODUCTOS:\n Producto: {nombreProducto}, Tipo: {tipoProducto}, Estado: {estadoPedido}, Desde: {fechaInicio?.ToShortDateString()}, Hasta: {fechaFin?.ToShortDateString()}";
                System.Diagnostics.Debug.WriteLine(mensaje);
            }
            else if (tipoReporte == "servicios")
            {
                // Filtros para Servicios
                string nombreServicio = (txtNombreServicio.Text.Trim().Length > 0) ? txtNombreServicio.Text.Trim() : null;
                string tipoServicio = (ddlTipoServicio.SelectedValue.Length > 0) ? ddlTipoServicio.SelectedValue : null;
                string nombreEmpleado = (txtNombreEmpleado.Text.Trim().Length > 0) ? txtNombreEmpleado.Text.Trim() : null;


                filtro.nombreServicio = nombreServicio;
                filtro.tipoServicio = tipoServicio;
                filtro.nombreEmpleado = nombreEmpleado;

                reporteData = reporteBO.generarReporteCitas(filtro);
                nombreArchivo = $"Reporte_Citas_{DateTime.Now:yyyyMMdd_HHmm}.pdf";

                
                string mensaje = $"Generando Reporte de SERVICIOS:\n Servicio: {nombreServicio}, Tipo: {tipoServicio}, Empleado ID: {nombreEmpleado}, Desde: {fechaInicio?.ToShortDateString()}, Hasta: {fechaFin?.ToShortDateString()}";
                System.Diagnostics.Debug.WriteLine(mensaje);
            }

            ForzarDescargaArchivo(reporteData,nombreArchivo);
        }

        protected void ForzarDescargaArchivo(byte[] reporteData,string nombreArchivo)
        {
            if (reporteData != null && reporteData.Length > 0)
            {
                HttpContext context = HttpContext.Current;

                // Prepara la respuesta HTTP
                context.Response.Clear();
                context.Response.Buffer = true;

                // Indica que el contenido es un PDF
                context.Response.ContentType = "application/pdf";

                // 🔑 Mantenemos 'attachment' para forzar la DESCARGA local
                context.Response.AppendHeader("Content-Disposition", $"attachment; filename=\"{nombreArchivo}\"");

                // Escribe el arreglo de bytes al flujo de salida del navegador
                context.Response.BinaryWrite(reporteData);

                // Finaliza la respuesta y el procesamiento de la página
                context.Response.Flush();
                context.Response.End();
            }
            else
            {
                string mensaje = "No se encontraron datos que coincidieran con los filtros seleccionados.";
                Response.Redirect($"Reportes.aspx?status=empty&msg={Server.UrlEncode(mensaje)}");
            }
        }
    }
}