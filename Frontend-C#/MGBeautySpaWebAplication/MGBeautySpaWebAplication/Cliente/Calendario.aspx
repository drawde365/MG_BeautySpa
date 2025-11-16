<%@ Page Title="Calendario" Language="C#" MasterPageFile="~/Cliente/Cliente.Master" AutoEventWireup="true" CodeBehind="Calendario.aspx.cs" Inherits="MGBeautySpaWebAplication.Cliente.Calendario" %>

<%-- 1. Contenido del Head (CSS y Fuentes) --%>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <%-- El ID de tu MasterPage es "HeadContent", no "ScriptsContent". Lo he ajustado. --%>
    <link rel="preconnect" href="https://fonts.googleapis.com">
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
    <link href="https://fonts.googleapis.com/css2?family=Plus+Jakarta+Sans:wght@400;500;700&family=ZCOOL+XiaoWei&display=swap" rel="stylesheet">
    
    <%-- Enlace a tu nuevo archivo CSS --%>
    <link rel="stylesheet" href="Calendario.css">
</asp:Content>


<%-- 2. Contenido Principal (El Cuerpo de la Página) --%>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <%-- El ScriptManager es necesario para el UpdatePanel --%>
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    
    <%-- El UpdatePanel evita que la página entera se recargue al cambiar de mes o seleccionar un día --%>
    <asp:UpdatePanel ID="upCalendario" runat="server">
        <ContentTemplate>
        
            <%-- HiddenField para guardar el día seleccionado entre postbacks --%>
            <asp:HiddenField ID="hdnSelectedDay" runat="server" Value="" />
            
            <div class="page-container">
                <nav class="breadcrumbs">
                    <a href="Servicios.aspx">Servicios</a>
                    <span>/</span>
                    <a href="SeleccionarEmpleado.aspx">Seleccionar Empleado</a>
                    <span>/</span>
                    <strong>Calendario</strong>
                </nav>

                <h1 class="main-title">Escoge la fecha y hora de tu reserva</h1>

                <main class="content-body">

                    <section class="calendar-column">
                        <div class="calendar-widget">
                            
                            <header class="calendar-header">
                                <asp:LinkButton ID="btnPrevMonth" runat="server" OnClick="btnPrevMonth_Click" CssClass="nav-arrow" Aria-Label="Mes anterior">&lt;</asp:LinkButton>
                                <h2><asp:Literal ID="litMonthYear" runat="server" /></h2>
                                <asp:LinkButton ID="btnNextMonth" runat="server" OnClick="btnNextMonth_Click" CssClass="nav-arrow" Aria-Label="Mes siguiente">&gt;</asp:LinkButton>
                            </header>
                             
                            <div class="calendar-weekdays">
                                <div class="weekday-cell">D</div>
                                <div class="weekday-cell">L</div>
                                <div class="weekday-cell">M</div>
                                <div class="weekday-cell">M</div>
                                <div class="weekday-cell">J</div>
                                <div class="weekday-cell">V</div>
                                <div class="weekday-cell">S</div>
                            </div>
                            
                            <asp:Repeater ID="rpCalendarDays" runat="server" 
                                OnItemDataBound="rpCalendarDays_ItemDataBound" 
                                OnItemCommand="rpCalendarDays_ItemCommand">
                                <HeaderTemplate>
                                    <div class="calendar-grid">
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnDay" runat="server" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    </div>
                                </FooterTemplate>
                            </asp:Repeater>
                        </div>
                    </section>

                    <section class="time-column">
                        <h2 class="time-title">Horarios Disponibles</h2>
                        
                        <div class="time-select-wrapper">
                            <asp:DropDownList ID="ddlHorarios" runat="server" 
                                CssClass="time-select-dropdown" 
                                Enabled="false" />
                        </div>
                        
                        <div class="action-buttons">
                            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" 
                                CssClass="btn-action btn-cancel" 
                                OnClick="btnCancelar_Click" 
                                CausesValidation="false" />
                            <asp:Button ID="btnReservar" runat="server" Text="Reservar Cita" 
                                CssClass="btn-action btn-submit" 
                                OnClick="btnReservar_Click" />
                        </div>
                    </section>

                </main>
            </div>
            
        </ContentTemplate>
    </asp:UpdatePanel>
    
</asp:Content>