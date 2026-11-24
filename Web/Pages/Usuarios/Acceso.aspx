<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Acceso.aspx.cs" Inherits="Es.Udc.DotNet.PracticaMAD.Web.Pages.Usuarios.Acceso" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <title>Acceso</title>
    <script type="text/javascript">
        function alternarContrasena(boton) {
            const campo = document.getElementById("txtContrasena");
            const mostrar = campo.type === "password";
            campo.type = mostrar ? "text" : "password";
            boton.value = mostrar ? "Ocultar contraseña" : "Ver contraseña";
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <main>
            <h1>Acceso</h1>
        <p>
            <asp:Label ID="lblReg" runat="server" ForeColor="Green"></asp:Label>
        </p>
        <p>
            <asp:Label ID="Label3" runat="server" Text="Usuario"></asp:Label>
        </p>
        <p>
            <asp:TextBox ID="txtLogin" runat="server" Height="37px" OnTextChanged="txtLogin_TextChanged" Width="346px"></asp:TextBox>
        </p>
        <p>
            <asp:Label ID="Label2" runat="server" Text="Contraseña"></asp:Label>
        </p>
        <p>
            <asp:TextBox ID="txtContrasena" runat="server" Height="37px" TextMode="Password" Width="345px" ClientIDMode="Static"></asp:TextBox>
            <asp:Button ID="btnVerContrasena" runat="server" CausesValidation="False" Height="41px" OnClientClick="alternarContrasena(this); return false;" Text="Ver contraseña" UseSubmitBehavior="False" Width="150px" />
        </p>
        <p>
            <asp:CheckBox ID="Recuerdame" runat="server" OnCheckedChanged="CheckBox1_CheckedChanged" Text="Recuerdame" />
        </p>
        <p>
            <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
        </p>
        <p>
            <asp:Button ID="btnAcceder" runat="server" Height="58px" Text="Acceder" Width="215px" OnClick="btnAcceder_Click" />
        </p>
        <p>
            ¿No estás registrado?&nbsp;&nbsp;&nbsp; <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Pages/Usuarios/Registro.aspx">Registrarse</asp:HyperLink>
        </p>
        </main>
    </form>
</body>
</html>
