package pe.edu.pucp.softinv.daoImp;

import pe.edu.pucp.softinv.dao.CalendarioDAO;
import pe.edu.pucp.softinv.daoImp.util.Columna;
import pe.edu.pucp.softinv.model.Disponibilidad.CalendarioDTO;
import pe.edu.pucp.softinv.model.Personas.EmpleadoDTO;

import java.sql.SQLException;
import java.sql.Timestamp;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;
import java.util.Calendar; // Import necesario para la lógica agnóstica
import java.util.HashMap;
import java.util.Map;
import pe.edu.pucp.softinv.model.Disponibilidad.HorarioTrabajoDTO;

public class CalendarioDAOImpl extends DAOImplBase implements CalendarioDAO {
    CalendarioDTO calendario;

    public CalendarioDAOImpl() {
        super("CALENDARIOS_EMPLEADOS");
        this.retornarLlavePrimaria = false;
        this.calendario = null;
    }

    @Override
    protected void configurarListaDeColumnas() {
        this.listaColumnas.add(new Columna("EMPLEADO_ID", true, false));
        this.listaColumnas.add(new Columna("FECHA", true, false));
        this.listaColumnas.add(new Columna("CANT_LIBRE", false, false));
        this.listaColumnas.add(new Columna("MOTIVO", false, false));
    }

    @Override
    protected void incluirValorDeParametrosParaInsercion() throws SQLException {
        this.statement.setInt(1, calendario.getEmpleado().getIdUsuario());
        this.statement.setTimestamp(2, new Timestamp(calendario.getFecha().getTime()));
        this.statement.setInt(3, calendario.getCantLibre());
        this.setStringEnST(4,calendario.getMotivo());
    }

    @Override
    protected void incluirValorDeParametrosParaModificacion() throws SQLException {
        this.statement.setInt(1, calendario.getCantLibre());
        this.setStringEnST(2,calendario.getMotivo());
        this.statement.setInt(3, calendario.getEmpleado().getIdUsuario());
        this.statement.setTimestamp(4, new Timestamp(calendario.getFecha().getTime()));
    }

    @Override
    protected void incluirValorDeParametrosParaEliminacion() throws SQLException {
        this.statement.setInt(1, calendario.getEmpleado().getIdUsuario());
        this.statement.setTimestamp(2, new Timestamp(calendario.getFecha().getTime()));
    }

    @Override
    protected void incluirValorDeParametrosParaObtenerPorId() throws SQLException {
        this.statement.setInt(1, calendario.getEmpleado().getIdUsuario());
        this.statement.setTimestamp(2, new Timestamp(calendario.getFecha().getTime()));
    }

    @Override
    protected void instanciarObjetoDelResultSet() throws SQLException {
        this.calendario = new CalendarioDTO();
        EmpleadoDTO empleado = new EmpleadoDTO();
        empleado.setIdUsuario(this.resultSet.getInt("EMPLEADO_ID"));

        this.calendario.setEmpleado(empleado);
        this.calendario.setCantLibre(this.resultSet.getInt("CANT_LIBRE"));
        this.calendario.setFecha(new Timestamp (this.resultSet.getTimestamp("FECHA").getTime()));
        this.calendario.setMotivo(this.resultSet.getString("MOTIVO"));
    }

    @Override
    protected void limpiarObjetoDelResultSet() {
        this.calendario = null;
    }

    @Override
    protected void agregarObjetoALaLista(List lista) throws SQLException {
        this.instanciarObjetoDelResultSet();
        lista.add(calendario);
    }

    @Override
    public Integer insertar(CalendarioDTO calendario) {
        this.calendario = calendario;
        return super.insertar();
    }

    @Override
    public CalendarioDTO obtenerPorId(Integer empleadoId, Date fecha) {
        this.calendario = new CalendarioDTO();
        EmpleadoDTO empleado = new EmpleadoDTO();
        empleado.setIdUsuario(empleadoId);
        this.calendario.setEmpleado(empleado);
        this.calendario.setFecha(new java.sql.Date(fecha.getTime()));
        super.obtenerPorId();
        return this.calendario;
    }

    @Override
    public Integer modificar(CalendarioDTO calendario){
        this.calendario = calendario;
        return super.modificar();
    }

    @Override
    public Integer eliminar(CalendarioDTO calendario){
        this.calendario = calendario;
        return super.eliminar();
    }
    @Override
    public ArrayList<CalendarioDTO> listarCalendarioEnRango(Integer empleadoId, Date fechaInicio, Date fechaFin) {
        String sql = "SELECT EMPLEADO_ID, FECHA, CANT_LIBRE, MOTIVO FROM CALENDARIOS_EMPLEADOS WHERE EMPLEADO_ID = ? AND FECHA BETWEEN ? AND ?";
        
        return (ArrayList<CalendarioDTO>)listarTodos(sql, this::incluirTresParametros, new Object[]{empleadoId, fechaInicio, fechaFin});
    }
    
    @Override
    public ArrayList<CalendarioDTO> listarCalendarioDeEmpleado(Integer empleadoId) {
        java.sql.Date hoy = new java.sql.Date(System.currentTimeMillis());
        java.sql.Date fechaFin = new java.sql.Date(System.currentTimeMillis() + (30L * 24L * 60L * 60L * 1000L * 30L)); // Aproximadamente 30 días
        
        return listarCalendarioEnRango(empleadoId, hoy, fechaFin);
    }
    private void setStringEnST(int indice, String valor) throws SQLException {
        if (valor != null) {
            this.statement.setString(indice, valor);
        } else {
            this.statement.setNull(indice, java.sql.Types.VARCHAR);
        }
    }
    
    /**
     * Inserta 30 días de disponibilidad para un empleado a partir de mañana.
     * La lógica de fechas se maneja en Java para ser agnóstica a la base de datos.
     */
     public Integer insertar30DiasFuturos(Integer empleadoId) {
        int registrosInsertados = 0;
        Calendar calendar = Calendar.getInstance();
        
        // 1. Obtener Horarios desde la BD usando el DAO existente
        HorarioTrabajoDAOImpl horarioDAO = new HorarioTrabajoDAOImpl();
        ArrayList<HorarioTrabajoDTO> listaHorarios = horarioDAO.listarPorEmpleado(empleadoId);
        
        // 2. Mapear Horarios (DíaSemana -> TotalIntervalos)
        Map<Integer, Integer> horarioSemanal = new HashMap<>();
        
        // Inicializar todos los días (1=Lunes...7=Domingo) con 0
        for (int i = 1; i <= 7; i++) {
            horarioSemanal.put(i, 0);
        }
        
        // Llenar con los datos reales del empleado (sumando intervalos si hay turnos partidos)
        for (HorarioTrabajoDTO h : listaHorarios) {
            int dia = h.getDiaSemana();
            int intervalos = h.getNumIntervalo();
            horarioSemanal.put(dia, horarioSemanal.getOrDefault(dia, 0) + intervalos);
        }

        // 3. Configurar fecha de inicio (mañana)
        calendar.add(Calendar.DAY_OF_YEAR, 1);
        calendar.set(Calendar.HOUR_OF_DAY, 0);
        calendar.set(Calendar.MINUTE, 0);
        calendar.set(Calendar.SECOND, 0);
        calendar.set(Calendar.MILLISECOND, 0);
        
        // 4. Bucle de inserción
        for (int i = 0; i < 30; i++) {
            this.calendario = new CalendarioDTO();

            EmpleadoDTO emp = new EmpleadoDTO();
            emp.setIdUsuario(empleadoId);
            this.calendario.setEmpleado(emp);

            this.calendario.setFecha(new Timestamp(calendar.getTimeInMillis()));

            // Calcular día de la semana (Lunes=1 ... Domingo=7)
            int diaSemanaJava = calendar.get(Calendar.DAY_OF_WEEK);
            int diaSemanaLogico = (diaSemanaJava == Calendar.SUNDAY) ? 7 : diaSemanaJava - 1;

            // Obtener cantidad de intervalos (será 0 si no trabaja)
            int cantidadIntervalos = horarioSemanal.get(diaSemanaLogico);

            this.calendario.setCantLibre(cantidadIntervalos); 
            this.calendario.setMotivo(null);

            // Gestión de conexión manual
            if(i == 0) {
                super.insertar(true, false);
            } else if(i == 29) {
                super.insertar(false, true);
            } else {
                super.insertar(true, true);
            }

            registrosInsertados++;

            // Avanzamos al siguiente día
            calendar.add(Calendar.DAY_OF_YEAR, 1);
        }
        
        return registrosInsertados;
    }
    
    @Override
    public Integer eliminar30DiasFuturos(Integer empleadoId) {
        int registrosEliminados = 0;
        Calendar calendar = Calendar.getInstance();

        // 1. Configurar fecha de inicio (mañana)
        calendar.add(Calendar.DAY_OF_YEAR, 1);
        calendar.set(Calendar.HOUR_OF_DAY, 0);
        calendar.set(Calendar.MINUTE, 0);
        calendar.set(Calendar.SECOND, 0);
        calendar.set(Calendar.MILLISECOND, 0);

        // 2. Bucle de eliminación
        for (int i = 0; i < 30; i++) {
            this.calendario = new CalendarioDTO();

            EmpleadoDTO emp = new EmpleadoDTO();
            emp.setIdUsuario(empleadoId);
            this.calendario.setEmpleado(emp);

            // Establecemos la fecha que coincide con la PK para eliminar
            this.calendario.setFecha(new Timestamp(calendar.getTimeInMillis()));

            // Gestión de conexión manual (Misma lógica que insertar)
            if(i == 0) {
                // Primera iteración: Deja conexión abierta, NO asume transacción iniciada
                super.eliminar(true, false);
            } else if(i == 29) {
                // Última iteración: Cierra conexión, ASUME transacción iniciada (para hacer commit)
                super.eliminar(false, true);
            } else {
                // Iteraciones intermedias
                super.eliminar(true, true);
            }

            registrosEliminados++;

            // Avanzamos al siguiente día
            calendar.add(Calendar.DAY_OF_YEAR, 1);
        }

        return registrosEliminados;
    }
    
    private void incluirTresParametros(Object objetoParametros) {
        Object[] params = (Object[]) objetoParametros;
        try {
            this.statement.setInt(1, (Integer) params[0]);
            this.statement.setTimestamp(2, new Timestamp(((Date) params[1]).getTime()));
            this.statement.setTimestamp(3, new Timestamp(((Date) params[2]).getTime()));
        } catch (SQLException e) {
            throw new RuntimeException(e);
        }
    }
    
    private void incluirId(Object objetoParametros){
        Integer id = (Integer) objetoParametros;
        try {
            this.statement.setInt(1,id);
        } catch (SQLException e) {
            throw new RuntimeException(e);
        }
    }
}