using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoftInvBusiness;
using SoftInvBusiness.SoftInvWSProductos;
using System.Web.Script.Serialization;

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

                    if (tiposProductos != null && tiposProductos.Count > 0)
                    {
                        var serializer = new JavaScriptSerializer();
                        var listaParaJson = new List<TipoProductoInput>();
                        bool esProductoFacial = false;

                        var tiposDePielFacial = new List<string> { "Grasa", "Mixta", "Sensible", "Seca" };

                        foreach (var itemDTO in tiposProductos)
                        {
                            if (itemDTO.tipo == null) continue;

                            listaParaJson.Add(new TipoProductoInput
                            {
                                tipo = itemDTO.tipo.nombre,
                                ingredientes = itemDTO.ingredientes,
                                stock = itemDTO.stock_fisicoSpecified ? itemDTO.stock_fisico : 0
                            });

                            if (tiposDePielFacial.Contains(itemDTO.tipo.nombre))
                            {
                                esProductoFacial = true;
                                var itemCheckbox = cblTiposPiel.Items.FindByValue(itemDTO.tipo.nombre);
                                if (itemCheckbox != null)
                                {
                                    itemCheckbox.Selected = true;
                                }
                            }
                        }

                        if (esProductoFacial)
                        {
                            rblTipoProducto.SelectedValue = "Facial";
                        }
                        else if (tiposProductos.Count > 0 && tiposProductos[0].tipo != null)
                        {
                            rblTipoProducto.SelectedValue = tiposProductos[0].tipo.nombre;
                        }

                        hdnIngredientesPorTipo.Value = serializer.Serialize(listaParaJson);
                    }

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

                string carpetaGuardado = Server.MapPath("~/Content/images/Productos/");
                if (!Directory.Exists(carpetaGuardado))
                {
                    Directory.CreateDirectory(carpetaGuardado);
                }

                if (fileUpload.HasFile)
                {
                    string nombreArchivo = Path.GetFileName(fileUpload.FileName);
                    string extension = Path.GetExtension(nombreArchivo).ToLower();
                    string[] extensionesValidas = { ".jpg", ".jpeg", ".png" };

                    if (Array.IndexOf(extensionesValidas, extension) < 0)
                    {
                        // Si la extensión no es válida, lanza una excepción o muestra un mensaje de error.
                        litError.Text = "Formato de archivo no válido. Solo se permiten imágenes (JPG, JPEG, PNG).";
                        return; // Detiene la ejecución
                    }
                    string nombreUnico = Guid.NewGuid().ToString() + extension;
                    string rutaCompleta = Path.Combine(carpetaGuardado, nombreUnico);
                    fileUpload.SaveAs(rutaCompleta);
                    rutaImagenServidor = "~/Content/images/Productos/" + nombreUnico;
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

                string jsonTipos = hdnIngredientesPorTipo.Value;

                var serializer = new JavaScriptSerializer();
                var listaDeTiposInput = serializer.Deserialize<List<TipoProductoInput>>(jsonTipos);

                var listaDto = new List<productoTipoDTO>();
                if (listaDeTiposInput != null)
                {
                    foreach (var itemInput in listaDeTiposInput)
                    {
                        var productoTipo = new productoTipoDTO();

                        var tipoProd = new tipoProdDTO();

                        switch (itemInput.tipo)
                        {
                            case "Corporal": tipoProd.id = 1; break;
                            case "Grasa": tipoProd.id = 2; break;
                            case "Seca": tipoProd.id = 3; break;
                            case "Mixta": tipoProd.id = 4; break;
                            case "Sensible": tipoProd.id = 5; break;
                            default: tipoProd.id = 0; break;
                        }
                        tipoProd.nombre = itemInput.tipo;

                        productoTipo.tipo = tipoProd;
                        productoTipo.ingredientes = itemInput.ingredientes;
                        productoTipo.stock_fisico = itemInput.stock;
                        productoTipo.stock_despacho = 0;
                        productoTipo.activo = 1;

                        productoTipo.stock_fisicoSpecified = true;
                        productoTipo.stock_despachoSpecified = true;
                        productoTipo.activoSpecified = true;

                        listaDto.Add(productoTipo);
                    }
                }

                producto.productosTipos = listaDto.ToArray();

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

    public class TipoProductoInput
    {
        public string tipo { get; set; }
        public string ingredientes { get; set; }
        public int stock { get; set; }
    }
}