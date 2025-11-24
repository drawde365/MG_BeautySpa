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
    </style>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container py-4">

        <h1 class="h1-admin-title">Gestion de usuarios</h1>

        <!-- FILA DE BUSQUEDA Y FILTRO -->
        <div class="d-flex align-items-center gap-3 mb-4">

            <!-- BUSCADOR -->
            <div class="position-relative flex-grow-1">
                <i class="fa fa-search search-icon"></i>
                <input type="text" id="searchInput" class="form-control search-box"
                       placeholder="Buscar por nombre, email, teléfono..." />
            </div>

            <!-- FILTRO POR ROL -->
            <select id="filterRole" class="form-select" style="max-width:200px;">
                <option value="">Todos</option>
                <option value="1">Cliente</option>
                <option value="2">Empleado</option>
            </select>

            <select id="filterActive" class="form-select" style="max-width:180px;">
                <option value="">Todos</option>
                <option value="1">Activos</option>
                <option value="0">Inactivos</option>
            </select>

        </div>

        <!-- TABLA -->
        <div class="table-responsive shadow-sm rounded">
            <table class="table table-hover align-middle mb-0">
                <thead class="border-bottom">
                    <tr>
                        <th>Nombre completo</th>
                        <th>Email</th>
                        <th>Teléfono</th>
                        <th>Rol</th>
                        <th>Servicios</th>
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

                                <td>
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
        const searchInput = document.getElementById("searchInput");
        const filterRole = document.getElementById("filterRole");
        const filterActive = document.getElementById("filterActive");
        const rows = document.querySelectorAll("#userTable tr");

        function filterTable() {
            const searchText = searchInput.value.toLowerCase();
            const role = filterRole.value;     // "" | "1" | "2"
            const active = filterActive.value; // "" | "1" | "0"

            rows.forEach(row => {
                const rowRole = row.dataset.role;
                const rowActive = row.dataset.active;

                const matchesText = row.innerText.toLowerCase().includes(searchText);
                const matchesRole = role === "" || rowRole === role;
                const matchesActive = active === "" || rowActive === active;

                row.style.display = (matchesText && matchesRole && matchesActive)
                    ? ""
                    : "none";
            });
        }

        if (searchInput)   searchInput.addEventListener("input", filterTable);
        if (filterRole)    filterRole.addEventListener("change", filterTable);
        if (filterActive)  filterActive.addEventListener("change", filterTable);
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
