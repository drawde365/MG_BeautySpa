<%@ Page Title="Resultados" Language="C#" MasterPageFile="~/Cliente/Cliente.Master"
    AutoEventWireup="true" CodeBehind="Resultados.aspx.cs"
    Inherits="MGBeautySpaWebAplication.Cliente.Resultados" %>

<asp:Content ID="ct1" ContentPlaceHolderID="TitleContent" runat="server">Resultados</asp:Content>

<asp:Content ID="ct2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="d-flex align-items-center justify-content-between mb-3">
        <h4 class="m-0">Resultados para: <span class="text-muted">"<asp:Literal ID="litQuery" runat="server" /></span>"</h4>
        <small class="text-muted"><asp:Literal ID="litCount" runat="server" /></small>
    </div>

    <asp:Panel runat="server" ID="pnlNoResults" Visible="false" CssClass="alert alert-light border">
        No hay resultados. Intenta con otra palabra (p. ej., <em>crema</em>).
    </asp:Panel>

    <asp:Repeater ID="rptProductos" runat="server">
        <ItemTemplate>
            <div class="card mb-3 shadow-sm">
                <div class="row g-0 align-items-center">
                    <div class="col-auto p-3">
                        <img src='<%# Eval("ImagenUrl") %>' alt="" style="width:80px;height:80px;object-fit:cover;border-radius:12px;">
                    </div>
                    <div class="col">
                        <div class="card-body py-3">
                            <h6 class="card-title mb-1"><%# Eval("Nombre") %></h6>
                            <small class="text-muted">S/ <%# String.Format("{0:0.00}", Eval("Precio")) %></small>
                        </div>
                    </div>
                    <div class="col-auto pe-3">
                        <a class="btn btn-primary" href='<%# ResolveUrl("~/Cliente/DetalleProducto.aspx?slug=crema-hidratante") %>'>
                            Ver detalle
                        </a>
                    </div>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>
</asp:Content>
