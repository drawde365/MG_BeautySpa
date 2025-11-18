using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoftInvBusiness;
using SoftInvBusiness.SoftInvWSCita;
using SoftInvBusiness.SoftInvWSUsuario;

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

            if (ListaCompletaReservas == null)
            {
                SoftInvBusiness.SoftInvWSCita.usuarioDTO user = new SoftInvBusiness.SoftInvWSCita.usuarioDTO();
                user.idUsuario = usuario.idUsuario;
                user.idUsuarioSpecified = true;
                user.rol = 1;
                user.rolSpecified = true;

                var reservas = citaBO.ListarPorUsuario(user);
                ListaCompletaReservas = (reservas != null) ? reservas.ToList() : new List<citaDTO>();
            }

            var listaCompleta = ListaCompletaReservas;

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

                var listaMapeada = listaCompleta.Select(c => new {
                    CitaId = c.id,
                    NumeroReserva = c.id.ToString("D3"),
                    Servicio = c.servicio.nombre,
                    Fecha = c.fechaSpecified ? c.fecha.ToString("dd/MM/yyyy") : "N/A",
                    Empleado = $"{c.empleado.nombre} {c.empleado.primerapellido}",
                    Total = c.servicio.precioSpecified ?
                            c.servicio.precio.ToString("C", new CultureInfo("es-PE"))
                            : "S/ 0.00",
                    HoraInicio = c.horaIni,
                    EmpleadoCorreo = c.empleado.correoElectronico,
                    EmpleadoCelular = c.empleado.celular,
                    FechaReal = c.fecha
                    
                });

                var listaLimitada = listaMapeada.Take(LimiteReservas).ToList();
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

        // ✅ 2. CORREGIDO ItemDataBound
        protected void rptReservas_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item ||
                e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var data = (dynamic)e.Item.DataItem;
                LinkButton btnCancelar = (LinkButton)e.Item.FindControl("btnCancelarCita");

                // Obtenemos los datos para construir la fecha y hora completas
                DateTime fechaReserva = data.FechaReal;
                string horaString = data.HoraInicio; // E.g., "14:30"
                int citaId = data.CitaId; // Obtenemos el ID real

                DateTime fullDateTime = fechaReserva.Date; // Empezamos con la fecha
                if (!string.IsNullOrEmpty(horaString) && TimeSpan.TryParse(horaString, out TimeSpan hora))
                {
                    fullDateTime = fullDateTime.Add(hora); // Le añadimos la hora
                }

                // Comparamos la fecha y hora completas
                if (fullDateTime > DateTime.Now)
                {
                    btnCancelar.Visible = true;
                    // ✅ 3. ASIGNAMOS EL ID AL CommandArgument
                    btnCancelar.CommandArgument = citaId.ToString();
                    btnCancelar.OnClientClick = "return confirm('¿Estás seguro de que deseas cancelar esta cita?');";
                }
            }
        }

        // ✅ 4. CORREGIDO ItemCommand
        protected void rptReservas_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Cancelar")
            {
                // ✅ 5. LEEMOS EL ID DESDE CommandArgument
                int citaIdParaEliminar = Convert.ToInt32(e.CommandArgument);

                // Buscamos la cita en la lista completa usando el ID correcto
                var citaAEliminar = ListaCompletaReservas
                    .FirstOrDefault(r => r.id == citaIdParaEliminar);

                if (citaAEliminar != null)
                {
                    citaBO.EliminarCita(citaAEliminar); // Asumimos que EliminarCita toma el objeto

                    ListaCompletaReservas = null; // Forzamos la recarga desde la BD
                    CargarReservas();
                }
            }
        }
    }
}