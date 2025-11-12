using SoftInvBusiness;
using SoftInvBusiness.SoftInvWSServicio; // Tu capa de negocio y DTO
using System;
using System.Globalization; // Para convertir el precio (decimal)
using System.IO; // Para manejar la subida de archivos (Path, Directory)
using System.Web.UI;
// Asumo que también tienes estos DTOs referenciados si los necesitas
// using SoftInvBusiness.SoftInvWSComentario; 
// using SoftInvBusiness.SoftInvWSEmpleado;
// using SoftInvBusiness.SoftInvWSCita;

namespace MGBeautySpaWebAplication.Admin
{
    public partial class InsertarServicio : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Aquí es donde cargarías dinámicamente los "Tipos de Servicio"
                // desde tu base de datos si no quisieras usar los valores estáticos del ASPX.
                // Ej: CargarTiposServicio();
            }
        }

        /// <summary>
        /// Maneja el clic en el botón "Cancelar".
        /// Simplemente redirige al usuario de vuelta a la lista de servicios.
        /// </summary>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Admin/AdmServicios.aspx");
        }

        /// <summary>
        /// Maneja el clic en "Añadir Servicio".
        /// Valida los datos, guarda la imagen y llama a la capa de negocio.
        /// </summary>
        protected void btnInsertarServicio_Click(object sender, EventArgs e)
        {
            // --- 1. VALIDAR Y PARSEAR LOS DATOS ---

            decimal precioServicio;
            // Usamos InvariantCulture para asegurarnos de que el decimal se parsee correctamente
            // independientemente de la configuración regional (ej. si usa ',' o '.')
            if (!decimal.TryParse(txtPrecio.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out precioServicio))
            {
                // Opcional: Mostrar un mensaje de error al usuario
                System.Diagnostics.Debug.WriteLine("Error: El precio ingresado no es un número válido.");
                return;
            }

            // --- 1.5. PARSEAR NUEVOS CAMPOS (Duración) ---
            int duracion = 0; // Valor por defecto

            // Esta es la forma correcta de convertir el texto a un entero (int)
            // Usamos TryParse porque es seguro: si el texto está vacío o no es un número,
            // 'duracion' simplemente se quedará en 0, y el programa no se caerá.
            if (!int.TryParse(txtDuracion.Text.Trim(), out duracion))
            {
                // Si el valor no es un número válido (ej: "abc"), 
                // asignamos 0.
                duracion = 0;
                // Opcional: Mostrar un mensaje de error al usuario
                System.Diagnostics.Debug.WriteLine("Advertencia: La duración no es un número válido, se usará 0.");
                // Si la duración es OBLIGATORIA, deberías hacer 'return;' aquí.
            }


            // --- 2. MANEJAR LA SUBIDA DE LA IMAGEN ---

            string relativeImagePath = "/Content/images/placeholder.png"; // Imagen por defecto

            if (fileUpload.HasFile) // ¿El usuario seleccionó un archivo?
            {
                try
                {
                    string fileName = Path.GetFileName(fileUpload.FileName);
                    string fileExtension = Path.GetExtension(fileName).ToLower();

                    // Validar que sea una imagen
                    if (fileExtension == ".jpg" || fileExtension == ".png" || fileExtension == ".jpeg" || fileExtension == ".gif")
                    {
                        // Crear un nombre de archivo único para evitar colisiones
                        string uniqueFileName = Guid.NewGuid().ToString() + fileExtension;

                        // Ruta física donde se guardará la imagen en el servidor
                        // Asegúrate de que la carpeta '~/Content/images/servicios/' exista
                        string serverSavePath = Server.MapPath("~/Content/images/servicios/");

                        // Si la carpeta no existe, la creamos
                        if (!Directory.Exists(serverSavePath))
                        {
                            Directory.CreateDirectory(serverSavePath);
                        }

                        string physicalPath = Path.Combine(serverSavePath, uniqueFileName);

                        // Guardar la imagen en el servidor
                        fileUpload.SaveAs(physicalPath);

                        // Esta es la ruta relativa que guardaremos en la Base de Datos
                        relativeImagePath = "/Content/images/servicios/" + uniqueFileName;
                    }
                    else
                    {
                        // Opcional: Mostrar error de tipo de archivo
                        System.Diagnostics.Debug.WriteLine("Error: Tipo de archivo no válido.");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    // Opcional: Mostrar error de subida
                    System.Diagnostics.Debug.WriteLine("Error al subir la imagen: " + ex.Message);
                    return;
                }
            }

            // --- 3. CREAR EL OBJETO DTO (Data Transfer Object) ---

            // Verificamos el tipo de 'precio'. Tu DTO Java usa 'Double'. 
            // Tu proxy WCF probablemente lo expone como 'double' en C#.
            // 'decimal' (C#) es mejor para dinero, pero lo convertimos a 'double'
            // para que coincida con tu DTO.
            double precioDto = (double)precioServicio;

            // ¡CONFLICTO IMPORTANTE!
            // Tu DTO Java usa 'TipoServicio tipo' (un objeto).
            // Tu DropDownList ('ddlTipoServicio') guarda un string ("Facial", "Masaje").
            //
            // SOLUCIÓN: Lo más probable es que tu DTO proxy de C# (SoftInvBusiness.SoftInvWSServicio)
            // NO espere un 'string', sino un objeto 'tipoServicioDTO'.
            //
            // DEBES CAMBIAR tu DropDownList en el .aspx para que guarde el ID del tipo:
            // <asp:ListItem Value="1" Text="Tratamiento Facial"></asp:ListItem>
            // <asp:ListItem Value="2" Text="Masaje Corporal"></asp:ListItem>
            //
            // Y luego crear el objeto así:
            /*
            tipoServicioDTO tipoDto = new tipoServicioDTO
            {
                // Asumiendo que el DTO de TipoServicio tiene un campo 'idTipoServicio'
                idTipoServicio = Convert.ToInt32(ddlTipoServicio.SelectedValue)
            };
            */


            servicioDTO nuevoServicio = new servicioDTO
            {
                // --- Campos que ya tenías ---
                nombre = txtTitulo.Text.Trim(),
                descripcion = txtDescripcion.Text.Trim(),

                // --- CAMBIOS BASADOS EN TU DTO ---

                // 1. 'precio' (Convertido de decimal a double)
                precio = precioDto,
                precioSpecified = true, // Indica que 'precio' tiene un valor

                // 2. 'tipo' (¡REVISAR CONFLICTO ARRIBA!)
                // Asumiendo que tu DTO proxy espera un objeto 'tipoServicioDTO'
                // tipo = tipoDto, // <- Esta sería la forma correcta

                // Dejaré la forma 'string' comentada por si tu proxy es diferente
                tipo = ddlTipoServicio.SelectedValue, // <-- Esto probablemente falle.

                // 3. 'urlImagen' (Cambiado de 'rutaImagen' para coincidir con tu DTO)
                urlImagen = relativeImagePath,

                // 4. 'promedioValoracion' (Nuevo servicio, valoración 0)
                promedioValoracion = 0,
                promedioValoracionSpecified = true, // Indica que 'promedioValoracion' tiene un valor

                // 5. 'activo' (Nuevo servicio, por defecto 1 = activo)
                activo = 1,
                activoSpecified = true,

                // 6. 'duracionHora' (Desde el campo que acabas de añadir al .aspx)
                duracionHora = duracion,
                duracionHoraSpecified = true, // Indica que 'duracionHora' tiene un valor

                // 7. Listas (Nulas para un nuevo servicio, la BD no las necesita)
                comentarios = null,
                empleados = null,
                citas = null

                // 'idServicio' no se envía, la BD lo genera (AUTO_INCREMENT).
            };


            // --- 4. LLAMAR A LA CAPA DE NEGOCIO (Business Object) ---

            try
            {
                ServicioBO bo = new ServicioBO();

                // Asumo que tu método para insertar se llama 'RegistrarServicio' o 'InsertarServicio'.
                // ¡Asegúrate de que este nombre de método sea correcto!
                bo.insertar(nuevoServicio);

                // --- 5. REDIRIGIR A LA LISTA ---
                // Si llegamos aquí, todo salió bien.
                Response.Redirect("~/Admin/AdmServicios.aspx");
            }
            catch (Exception ex)
            {
                // Opcional: Mostrar error si falla la inserción en la BD
                System.Diagnostics.Debug.WriteLine("Error al guardar en Base de Datos: " + ex.Message);
            }
        }
    }
}