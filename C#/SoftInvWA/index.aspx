<%@ Page Language="C#" MasterPageFile="~/MGBeautySpa.Master" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="SoftInvWA.WebForm1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    MG Beauty Spa - Home
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" runat="server">
    <div class="header">
    <h1>MG Beauty Spa</h1>
    </div>
    <div class="container">
        
        <div class="col-md-4">
            <h2>Our Services</h2>
            <ul>
                <li>Facials</li>
                <li>Massages</li>
                <li>Manicures & Pedicures</li>
                <li>Hair Treatments</li>
            </ul>
        </div>
        <div class="col-md-4">
            <h2>About Us</h2>
            <p>At MG Beauty Spa, we are dedicated to providing top-notch beauty and wellness services. Our experienced staff ensures that you leave feeling refreshed and rejuvenated.</p>
        </div>
        <div class="col-md-4">
            <h2>Contact Us</h2>
            <address>
                123 Beauty St.<br />
                Wellness City, WC 45678<br />
                Phone: (123) 456-7890<br />
                Email:
            </address>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphFooter" runat="server">
    <div class="header">
        <p>&copy; 2025 MG Beauty Spa. All rights reserved.</p>
    </div>
</asp:Content>