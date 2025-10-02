using System;
using System.ComponentModel;

namespace softinv.model
{
    public class ClienteDTO: UsuarioDTO
    {
    private BindingList<PedidoDTO> pedidos;

    public ClienteDTO():base() {
        pedidos = null;
    }

    public override void setRol()
    {
        base.rol = "Cliente";
    }

    public ClienteDTO(String nombre, String PrimerApellido, String SegundoApellido, String correoElectronico,
                      String contrasenha, String celular, int idUsuario, BindingList<PedidoDTO> pedidos,
                      String urlFotoPerfil): base(nombre, PrimerApellido, SegundoApellido, correoElectronico, contrasenha, celular, urlFotoPerfil, idUsuario)
        {
        this.pedidos = pedidos;
        setRol();
    }

    public BindingList<PedidoDTO> getPedidos()
    {
        return pedidos;
    }

}

}