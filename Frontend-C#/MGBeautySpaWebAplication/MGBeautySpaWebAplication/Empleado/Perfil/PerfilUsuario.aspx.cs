using System;
using System.Web.UI;

namespace MGBeautySpaWebAplication.Empleado
{
    public partial class PerfilUsuario : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarPerfil();
            }
        }

        private void CargarPerfil()
        {
            // Aquí simulo los datos que vendrían del usuario autenticado.
            // En tu caso, deberías reemplazar esto por la obtención real desde sesión o base de datos.
            string nombreUsuario = "Sophia Bennett";
            string nombres = "Sophia";
            string apellidos = "Bennett";
            string email = "a20230636@pucp.edu.pe";
            string telefono = "999888777";
            string fechaNacimiento = "12/02/2000";
            string sexo = "Femenino";

            // Asignación de valores a los controles ASP.NET Literal
            litUserNameGreeting.Text = nombreUsuario;
            litNombres.Text = nombres;
            litApellidos.Text = apellidos;
            litEmail.Text = email;
            litTelefono.Text = telefono;
            litFechaNac.Text = fechaNacimiento;
            litSexo.Text = sexo;
        }
    }
}