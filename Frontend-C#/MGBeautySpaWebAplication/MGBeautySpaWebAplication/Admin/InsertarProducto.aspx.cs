using System;
using System.Collections.Generic;
using System.IO; // Necesario para manejar archivos
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoftInvBusiness;
using SoftInvBusiness.SoftInvWSProductos;

namespace MGBeautySpaWebAplication.Admin
{
    public partial class InsertarProducto : System.Web.UI.Page
    {
        private ProductoBO productoBO;

        protected void Page_Load(object sender, EventArgs e)
        {
            productoBO = new ProductoBO();
            if (!IsPostBack)
            {
                // Verifica si viene un "id" en la URL (ej. ...?id=5)
                if (Request.QueryString["id"] != null)
                {
                    // --- MODO EDITAR ---
                    int idProducto = Convert.ToInt32(Request.QueryString["id"]);
                    CargarDatosProducto(idProducto);
                }
                else
                {
                    // --- MODO INSERTAR ---
                    h1Titulo.InnerText = "Añadir Producto";
                    btnGuardar.Text = "Añadir Producto";
                }
            }
        }

        /// <summary>
        /// Obtiene los datos de un producto (de la BD) y rellena el formulario.
        /// </summary>
        private void CargarDatosProducto(int idProducto)
        {
            try
            {
                // 1. Llama a tu servicio para obtener el producto
                productoDTO producto = productoBO.buscarPorId(idProducto);

                if (producto != null)
                {
                    // 2. Cambia los textos de la UI
                    h1Titulo.InnerText = "Editar Producto";
                    btnGuardar.Text = "Actualizar Producto";

                    // 3. Rellena los campos del formulario
                    txtTitulo.Text = producto.nombre;
                    txtDescripcion.Text = producto.descripcion;
                    txtPrecio.Text = producto.precio.ToString("F2");
                    txtTamaño.Text = producto.tamanho.ToString();
                    //txtBeneficios.Text = producto.beneficios;
                    txtComoUsar.Text = producto.modoUso;
                    //ddlTipoProducto.SelectedValue = producto.productosTipos.ToString();

                    // 4. Maneja la imagen actual
                    if (!string.IsNullOrEmpty(producto.urlImagen))
                    {
                        // Guarda la ruta de la imagen "vieja" en el campo oculto
                        hdnImagenActual.Value = producto.urlImagen;

                        // Aplica los estilos de preview al 'wrapper' desde el servidor
                        fileUploadWrapper.Attributes["style"] = "background-image: url(" + ResolveUrl(producto.urlImagen) + ")";
                        fileUploadWrapper.Attributes["class"] += " has-preview";
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar error (ej. mostrar un literal de error)
                 litError.Text = "Error al cargar el producto: " + ex.Message;
            }
        }

        /// <summary>
        /// Lógica principal que se dispara al hacer clic en "Añadir" o "Actualizar".
        /// </summary>
        protected void btnInsertarProducto_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsValid)
                {
                    return;
                }
                productoDTO producto = new productoDTO();
                producto.productosTipos = new productoTipoDTO[0];
                producto.comentarios = new comentarioDTO[0];

                string rutaImagenServidor = string.Empty;

                // 2. Lógica de Subida de Archivo (Tu código aquí estaba bien)
                string carpetaGuardado = Server.MapPath("~/Content/Uploads/Productos/");
                if (!Directory.Exists(carpetaGuardado))
                {
                    Directory.CreateDirectory(carpetaGuardado);
                }

                if (fileUpload.HasFile)
                {
                    // ... (tu lógica de guardar archivo) ...
                    string nombreArchivo = Path.GetFileName(fileUpload.FileName);
                    string extension = Path.GetExtension(nombreArchivo).ToLower();
                    if (extension != ".jpg" && extension != ".png" && extension != ".jpeg" && extension != ".gif")
                    {
                        litError.Text = "Solo se permiten archivos .jpg, .png o .gif.";
                        return;
                    }
                    string nombreUnico = Guid.NewGuid().ToString() + extension;
                    string rutaCompleta = Path.Combine(carpetaGuardado, nombreUnico);
                    fileUpload.SaveAs(rutaCompleta);
                    rutaImagenServidor = "~/Content/Uploads/Productos/" + nombreUnico;
                }
                else if (Request.QueryString["id"] != null)
                {
                    rutaImagenServidor = hdnImagenActual.Value;
                }

                // 3. Recopilar datos del formulario
                producto.nombre = txtTitulo.Text.Trim();
                producto.descripcion = txtDescripcion.Text.Trim();
                producto.modoUso = txtComoUsar.Text.Trim();
                producto.urlImagen = rutaImagenServidor;

                // --- Conversión Segura de Números (incluyendo ...Specified) ---
                double precio;
                if (double.TryParse(txtPrecio.Text, System.Globalization.NumberStyles.Any,
                                     System.Globalization.CultureInfo.InvariantCulture, out precio))
                {
                    producto.precio = precio;
                    producto.precioSpecified = true;
                }

                double tamanho;
                if (double.TryParse(txtTamaño.Text, System.Globalization.NumberStyles.Any,
                                     System.Globalization.CultureInfo.InvariantCulture, out tamanho))
                {
                    producto.tamanho = tamanho;
                    producto.tamanhoSpecified = true;
                }

                // Asignamos 0 a los campos numéricos que no se llenan en el form
                producto.promedioValoracion = 0;
                producto.promedioValoracionSpecified = true;

                producto.activo = 1;
                producto.activoSpecified = true;

                // --- Lógica para el Tipo de Producto (DropDownList) ---
                int idTipo = 0;
                if (!string.IsNullOrEmpty(ddlTipoProducto.SelectedValue))
                {
                    // (Asumiendo que el Value del DDL es el ID, ej: "1", "2")
                    int.TryParse(ddlTipoProducto.SelectedValue, out idTipo);
                }


                // 4. Decidir si Insertar o Actualizar
                if (Request.QueryString["id"] != null)
                {
                    producto.idProducto = Convert.ToInt32(Request.QueryString["id"]);
                    producto.idProductoSpecified = true;
                    productoBO.modificarProducto(producto);
                }
                else
                {
                    productoBO.insertarProducto(producto);
                }

                // 5. Redirigir a la lista de productos
                Response.Redirect("AdmProductos.aspx", false);
            }
            catch (Exception ex)
            {
                // Esto ahora te mostrará el *siguiente* error (si es que hay uno)
                litError.Text = "Error al guardar el producto: " + ex.Message;
            }
        }

        /// <summary>
        /// Lógica para el botón "Cancelar".
        /// </summary>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("AdmProductos.aspx");
        }
    }
}