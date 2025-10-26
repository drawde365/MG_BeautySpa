<%@ Page Title="Detalle" Language="C#" MasterPageFile="~/Cliente/Cliente.Master"
    AutoEventWireup="true" CodeBehind="DetalleProducto.aspx.cs"
    Inherits="MGBeautySpaWebAplication.Cliente.DetalleProducto" %>

<asp:Content ID="ct1" ContentPlaceHolderID="TitleContent" runat="server">Detalle</asp:Content>

<asp:Content ID="ct2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row g-4">
        <div class="col-md-4">
            <img src="/Content/images/CremaHidratante.jpg" alt="Crema hidratante"
                 class="img-fluid rounded shadow-sm" />
        </div>
        <div class="col-md-8">
            <h3 class="mb-2">Crema hidratante</h3>
            <div class="text-muted mb-3">S/ 49.90</div>
            <p class="mb-4">
                Hidratación profunda con textura ligera y rápida absorción. Ideal para uso diario.
            </p>
            <a href='<%: ResolveUrl("~/Cliente/Carrito.aspx") %>' class="btn btn-primary">Añadir al carrito</a>
        </div>
    </div>
</asp:Content>
