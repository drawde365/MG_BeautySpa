<%@ Page Title="Generar Reportes" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Reportes.aspx.cs" Inherits="MGBeautySpaWebAplication.Admin.Reportes" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        /* Estilos generales de la fuente */
        .font-jakarta { font-family: 'Plus Jakarta Sans', sans-serif; }
        .report-title { font-family: 'ZCOOL XiaoWei', cursive; font-size: 48px; line-height: 40px; color: #1A0F12; height: 40px; }
        .tab-text { font-size: 20px; line-height: 21px; font-weight: 700; }
        .tab-active { color: #148C76; border-bottom: 3px solid #148C76; cursor: pointer; }
        .tab-inactive { color: #757575; border-bottom: 3px solid #E6E8EB; cursor: pointer; }
        
        /* Estilo del input (Texto, Dropdown) */
        .custom-input {
            box-sizing: border-box;
            background: #FFFFFF;
            border: 1px solid #E3D4D9;
            border-radius: 12px;
            padding: 15px;
            height: 56px;
            font-size: 16px;
            line-height: 24px;
            color: #1C0D12;
            width: 100%;
        }
    </style>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    
    <div style="display: flex; flex-direction: column; align-items: flex-start; padding: 0; width: 908px; max-width: 960px; min-height: 700px; margin: 0 auto;">

        <div style="display: flex; flex-direction: row; flex-wrap: wrap; justify-content: space-between; align-items: center; padding: 16px; gap: 12px; width: 902px; height: 76px;">
            <div style="width: 519px; height: 42.76px;">
                <h1 class="report-title">Generar Reporte</h1>
            </div>
        </div>

        <span id="contenedorMensajeError">
            <asp:Literal ID="litMensaje" runat="server"></asp:Literal>
        </span>

        <div style="display: flex; flex-direction: column; align-items: flex-start; padding: 0px 0px 12px; width: 908px; height: 67px; align-self: stretch;">
            <div style="box-sizing: border-box; display: flex; flex-direction: row; align-items: flex-start; padding: 0px 16px; gap: 32px; width: 908px; height: 55px; align-self: stretch;">
                
                <div id="tabProductos" onclick="switchTab('productos')" runat="server" ClientIDMode="Static"
                     class="tab-active" style="box-sizing: border-box; display: flex; justify-content: center; align-items: center; padding: 16px 0px 13px; width: 135px; height: 54px; flex-grow: 0;">
                    <span class="font-jakarta tab-text">Productos</span>
                </div>

                <div id="tabServicios" onclick="switchTab('servicios')" runat="server" ClientIDMode="Static"
                     class="tab-inactive" style="box-sizing: border-box; display: flex; justify-content: center; align-items: center; padding: 16px 0px 13px; width: 118px; height: 54px; flex-grow: 0;">
                    <span class="font-jakarta tab-text">Servicios</span>
                </div>
            </div>
        </div>

        <div style="display: flex; flex-direction: column; align-items: flex-start; padding: 20px 16px 12px; width: 908px; height: 60px; align-self: stretch;">
            <h2 class="font-jakarta" style="font-weight: 700; font-size: 22px; line-height: 28px; color: #1A0F12; margin: 0;">
                Seleccione los filtros que quiere aplicar al reporte
            </h2>
        </div>

        <div id="Group36" style="width: 908px; display: flex; flex-wrap: wrap; padding: 0 16px 16px 16px; gap: 16px;">
            
            <asp:Panel ID="pnlFiltrosProductos" runat="server" ClientIDMode="Static" Style="display: flex; flex-wrap: wrap; gap: 16px;">
                
                <div style="width: 291px; height: 88px; flex-grow: 0;">
                    <div style="margin-bottom: 8px;">
                        <label for="<%= txtNombreProducto.ClientID %>" class="font-jakarta" style="font-weight: 500; font-size: 16px; line-height: 24px; color: #1C0D12;">Nombre del producto</label>
                    </div>
                    <asp:TextBox ID="txtNombreProducto" runat="server" CssClass="custom-input" placeholder="Ingrese el nombre del producto" Width="291px" />
                </div>

                <div style="width: 291px; height: 88px; flex-grow: 0;">
                    <div style="margin-bottom: 8px;">
                        <label for="<%= ddlTipoProducto.ClientID %>" class="font-jakarta" style="font-weight: 500; font-size: 16px; line-height: 24px; color: #1C0D12;">Tipo de producto</label>
                    </div>
                    <asp:DropDownList ID="ddlTipoProducto" runat="server" CssClass="custom-input" Width="291px">
                        <asp:ListItem Text="-- Seleccione --" Value="" Selected="True" />
                        <asp:ListItem Text="Corporal" Value="Corporal" />
                        <asp:ListItem Text="Grasa" Value="Grasa" />
                        <asp:ListItem Text="Seca" Value="Seca" />
                        <asp:ListItem Text="Mixta" Value="Mixta" />
                        <asp:ListItem Text="Sensible" Value="Sensible" />
                    </asp:DropDownList>
                </div>
                
                <div style="width: 291px; height: 88px; flex-grow: 0;">
                    <div style="margin-bottom: 8px;">
                        <label for="<%= ddlEstadoPedido.ClientID %>" class="font-jakarta" style="font-weight: 500; font-size: 16px; line-height: 24px; color: #1C0D12;">Estado del pedido</label>
                    </div>
                    <asp:DropDownList ID="ddlEstadoPedido" runat="server" CssClass="custom-input" Width="291px">
                        <asp:ListItem Text="-- Seleccione --" Value="" Selected="True" />
                        <asp:ListItem Text="Confirmado" Value="CONFIRMADO" />
                        <asp:ListItem Text="Listo para recoger" Value="LISTO_PARA_RECOGER" />
                        <asp:ListItem Text="Recogido" Value="RECOGIDO" />
                        <asp:ListItem Text="No recogido" Value="NO_RECOGIDO" />
                    </asp:DropDownList>
                </div>

            </asp:Panel>

            <asp:Panel ID="pnlFiltrosServicios" runat="server" ClientIDMode="Static" Style="display: none; flex-wrap: wrap; gap: 16px;">
                
                <div style="width: 291px; height: 88px; flex-grow: 0;">
                    <div style="margin-bottom: 8px;">
                        <label for="<%= txtNombreServicio.ClientID %>" class="font-jakarta" style="font-weight: 500; font-size: 16px; line-height: 24px; color: #1C0D12;">Nombre del servicio</label>
                    </div>
                    <asp:TextBox ID="txtNombreServicio" runat="server" CssClass="custom-input" placeholder="Ingrese el nombre del servicio" Width="291px" />
                </div>

                <div style="width: 291px; height: 88px; flex-grow: 0;">
                    <div style="margin-bottom: 8px;">
                        <label for="<%= ddlTipoServicio.ClientID %>" class="font-jakarta" style="font-weight: 500; font-size: 16px; line-height: 24px; color: #1C0D12;">Tipo de servicio</label>
                    </div>
                    <asp:DropDownList ID="ddlTipoServicio" runat="server" CssClass="custom-input" Width="291px">
                        <asp:ListItem Text="-- Seleccione --" Value="" Selected="True" />
                        <asp:ListItem Text="Facial" Value="Facial" />
                        <asp:ListItem Text="Corporal" Value="Corporal" />
                        <asp:ListItem Text="Terapia Complementaria" Value="Terapia Complementaria" />
                    </asp:DropDownList>
                </div>
                
                <div style="width: 291px; height: 88px; flex-grow: 0;">
                    <div style="margin-bottom: 8px;">
                        <label for="<%= txtNombreEmpleado.ClientID %>" class="font-jakarta" style="font-weight: 500; font-size: 16px; line-height: 24px; color: #1C0D12;">Nombre del empleado a cargo</label>
                    </div>
                    <asp:TextBox ID="txtNombreEmpleado" runat="server" CssClass="custom-input" placeholder="Ingrese el nombre del empleado" Width="291px" />
                </div>

            </asp:Panel>

            <div id="PeriodoTiempoWrapper" style="width: 908px; align-self: stretch; display: flex; flex-wrap: wrap; gap: 16px;">
                
                <div style="width: 291px; height: auto;">
                    <div style="margin-bottom: 8px;">
                        <label for="<%= ddlPeriodoTiempo.ClientID %>" class="font-jakarta" style="font-weight: 500; font-size: 16px; line-height: 24px; color: #1C0D12;">Periodo de tiempo</label>
                    </div>
                    <asp:DropDownList ID="ddlPeriodoTiempo" runat="server" CssClass="custom-input" ClientIDMode="Static" Width="291px" onchange="toggleFechaEspecifica(this);">
                        <asp:ListItem Text="--Seleccione--" Value="" Selected="True" />
                        <asp:ListItem Text="Último mes" Value="mes" />
                        <asp:ListItem Text="Último año" Value="anual" />
                        <asp:ListItem Text="Fecha específica" Value="especifico" />
                    </asp:DropDownList>
                </div>

               <asp:Panel ID="pnlFiltroFechaEspecifica" runat="server" ClientIDMode="Static" Style="display: none; flex-grow: 1; min-width: 300px;">
                    <div style="display: flex; gap: 16px;">
                        
                        <div style="flex-grow: 1;">
                            <label for="<%= txtFechaInicio.ClientID %>" class="font-jakarta" style="font-weight: 400; font-size: 14px; color: #757575;">Fecha Inicio</label>
                            <asp:TextBox ID="txtFechaInicio" runat="server" TextMode="Date" CssClass="custom-input" Width="100%" />
                        </div>
                        <div style="flex-grow: 1;">
                            <label for="<%= txtFechaFin.ClientID %>" class="font-jakarta" style="font-weight: 400; font-size: 14px; color: #757575;">Fecha Fin</label>
                            <asp:TextBox ID="txtFechaFin" runat="server" TextMode="Date" CssClass="custom-input" Width="100%" />
                        </div>
                    </div>
                </asp:Panel>

            </div>

        </div>

        <div style="display: flex; flex-direction: row; justify-content: flex-end; align-items: flex-start; padding: 38px 16px 12px; width: 908px; align-self: stretch;">
            <div style="display: flex; flex-direction: row; justify-content: center; align-items: center; padding: 0px 16px; width: 230px; height: 43px; background: #1EC3B6; border-radius: 20px;">
                <asp:Button ID="btnGenerarReporte" runat="server" Text="Generar Reporte (PDF)"
                            CssClass="font-jakarta" style="border: none; background: none; color: #FCF7FA; font-weight: 700; font-size: 16px; line-height: 21px; cursor: pointer; height: 100%;" 
                            OnClick="btnGenerarReporte_Click" />
                <asp:HiddenField ID="hdnTipoReporte" runat="server" Value="productos" ClientIDMode="Static" />
            </div>
        </div>
        
    </div>
        

    <script type="text/javascript">
        // Esperar a que el documento cargue completamente
        document.addEventListener("DOMContentLoaded", function () {
            var hdnTipoReporte = document.getElementById('hdnTipoReporte');
            switchTab(hdnTipoReporte.value); 
            configurarRestriccionesFechas();

            document.addEventListener('click', function (event) {
                var contenedor = document.getElementById('contenedorMensajeError');
                // Si el contenedor existe y tiene contenido...
                if (contenedor && contenedor.innerHTML.trim() !== "") {
                    // Ocultamos el contenido visualmente o lo vaciamos
                    // Usamos un pequeño timeout para que no se borre INSTANTANEAMENTE 
                    setTimeout(function () {
                        contenedor.innerHTML = "";
                    }, 100);
                }
            });
        });

        

        function configurarRestriccionesFechas() {
            // 1. Obtener referencias a los controles ASP.NET usando ClientID
            var txtInicio = document.getElementById('<%= txtFechaInicio.ClientID %>');
        var txtFin = document.getElementById('<%= txtFechaFin.ClientID %>');

        // 2. Obtener la fecha de HOY en formato YYYY-MM-DD
        var hoy = new Date().toISOString().split('T')[0];

        // 3. REGLA A: Ninguna fecha puede ser futuro
        // Establecemos el atributo 'max' en ambos inputs
        if (txtInicio) txtInicio.setAttribute('max', hoy);
        if (txtFin) txtFin.setAttribute('max', hoy);

        // 4. REGLA B: Fecha Fin depende de Fecha Inicio
        // Agregamos un "Escucha" (Listener) al cambio de la fecha inicio
        if (txtInicio && txtFin) {
            txtInicio.addEventListener('change', function () {
                var fechaSeleccionada = this.value;

                if (fechaSeleccionada) {
                    // El mínimo de la fecha fin es la fecha inicio seleccionada
                    txtFin.setAttribute('min', fechaSeleccionada);

                    // UX: Si la fecha fin actual es menor a la nueva fecha inicio, la borramos
                    // para evitar inconsistencias visuales
                    if (txtFin.value && txtFin.value < fechaSeleccionada) {
                        txtFin.value = ""; 
                    }
                } else {
                    // Si borran la fecha inicio, quitamos la restricción mínima
                    txtFin.removeAttribute('min');
                }
            });
        }
    }

    // Mantenemos tu función original para mostrar/ocultar el panel
    function toggleFechaEspecifica(ddl) {
        var panel = document.getElementById('pnlFiltroFechaEspecifica');
        if (ddl.value === 'especifico') {
            panel.style.display = 'flex';
        } else {
            panel.style.display = 'none';
        }
    }
    
        // Función para cambiar entre pestañas de Productos y Servicios
        function switchTab(reporte) {
            var tabProductos = document.getElementById('tabProductos');
            var tabServicios = document.getElementById('tabServicios');
            var litMensaje = document.getElementById('<%= litMensaje.ClientID %>');

            var pnlProductos = document.getElementById('pnlFiltrosProductos');
            var pnlServicios = document.getElementById('pnlFiltrosServicios');
            var hdnTipoReporte = document.getElementById('hdnTipoReporte');

            if (litMensaje) {
                litMensaje.innerHTML = '';
            }

            if (reporte === 'productos') {
                // Activar Productos
                tabProductos.className = 'tab-active';
                tabServicios.className = 'tab-inactive';
                pnlProductos.style.display = 'flex';
                pnlServicios.style.display = 'none';
                hdnTipoReporte.value = 'productos';
            } else if (reporte === 'servicios') {
                // Activar Servicios
                tabServicios.className = 'tab-active';
                tabProductos.className = 'tab-inactive';
                pnlServicios.style.display = 'flex';
                pnlProductos.style.display = 'none';
                hdnTipoReporte.value = 'servicios';
            }
        }

    // Mantenemos la validación al hacer click (Backup de seguridad)
    function validarFechas() {
        var ddlPeriodo = document.getElementById('ddlPeriodoTiempo');
        if (ddlPeriodo.value !== 'especifico') return true;

        var txtInicio = document.getElementById('<%= txtFechaInicio.ClientID %>');
        var txtFin = document.getElementById('<%= txtFechaFin.ClientID %>');

            if (!txtInicio.value || !txtFin.value) {
                alert("Por favor, seleccione ambas fechas.");
                return false;
            }
            return true;
        }
    </script>
</asp:Content>