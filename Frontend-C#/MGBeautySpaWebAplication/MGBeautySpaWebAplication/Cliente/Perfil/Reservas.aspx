<%@ Page Title="Historial de Reservas" Async="true"
Language="C#"
MasterPageFile="~/Cliente/Perfil/Perfil.Master"
AutoEventWireup="true"
CodeBehind="Reservas.aspx.cs"
Inherits="MGBeautySpaWebAplication.Cliente.Perfil.Reservas" %>

<asp:Content ID="ctBody" ContentPlaceHolderID="ProfileBodyContent" runat="server">
    
    <link rel="stylesheet" href="Reservas.css" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css">

    <style>
        .citas-vacias-mensaje {
            text-align: center; padding: 40px 20px; color: #757575;
            font-family: 'Plus Jakarta Sans', sans-serif; font-size: 1.1em;
            background-color: #f9f9f9; border-radius: 8px; margin-top: 1.5rem;
        }

        /* 👉 Alinear estructura de la tarjeta y el botón a la derecha */
        .reserva-item {
            display: flex;
            justify-content: space-between;
            gap: 24px;
        }

        .reserva-info {
            flex: 1;
        }

        .reserva-estado-y-total {
            display: flex;
            flex-direction: column;
            align-items: flex-end;
            min-width: 180px;
        }

        .reserva-total {
            text-align: right;
            margin-bottom: 8px;
        }

        .reserva-accion {
            width: 100%;
            text-align: right;
        }

        .btn-cancelar {
            display: inline-flex;
            align-items: center;
            gap: 4px;
            color: #dc3545;
            border: none;
            background: transparent;
            font-weight: 500;
        }

        .btn-cancelar:hover {
            text-decoration: underline;
            color: #b02a37;
        }

        .modal-body .form-label { font-weight: 600; margin-top: 10px; }
        .modal-body .form-control { border-radius: 6px; }
        
        .btn-action-group { display: flex; gap: 8px; }
        .btn-accept { color: #198754; border-color: #198754; }
        .btn-accept:hover { background-color: #198754; color: white; }
        .btn-cancel { color: #dc3545; border-color: #dc3545; }
        .btn-cancel:hover { background-color: #dc3545; color: white; }

        .badge {
            display: inline-block;
            padding: 0.35em 0.65em;
            font-size: 0.75em;
            font-weight: 700;
            line-height: 1;
            color: #fff;
            text-align: center;
            white-space: nowrap;
            vertical-align: baseline;
            border-radius: 0.25rem;
        }
        .rounded-pill { border-radius: 50rem !important; }
        .text-white { color: #fff !important; }
        .text-dark { color: #212529 !important; }
    </style>

    <div class="reservas-container">
        <h2 class="reservas-titulo">Historial de Reservas</h2>

        <asp:Repeater ID="rptReservas" runat="server"
            OnItemDataBound="rptReservas_ItemDataBound" 
            >

            <ItemTemplate>
                <div class="reserva-item" 
                     data-fecha='<%# Eval("FechaReal", "{0:yyyy-MM-dd}") %>' 
                     data-hora='<%# Eval("HoraRealStr") %>'>
                     
                    <div class="reserva-info">
                        <h4 class="reserva-numero">Reserva N.º <%# Eval("NumeroReserva") %></h4>
                        
                        <p><strong>Servicio:</strong> <%# Eval("Servicio") %></p>
                        <p><strong>Fecha:</strong> <%# Eval("Fecha") %></p>
                        <p><strong>Hora:</strong> <%# Eval("HoraInicio") %></p>
                        <p><strong>Estado:</strong> <%# Eval("Estado") %></p>
                        
                        <p><strong>Empleado:</strong> <%# Eval("Empleado") %></p>

                        <div class="empleado-contacto">
                            <span class="contacto-item">
                                <i class="bi bi-envelope-fill"></i> <%# Eval("EmpleadoCorreo") %>
                            </span>
                            <span class="contacto-item">
                                <i class="bi bi-phone-fill"></i> <%# Eval("EmpleadoCelular") %>
                            </span>
                        </div>
                    </div>

                    <div class="reserva-estado-y-total">
                        <div class="reserva-total">
                            <span class="total-label">Total:</span>
                            <strong><%# Eval("Total") %></strong>
                        </div>

                        <div class="reserva-accion">
                            <asp:LinkButton 
                                ID="btnCancelarCita" 
                                runat="server" 
                                CssClass="btn-cancelar"
                                CommandName="Cancelar"
                                ToolTip="Cancelar Cita"
                                Visible="false">
                                <i class="bi bi-trash-fill"></i> Cancelar
                            </asp:LinkButton>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>

        <asp:Panel ID="pnlNoReservas" runat="server" CssClass="no-pedidos-mensaje" Visible="false">
            Aún no tienes reservas en tu historial.
        </asp:Panel>

        <div class="reservas-footer">
            <asp:Button ID="btnVerMas" runat="server"
                        Text="Ver Más"
                        CssClass="btn-vermas"
                        OnClick="btnVerMas_Click" />
            
            <asp:Button ID="btnRegresar" runat="server"
                        Text="Regresar"
                        CssClass="btn-regresar"
                        OnClick="btnRegresar_Click" />
        </div>
    </div>

    <!-- 👉 HiddenField para guardar el id de la cita a cancelar -->
    <asp:HiddenField ID="hfCitaIdCancelar" runat="server" />

    <!-- 👉 Modal Bootstrap de confirmación -->
    <div class="modal fade" id="mdlCancelarReserva" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Cancelar reserva</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
                </div>
                <div class="modal-body">
                    <p>
                        ¿Estás seguro de que deseas cancelar esta reserva?<br />
                        Esta acción no se puede deshacer.
                    </p>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-outline-secondary" data-bs-dismiss="modal">
                        Volver
                    </button>
                    <asp:Button ID="btnConfirmarCancelacion" runat="server"
                                CssClass="btn btn-danger"
                                Text="Sí, cancelar"
                                OnClick="btnConfirmarCancelacion_Click" />
                </div>
            </div>
        </div>
    </div>

    <script>
        document.addEventListener("DOMContentLoaded", function () {
            const ahora = new Date();

            document.querySelectorAll(".reserva-item").forEach(item => {
                const fecha = item.getAttribute("data-fecha");
                const hora = item.getAttribute("data-hora");

                if (fecha && hora) {
                    const fechaReserva = new Date(fecha + "T" + hora);

                    // Validación visual extra en el cliente
                    if (fechaReserva > ahora) {
                        const btn = item.querySelector(".btn-cancelar");
                        if (btn) {
                            // Aseguramos que se muestre si el servidor lo renderizó
                            btn.style.display = "inline-flex";
                        }
                    }
                }
            });

            // 👉 manejar apertura del modal
            document.querySelectorAll(".btn-cancelar").forEach(btn => {
                btn.addEventListener("click", function (e) {
                    e.preventDefault();
                    const citaId = this.getAttribute("data-citaid");
                    const hf = document.getElementById("<%= hfCitaIdCancelar.ClientID %>");
                    if (hf && citaId) {
                        hf.value = citaId;
                    }

                    const modalEl = document.getElementById("mdlCancelarReserva");
                    if (modalEl) {
                        const modal = new bootstrap.Modal(modalEl);
                        modal.show();
                    }
                });
            });
        });
    </script>

</asp:Content>
