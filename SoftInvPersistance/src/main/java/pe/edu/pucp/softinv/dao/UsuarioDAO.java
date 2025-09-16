package pe.edu.pucp.softinv.dao;
import pe.edu.pucp.softinv.model.Personas.UsuarioDTO;

//TIP To <b>Run</b> code, press <shortcut actionId="Run"/> or
// click the <icon src="AllIcons.Actions.Execute"/> icon in the gutter.
public interface UsuarioDAO {
    public Integer insertar(UsuarioDTO usuario);
    public UsuarioDTO optenerPorId(Integer id);
    public Integer modificar(UsuarioDTO usuario);
    public Integer eliminar(Integer id);
}