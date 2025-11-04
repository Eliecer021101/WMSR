<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registro.aspx.cs" Inherits="WMSR.Registro" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Registro de Usuario</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            background: #f0f2f5;
            margin: 0;
            height: 100vh;
            display: flex;
            justify-content: center;
            align-items: center;
        }

        form#formRegistro {
            position: relative;
            width: 100%;
            max-width: 320px;
        }

        .btn-inicio {
            position: fixed; /* fijo en la ventana */
            top: 15px;
            right: 15px;
            z-index: 1000;
            background-color: #6c757d;
            color: white;
            border: none;
            padding: 8px 14px;
            font-size: 13px;
            border-radius: 4px;
            cursor: pointer;
        }

        .btn-inicio:hover {
            background-color: #5a6268;
        }

        .formulario {
            background: white;
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0 6px 12px rgba(0, 0, 0, 0.08);
            font-size: 13px;
        }

        .formulario h2 {
            text-align: center;
            margin-bottom: 15px;
            font-size: 18px;
            color: #333;
        }

        .formulario .campo {
            margin-bottom: 12px;
        }

        .formulario label {
            display: block;
            margin-bottom: 4px;
            color: #444;
        }

        .asp-control,
        .formulario select {
            width: 100% !important;
            padding: 8px;
            border: 1px solid #ccc;
            border-radius: 5px;
            font-size: 13px;
            box-sizing: border-box;
        }

        .formulario .btn {
            width: 100%;
            padding: 10px;
            background-color: #007bff;
            border: none;
            border-radius: 5px;
            color: white;
            font-size: 14px;
            cursor: pointer;
        }

        .formulario .btn:hover {
            background-color: #0056b3;
        }

        .formulario .resultado {
            text-align: center;
            margin-top: 10px;
            color: green;
            font-size: 13px;
        }
    </style>
</head>
<body>
    <form id="formRegistro" runat="server">

        <%-- Botón inicio: dentro del formulario pero posicionado fijo en la esquina --%>
        <asp:Button ID="btnInicio" runat="server" Text="Inicio" CssClass="btn-inicio" OnClick="btnInicio_Click" />

        <div class="formulario">
            <h2>Registro de Usuario</h2>

            <div class="campo">
                <asp:Label ID="lblCedula" runat="server" AssociatedControlID="txtCedula" Text="Cédula:" />
                <asp:TextBox ID="txtCedula" runat="server" CssClass="asp-control" />
            </div>

            <div class="campo">
                <asp:Label ID="lblNombre" runat="server" AssociatedControlID="txtNombre" Text="Nombre:" />
                <asp:TextBox ID="txtNombre" runat="server" CssClass="asp-control" />
            </div>

            <div class="campo">
                <asp:Label ID="lblApellido" runat="server" AssociatedControlID="txtApellido" Text="Apellido:" />
                <asp:TextBox ID="txtApellido" runat="server" CssClass="asp-control" />
            </div>

            <div class="campo">
                <asp:Label ID="lblCargo" runat="server" AssociatedControlID="txtCargo" Text="Cargo:" />
                <asp:TextBox ID="txtCargo" runat="server" CssClass="asp-control" />
            </div>

            <div class="campo">
                <asp:Label ID="lblUsuario" runat="server" AssociatedControlID="txtUsuario" Text="Usuario:" />
                <asp:TextBox ID="txtUsuario" runat="server" CssClass="asp-control" />
            </div>

            <div class="campo">
                <asp:Label ID="lblClave" runat="server" AssociatedControlID="txtClave" Text="Clave:" />
                <asp:TextBox ID="txtClave" runat="server" TextMode="Password" CssClass="asp-control" />
            </div>

            <div class="campo">
                <asp:Label ID="lblPerfil" runat="server" AssociatedControlID="ddlPerfil" Text="Perfil:" />
                <asp:DropDownList ID="ddlPerfil" runat="server" CssClass="asp-control">
                    <asp:ListItem Value="1" Text="Administrador" />
                    <asp:ListItem Value="2" Text="Usuario" />
                </asp:DropDownList>
            </div>

            <div class="campo">
                <asp:Button ID="btnRegistrar" runat="server" Text="Registrar" CssClass="btn" OnClick="btnRegistrar_Click" />
            </div>

            <div class="resultado">
                <asp:Label ID="lblResultado" runat="server" />
            </div>
        </div>

    </form>
</body>
</html>
