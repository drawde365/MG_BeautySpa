using System;
using System.ComponentModel;

namespace softinv.model
{
    public class PedidoDTO
    {
        private int? idPedido;
        private DateTime? fechaPago;
        private DateTime? fechaRecojo;
        private DateTime? fechaListaParaRecojo;
        private double? total;
        private double? IGV;
        private EstadoPedido? estadoPedido;
        private BindingList<DetallePedidoDTO> detallesPedido;
        private ClienteDTO cliente;

        public int? IdPedido { get => idPedido; set => idPedido = value; }
        public DateTime? FechaPago { get => fechaPago; set => fechaPago = value; }
        public DateTime? FechaRecojo { get => fechaRecojo; set => fechaRecojo = value; }
        public DateTime? FechaListaParaRecojo { get => fechaListaParaRecojo; set => fechaListaParaRecojo = value; }
        public double? Total { get => total; set => total = value; }
        public double? IGV1 { get => IGV; set => IGV = value; }
        public EstadoPedido? EstadoPedido { get => estadoPedido; set => estadoPedido = value; }
        internal BindingList<DetallePedidoDTO> DetallesPedido { get => detallesPedido; set => detallesPedido = value; }
        public ClienteDTO Cliente { get => cliente; set => cliente = value; }

        public void agregarDetallesPedido(DetallePedidoDTO detallePedido)
        {
            this.DetallesPedido.Add(detallePedido);
        }

        public PedidoDTO()
        {
            IdPedido = null;
            Cliente = null;
            FechaPago = null;
            FechaRecojo = null;
            FechaListaParaRecojo = null;
            Total = null;
            IGV1 = null;
            EstadoPedido = null;
            DetallesPedido = null;
        }

        public PedidoDTO(int idPedido, DateTime fechaPago, DateTime fechaRecojo, DateTime fechaListo, Double total, Double IGV, EstadoPedido estadoPedido,
                         BindingList<DetallePedidoDTO> detallesPedido, ClienteDTO cliente)
        {
            this.IdPedido = idPedido;
            this.DetallesPedido = detallesPedido;
            this.Total = total;
            this.IGV1 = IGV;
            this.EstadoPedido = estadoPedido;
            this.FechaPago = fechaPago;
            this.FechaRecojo = fechaRecojo;
            this.FechaListaParaRecojo = fechaListo;
            this.Cliente = cliente;
        }

    }
}
