<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Consultar.aspx.cs" Inherits="WMSR.Consultar" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Consultar</title>
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

        .button {
            margin-left: 10px;
        }

        .button input, .button asp\:button {
            background-color: #9acd32;
            border: 1px solid black;
            padding: 6px 12px;
            font-weight: bold;
            cursor: pointer;
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

        .msg-warn {
        color: orange;
        font-weight: bold;
        }

        .msg-err {
        color: red;
        font-weight: bold;
        }

    </style>
</head>
<body>
    <form id="form1" runat="server" defaultbutton="btnConsultar">
        <div class="container">
            <div class="header">MODULO DE CONSULTA</div>


        <asp:Button ID="btnInicio" runat="server" Text="Inicio" CssClass="btn-inicio" OnClick="btnInicio_Click" />


            <div class="form-row">
                <label for="txtLote">Lote:</label>
                <asp:TextBox ID="txtLote" runat="server" CssClass="asp-textbox" />
            &nbsp;<asp:TextBox ID="txtReopera" runat="server" CssClass="asp-textboxR" Width="28px" >0</asp:TextBox>
            </div>

            <div class="form-row">
                <label for="txtPiezas">Piezas:</label>
                <asp:TextBox ID="txtPiezas" runat="server" CssClass="asp-textbox" />
            </div>

            <div class="form-row">
                <label for="txtAlbaran">Albaran:</label>
                <asp:TextBox ID="txtAlbaran" runat="server" CssClass="asp-textbox" />
                <div class="button">
                    <asp:Button ID="btnConsultar" runat="server" Text="Consultar" OnClick="btnConsultar_Click" />
                </div>
            </div>

            <div class="form-row">

                <asp:Label ID="lblTotalRollos" runat="server" ForeColor="Black"></asp:Label>
                <asp:Label ID="lblResultado" runat="server" CssClass="msg-ok"></asp:Label>

            </div>

            <div class="gridview">
                <asp:GridView ID="gvDatos" runat="server" AutoGenerateColumns="false" BorderWidth="1px" 
                              CellPadding="5" ForeColor="Black" GridLines="Both" Width="100%">
                    <Columns>
                        <%-- sp_ConsultarAlbaran --%>
                        <asp:BoundField DataField="CliNom" HeaderText="Cliente" />
                        <asp:BoundField DataField="AlbRef" HeaderText="Referencia" />
                        <asp:BoundField DataField="zona" HeaderText="Zona" />
                        <asp:BoundField DataField="celda" HeaderText="Celda" />
                        <asp:BoundField DataField="piezas_en_celda" HeaderText="Piezas en Celda" />

                        <%-- sp_ConsultarLote --%>
                        <asp:BoundField DataField="CliNom" HeaderText="Cliente" />
                        <asp:BoundField DataField="BarColNom" HeaderText="Color" />
                        <asp:BoundField DataField="AlbRef" HeaderText="Referencia" />
                        <asp:BoundField DataField="zona" HeaderText="Zona" />
                        <asp:BoundField DataField="celda" HeaderText="Celda" />
                        <asp:BoundField DataField="piezas_en_celda" HeaderText="Piezas en Celda" />

                        <%-- sp_ConsultarPiezas --%>
                        <asp:BoundField DataField="numero_pieza" HeaderText="N° Pieza" />
                        <asp:BoundField DataField="albaran" HeaderText="N° Albaran" />
                        <asp:BoundField DataField="zona" HeaderText="Zona" />
                        <asp:BoundField DataField="celda" HeaderText="Celda" />

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