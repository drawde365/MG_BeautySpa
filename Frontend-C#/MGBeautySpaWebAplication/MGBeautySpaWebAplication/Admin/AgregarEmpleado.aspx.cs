using SoftInvBusiness;
using SoftInvBusiness.SoftInvWSHorarioTrabajo;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MGBeautySpaWebAplication.Admin
{
    public partial class AgregarEmpleado : System.Web.UI.Page
    {
        private ServicioBO servicioBO;
        private HorarioTrabajoBO horarioTrabajoBO;
        private EmpleadoBO empleadoBO;
        CalendarioBO calendarioBO;
        protected void Page_Load(object sender, EventArgs e)
        {
            servicioBO = new ServicioBO();
            horarioTrabajoBO = new HorarioTrabajoBO();
            empleadoBO = new EmpleadoBO();
            calendarioBO = new CalendarioBO();

            if (!IsPostBack)
            {
                CargarServiciosCosmetologicos();
            }
        }

        // ================== CARGA DE SERVICIOS COSMETOLÓGICOS ==================

        private void CargarServiciosCosmetologicos()
        {
            try
            {
                servicioBO = new ServicioBO();
                var listaServicios = servicioBO.ListarTodoActivo();

                cblServicios.DataSource = listaServicios;
                cblServicios.DataTextField = "nombre";
                cblServicios.DataValueField = "idServicio";
                cblServicios.DataBind();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al cargar servicios cosmetológicos: " + ex.Message);
            }
        }

        // ================== CLICK GUARDAR ==================

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            Page.Validate();
            if (!Page.IsValid) return;

            // Refuerzo de longitudes por seguridad
            if (txtNombre.Text.Length > 30 ||
                txtPrimerApellido.Text.Length > 30 ||
                txtSegundoApellido.Text.Length > 30 ||
                txtCorreo.Text.Length > 100 ||
                txtContrasenha.Text.Length > 100 ||
                txtTelefono.Text.Length > 12)
            {
                var cvLen = new CustomValidator
                {
                    IsValid = false,
                    ErrorMessage = "Algunos campos superan la longitud máxima permitida."
                };
                Page.Validators.Add(cvLen);
                return;
            }

            var errores = new List<string>();

            // 1) Construir horarios desde los HiddenField (uno o varios rangos por día)
            List<HorarioDia> horarios = null;
            try
            {
                horarios = ConstruirHorariosDesdeHiddenFields();
                if (horarios.Count == 0)
                {
                    errores.Add("Debes definir al menos un rango de horario en algún día.");
                }
            }
            catch (Exception ex)
            {
                errores.Add(ex.Message);
            }

            if (errores.Count > 0)
            {
                foreach (var msg in errores)
                {
                    var cv = new CustomValidator
                    {
                        IsValid = false,
                        ErrorMessage = msg
                    };
                    Page.Validators.Add(cv);
                }
                return;
            }

            try
            {
                // 2) Crear empleado en WS Java (Empleado.InsertarEmpleadoPorPartes)
                int empleadoId = CrearEmpleadoSOAP();

                // 3) Registrar todos los horarios (HorarioTrabajo.InsertarHorarioTrabajo)
                RegistrarHorariosEmpleadoSOAP(empleadoId, horarios);
                
                calendarioBO.insertar30DiasCalendarioEmpleado(empleadoId);

                // 4) Asignar servicios cosmetológicos (Empleado.AgregarServicioAEmpleado)
                AsignarServiciosEmpleadoSOAP(empleadoId);

                // 5) Redirigir
                Response.Redirect("~/Admin/PanelDeControl.aspx");
            }
            catch (Exception ex)
            {
                var cv = new CustomValidator
                {
                    IsValid = false,
                    ErrorMessage = "Ocurrió un error al registrar el empleado: " + ex.Message
                };
                Page.Validators.Add(cv);
            }
        }

        // ================== LEER HORARIOS DESDE LOS HIDDENFIELD ==================

        private List<HorarioDia> ConstruirHorariosDesdeHiddenFields()
        {
            var lista = new List<HorarioDia>();

            ParseHorariosDeDia(hfHorariosLunes.Value, 1, "Lunes", lista);
            ParseHorariosDeDia(hfHorariosMartes.Value, 2, "Martes", lista);
            ParseHorariosDeDia(hfHorariosMiercoles.Value, 3, "Miércoles", lista);
            ParseHorariosDeDia(hfHorariosJueves.Value, 4, "Jueves", lista);
            ParseHorariosDeDia(hfHorariosViernes.Value, 5, "Viernes", lista);
            ParseHorariosDeDia(hfHorariosSabado.Value, 6, "Sábado", lista);

            return lista;
        }

        /// <summary>
        /// valor: formato "08:00-11:00;15:00-18:00"
        /// </summary>
        private void ParseHorariosDeDia(string valor, int diaSemana, string nombreDia, List<HorarioDia> lista)
        {
            valor = (valor ?? "").Trim();
            if (string.IsNullOrEmpty(valor)) return;

            string[] segmentos = valor.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var seg in segmentos)
            {
                string s = seg.Trim();
                if (s.Length == 0) continue;

                string[] partes = s.Split('-');
                if (partes.Length != 2)
                {
                    throw new Exception($"Formato de horario inválido en {nombreDia}: '{s}'.");
                }

                string iniStr = partes[0].Trim();
                string finStr = partes[1].Trim();

                if (!TimeSpan.TryParse(iniStr, out var ini))
                    throw new Exception($"Hora de inicio inválida en {nombreDia}: '{iniStr}'. Usa HH:mm.");

                if (!TimeSpan.TryParse(finStr, out var fin))
                    throw new Exception($"Hora de fin inválida en {nombreDia}: '{finStr}'. Usa HH:mm.");

                // Solo horas en punto
                if (ini.Minutes != 0 || fin.Minutes != 0)
                    throw new Exception($"En {nombreDia} solo se permiten horas exactas (ejemplo: 08:00, 15:00).");

                if (fin <= ini)
                    throw new Exception($"En {nombreDia}, la hora de fin debe ser mayor que la de inicio ({s}).");

                lista.Add(new HorarioDia
                {
                    DiaSemana = diaSemana,
                    HoraInicio = ini,
                    HoraFin = fin
                });
            }
        }

        // ================== INTEGRACIÓN CON WS Empleado ==================

        private int CrearEmpleadoSOAP()
        {
            empleadoBO = new EmpleadoBO();

            string nombre = txtNombre.Text.Trim();
            string primerApellido = txtPrimerApellido.Text.Trim();
            string segundoApellido = txtSegundoApellido.Text.Trim();
            string correo = txtCorreo.Text.Trim();
            string contrasenha = txtContrasenha.Text;
            string celular = txtTelefono.Text.Trim();

            string urlFotoPerfil = "~/Content/default_profile.png";
            bool admin = false; // desde esta pantalla solo se crean empleados normales

            int idEmpleado = empleadoBO.InsertarEmpleadoPorPartes(
                nombre,
                primerApellido,
                segundoApellido,
                correo,
                contrasenha,
                celular,
                urlFotoPerfil,
                admin
            );

            return idEmpleado;
        }

        // ================== INTEGRACIÓN CON WS HorarioTrabajo ==================

        private void RegistrarHorariosEmpleadoSOAP(int empleadoId, List<HorarioDia> horarios)
        {
            if (horarios == null || horarios.Count == 0) return;

            horarioTrabajoBO = new HorarioTrabajoBO();

            foreach (var h in horarios)
            {
                var horarioDto = new horarioTrabajoDTO();

                var empDto = new empleadoDTO
                {
                    idUsuario = empleadoId,
                    idUsuarioSpecified = true
                };

                horarioDto.empleado = empDto;
                horarioDto.diaSemana = h.DiaSemana;
                horarioDto.diaSemanaSpecified = true;

                horarioDto.horaInicio = h.HoraInicio.ToString(@"hh\:mm\:ss");
                horarioDto.horaFin = h.HoraFin.ToString(@"hh\:mm\:ss");

                var diferencia = h.HoraFin - h.HoraInicio;
                int numIntervalos = (int)diferencia.TotalHours;

                if (numIntervalos <= 0)
                {
                    throw new Exception(
                        $"El rango {h.HoraInicio:hh\\:mm}-{h.HoraFin:hh\\:mm} debe cubrir al menos 1 hora completa."
                    );
                }

                horarioDto.numIntervalo = numIntervalos;
                // Si el proxy generó numIntervaloSpecified:
                horarioDto.numIntervaloSpecified = true;

                horarioTrabajoBO.insertarHorarioDeEmpleado(horarioDto);
            }
        }

        // ================== INTEGRACIÓN CON WS Empleado (servicios) ==================

        private void AsignarServiciosEmpleadoSOAP(int empleadoId)
        {
            var idsServicios = new List<int>();

            foreach (ListItem item in cblServicios.Items)
            {
                if (item.Selected && int.TryParse(item.Value, out int idServicio))
                {
                    idsServicios.Add(idServicio);
                }
            }

            if (idsServicios.Count == 0) return;

            var clienteEmpleado = new EmpleadoBO();

            foreach (int idServicio in idsServicios)
            {
                clienteEmpleado.AgregarServicioAEmpleado(empleadoId, idServicio);
            }
        }

        // ================== DTO interno para manejar horarios ==================

        private class HorarioDia
        {
            public int DiaSemana { get; set; }      // 1 = Lunes ... 6 = Sábado
            public TimeSpan HoraInicio { get; set; }
            public TimeSpan HoraFin { get; set; }
        }
    }
}
