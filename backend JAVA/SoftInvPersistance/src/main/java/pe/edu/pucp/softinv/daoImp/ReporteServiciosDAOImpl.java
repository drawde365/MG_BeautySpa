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
import pe.edu.pucp.softinv.dao.ReporteServiciosDAO;
import pe.edu.pucp.sotfinv.model.Reportes.DatoReporteServicios;
import pe.edu.pucp.sotfinv.model.Reportes.FiltroReporte;

/**
 *
 * @author Usuario
 */
public class ReporteServiciosDAOImpl extends DAOImplBase implements ReporteServiciosDAO, Reportable<DatoReporteServicios>{
    
    private DatoReporteServicios dato;

    public ReporteServiciosDAOImpl() {
        super("");
        dato = new DatoReporteServicios();
    }

    @Override
    protected void configurarListaDeColumnas() {
    }

    @Override
    public ArrayList<DatoReporteServicios> obtenerReporte(FiltroReporte filtro) {
        return (ArrayList<DatoReporteServicios>) super.obtenerReporte(filtro);
    }
    
    @Override
    protected String obtenerSQLBase(){
        return "SELECT \n" +
    "CONCAT(U.NOMBRE, ' ', U.PRIMER_APELLIDO, ' ', COALESCE(U.SEGUNDO_APELLIDO, '')) AS NOMBRE_CLIENTE, "+
    "S.NOMBRE AS NOMBRE_SERVICIO, S.TIPO, C.FECHA, S.PRECIO, " +
    "CONCAT(E.NOMBRE, ' ', E.PRIMER_APELLIDO, ' ', COALESCE(E.SEGUNDO_APELLIDO, '')) AS NOMBRE_EMPLEADO " +
    "FROM CITAS C\n" +
    "INNER JOIN SERVICIOS S ON C.SERVICIO_ID = S.SERVICIO_ID " +
    "INNER JOIN USUARIOS E ON C.EMPLEADO_ID = E.USUARIO_ID " +
    "INNER JOIN USUARIOS U ON C.CLIENTE_ID = U.USUARIO_ID " +
    "WHERE 1=1 ";
    }
    
    @Override
    protected void aplicarFiltros(StringBuilder sql, List<Object> params, FiltroReporte filtro){
        
        if (filtro.getFechaInicio()!=null){
            sql.append(" AND C.FECHA >= ?");
            params.add(new java.sql.Date(filtro.getFechaInicio().getTime()));
        }
        
        if (filtro.getFechaFin()!=null){
            sql.append(" AND C.FECHA <= ?");
            params.add(new java.sql.Date(filtro.getFechaFin().getTime()));
        }
        
        if (filtro.getNombreServicio()!=null){
            sql.append(" AND S.NOMBRE LIKE ?");
            params.add("%" + filtro.getNombreServicio() + "%");
        }
        
        if (filtro.getTipoServicio()!=null){
            sql.append(" AND S.TIPO = ?");
            params.add(filtro.getTipoServicio());
        }
        
        if (filtro.getNombreEmpleado() != null) {
            
            //Se divide el nombre completo en palabras
            String[] palabras = filtro.getNombreEmpleado().trim().split("\\s+");
            
            //Para cada palabra, se exige que aparezca en alguna parte del nombre completo
            for (String palabra : palabras) {
                
                sql.append(" AND (");
                //Se verifica si esta palabra está en el Nombre O en el Apellido 1 O en el Apellido 2
                sql.append(" E.NOMBRE LIKE ? ");
                sql.append(" OR E.PRIMER_APELLIDO LIKE ? ");
                sql.append(" OR E.SEGUNDO_APELLIDO LIKE ? ");
                sql.append(") ");
            
                String terminoBusqueda = "%" + palabra + "%";
                params.add(terminoBusqueda);
                params.add(terminoBusqueda);
                params.add(terminoBusqueda);
            }
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
        this.dato = new DatoReporteServicios();
        this.dato.setNombreCliente(this.resultSet.getString("NOMBRE_CLIENTE"));
        this.dato.setNombreServicio(this.resultSet.getString("NOMBRE_SERVICIO"));
        this.dato.setTipoServicio(this.resultSet.getString("TIPO"));
        this.dato.setFecha(this.resultSet.getTimestamp("FECHA"));
        this.dato.setPrecio(this.resultSet.getDouble("PRECIO"));
        this.dato.setNombreEmpleado(this.resultSet.getString("NOMBRE_EMPLEADO"));
    }
    
    @Override
    protected void agregarObjetoALaLista(List lista) throws SQLException {
        this.instanciarObjetoDelResultSet();
        lista.add(this.dato);
    }
    
    @Override
    public String[] getTitulosColumnas() {
        return new String[] {"Nombre del Cliente", "Servicio", "Tipo", "Empleado a cargo","Fecha", "Precio",};
    }

    @Override
    public String[] getDatosFila() {
        SimpleDateFormat sdf = new SimpleDateFormat("dd/MM/yyyy");
        return new String[] {
            this.dato.getNombreCliente(),
            this.dato.getNombreServicio(),
            this.dato.getTipoServicio(),
            this.dato.getNombreEmpleado(),
            sdf.format(this.dato.getFecha()),
            String.format("S/.%.2f", this.dato.getPrecio())
        };
    }
    
    @Override
    public float[] getAnchosColumnas() {
        return new float[] {3f, 2.5f, 1.5f, 3f, 1.5f, 1.5f};
    }

    @Override
    public double getMontoTotal() {
        return this.dato.getPrecio();
    }
    
    @Override
    public void assign(DatoReporteServicios dato){
        this.dato = new DatoReporteServicios(dato);
    }
    
    @Override
    public String getTitulo(){
        return "Reporte de Citas\n";
    }
    
    @Override
    public String[] getSubtitulos(FiltroReporte filtro){
        String serv = (filtro.getNombreServicio()!=null) ? filtro.getNombreServicio() : "Todos";
        String servicio = "Servicio: "  + serv + "\n";
        String tp = (filtro.getTipoServicio()!=null) ? filtro.getNombreServicio() : "Todos";
        String tipo = "Tipo: " + tp + "\n";
        String emp = (filtro.getNombreEmpleado()!=null) ? filtro.getNombreEmpleado() : "Todos";
        String empleado = "Empleado: " + emp + "\n";
        String[] subtitulos = {servicio,tipo,empleado};
        return subtitulos;
    }
}
