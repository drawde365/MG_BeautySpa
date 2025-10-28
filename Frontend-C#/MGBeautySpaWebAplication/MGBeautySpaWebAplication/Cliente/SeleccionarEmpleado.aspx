<%@ Page Title="Seleccionar Empleado" Language="C#" MasterPageFile="~/Cliente/Cliente.Master" AutoEventWireup="true" CodeBehind="SeleccionarEmpleado.aspx.cs" Inherits="MGBeautySpaWebAplication.Cliente.SeleccionarEmpleado" %>

<%-- 
  1. SE ELIMINARON las etiquetas <html>, <head>, <body>.
  2. Todos los <link> se mueven al <asp:Content> que va en el <head> de la MasterPage.
--%>
<asp:Content ID="Content1" ContentPlaceHolderID="ScriptsContent" runat="server">
    <link rel="preconnect" href="https://fonts.googleapis.com">
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
    <link href="https://fonts.googleapis.com/css2?family=Plus+Jakarta+Sans:wght@400;600;700&family=ZCOOL+XiaoWei&display=swap" rel="stylesheet">
    
    <link rel="stylesheet" href="SeleccionarEmpleado.css">
</asp:Content>

<%-- 
  3. SE ELIMINÓ el <form id="form1" runat="server">. 
     ¡Tu MasterPage ya tiene uno! No puedes tener un formulario dentro de otro.
  4. Todo el contenido visible (main, section, etc.) va en este bloque.
--%>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <%-- El ScriptManager debe ir DENTRO de un <asp:Content> (y por tanto, dentro del <form> de la MasterPage) --%>
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
        
    <main class="container">
        <header class="page-header">
            <h1 class="main-title">Selecciona el empleado</h1>
            <div class="service-info">
                <h2 class="service-title">Limpieza Facial Profunda</h2>
                <p class="service-subtitle">Empleados que realizan este servicio</p>
            </div>
        </header>

        <section class="employee-grid">
        
            <asp:Repeater ID="rpEmpleados" runat="server" OnItemCommand="rpEmpleados_ItemCommand">
                <ItemTemplate>
                    <article class="employee-card">
                        
                        <%-- Usa Eval("AvatarUrl") para poner la URL de la imagen --%>
                        <div class="avatar" style="background-image: url('<%# Eval("AvatarUrl") %>')"></div>
                        
                        <%-- Usa Eval("Nombre") para poner el nombre --%>
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