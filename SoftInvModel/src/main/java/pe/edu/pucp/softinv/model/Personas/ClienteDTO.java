package pe.edu.pucp.softinv.model.Personas;

import pe.edu.pucp.softinv.model.Pedido.PedidoDTO;
import pe.edu.pucp.softinv.model.Producto.ProductoDTO;

import java.util.ArrayList;

public class ClienteDTO extends UsuarioDTO {
    private ArrayList<PedidoDTO> pedidos;

    public ClienteDTO(){
        super();
        pedidos=null;
    }

    @Override
    public void setRol() {
        super.rol = "Cliente";
    }

    public ClienteDTO(String nombre,String PrimerApellido, String SegundoApellido, String correoElectronico,
                      String contrasenha, String celular,Integer idUsuario,ArrayList<PedidoDTO> pedidos,
                      String urlFotoPerfil){
        super(nombre, PrimerApellido, SegundoApellido, correoElectronico, contrasenha, celular,urlFotoPerfil,idUsuario);
        this.pedidos=pedidos;
        setRol();
    }

    public ArrayList<PedidoDTO> getPedidos() {
        return pedidos;
    }

    public void setPedidos(ArrayList<PedidoDTO> pedidos) {
        this.pedidos = pedidos;
    }

}
