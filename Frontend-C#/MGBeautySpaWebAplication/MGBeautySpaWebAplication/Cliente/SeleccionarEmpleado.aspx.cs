using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MGBeautySpaWebAplication.Cliente
{
    // --- 1. MODELO DE DATOS ---
    // (Puedes poner esta clase en un archivo separado, ej: Empleado.cs)
    public class Empleado
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string AvatarUrl { get; set; }
    }

    public partial class SeleccionarEmpleado : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Carga y enlaza los datos solo en la primera carga de la página
                CargarEmpleados();
            }
        }

        // --- 2. SIMULACIÓN DE DATOS ---
        private void CargarEmpleados()
        {
            // Esta es tu "base de datos" simulada de empleados.
            var listaEmpleados = new List<Empleado>
            {
                new Empleado
                {
                    Id = 1,
                    Nombre = "Sophia Bennet",
                    AvatarUrl = "https://images.unsplash.com/photo-1580489944761-15a19d654956?w=140&h=140&fit=crop&q=80"
                },
                new Empleado
                {
                    Id = 2,
                    Nombre = "Emma Raducana",
                    AvatarUrl = "https://images.unsplash.com/photo-1494790108377-be9c29b29330?w=140&h=140&fit=crop&q=80"
                },
                new Empleado
                {
                    Id = 3,
                    Nombre = "Ana Paula Cuadros",
                    AvatarUrl = "https://images.unsplash.com/photo-1544005313-94ddf0286df2?w=140&h=140&fit=crop&q=80"
                },
                new Empleado
                {
                    Id = 4,
                    Nombre = "Carlos Alcaraz",
                    AvatarUrl = "https://images.unsplash.com/photo-1500648767791-00dcc994a43e?w=140&h=140&fit=crop&q=80"
                }
            };

            // Enlaza la lista al control Repeater
            rpEmpleados.DataSource = listaEmpleados;
            rpEmpleados.DataBind(); // ¡Importante! Dibuja los datos en la página.
        }

        // --- 3. LÓGICA DE SIMULACIÓN (MANEJO DE CLICS) ---
        protected void rpEmpleados_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            // Verifica que el comando sea el que esperamos ("Select")
            if (e.CommandName == "Select")
            {
                // Obtiene los datos que pasamos en el CommandArgument
                string commandArgument = e.CommandArgument.ToString();
                string[] args = commandArgument.Split('|');
                
                string id = args[0];
                string nombre = args[1];

                // Simulación: Muestra una alerta JavaScript desde C#
                // Usamos '\n' para un salto de línea en la alerta.
                //string script = $"alert('Has seleccionado a {nombre} (ID: {id}).\\nAhora se mostraría su calendario...');";
                //ScriptManager.RegisterStartupScript(this, GetType(), "EmployeeSelectedAlert", script, true);

                // En una aplicación real, en lugar de la alerta, harías esto:
                // Response.Redirect($"Calendario.aspx?empleadoId={id}");
                Response.Redirect($"Calendario.aspx");
            }
        }
    }
}