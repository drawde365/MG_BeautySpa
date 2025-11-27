using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoftInvBusiness;
using SoftInvBusiness.SoftInvWSCalendario;
using SoftInvBusiness.SoftInvWSUsuario;
using SoftInvBusiness.SoftInvWSHorarioTrabajo;
using System.Globalization;

namespace MGBeautySpaWebAplication.Empleado
{
    public partial class AgregarExcepcion : System.Web.UI.Page
    {
        private CalendarioBO calendarioBO;
        private HorarioTrabajoBO horarioBO;

        public AgregarExcepcion()
        {
            calendarioBO = new CalendarioBO();
            horarioBO = new HorarioTrabajoBO();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var usuario = Session["UsuarioActual"] as SoftInvBusiness.SoftInvWSUsuario.usuarioDTO;
                if (usuario == null || usuario.rol != 2)
                {
                    Response.Redirect("~/Login.aspx");
                    return;
                }

                CargarFechasDisponibles(usuario.idUsuario);
            }
        }

        private void CargarFechasDisponibles(int idUsuario)
        {
            try
            {
                var listaDias = calendarioBO.ListarCalendarioDeEmpleado(idUsuario);
                var listaHorarios = horarioBO.ListarHorarioDeEmpleado(idUsuario);

                Dictionary<int, int> capacidadTeoricaPorDia = new Dictionary<int, int>();

                if (listaHorarios != null)
                {
                    foreach (var h in listaHorarios)
                    {
                        if (capacidadTeoricaPorDia.ContainsKey(h.diaSemana))
                            capacidadTeoricaPorDia[h.diaSemana] += h.numIntervalo;
                        else
                            capacidadTeoricaPorDia.Add(h.diaSemana, h.numIntervalo);
                    }
                }

                ddlFecha.Items.Clear();
                ddlFecha.Items.Add(new ListItem("Seleccione una fecha", ""));

                if (listaDias != null && listaDias.Count > 0)
                {
                    var diasFuturos = listaDias
                        .Where(c => c.fecha > DateTime.Now)
                        .OrderBy(c => c.fecha)
                        .ToList();

                    bool hayFechasSeleccionables = false;

                    foreach (var dia in diasFuturos)
                    {
                        if (dia.cantLibre <= 0) continue;

                        int diaSemanaBD = (int)dia.fecha.DayOfWeek;
                        if (diaSemanaBD == 0) diaSemanaBD = 7;

                        int capacidadTotal = 0;
                        if (capacidadTeoricaPorDia.ContainsKey(diaSemanaBD))
                        {
                            capacidadTotal = capacidadTeoricaPorDia[diaSemanaBD];
                        }

                        bool tieneReservas = (dia.cantLibre < capacidadTotal);

                        string textoVisual = dia.fecha.ToString("dddd, dd 'de' MMMM", new CultureInfo("es-ES"));
                        string valor = dia.fecha.ToString("yyyy-MM-dd");

                        if (tieneReservas)
                        {
                            textoVisual += " (Existen reservas - No se puede anular)";
                            ListItem item = new ListItem(textoVisual, valor);
                            item.Attributes["class"] = "option-disabled";
                            ddlFecha.Items.Add(item);
                        }
                        else
                        {
                            textoVisual += $" (Totalmente libre: {dia.cantLibre}h)";
                            ddlFecha.Items.Add(new ListItem(textoVisual, valor));
                            hayFechasSeleccionables = true;
                        }
                    }

                    if (!hayFechasSeleccionables)
                    {
                        if (ddlFecha.Items.Count == 1)
                        {
                            ddlFecha.Items.Add(new ListItem("No tienes días completos libres para excepción", ""));
                        }
                        btnRegistrarExcepcion.Enabled = false;
                    }
                    else
                    {
                        btnRegistrarExcepcion.Enabled = true;
                    }
                }
                else
                {
                    ddlFecha.Items.Add(new ListItem("No se encontró calendario configurado", ""));
                    btnRegistrarExcepcion.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MostrarError("Error al cargar calendario: " + ex.Message);
            }
        }

        protected void btnRegistrarExcepcion_Click(object sender, EventArgs e)
        {
            var usuario = Session["UsuarioActual"] as SoftInvBusiness.SoftInvWSUsuario.usuarioDTO;
            if (usuario == null) return;

            if (ddlFecha.SelectedValue == "" || ddlFecha.SelectedValue == "No tienes días completos libres para excepción")
            {
                MostrarError("Debes seleccionar una fecha válida.");
                return;
            }
            if (string.IsNullOrEmpty(txtMotivo.Text.Trim()))
            {
                MostrarError("Debes ingresar un motivo.");
                return;
            }

            DateTime fechaSeleccionada;
            if (!DateTime.TryParseExact(ddlFecha.SelectedValue, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out fechaSeleccionada))
            {
                MostrarError("Fecha inválida.");
                return;
            }

            try
            {
                calendarioDTO calendarioDia = new calendarioDTO();
                calendarioDia.empleado = new SoftInvBusiness.SoftInvWSCalendario.empleadoDTO { idUsuario = usuario.idUsuario, idUsuarioSpecified = true };
                calendarioDia.fecha = fechaSeleccionada;
                calendarioDia.fechaSpecified = true;

                calendarioDia.cantLibre = 0;
                calendarioDia.motivo = "[Excepción] " + txtMotivo.Text.Trim();

                int resultado = calendarioBO.EliminarCalendario(calendarioDia);

                if (resultado > 0)
                {
                    Session["FlashMessage"] = "¡Excepción registrada exitosamente!";
                    Response.Redirect("MiHorario.aspx");
                }
                else
                {
                    MostrarError("No se pudo registrar la excepción. Intente nuevamente.");
                }
            }
            catch (Exception ex)
            {
                MostrarError("Error al conectar con la base de datos: " + ex.Message);
            }
        }

        private void MostrarError(string mensaje)
        {
            lblError.Text = mensaje;
            lblError.Visible = true;
        }
    }
}