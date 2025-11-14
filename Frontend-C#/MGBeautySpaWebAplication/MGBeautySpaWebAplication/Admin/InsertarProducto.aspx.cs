using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoftInvBusiness;
using SoftInvBusiness.SoftInvWSProductos;
using System.Web.Script.Serialization; // Necesario para JSON

namespace MGBeautySpaWebAplication.Admin
{
    public partial class InsertarProducto : System.Web.UI.Page
    {
        private ProductoBO productoBO;
        private ProductoTipoBO productoTipoBO;

        protected void Page_Load(object sender, EventArgs e)
        {
            productoBO = new ProductoBO();
            productoTipoBO = new ProductoTipoBO();
            if (!IsPostBack)
            {
                if (Request.QueryString["id"] != null)
                {
                    int idProducto = Convert.ToInt32(Request.QueryString["id"]);
                    CargarDatosProducto(idProducto);
                }
                else
                {
                    h1Titulo.InnerText = "Añadir Producto";
                    btnGuardar.Text = "Añadir Producto";
                }
            }
        }

        private void CargarDatosProducto(int idProducto)
        {
            try
            {
                productoDTO producto = productoBO.buscarPorId(idProducto);

                if (producto != null)
                {
                    h1Titulo.InnerText = "Editar Producto";
                    btnGuardar.Text = "Actualizar Producto";

                    txtTitulo.Text = producto.nombre;
                    txtDescripcion.Text = producto.descripcion;
                    txtPrecio.Text = producto.precio.ToString("F2");
                    txtTamaño.Text = producto.tamanho.ToString();
                    txtComoUsar.Text = producto.modoUso;
                    var tiposProductos = productoTipoBO.ObtenerPorIdProductoActivo(idProducto);
                    // --- INICIO: Lógica para cargar Tipos, Ingredientes y Stock ---
                    if (tiposProductos != null && tiposProductos.Count > 0)
                    {
                        var serializer = new JavaScriptSerializer();
                        var listaParaJson = new List<TipoProductoInput>();
                        bool esProductoFacial = false;

                        // Define los tipos de piel para saber si es Facial
                        var tiposDePielFacial = new List<string> { "Grasa", "Mixta", "Sensible", "Seca" };

                        foreach (var itemDTO in tiposProductos)
                        {
                            // 1. Repoblar el JSON para el HiddenField
                            listaParaJson.Add(new TipoProductoInput
                            {
                                // --> CAMBIO AQUÍ: Mapeo de tu DTO
                                tipo = itemDTO.tipo,
                                ingredientes = itemDTO.ingredientes,
                                stock = (int)itemDTO.stock_fisico // Asumiendo que stock_fisico es int/double
                            });

                            // 2. Marcar los Checkboxes de Tipo de Piel
                            if (tiposDePielFacial.Contains(itemDTO.tipo))
                            {
                                esProductoFacial = true;
                                var itemCheckbox = cblTiposPiel.Items.FindByValue(itemDTO.tipo);
                                if (itemCheckbox != null)
                                {
                                    itemCheckbox.Selected = true;
                                }
                            }
                        }

                        // 3. Seleccionar el RadioButton Principal
                        if (esProductoFacial)
                        {
                            rblTipoProducto.SelectedValue = "Facial";
                        }
                        else if (tiposProductos.Count > 0)
                        {
                            // Asume el primer tipo si no es facial (ej. "Corporal")
                            rblTipoProducto.SelectedValue = tiposProductos[0].tipo;
                        }

                        // 4. Guardar el JSON en el HiddenField para que JS lo lea
                        hdnIngredientesPorTipo.Value = serializer.Serialize(listaParaJson);
                    }
                    // --- FIN: Lógica de carga ---

                    if (!string.IsNullOrEmpty(producto.urlImagen))
                    {
                        hdnImagenActual.Value = producto.urlImagen;
                        fileUploadWrapper.Attributes["style"] = "background-image: url(" + ResolveUrl(producto.urlImagen) + ")";
                        fileUploadWrapper.Attributes["class"] += " has-preview";
                    }
                }
            }
            catch (Exception ex)
            {
                litError.Text = "Error al cargar el producto: " + ex.Message;
            }
        }

        protected void btnInsertarProducto_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsValid)
                {
                    return;
                }
                productoDTO producto = new productoDTO();
                producto.comentarios = new comentarioDTO[0];

                string rutaImagenServidor = string.Empty;

                string carpetaGuardado = Server.MapPath("~/Content/Uploads/Productos/");
                if (!Directory.Exists(carpetaGuardado))
                {
                    Directory.CreateDirectory(carpetaGuardado);
                }

                if (fileUpload.HasFile)
                {
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

                producto.nombre = txtTitulo.Text.Trim();
                producto.descripcion = txtDescripcion.Text.Trim();
                producto.modoUso = txtComoUsar.Text.Trim();
                producto.urlImagen = rutaImagenServidor;

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

                producto.promedioValoracion = 0;
                producto.promedioValoracionSpecified = true;
                producto.activo = 1;
                producto.activoSpecified = true;


                // --- INICIO: LÓGICA PARA OBTENER TIPOS, INGREDIENTES Y STOCK ---

                // 1. Obtén el JSON del HiddenField
                string jsonTipos = hdnIngredientesPorTipo.Value;

                // 2. Deserializa el JSON
                var serializer = new JavaScriptSerializer();
                var listaDeTiposInput = serializer.Deserialize<List<TipoProductoInput>>(jsonTipos);

                // 3. Convierte la lista de "Input" a tu lista de "DTO"
                var listaDto = new List<productoTipoDTO>();
                if (listaDeTiposInput != null)
                {
                    foreach (var itemInput in listaDeTiposInput)
                    {
                        var productoTipo = new productoTipoDTO();

                        // --> CAMBIO AQUÍ: Mapeo basado en tu DTO
                        productoTipo.tipo = itemInput.tipo;
                        productoTipo.ingredientes = itemInput.ingredientes;
                        productoTipo.stock_fisico = itemInput.stock;
                        productoTipo.stock_despacho = 0; // Valor por defecto
                        productoTipo.activo = 1;         // Valor por defecto

                        // Asumiendo campos 'Specified' para Web Service
                        productoTipo.stock_fisicoSpecified = true;
                        productoTipo.stock_despachoSpecified = true;
                        productoTipo.activoSpecified = true;

                        listaDto.Add(productoTipo);
                    }
                }

                // 4. Asigna la lista convertida a tu objeto principal
                producto.productosTipos = listaDto.ToArray();

                // --- FIN: LÓGICA DE TIPOS ---


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

                Response.Redirect("AdmProductos.aspx", false);
            }
            catch (Exception ex)
            {
                litError.Text = "Error al guardar el producto: " + ex.Message;
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("AdmProductos.aspx");
        }
    }

    /// <summary>
    /// Clase Helper para deserializar el JSON que viene del JavaScript.
    /// Sus propiedades (tipo, ingredientes, stock) deben coincidir
    /// con las claves del JSON que creamos en el script.
    /// </summary>
    public class TipoProductoInput
    {
        public string tipo { get; set; }
        public string ingredientes { get; set; }
        public int stock { get; set; }
    }
}