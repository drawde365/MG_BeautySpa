package pe.edu.pucp.softinv.model.Personas;

import pe.edu.pucp.softinv.model.Pedido.PedidoDTO;

import java.util.ArrayList;

public class ClienteDTO extends UsuarioDTO {
    private ArrayList<PedidoDTO> pedidos;

    public ClienteDTO(){
        super();
        pedidos=null;
    }

    public ClienteDTO(String nombre,String PrimerApellido, String SegundoApellido, String correoElectronico,
                      String contrasenha, String celular,Integer idUsuario,ArrayList<PedidoDTO> pedidos,
                      String urlFotoPerfil){
        super(nombre, PrimerApellido, SegundoApellido, correoElectronico, contrasenha, celular,urlFotoPerfil,idUsuario);
        this.pedidos=pedidos;
    }

    public ArrayList<PedidoDTO> getPedidos() {
        return pedidos;
    }

    public void setPedidos(ArrayList<PedidoDTO> pedidos) {
        this.pedidos = pedidos;
    }


}
