<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Conformar.aspx.cs" Inherits="WMSR.Conformar" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Conformar</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f5f5f5;
            color: #333;
            margin: 0;
            padding: 0;
        }

        .btn-inicio {
            position: fixed;
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

        .container {
            max-width: 800px;
            margin: 50px auto;
            padding: 20px;
            background-color: white;
            border-radius: 8px;
            box-shadow: 0px 0px 10px rgba(0,0,0,0.1);
        }

        .header {
            font-size: 28px;
            font-weight: bold;
            text-align: center;
            margin-bottom: 30px;
            color: #2c3e50;
        }

        .form-row {
            display: flex;
            justify-content: flex-start;
            align-items: center;
            gap: 5px;
            margin-bottom: 15px;
        }

        .form-row label {
            flex: 0 0 auto;
            text-align: left;
            font-weight: bold;
            margin: 0;
        }

        .asp-textbox {
            width: 10ch;
            padding: 5px;
            border: 1px solid #ccc;
            background-color: #eaeaea;
            border-radius: 4px;
            box-sizing: border-box;
        }

        .asp-textboxR {
            padding: 5px;
            border: 1px solid #ccc;
            background-color: #eaeaea;
            border-radius: 4px;
            box-sizing: border-box;
            text-align: center;
        }

        .button input, .button asp\:button {
            background-color: #9acd32;
            border: 1px solid black;
            padding: 6px 12px;
            font-weight: bold;
            cursor: pointer;
            border-radius: 4px;
        }

        .button input:hover {
            background-color: #0056b3;
        }

        .gridview {
            margin-top: 30px;
            text-align: center;
        }

        .footer {
            text-align: center;
            margin-top: 30px;
            font-style: italic;
            color: #888;
        }

        .msg-ok {
            color: green;
            font-weight: bold;
        }

        .msg-err {
            color: red;
            font-weight: bold;
        }

        .msg-warn {
            color: orange;
            font-weight: bold;
        }

    </style>
</head>
<body>
    <form id="form1" runat="server" defaultbutton="btnRegistrar">
        <div class="container">
            <div class="header">MODULO DE CONFORMAR</div>

            <asp:Button ID="btnInicio" runat="server" Text="Inicio" CssClass="btn-inicio" OnClick="btnInicio_Click" />

            <div class="form-row">
                <label for="txtLote">Lote:</label>
                <asp:TextBox ID="txtLote" runat="server" CssClass="asp-textbox" />
                &nbsp;
                <asp:TextBox ID="txtReopera" runat="server" CssClass="asp-textboxR" Width="28px">0</asp:TextBox>
            &nbsp;
                <div class="button">
                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />
                   </div>
            </div>

            <div class="form-row">
                <label for="txtPieza">Código de Pieza:</label>
                <asp:TextBox ID="txtPieza" runat="server" CssClass="asp-textbox" Width="150px" />
                <div class="button">
                    <asp:Button ID="btnRegistrar" runat="server" Text="Registrar" OnClick="btnRegistrar_Click" />
                </div>
            </div>

            <div class="form-row">
                <div class="button">
                    <asp:Button ID="btnConfirmar" runat="server" Text="Confirmar Lote" OnClick="btnConfirmar_Click" />
                </div>
                <div class="button">
                    <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" OnClick="btnLimpiar_Click" />
                </div>

            </div>

            <div class="form-row">
                <asp:Label ID="lblMensaje" runat="server" CssClass="msg-ok"></asp:Label>
                <br />
                
            </div>

            <div class="gridview">
                <asp:GridView ID="gvPendientes" runat="server" AutoGenerateColumns="false" BorderWidth="1px"
                             CellPadding="5" ForeColor="Black" GridLines="Both" Width="100%" OnRowDataBound="gvPendientes_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="numero_pieza" HeaderText="N° Rollo" />
                        <asp:BoundField DataField="Referencia" HeaderText="Referencia" />
                        <asp:BoundField DataField="Cliente" HeaderText="Cliente" />
                        <asp:BoundField DataField="Estado" HeaderText="Estado" />
                        <asp:BoundField DataField="fecha_lectura" HeaderText="Fecha Lectura" />
                    </Columns>
                </asp:GridView>
            </div>

            <div class="footer">
                # ESTA PÁGINA ESTÁ EN DESARROLLO #
            </div>
        </div>
    </form>
</body>
</html>
