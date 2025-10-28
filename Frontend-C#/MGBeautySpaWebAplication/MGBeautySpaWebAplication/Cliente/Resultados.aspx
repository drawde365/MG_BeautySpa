<%@ Page Title="Resultados" Language="C#" MasterPageFile="~/Cliente/Cliente.Master"
    AutoEventWireup="true" CodeBehind="Resultados.aspx.cs"
    Inherits="MGBeautySpaWebAplication.Cliente.Resultados" %>

<asp:Content ID="ct2" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="ResultadosClienteStyle.css" />
    <div class="results-header">
        <h1 class="results-title">Resultados para "<asp:Literal ID="litQuery" runat="server" />" 
            <small class="text-muted"><asp:Literal ID="litCount" runat="server" /></small>
        </h1>
    </div>

    <asp:Panel runat="server" ID="pnlNoResults" Visible="false" CssClass="alert alert-light border">
        No hay resultados para "<asp:Literal ID="Literal1" runat="server" />". Intenta con otra palabra (p. ej., <em>crema</em>).
    </asp:Panel>

    <div class="products-grid-container">
        <div class="products-grid-row">

            <asp:Repeater ID="rptProductos" runat="server">
                <ItemTemplate>
                    <a href='<%# ResolveUrl("~/Cliente/DetalleProducto.aspx?id=" + Eval("Id")) %>' class="product-card">
                        
                        <div class="product-image-container">
                            <img src='<%# Eval("ImagenUrl") %>' alt='<%# Eval("Nombre") %>' style="width:100%;height:100%;object-fit:cover;" />
                        </div>

                        <div class="product-details">
                            <h2 class="product-name"><%# Eval("Nombre") %></h2>
                            <p class="product-description">S/ <%# String.Format("{0:0.00}", Eval("Precio")) %></p> 
                     </div>
                    </a>
                </ItemTemplate>
                
                <SeparatorTemplate>
                    <%-- Este Template se puede dejar vacío --%>
                </SeparatorTemplate>
            </asp:Repeater>
            </div>
    </div>

</asp:Content>