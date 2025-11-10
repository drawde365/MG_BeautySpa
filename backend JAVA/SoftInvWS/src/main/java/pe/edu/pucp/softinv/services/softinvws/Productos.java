/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/WebServices/WebService.java to edit this template
 */
package pe.edu.pucp.softinv.services.softinvws;

import jakarta.jws.WebService;
import jakarta.jws.WebMethod;
import jakarta.jws.WebParam;
import java.util.ArrayList;
import pe.edu.pucp.softinv.business.BO.Impl.ProductoBO;
import pe.edu.pucp.softinv.model.Producto.ProductoDTO;

/**
 *
 * @author Rodrigo Cahuana
 */
@WebService(serviceName = "Productos")
public class Productos {
    private ProductoBO productoBO;
    
    public Productos(){
        this.productoBO = new ProductoBO();
    }
    
    @WebMethod(operationName = "insertarConDatos")
    public Integer insertarProducto1(@WebParam(name = "nombre")String nombre,@WebParam(name = "descripcion")String descripcion,@WebParam(name = "precio")Double precio,
                            @WebParam(name = "tamanho")Double tamanho,@WebParam(name = "urlImagen")String urlImagen,
                            @WebParam(name = "tipos")ArrayList<String> tipos,@WebParam(name = "ingreientes")ArrayList<String> ingredientes, 
                            @WebParam(name = "stocks")ArrayList<Integer> stock_fisico){
        return productoBO.insertar(nombre, descripcion, precio, tamanho, urlImagen, tipos, ingredientes, stock_fisico);
    }
    
    @WebMethod(operationName = "insertarProducto")
    public Integer insertarProducto2(@WebParam(name = "producto")ProductoDTO producto){
        return productoBO.insertar(producto);
    }
    
    @WebMethod(operationName = "ObtenerProducto")
    public ProductoDTO obtenerPorIdProducto(@WebParam(name = "id")Integer idProd){
        return productoBO.obtenerPorId(idProd);
    }
    
    @WebMethod(operationName = "ModificarProducto")
    public Integer modificarProducto(@WebParam(name = "producto")ProductoDTO producto){
        return productoBO.modificar(producto);
    }
    
    @WebMethod(operationName = "EliminarProducto")
    public Integer eliminarProducto(@WebParam(name = "producto")ProductoDTO producto){
        return productoBO.eliminar(producto);
    }
    
    @WebMethod(operationName = "buscarProductos")
    public ArrayList<ProductoDTO> buscarPorNombreProducto (@WebParam(name = "nombre")String nombre){
        return productoBO.buscarPorNombre(nombre);
    }
    
    @WebMethod(operationName = "obtenerProdPag")
    public ArrayList<ProductoDTO> obtenerPaginaProducto (@WebParam(name = "pagina")Integer pagina){
        return productoBO.obtenerPagina(pagina);
    }
    
    @WebMethod(operationName = "cantidadPagProd")
    public Integer cantidadPaginasProducto(){
        return productoBO.cantidadPaginas();
    }
}
