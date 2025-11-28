using SoftInvBusiness;
using SoftInvBusiness.SoftInvWSCita;
using SoftInvBusiness.SoftInvWSEmpleado;
using SoftInvBusiness.SoftInvWSServicio;
using SoftInvBusiness.SoftInvWSUsuario;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MGBeautySpaWebAplication.Cliente.Perfil
{
    public partial class Reservas : Page
    {
        private CitaBO citaBO;
        private EnvioCorreo envio;

        private int LimiteReservas
        {
            get { return (int)(ViewState["LimiteReservas"] ?? 3); }
            set { ViewState["LimiteReservas"] = value; }
        }

        private List<SoftInvBusiness.SoftInvWSCita.citaDTO> ListaCompletaReservas
        {
            get { return (List<SoftInvBusiness.SoftInvWSCita.citaDTO>)Session["ListaReservasCliente"]; }
            set { Session["ListaReservasCliente"] = value; }
        }

        public Reservas()
        {
            citaBO = new CitaBO();
            envio = new EnvioCorreo();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LimiteReservas = 3;
            }
            CargarReservas();
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
                ListaCompletaReservas = (reservas != null) ? reservas.ToList() : new List<SoftInvBusiness.SoftInvWSCita.citaDTO>();
            }
            else if (ListaCompletaReservas.Count == 0)
            {
                SoftInvBusiness.SoftInvWSCita.usuarioDTO user = new SoftInvBusiness.SoftInvWSCita.usuarioDTO();
                user.idUsuario = usuario.idUsuario;
                user.idUsuarioSpecified = true;
                user.rol = 1;
                user.rolSpecified = true;

                var reservas = citaBO.ListarPorUsuario(user);
                ListaCompletaReservas = (reservas != null) ? reservas.ToList() : new List<SoftInvBusiness.SoftInvWSCita.citaDTO>();
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

                var listaOrdenada = listaCompleta
                    .OrderByDescending(c => c.fecha)
                    .ThenByDescending(c => c.horaIni);

                var listaMapeada = listaOrdenada.Select(c => new {
                    CitaId = c.id,
                    NumeroReserva = c.id.ToString("D3"),
                    Servicio = c.servicio.nombre,
                    Fecha = c.fechaSpecified ? c.fecha.ToString("dd/MM/yyyy") : "N/A",
                    Empleado = $"{c.empleado.nombre} {c.empleado.primerapellido}",
                    Total = c.servicio.precioSpecified ?
                            c.servicio.precio.ToString("C", new CultureInfo("es-PE"))
                            : "S/ 0.00",
                    HoraInicio = !string.IsNullOrEmpty(c.horaIni)
                                     ? DateTime.Parse(c.horaIni, new CultureInfo("es-ES")).ToString("hh:mm tt")
                                     : "N/A",
                    Activo = c.activo,
                    FechaReal = c.fecha,
                    HoraRealStr = c.horaIni,
                    EmpleadoCorreo = c.empleado.correoElectronico,
                    EmpleadoCelular = c.empleado.celular,
                    Estado = c.activo == 1 ? "Pendiente" : (c.activo == 2 ? "Atendido" : "Cancelado")
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

        protected void rptReservas_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item ||
                e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var data = (dynamic)e.Item.DataItem;

                LinkButton btnCancelar = (LinkButton)e.Item.FindControl("btnCancelarCita");

                int activo = data.Activo;
                DateTime fechaReserva = data.FechaReal;
                string horaString = data.HoraRealStr;
                int citaId = data.CitaId;

                DateTime fullDateTime = fechaReserva.Date;
                if (!string.IsNullOrEmpty(horaString) && TimeSpan.TryParse(horaString, out TimeSpan hora))
                {
                    fullDateTime = fullDateTime.Add(hora);
                }

                bool esCancelable = (activo != 0) && (fullDateTime > DateTime.Now);

                if (esCancelable)
                {
                    btnCancelar.Visible = true;
                    btnCancelar.CommandArgument = citaId.ToString();
                    btnCancelar.OnClientClick = "";
                    btnCancelar.Attributes["data-citaid"] = citaId.ToString();
                }
                else
                {
                    btnCancelar.Visible = false;
                }
            }
        }

        protected async void btnConfirmarCancelacion_Click(object sender, EventArgs e)
        {
            if (int.TryParse(hfCitaIdCancelar.Value, out int citaIdParaEliminar))
            {
                var citaAEliminar = ListaCompletaReservas?
                    .FirstOrDefault(r => r.id == citaIdParaEliminar);

                if (citaAEliminar != null)
                {
                    citaBO.EliminarCita(citaAEliminar);
                    _ = Task.Run(async () =>
                    {
                        EnviarCorreoEmpleado(citaAEliminar);
                    });
                    ListaCompletaReservas = null;
                    CargarReservas();
                }
            }
        }

        private async Task EnviarCorreoEmpleado(SoftInvBusiness.SoftInvWSCita.citaDTO citaAEliminar)
        {
            var usuario = Session["UsuarioActual"] as SoftInvBusiness.SoftInvWSUsuario.usuarioDTO;

            string asunto = "Cancelación de reserva | MG Beauty SPA";
            string cuerpo = "¡Hola, " + citaAEliminar.empleado.nombre + "!\n\n" +
                            "Te informamos que la reserva programada por el cliente " + usuario.nombre + " " + usuario.primerapellido + " " + usuario.segundoapellido + " para el servicio " + citaAEliminar.servicio.nombre +
                            ", con fecha " + citaAEliminar.fecha.ToString("dd/MM/yyyy") + " " + citaAEliminar.horaIni.ToString() + ", ha sido cancelada.\n" + "Por favor, toma en cuenta este cambio en tu agenda. ¡Gracias!";
            await envio.enviarCorreo(citaAEliminar.empleado.correoElectronico,asunto,cuerpo,null);
        }
    }
}