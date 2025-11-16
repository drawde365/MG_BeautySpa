using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoftInvBusiness; // Para los BOs
using SoftInvBusiness.SoftInvWSCita; // Para citaDTO
using SoftInvBusiness.SoftInvWSUsuario; // Para usuarioDTO

namespace MGBeautySpaWebAplication.Cliente.Perfil
{
    public partial class Reservas : Page
    {
        private CitaBO citaBO;

        private int LimiteReservas
        {
            get { return (int)(ViewState["LimiteReservas"] ?? 3); }
            set { ViewState["LimiteReservas"] = value; }
        }

        private List<citaDTO> ListaCompletaReservas
        {
            get { return (List<citaDTO>)Session["ListaReservasCliente"]; }
            set { Session["ListaReservasCliente"] = value; }
        }

        public Reservas()
        {
            citaBO = new CitaBO();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LimiteReservas = 3;
                CargarReservas();
            }
        }

        private void CargarReservas()
        {
            var usuario = Session["UsuarioActual"] as SoftInvBusiness.SoftInvWSUsuario.usuarioDTO;
            if (usuario == null)
            {
                Response.Redirect("~/Login.aspx?ReturnUrl=" + Request.RawUrl);
                return;
            }

            // 1. Obtener datos (de la BD o de la Sesión)
            if (ListaCompletaReservas == null)
            {
                SoftInvBusiness.SoftInvWSCita.usuarioDTO user = new SoftInvBusiness.SoftInvWSCita.usuarioDTO();
                user.idUsuario = usuario.idUsuario;
                user.idUsuarioSpecified = true;
                user.rol = 1;
                user.rolSpecified = true;
                // El BO de C# debe exponer el método del WS
                var reservas = citaBO.ListarPorUsuario(user);
                ListaCompletaReservas = (reservas != null) ? reservas.ToList() : new List<citaDTO>();
            }

            var listaCompleta = ListaCompletaReservas;

            // 2. Validar si hay reservas
            if (listaCompleta == null || !listaCompleta.Any())
            {
                rptReservas.Visible = false;
                btnVerMas.Visible = false;
                pnlNoReservas.Visible = true;
            }
            else
            {
                rptReservas.Visible = true;
                pnlNoReservas.Visible = false;

                // 3. Mapear y Limitar la lista para el Repeater
                var listaMapeada = listaCompleta.Select(c => new {
                    NumeroReserva = c.id.ToString("D3"), // Formato "001"
                    Servicio = c.servicio.nombre,
                    Fecha = c.fechaSpecified ? c.fecha.ToString("dd/MM/yyyy") : "N/A",
                    Empleado = $"{c.empleado.nombre} {c.empleado.primerapellido}",
                    Total = c.servicio.precioSpecified ? c.servicio.precio.ToString("C", new CultureInfo("es-PE")) : "S/ 0.00"
                });

                var listaLimitada = listaMapeada.Take(LimiteReservas).ToList();

                // 4. Enlazar Datos y mostrar/ocultar botón
                rptReservas.DataSource = listaLimitada;
                rptReservas.DataBind();

                btnVerMas.Visible = (LimiteReservas < listaCompleta.Count);
            }
        }

        protected void btnVerMas_Click(object sender, EventArgs e)
        {
            LimiteReservas += 3;
            CargarReservas();
        }

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Cliente/Perfil/PerfilUsuario.aspx");
        }
    }
}