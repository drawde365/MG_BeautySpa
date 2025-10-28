using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MGBeautySpaWebAplication.Empleado
{
    public partial class MiHorario : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarHorario();
            }
            if (!IsPostBack)
            {
                // Revisa si hay un mensaje "flash" ANTES de cargar el horario
                MostrarMensajeFlash();

                // Tu lógica existente para cargar la tabla
                CargarHorario();
            }
        }

        protected void MostrarMensajeFlash()
        {
            // 1. Revisa si hay un mensaje en la Sesión
            if (Session["FlashMessage"] != null)
            {
                // 2. Asigna el texto al Literal
                litAlertMessage.Text = Session["FlashMessage"].ToString();

                // 3. Asigna la clase CSS de Bootstrap (verde)
                pnlAlert.CssClass = "alert alert-success alert-dismissible fade show";

                // 4. Haz visible el panel
                pnlAlert.Visible = true;

                // 5 Borra el mensaje de la Sesión para que no aparezca de nuevo
                Session["FlashMessage"] = null;
            }
        }

        private void CargarHorario()
        {
            // En un futuro, estos datos vendrían de la base de datos
            // new HorarioService().GetHorarioPorEmpleado(empleadoId);

            // Usamos "true" para 'ocupado' y "false" para 'libre'
            var listaHorario = new List<HorarioRow>
            {
                new HorarioRow { Hora = "8:00", Lunes = false, Martes = false, Miercoles = false, Jueves = false, Viernes = false, Sabado = false },
                new HorarioRow { Hora = "9:00", Lunes = true,  Martes = false, Miercoles = true,  Jueves = false, Viernes = true,  Sabado = false },
                new HorarioRow { Hora = "10:00", Lunes = true,  Martes = false, Miercoles = true,  Jueves = false, Viernes = true,  Sabado = false },
                new HorarioRow { Hora = "11:00", Lunes = true,  Martes = false, Miercoles = true,  Jueves = false, Viernes = true,  Sabado = false },
                new HorarioRow { Hora = "12:00", Lunes = true,  Martes = false, Miercoles = true,  Jueves = false, Viernes = true,  Sabado = false },
                new HorarioRow { Hora = "13:00", Lunes = false, Martes = false, Miercoles = false, Jueves = false, Viernes = false, Sabado = false },
                new HorarioRow { Hora = "14:00", Lunes = false, Martes = true,  Miercoles = false, Jueves = true,  Viernes = false, Sabado = false },
                new HorarioRow { Hora = "15:00", Lunes = true,  Martes = true,  Miercoles = true,  Jueves = true,  Viernes = false, Sabado = false },
                new HorarioRow { Hora = "16:00", Lunes = true,  Martes = true,  Miercoles = true,  Jueves = true,  Viernes = false, Sabado = false },
                new HorarioRow { Hora = "17:00", Lunes = true,  Martes = true,  Miercoles = true,  Jueves = true,  Viernes = false, Sabado = false },
                new HorarioRow { Hora = "18:00", Lunes = false, Martes = true,  Miercoles = false, Jueves = true,  Viernes = false, Sabado = false },
                new HorarioRow { Hora = "19:00", Lunes = false, Martes = false, Miercoles = false, Jueves = true,  Viernes = false, Sabado = true  },
                new HorarioRow { Hora = "20:00", Lunes = false, Martes = false, Miercoles = false, Jueves = true,  Viernes = false, Sabado = true  }
            };

            // Enlazamos la lista de datos con el control Repeater del .aspx
            rptHorario.DataSource = listaHorario;
            rptHorario.DataBind();
        }

        // --- MÉTODO DE AYUDA ---
        // Este método se llamará desde el .aspx para poner la clase CSS correcta
        protected string GetCellClass(bool isOccupied)
        {
            return isOccupied ? "cell-occupied" : "cell-free";
        }
    }
}