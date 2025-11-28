<%@ Page Title="Buscar Usuarios" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="BuscarUsuarios.aspx.cs" Inherits="MGBeautySpaWebAplication.Admin.BuscarUsuarios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Administrar Usuarios - MG Beauty Spa
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" />



    <style>

        /* Forzar el color verde del ítem activo del menú lateral */
        .sidebar .nav-link.active,
        .sidebar .nav-link.active:hover,
        .sidebar .nav-link.active:focus {
            background-color: #107369 !important;  /* tu verde */
            color: white !important;
        }

        /* Forzar que el texto del menú nunca sea azul */
        .sidebar .nav-link,
        .sidebar .nav-link span,
        .sidebar .nav-link i {
            color: white !important;
        }


        .h1-admin-title { font-family: 'ZCOOL XiaoWei', serif; font-size: 48px; line-height: 40px; color: #1A0F12; }
        .search-box {
            border-radius: 12px;
            padding-left: 40px;
        }

        .search-icon {
            position: absolute;
            top: 50%;
            left: 15px;
            transform: translateY(-50%);
            color: #888;
        }

        .table thead {
            background: white;
        }

        .badge-role {
            font-size: 0.75rem;
            padding: 6px 10px;
            border-radius: 12px;
        }

        .badge-personal { background: #dbeafe; color: #1e40af; }
        .badge-client { background: #e5e7eb; color: #374151; }
        .badge-admin { background: #f3e8ff; color: #6b21a8; }

        a.view-link {
            color: #10b981;
            text-decoration: none;
            font-weight: 500;
        }

        a.view-link:hover {
            text-decoration: underline;
        }

        .btn-delete {
            background: #fee2e2;
            color: #b91c1c;
            border-radius: 50%;
            padding: 8px 10px;
        }

        .btn-delete:hover {
            background: #fecaca;
            color: #7f1d1d;
        }

        .tab-active { color: #148C76; border-bottom: 3px solid #148C76; cursor: pointer; }
        .tab-inactive {
            color: #757575;
            border-bottom: 3px solid #E6E8EB;
            cursor: pointer;
        }
        .font-jakarta { font-family: 'Plus Jakarta Sans', sans-serif; }
        .tab-text { font-size: 18px; line-height: 21px; font-weight: 700; }
    </style>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container py-4">

        <h1 class="h1-admin-title">Gestion de usuarios</h1>

        <div style="display: flex; flex-direction: column; align-items: flex-start; padding: 0px 0px 12px; width: 908px; height: 67px; align-self: stretch;">
            <div style="box-sizing: border-box; display: flex; flex-direction: row; align-items: flex-start; padding: 0px 16px; gap: 32px; width: 908px; height: 55px; align-self: stretch;">
                <a href="BuscarUsuarios.aspx" style="text-decoration: none;">
                <div id="tabEmpleados" runat="server" ClientIDMode="Static"
                    class="tab-active" style="box-sizing: border-box; display: flex; justify-content: center; align-items: center; padding: 16px 0px 13px; width: 135px; height: 54px; flex-grow: 0;">
                    <span class="font-jakarta tab-text">Empleados</span>
                </div>
                </a>

                <div id="tabClientes" onclick="switchTab('clientes')" runat="server" ClientIDMode="Static"
                    class="tab-inactive" style="box-sizing: border-box; display: flex; justify-content: center; align-items: center; padding: 16px 0px 13px; width: 118px; height: 54px; flex-grow: 0;">
                    <span class="font-jakarta tab-text">Clientes</span>
                </div>
            </div>
        </div>

        <asp:HiddenField ID="hfTipoUsuario" runat="server" Value="empleados" ClientIDMode="Static" />

        <!-- FILA DE BUSQUEDA Y FILTRO -->
        <div class="d-flex align-items-center gap-3 mb-4">

            <!-- BUSCADOR -->
            <div class="position-relative flex-grow-1">
                <input type="text" id="searchNombre" runat="server" class="form-control"
                       placeholder="Nombre" ClientIDMode="Static"/>
            </div>
            <div class="position-relative flex-grow-1">
                <input type="text" id="searchPrimerApellido" runat="server" class="form-control"
                        placeholder="Primer apellido" ClientIDMode="Static"/>
            </div>
            <div class="position-relative flex-grow-1">
                <input type="text" id="searchSegundoApellido" runat="server" class="form-control"
                        placeholder="Segundo apellido" ClientIDMode="Static"/>
            </div>
            <div class="position-relative flex-grow-1">
                <input type="text" id="searchCorreo" runat="server" class="form-control"
                        placeholder="Correo electrónico" ClientIDMode="Static"/>
            </div>
            <div class="position-relative flex-grow-1">
                <input type="text" id="searchCelular" runat="server" class="form-control"
                        placeholder="Nro. de celular" ClientIDMode="Static" />
            </div>

            <select id="filterActive" runat="server" class="form-select col-servicios" style="max-width:180px;" ClientIDMode="Static">
                <option value="">Todos</option>
                <option value="1">Activos</option>
                <option value="0">Inactivos</option>
            </select>

        </div>

        <div id="divErrorBusqueda" class="ps-4 mb-2" style="display: none;">
            <span class="text-danger small">
                <i class="fa fa-exclamation-circle"></i> 
                Por favor, ingrese un criterio de búsqueda o seleccione un estado.
            </span>
        </div>

        <div id="divBotonBuscar" style="display: flex; flex-direction: row; justify-content: flex-start; align-items: flex-start; padding: 5px 16px 12px; width: 908px; align-self: stretch;">
        <div style="display: flex; flex-direction: row; justify-content: center; align-items: center; padding: 0px 16px; width: 230px; height: 43px; background: #1EC3B6; border-radius: 20px;">
            <asp:Button ID="btnBuscarUsuarios" runat="server" Text="Buscar"
                    CssClass="font-jakarta" style="border: none; background: none; color: #FCF7FA; font-weight: 700; font-size: 16px; line-height: 21px; cursor: pointer; height: 100%;"
                OnClientClick="return validarBusquedaCliente();" 
                OnClick="btnBuscar_Click"/>
        </div>
        </div>

        <!-- TABLA -->
        <div id="contenedorTabla" runat="server" class="table-responsive shadow-sm rounded" Visible="false">
            <table class="table table-hover align-middle mb-0">
                <thead class="border-bottom">
                    <tr>
                        <th>Nombre completo</th>
                        <th>Email</th>
                        <th>Teléfono</th>
                        <th>Rol</th>
                        <th class="col-servicios">Servicios</th>
                        <th class="text-center">Acciones</th>
                    </tr>
                </thead>
                <tbody id="userTable">
                    <asp:Repeater ID="rpUsuarios" runat="server" OnItemDataBound="rpUsuarios_ItemDataBound">
                        <ItemTemplate>
                            <tr data-role='<%# Eval("rol") %>' data-active='<%# Eval("activo") %>'>
                                <td>
                                    <%# Eval("nombre") %> <%# Eval("primerapellido") %> <%# Eval("segundoapellido") %>
                                </td>
                                <td><%# Eval("correoElectronico") %></td>
                                <td><%# Eval("celular") %></td>

                                <td>
                                    <span class='badge-role 
                                        <%# Eval("rol").ToString() == "2" ? "badge-personal" :
                                            Eval("rol").ToString() == "1" ? "badge-client" :
                                            "badge-admin" %>'>
                                        <%# Convert.ToInt32(Eval("rol")) == 1 ? "Cliente" : "Empleado" %>
                                    </span>
                                </td>

                                <td class="col-servicios">
                                    <asp:LinkButton ID="lnkServicios"
                                            runat="server"
                                            CssClass="view-link"
                                            Visible='<%# Convert.ToInt32(Eval("rol"))==2 %>'
                                            CommandArgument='<%# Eval("idUsuario") %>'
                                            OnClick="btnServicios_Click">
                                        Ver Asignados
                                    </asp:LinkButton>

                                    <asp:Label ID="lblNA" runat="server" Text="N/A" 
                                               Visible='<%# Convert.ToInt32(Eval("rol")) == 1 %>' />
                                </td>

                                <td class="text-center">

                                    <!-- Usuario ACTIVO → Mostrar icono de eliminar -->
                                    <asp:LinkButton 
                                        ID="btnEliminar" 
                                        runat="server" 
                                        Text="<i class='fa fa-trash'></i>"
                                        CssClass="btn btn-sm btn-danger rounded-circle"
                                        CommandArgument='<%# Eval("idUsuario") %>' 
                                        OnClick="btnEliminar_Click"
                                        Visible="false">
                                    </asp:LinkButton>

                                    <!-- Botón para reactivar (solo si ACTIVO = 0) -->
                                    <asp:LinkButton 
                                        ID="btnReactivar" 
                                        runat="server" 
                                        Text="<i class='fa fa-rotate-right'></i>"
                                        CssClass="btn btn-sm btn-success rounded-circle"
                                        CommandArgument='<%# Eval("idUsuario") %>' 
                                        OnClick="btnReactivar_Click"
                                        Visible="false">
                                    </asp:LinkButton>

                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
        </div>

    </div>

    <!-- MODAL SERVICIOS EMPLEADO -->
    <div class="modal fade" id="modalServicios" tabindex="-1">
        <div class="modal-dialog modal-lg modal-dialog-centered">
            <div class="modal-content">

                <div class="modal-header">
                    <h5 class="modal-title">Servicios del empleado</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                </div>

                <div class="modal-body">

                    <div id="empleadoDatos" class="mb-3 border p-3 rounded bg-light">
                        <h6 class="mb-1">
                            <strong>Empleado:</strong>
                            <asp:Literal ID="litEmpNombre" runat="server"></asp:Literal>
                        </h6>
                        <p class="mb-0">
                            <strong>Correo:</strong>
                            <asp:Literal ID="litEmpCorreo" runat="server"></asp:Literal>
                        </p>
                    </div>

                    <table class="table table-bordered table-striped">
                        <thead>
                            <tr>
                                <th>Servicio</th>
                                <th>Precio</th>
                                <th class="text-center">Acción</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="rpServiciosEmpleado" runat="server"
                                          OnItemCommand="rpServiciosEmpleado_ItemCommand">
                                <ItemTemplate>
                                    <tr>
                                        <td><%# Eval("nombre") %></td>
                                        <td><%# Eval("precio") %></td>
                                        <td class="text-center">
                                            <asp:LinkButton ID="btnEliminarServicio" runat="server"
                                                CssClass="btn btn-danger btn-sm"
                                                CommandName="EliminarServicio"
                                                CommandArgument='<%# Eval("idServicio") %>'>
                                                <i class="fa fa-trash"></i>
                                            </asp:LinkButton>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>

                    <div class="text-end">
                        <button type="button" class="btn btn-primary" onclick="abrirAgregarServicio()">
                            <i class="fa fa-plus"></i> Agregar servicio
                        </button>
                    </div>

                </div>

            </div>
        </div>
    </div>

    <!-- MODAL AGREGAR SERVICIO -->
    <div class="modal fade" id="modalAgregarServicio" tabindex="-1">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">

                <div class="modal-header">
                    <h5 class="modal-title">Agregar servicio al empleado</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                </div>

                <div class="modal-body">

                    <div class="mb-3">
                        <label class="form-label">Servicio</label>
                        <asp:DropDownList ID="ddlServiciosDisponibles" runat="server"
                                          CssClass="form-select">
                        </asp:DropDownList>
                    </div>

                </div>

                <div class="modal-footer">
                    <asp:Button ID="btnGuardarServicio" runat="server"
                                CssClass="btn btn-primary"
                                Text="Guardar"
                                OnClick="btnGuardarServicio_Click" />
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">
                        Cancelar
                    </button>
                </div>

            </div>
        </div>
    </div>

    <asp:HiddenField ID="hfEmpleadoId" runat="server" />

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ScriptsContent" runat="server">
    <script>
        const tabEmpleados = document.getElementById("tabEmpleados");
        const tabClientes = document.getElementById("tabClientes");
        const hfTipoUsuario = document.getElementById("hfTipoUsuario");
        const divBotonBuscar = document.getElementById("divBotonBuscar");
        const contenedorTabla = document.getElementById("<%= contenedorTabla.ClientID %>");

        const inpNombre = document.getElementById("searchNombre");
        const inpPrimerApellido = document.getElementById("searchPrimerApellido");
        const inpSegundoApellido = document.getElementById("searchSegundoApellido");
        const inpCorreo = document.getElementById("searchCorreo");
        const inpCelular = document.getElementById("searchCelular");
        const selActive = document.getElementById("filterActive");

        function switchTab(tipo) {
            hfTipoUsuario.value = tipo;
            const tabEmpleados = document.getElementById("tabEmpleados");
            const tabClientes = document.getElementById("tabClientes");
            const divBotonBuscar = document.getElementById("divBotonBuscar");
            const colServicios = document.querySelectorAll('.col-servicios');

            if (tipo === 'empleados') {
                tabEmpleados.className = 'tab-active';
                tabClientes.className = 'tab-inactive';
                divBotonBuscar.style.display = 'none';

                if (contenedorTabla) contenedorTabla.style.display = 'block';

                colServicios.forEach(el => el.style.display = '');
                filterTable();
            } else {
                tabClientes.className = 'tab-active';
                tabEmpleados.className = 'tab-inactive';
                divBotonBuscar.style.display = 'flex';
                
                colServicios.forEach(el => el.style.display = 'none');

                const primeraFila = document.querySelector("#userTable tr");
                const esTablaDeEmpleados = primeraFila && primeraFila.dataset.role === '2';
                if (esTablaDeEmpleados) {
                    if (contenedorTabla) contenedorTabla.style.display = 'none';
                }
            }
        }

        document.addEventListener("DOMContentLoaded", () => {
            const tipoActual = hfTipoUsuario.value || 'empleados';

            if (tipoActual === 'empleados') {
                switchTab('empleados');
            } else {
                tabClientes.className = 'tab-active';
                tabEmpleados.className = 'tab-inactive'; 

                divBotonBuscar.style.display = 'flex';

                document.querySelectorAll('.col-servicios').forEach(el => el.style.display = 'none');
            }
        });

        function filterTable() {
            if (hfTipoUsuario.value === 'clientes') return;

            const valNombre = inpNombre.value.toLowerCase().trim();
            const valApellido1 = inpPrimerApellido.value.toLowerCase().trim();
            const valApellido2 = inpSegundoApellido.value.toLowerCase().trim();
            const valCorreo = inpCorreo.value.toLowerCase().trim();
            const valCelular = inpCelular.value.toLowerCase().trim();
            const valActivo = selActive.value;

            const rows = document.querySelectorAll("#userTable tr");
            rows.forEach(row => {

                const rowRole = (row.dataset.role || "").toString().trim();
                const rowActive = (row.dataset.active || "").toString().trim();

                const textNombreCompleto = row.cells[0].innerText.toLowerCase();
                const textEmail = row.cells[1].innerText.toLowerCase();
                const textCelular = row.cells[2].innerText.toLowerCase();

                const matchesTab = (rowRole === '2');
                const matchesActive = (valActivo === "") || (rowActive === valActivo);
                const matchesNombre = textNombreCompleto.includes(valNombre);
                const matchesApellido1 = textNombreCompleto.includes(valApellido1);
                const matchesApellido2 = textNombreCompleto.includes(valApellido2);
                const matchesCorreo = textEmail.includes(valCorreo);
                const matchesCelular = textCelular.includes(valCelular);


                if (matchesTab && matchesActive && matchesNombre && matchesApellido1 && matchesApellido2 && matchesCorreo && matchesCelular) {
                    row.style.display = "";
                } else {
                    row.style.display = "none";
                }

            });

        }

        const inputs = [inpNombre, inpPrimerApellido, inpSegundoApellido, inpCorreo, inpCelular, selActive];

        inputs.forEach(input => {
            if (input) {
                input.addEventListener("input", function () {
                    if (hfTipoUsuario.value === 'empleados') filterTable();
                });
                if (input.tagName === "SELECT") {
                    input.addEventListener("change", function () {
                        if (hfTipoUsuario.value === 'empleados') filterTable();
                    });
                }
            }
        });

        document.addEventListener("DOMContentLoaded", () => {
            const tipoActual = hfTipoUsuario.value || 'empleados';
            switchTab(tipoActual);
        });

        function validarBusquedaCliente() {
            const vNombre = inpNombre.value.trim();
            const vApellido1 = inpPrimerApellido.value.trim();
            const vApellido2 = inpSegundoApellido.value.trim();
            const vCorreo = inpCorreo.value.trim();
            const vCelular = inpCelular.value.trim();
            const vActivo = selActive.value;

            const divError = document.getElementById("divErrorBusqueda");

            if (vNombre === "" && vApellido1 === "" && vApellido2 === "" && vCorreo === "" && vCelular === "" && vActivo === "") {
                if (divError) divError.style.display = "block";
                return false;
            }
            if (divError) divError.style.display = "none";
            return true;
        }
    </script>

    <script>
        function showModalFormEmpleado() {
            var modalFormGroup = new bootstrap.Modal(document.getElementById('modalServicios'));
            modalFormGroup.show();
        }

        function abrirAgregarServicio() {
            var modal = new bootstrap.Modal(document.getElementById('modalAgregarServicio'));
            modal.show();
        }
    </script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
