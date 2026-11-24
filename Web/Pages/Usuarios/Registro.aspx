<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registro.aspx.cs" Inherits="Es.Udc.DotNet.PracticaMAD.Web.Pages.Usuarios.Registro" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <title>Registro</title>
    <script type="text/javascript">
        function alternarContrasena(boton, idCampo) {
            const campo = document.getElementById(idCampo);
            const mostrar = campo.type === "password";
            campo.type = mostrar ? "text" : "password";
            boton.value = mostrar ? "Ocultar contraseña" : "Ver contraseña";
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <main>
            <h1>Regístrate</h1>
            <p>
                &nbsp;</p>
            <p>
                <asp:Label ID="Label3" runat="server" Text="Nombre"></asp:Label>
            </p>
            <p>
                <asp:TextBox ID="txtNombreReg" runat="server" Height="37px" Width="346px"></asp:TextBox>
            </p>
            <p>
                <asp:Label ID="Label4" runat="server" Text="Apellido/s"></asp:Label>
            </p>
            <p>
                <asp:TextBox ID="txtApellidoReg" runat="server" Height="37px" Width="346px"></asp:TextBox>
            </p>
            <p>
                Correo Electrónico</p>
            <p>
                <asp:TextBox ID="txtCorreoReg" runat="server" Height="37px" Width="346px"></asp:TextBox>
            </p>
            <p>
                <asp:Label ID="Label6" runat="server" Text="Nombre de Usuario"></asp:Label>
            </p>
            <p>
                <asp:TextBox ID="txtUserReg" runat="server" Height="37px" Width="346px"></asp:TextBox>
            </p>
            <p>
                &nbsp;</p>
            <p>
                <asp:Label ID="Label2" runat="server" Text="Contraseña"></asp:Label>
            </p>
            <p>
                <asp:TextBox ID="txtContrasenaReg1" runat="server" Height="37px" TextMode="Password" Width="345px" ClientIDMode="Static"></asp:TextBox>
                <asp:Button ID="btnVerContrasenaReg" runat="server" CausesValidation="False" Height="41px" OnClientClick="alternarContrasena(this, 'txtContrasenaReg1'); return false;" Text="Ver contraseña" UseSubmitBehavior="False" Width="150px" />
            </p>
            <p>
                <asp:Label ID="Label7" runat="server" Text="Confirmar Contraseña"></asp:Label>
            </p>
            <p>
                <asp:TextBox ID="txtContrasenaReg2" runat="server" Height="37px" TextMode="Password" Width="345px" ClientIDMode="Static"></asp:TextBox>
                <asp:Button ID="btnVerContrasenaReg0" runat="server" CausesValidation="False" Height="41px" OnClientClick="alternarContrasena(this, 'txtContrasenaReg2'); return false;" Text="Ver contraseña" UseSubmitBehavior="False" Width="150px" />
            </p>
            <p>
                &nbsp;</p>
            <p>
                &nbsp;</p>
            <p>
                <asp:Label ID="lblErrorReg" runat="server" ForeColor="Red"></asp:Label>
            </p>
            <p>
                <asp:Button ID="btnRegistarse" runat="server" Height="58px" Text="Registrarse" Width="215px" OnClick="btnRegistrarse_Click" />
            </p>
            <p>
                &nbsp;</p>
            <p>
                ¿Ya estás registrado? <asp:HyperLink ID="LinkAcceso" runat="server" NavigateUrl="~/Pages/Usuarios/Acceso.aspx">Inicia Sesión</asp:HyperLink>
            </p>
            <p>
                &nbsp;</p>
            <p>&nbsp;</p>
        </main>
    </form>
</body>
</html>
