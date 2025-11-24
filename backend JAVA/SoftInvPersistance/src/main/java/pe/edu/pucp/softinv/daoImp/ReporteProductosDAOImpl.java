/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package pe.edu.pucp.softinv.daoImp;

import java.sql.SQLException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.List;
import pe.edu.pucp.softinv.dao.Reportable;
import pe.edu.pucp.softinv.dao.ReporteProductosDAO;
import pe.edu.pucp.sotfinv.model.Reportes.DatoReporteProductos;
import pe.edu.pucp.sotfinv.model.Reportes.FiltroReporte;

/**
 *
 * @author Usuario
 */
public class ReporteProductosDAOImpl extends DAOImplBase implements ReporteProductosDAO, Reportable<DatoReporteProductos>{
    
    private DatoReporteProductos dato;

    public ReporteProductosDAOImpl() {
        super("");
        dato = new DatoReporteProductos();
    }

    @Override
    protected void configurarListaDeColumnas() {
    }

    @Override
    public ArrayList<DatoReporteProductos> obtenerReporte(FiltroReporte filtro) {
        return (ArrayList<DatoReporteProductos>) super.obtenerReporte(filtro);
    }
    
    
    @Override
    protected String obtenerSQLBase(){
        return "SELECT T.PEDIDO_ID, T.ESTADO, " +
                "P.NOMBRE AS NOMBRE_PRODUCTO, S.NOMBRE as TIPO, P.PRECIO AS PRECIO_UNITARIO, " +
                "D.CANTIDAD, D.SUBTOTAL, T.FECHA_PAGO, T.FECHA_RECOJO " +
                "FROM PEDIDOS T " +
                "INNER JOIN DETALLES_PEDIDOS D ON T.PEDIDO_ID = D.PEDIDO_ID " +
                "INNER JOIN PRODUCTOS P ON D.PRODUCTO_ID = P.PRODUCTO_ID " +
                "INNER JOIN TIPOS_PRODS S ON D.TIPO_ID = S.TIPO_ID " +
                "WHERE T.ESTADO NOT IN ('EnCarrito', 'ELIMINADO') ";
    }

    @Override
    protected void aplicarFiltros(StringBuilder sql, List<Object> params, FiltroReporte filtro){
        
        if (filtro.getFechaInicio()!=null){
            sql.append(" AND T.FECHA_PAGO >= ?");
            params.add(new java.sql.Date(filtro.getFechaInicio().getTime()));
            //Modificable: Por un lado, el no usar el java.sql.Data en los DTOs los hace mucho más robustos
            // y no los ata a sql, por el otro, lleva a este tipo de conversión para que la bd lo entienda
        }
        
        if (filtro.getFechaFin()!=null){
            sql.append(" AND T.FECHA_PAGO <= ?");
            params.add(new java.sql.Date(filtro.getFechaFin().getTime()));
        }
        
        if (filtro.getEstadoPedido()!=null){
            sql.append(" AND T.ESTADO = ?");
            params.add(filtro.getEstadoPedido());
        }
        
        if (filtro.getNombreProducto()!=null){
            sql.append(" AND P.NOMBRE LIKE ?");
            params.add("%" + filtro.getNombreProducto() + "%");
        }
        
        if (filtro.getTipoProducto()!=null){
            sql.append(" AND S.NOMBRE = ?");
            params.add(filtro.getTipoProducto());
        }
        
    }
    
    @Override
    protected void incluirValorDeParametrosParaReporte(List<Object> params) throws SQLException {
        if (params == null || params.isEmpty()) return;
        for (int i = 0; i < params.size(); i++)
            this.statement.setObject(i + 1, params.get(i));
    }
    
    @Override
    protected void instanciarObjetoDelResultSet() throws SQLException {
        this.dato = new DatoReporteProductos();
        this.dato.setPedido_id(this.resultSet.getInt("PEDIDO_ID"));
        this.dato.setEstado(this.resultSet.getString("ESTADO"));
        this.dato.setNombreProducto(this.resultSet.getString("NOMBRE_PRODUCTO"));
        this.dato.setTipo(this.resultSet.getString("TIPO"));
        this.dato.setPrecioUnitario(this.resultSet.getDouble("PRECIO_UNITARIO"));
        this.dato.setCantidad(this.resultSet.getInt("CANTIDAD"));
        this.dato.setSubtotal(this.resultSet.getDouble("SUBTOTAL"));
        this.dato.setFecha_pago(this.resultSet.getTimestamp("FECHA_PAGO"));
        this.dato.setFecha_recojo(this.resultSet.getTimestamp("FECHA_RECOJO"));
    }
    
    @Override
    protected void agregarObjetoALaLista(List lista) throws SQLException {
        this.instanciarObjetoDelResultSet();
        lista.add(this.dato);
    }
    
    @Override
    public String[] getTitulosColumnas() {
        return new String[] {"Id. Pedido", "Estado del Pedido","Fecha de pago", "Fecha de recojo", 
            "Nombre del Producto", "Tipo","Precio Unitario", "Cantidad", "Subtotal",};
    }
    
    @Override
    public String[] getDatosFila() {
        SimpleDateFormat sdf = new SimpleDateFormat("dd/MM/yyyy");
        return new String[] {
            this.dato.getPedido_id().toString(),
            this.dato.getEstado(),
            (this.dato.getFecha_pago()!=null) ? sdf.format(this.dato.getFecha_pago()) : "-",
            (this.dato.getFecha_recojo()!=null) ? sdf.format(this.dato.getFecha_recojo()) : "-",
            this.dato.getNombreProducto(),
            this.dato.getTipo(),
            String.format("S/.%.2f", this.dato.getPrecioUnitario()),
            this.dato.getCantidad().toString(),
            String.format("S/.%.2f", this.dato.getSubtotal()),
        };
    }

    @Override
    public float[] getAnchosColumnas() {
        return new float[] {1f, 1.5f, 1.5f, 1.5f, 3f, 1f, 1.5f, 1.5f, 1.5f};
    }

    @Override
    public double getMontoTotal() {
        return this.dato.getSubtotal();
    }
    
    @Override
    public void assign(DatoReporteProductos dato){
        this.dato = new DatoReporteProductos(dato);
    }
    
    @Override
    public String getTitulo(){
        return "Reporte de Ventas\n";
    }
    
    @Override
    public String[] getSubtitulos(FiltroReporte filtro){
        String prod = (filtro.getNombreProducto()!=null) ? filtro.getNombreProducto() : "Todos";
        String producto = "Producto: " + prod + "\n";
        String tp =(filtro.getNombreProducto()!=null) ? filtro.getTipoProducto(): "Todos";
        String tipo = "Tipo: " + tp + "\n";
        String est = (filtro.getEstadoPedido()!=null) ? filtro.getEstadoPedido(): "Cualquiera";
        String estado = "Estado: " + est + "\n";
        String [] subtitulos = {producto,tipo,estado};
        return subtitulos;
    }
}
