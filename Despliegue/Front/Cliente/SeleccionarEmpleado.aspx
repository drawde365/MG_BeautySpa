<%@ Page Title="Seleccionar Empleado" Language="C#" MasterPageFile="~/Cliente/Cliente.Master" AutoEventWireup="true" CodeBehind="SeleccionarEmpleado.aspx.cs" Inherits="MGBeautySpaWebAplication.Cliente.SeleccionarEmpleado" %>

<%-- 1. CSS y Fuentes (Maldado al HeadContent para que cargue antes) --%>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="preconnect" href="https://fonts.googleapis.com">
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
    <link href="https://fonts.googleapis.com/css2?family=Plus+Jakarta+Sans:wght@400;600;700&family=ZCOOL+XiaoWei&display=swap" rel="stylesheet">
    <link rel="stylesheet" href="SeleccionarEmpleado.css">

    <style>
        /* ✅ PARCHE PARA STICKY FOOTER */
        /* Aseguramos que el HTML y el Body ocupen toda la altura */
        html, body {
            height: 100%;
            margin: 0;
        }

        /* El formulario (contenedor principal en WebForms) se convierte en Flex Column */
        #form1 {
            display: flex;
            flex-direction: column;
            min-height: 100vh; /* Ocupa al menos el 100% de la ventana */
        }

        /* El wrapper del contenido crece para empujar el footer abajo */
        .content-body-wrapper {
            flex: 1 0 auto; /* Grow: 1, Shrink: 0, Basis: auto */
            display: flex;
            flex-direction: column;
        }

        /* Ajuste visual para esta página específica */
        .employee-grid {
            margin-bottom: 40px; /* Espacio extra antes del footer */
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
        
    <main class="container py-4"> <%-- Añadido py-4 para espaciado vertical --%>
        <header class="page-header text-center mb-5">
            <h1 class="main-title">Selecciona el empleado</h1>
            <div class="service-info">
                <h2 class="service-title text-primary">
                    <asp:Literal ID="litNombreServicio" runat="server" Text="Servicio Seleccionado"></asp:Literal>
                </h2>
                <p class="service-subtitle text-muted">Empleados que realizan este servicio</p>
            </div>
        </header>

        <section class="employee-grid">
        
            <asp:Panel ID="pnlNoEmpleados" runat="server" Visible="false" CssClass="no-results-box text-center py-5">
                <div style="font-size: 40px; margin-bottom: 10px;">😕</div>
                <h3>Lo sentimos</h3>
                <p class="text-muted">No hay empleados disponibles para este servicio en este momento.</p>
                <a href="Servicios.aspx" class="btn btn-outline-primary mt-3">Volver a Servicios</a>
            </asp:Panel>

            <asp:Repeater ID="rpEmpleados" runat="server" OnItemCommand="rpEmpleados_ItemCommand">
                <ItemTemplate>
                    <article class="employee-card">
                        <%-- Usamos ResolveUrl para asegurar que la imagen cargue --%>
                        <div class="avatar" style="background-image: url('<%# ResolveUrl(Eval("AvatarUrl").ToString()) %>')"></div>
                        <h3 class="employee-name"><%# Eval("Nombre") %></h3>
                        <asp:Button ID="btnSeleccionar" runat="server" 
                            CssClass="btn-select" 
                            Text="Revisar Calendario" 
                            CommandName="Select" 
                            CommandArgument='<%# Eval("Id") + "|" + Eval("Nombre") %>' />
                    </article>
                </ItemTemplate>
            </asp:Repeater>
            
        </section>

    </main>
    
</asp:Content>

<%-- Si tienes scripts específicos, usa este ContentPlaceHolder --%>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsContent" runat="server">
</asp:Content>