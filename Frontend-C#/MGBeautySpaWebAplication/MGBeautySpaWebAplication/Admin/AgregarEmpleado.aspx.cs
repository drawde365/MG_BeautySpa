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
        protected void Page_Load(object sender, EventArgs e)
        {
            servicioBO = new ServicioBO();
            horarioTrabajoBO = new HorarioTrabajoBO();
            empleadoBO = new EmpleadoBO();
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
                // Ajusta "ServicioWS" al namespace de la Service Reference que crees en VS
                // (Add Service Reference -> Namespace = ServicioWS)
                //var clienteServicio = new ServicioWS.ServicioClient();
                servicioBO=new ServicioBO();
                // En Java:
                // @WebMethod(operationName = "ListarTodosActivos")
                // public ArrayList<ServicioDTO> listarTodosActivosServicios ()
                var listaServicios = servicioBO.ListarTodoActivo();

                cblServicios.DataSource = listaServicios;
                cblServicios.DataTextField = "nombre";       // ServicioDTO.nombre
                cblServicios.DataValueField = "idServicio";  // ServicioDTO.idServicio
                cblServicios.DataBind();
            }
            catch (Exception ex)
            {
                // No romper la página si falla la carga
                System.Diagnostics.Debug.WriteLine("Error al cargar servicios cosmetológicos: " + ex.Message);
            }
        }

        // ================== CLICK GUARDAR ==================

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            Page.Validate();
            if (!Page.IsValid) return;

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

                // 4) Asignar servicios cosmetológicos (Empleado.AgregarServicioAEmpleado)
                AsignarServiciosEmpleadoSOAP(empleadoId);

                // 5) Redirigir a la pantalla principal (puedes cambiarlo luego a lista de empleados)
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

            // Los HiddenField deben existir en el .aspx con estos IDs y ClientIDMode="Static"
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

                // El <input type="time"> envía formato HH:mm, TimeSpan lo entiende bien
                if (!TimeSpan.TryParse(iniStr, out var ini))
                    throw new Exception($"Hora de inicio inválida en {nombreDia}: '{iniStr}'. Usa HH:mm.");

                if (!TimeSpan.TryParse(finStr, out var fin))
                    throw new Exception($"Hora de fin inválida en {nombreDia}: '{finStr}'. Usa HH:mm.");

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

        /// <summary>
        /// Llama a Empleado.InsertarEmpleadoPorPartes y devuelve el ID del empleado creado.
        /// </summary>
        private int CrearEmpleadoSOAP()
        {
            // ServiceName en Java: "Empleado"
            // @WebMethod(operationName = "InsertarEmpleadoPorPartes")
            // public Integer insertarEmpleadoPorPartes(String nombre, String Primerapellido,
            //     String Segundoapellido, String correoElectronico, String contrasenha,
            //     String celular, String urlFotoPerfil, Boolean admin)

            empleadoBO=new EmpleadoBO();

            string nombreCompleto = txtNombreCompleto.Text.Trim();
            string nombre = nombreCompleto;
            string primerApellido = "";
            string segundoApellido = "";

            if (!string.IsNullOrEmpty(nombreCompleto))
            {
                var partes = nombreCompleto.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (partes.Length == 1)
                {
                    nombre = partes[0];
                }
                else if (partes.Length == 2)
                {
                    nombre = partes[0];
                    primerApellido = partes[1];
                }
                else
                {
                    nombre = partes[0];
                    primerApellido = partes[1];
                    segundoApellido = string.Join(" ", partes, 2, partes.Length - 2);
                }
            }

            string correo = txtCorreo.Text.Trim();
            string contrasenha = txtContrasenha.Text;
            string celular = txtTelefono.Text.Trim();

            // Puedes usar la misma URL que la master usa por defecto
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

        /// <summary>
        /// Inserta todos los horarios de un empleado usando HorarioTrabajo.InsertarHorarioTrabajo.
        /// </summary>
        private void RegistrarHorariosEmpleadoSOAP(int empleadoId, List<HorarioDia> horarios)
        {
            if (horarios == null || horarios.Count == 0) return;

            // ServiceName en Java: "HorarioTrabajo"
            // @WebMethod(operationName = "InsertarHorarioTrabajo")
            // public Integer insertarHorarioTrabajo(@WebParam(name = "horarioTrabajo") HorarioTrabajoDTO horarioTrabajo)

            horarioTrabajoBO=new HorarioTrabajoBO();

            foreach (var h in horarios)
            {
                var horarioDto = new horarioTrabajoDTO();

                // EmpleadoDTO dentro del namespace de HorarioTrabajoWS
                var empDto = new empleadoDTO();
                // Ajusta el nombre de la propiedad según lo que genere la referencia:
                // normalmente será idUsuario o IdUsuario
                empDto.idUsuario = empleadoId;
                empDto.idUsuarioSpecified = true;

                horarioDto.empleado = empDto;
                horarioDto.diaSemana = h.DiaSemana;
                horarioDto.diaSemanaSpecified = true;

                // java.sql.Time se mapea a DateTime en C# en el proxy;
                // usamos una fecha dummy y solo importa la hora.
                var fechaDummy = new DateTime(2000, 1, 1);

                //horarioDto.horaInicio = fechaDummy
                //    .AddHours(h.HoraInicio.Hours)
                //    .AddMinutes(h.HoraInicio.Minutes).ToString();

                //horarioDto.horaFin = fechaDummy
                //    .AddHours(h.HoraFin.Hours)
                //    .AddMinutes(h.HoraFin.Minutes).ToString();

                horarioDto.horaInicio = h.HoraInicio.ToString();
                horarioDto.horaFin = h.HoraFin.ToString();


                horarioTrabajoBO.insertarHorarioDeEmpleado(horarioDto);
            }
        }

        // ================== INTEGRACIÓN CON WS Empleado (servicios) ==================

        /// <summary>
        /// Usa Empleado.AgregarServicioAEmpleado para asignar todos los servicios seleccionados.
        /// </summary>
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

            if (idsServicios.Count == 0)
            {
                // no es obligatorio que tenga servicios al inicio
                return;
            }

            var clienteEmpleado = new EmpleadoBO();

            // @WebMethod(operationName = "AgregarServicioAEmpleado")
            // public void agregarServicioAEmpleado(Integer empleadoId, Integer servicioId)
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
