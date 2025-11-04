<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Operaciones.aspx.cs" Inherits="WMSR.Operaciones" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
<title>Operaciones</title>

<!-- Iconos Font Awesome -->
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" />

<style>
    body, html {
        height: 100%;
        margin: 0;
        font-family: Arial, sans-serif;
        background: #f0f2f5;
        display: flex;
        justify-content: center;
        align-items: center;
    }

    #formContainer {
        background: white;
        padding: 30px 40px 40px 40px;
        border-radius: 10px;
        box-shadow: 0 4px 15px rgba(0,0,0,0.1);
        width: 320px;
        box-sizing: border-box;
        text-align: center;
    }

    /* Logo */
    #logo {
        width: 250px;
        margin-bottom: 20px;
        display: inline-block;
    }


    .input-icon {
        position: relative;
        margin-bottom: 15px;
        width: 250px;
        margin-left: auto;
        margin-right: auto;
        text-align: left;
    }

    .input-icon i {
        position: absolute;
        left: 12px;
        top: 50%;
        transform: translateY(-50%);
        color: #999;
        font-size: 16px;
        pointer-events: none;
    }

    .input-with-icon {
        padding-left: 35px !important;
        width: 100%;
        height: 30px;
        box-sizing: border-box;
        border: 1px solid #ccc;
        border-radius: 5px;
        font-size: 14px;
        transition: border-color 0.3s ease;
    }

    .input-with-icon:focus {
        border-color: #007bff;
        outline: none;
    }


    .btn {
        width: 250px;
        background-color: #007bff;
        color: white;
        font-weight: bold;
        padding: 12px 0;
        border: none;
        border-radius: 5px;
        cursor: pointer;
        font-size: 16px;
        transition: background-color 0.3s ease;
        margin-top: 10px;
    }



    </style>
</head>
<body>
    <form id="formLogin" runat="server">
        <div id="formContainer">

            <!-- Logo aquí -->
            <!-- Logo local -->
            <img id="logo" src="Content/Images/logo.png" alt="Logo" />


            <asp:Button ID="btnUbicar" runat="server" Text="Ubicar" CssClass="btn" OnClick="btnUbicar_Click" />

            <br />

            <asp:Button ID="btnConsultar" runat="server" Text="Consultar" CssClass="btn" OnClick="btnConsultar_Click" />

            <br />

            <asp:Button ID="btnMovimientos" runat="server" Text="Movimientos" CssClass="btn" OnClick="btnMovimientos_Click" />

            <br />

            <asp:Button ID="btnAdministrar" runat="server" Text="Administrar" CssClass="btn" OnClick="btnAdministrar_Click" />

        </div>
    </form>
</body>
</html>

