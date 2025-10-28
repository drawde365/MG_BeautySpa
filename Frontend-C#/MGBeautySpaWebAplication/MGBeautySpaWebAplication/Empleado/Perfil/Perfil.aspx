<%@ Page Title="" Language="C#" MasterPageFile="~/Empleado/Empleado.Master" AutoEventWireup="true" CodeBehind="Perfil.aspx.cs" Inherits="MGBeautySpaWebAplication.Empleado.Perfil.Perfil" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <%-- 
      Necesitarás Bootstrap Icons para los íconos del menú. 
      Si aún no lo tienes, agrega esta línea:
    --%>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.5/font/bootstrap-icons.css">

    <style>
        /* Estilo para la fuente del título 'ZCOOL XiaoWei' */
        .empleado-title-font {
            font-family: 'ZCOOL XiaoWei', serif;
            font-weight: 400;
            font-size: 40px;
            color: #148C76; /* Color del título '¡Hola...' */
        }

        /* Contenedor de la barra lateral de navegación */
        .sidebar-nav-empleado {
            background-color: #ECFFFD;
            font-family: 'Plus Jakarta Sans', sans-serif;
            color: #0D1C17;
            height: 100%; /* Para que ocupe toda la altura */
        }

        /* Estilos para los enlaces de navegación */
        .nav-empleado .nav-link {
            font-family: 'Plus Jakarta Sans', sans-serif;
            font-weight: 500;
            font-size: 14px;
            color: #1C0D12; /* Color de texto normal */
            padding: 8px 12px;
            gap: 12px;
        }

        /* Estilo para el enlace ACTIVO (Mi perfil) */
        .nav-empleado .nav-link.active {
            background-color: rgba(125, 163, 161, 0.5);
            color: #1C0D12;
            border-radius: 8px;
        }

        /* Hover para enlaces no activos */
        .nav-empleado .nav-link:not(.active):hover {
            background-color: rgba(125, 163, 161, 0.1);
        }

        /* Estilo para el enlace de Cerrar Sesión */
        .nav-empleado .nav-link.text-danger {
            color: #C31E21 !important; /* Color de peligro */
        }
        
        /* Estilos para la lista de datos */
        .info-list-label {
            font-size: 0.9rem;
            color: #7DA3A1; /* Color de 'Nombres', 'Apellidos', etc. */
            margin-bottom: 4px;
        }

        .info-list-value {
            font-size: 1rem;
            font-weight: 500;
            color: #1C0D12; /* Color de 'Sophia', 'Bennett', etc. */
        }
    </style>
</asp:Content>
<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">

    <%-- 
      Usamos un contenedor de Bootstrap para centrar el contenido y
      darle los márgenes (padding: 20px 24px) 
    --%>
    <div class="container-xl my-4">
        
        <%-- 
          Fila principal que divide la página en dos columnas:
          - Barra lateral (col-lg-3)
          - Contenido principal (col-lg-9)
          'g-4' añade el (gap: 4px, aprox.)
        --%>
        <div class="row g-4">

            <div class="col-lg-3">
                <%-- Contenedor 'Depth 4, Frame 0' --%>
                <div class="sidebar-nav-empleado p-3 rounded-3 d-flex flex-column">
                    
                    <div class="d-flex flex-column align-items-center p-3">
                        <img src="<%: ResolveUrl("~/Content/images/Empleado/blank-photo.jpg") %>" 
                             alt="Foto de perfil" 
                             class="rounded-circle mb-3" 
                             style="width: 128px; height: 128px; object-fit: cover;" />
                        
                        <h3 class="fw-bold text-center" style="font-size: 22px; color: #0D1C17;">
                            <%-- "Sophia Bennett" --%>
                            <asp:Literal ID="litNombreEmpleado" runat="server" Text="Sophia Bennett" />
                        </h3>
                    </div>

                    <nav class="nav nav-pills flex-column mt-3 nav-empleado">
                        
                        <%-- "Mi perfil" (Activo) --%>
                        <a class="nav-link active d-flex align-items-center" href="#!">
                            <i class="bi bi-person-fill" style="font-size: 20px;"></i>
                            <span>Mi perfil</span>
                        </a>
                        
                        <%-- "Cambiar contraseña" --%>
                        <a class="nav-link d-flex align-items-center" href="../../Cuenta/ModificarContraseña.aspx">
                            <i class="bi bi-lock-fill" style="font-size: 20px;"></i>
                            <span>Cambiar contraseña</span>
                        </a>

                        <%-- "Cerrar sesión" --%>
                        <asp:LinkButton ID="btnLogoutSidebar" runat="server" 
                                        CssClass="nav-link text-danger d-flex align-items-center" 
                                        OnClick="btnLogout_Click">
                            <i class="bi bi-box-arrow-right" style="font-size: 20px;"></i>
                            <span>Cerrar sesión</span>
                        </asp:LinkButton>
                    </nav>

                </div>
            </div>

            <div class="col-lg-9">
                <%-- Contenedor 'Depth 3, Frame 1' --%>
                <div class="bg-white p-4 rounded-3">
                    
                    <div class="mb-3">
                        <h1 class="empleado-title-font">
                             <%-- "¡Hola, Sophia Bennett!" --%>
                            ¡Hola, <asp:Literal ID="litNombreSaludo" runat="server" Text="Sophia" />!
                        </h1>
                        <p class="fs-6 text-dark">
                            Aquí puedes revisar y actualizar tus datos personales.
                        </p>
                    </div>

                    <%-- 
                      Esto replica la estructura de 2 columnas de tus datos.
                      Usamos el grid de Bootstrap (row/col)
                    --%>
                    <div class="row">
                        
                        <div class="col-md-6 border-top py-3 px-2">
                            <div class="info-list-label">Nombres</div>
                            <div class="info-list-value">
                                <asp:Literal ID="litNombres" runat="server" Text="Sophia" />
                            </div>
                        </div>
                        
                        <div class="col-md-6 border-top py-3 px-2">
                            <div class="info-list-label">Apellidos</div>
                            <div class="info-list-value">
                                <asp:Literal ID="litApellidos" runat="server" Text="Bennett" />
                            </div>
                        </div>

                        <div class="col-md-6 border-top py-3 px-2">
                            <div class="info-list-label">Email</div>
                            <div class="info-list-value">
                                <asp:Literal ID="litEmail" runat="server" Text="a20230636@pucp.edu.pe" />
                            </div>
                        </div>
                        
                        <div class="col-md-6 border-top py-3 px-2">
                            <div class="info-list-label">Teléfono</div>
                            <div class="info-list-value">
                                <asp:Literal ID="litTelefono" runat="server" Text="999888777" />
                            </div>
                        </div>

                        <div class="col-md-6 border-top py-3 px-2">
                            <div class="info-list-label">Fecha de nacimiento</div>
                            <div class="info-list-value">
                                <asp:Literal ID="litFechaNacimiento" runat="server" Text="12/02/2000" />
                            </div>
                        </div>
                        
                        <%-- La última parte de tu código se cortó, pero aquí iría el siguiente campo --%>
                        <div class="col-md-6 border-top py-3 px-2">
                             <div class="info-list-label">DNI (Ejemplo)</div>
                            <div class="info-list-value">
                                <asp:Literal ID="litDNI" runat="server" Text="12345678" />
                            </div>
                        </div>

                    </div> <%-- fin de .row --%>
                    
                    <div class="text-end mt-4">
                        <asp:Button ID="btnEditar" runat="server" Text="Editar Perfil" CssClass="btn btn-primary" OnClick="btnEditarPerfil" />
                    </div>

                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ScriptsContent" runat="server">
</asp:Content>
