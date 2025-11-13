<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Ubicar.aspx.cs" Inherits="WMSR.Ubicar" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Ubicar</title>
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
            width: 3ch;
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
    </style>
</head>
<body>
    <form id="form1" runat="server" defaultbutton="btnUbicar">
        <div class="container">
            <div class="header">MODULO DE UBICAR</div>

        <asp:Button ID="btnInicio" runat="server" Text="Inicio" CssClass="btn-inicio" OnClick="btnInicio_Click" />

            <div class="form-row">
                <label for="txtAlbaran">Albaran:</label>
                <asp:TextBox ID="txtAlbaran" runat="server" CssClass="asp-textbox" />
                <asp:DropDownList ID="listZonas" runat="server">
                <asp:ListItem Text="Con Programa" Value="1" />
                <asp:ListItem Text="Con Programa-Prefija" Value="4" />
                <asp:ListItem Text="Sin Programa" Value="2" />
                <asp:ListItem Text="Sin Programa-Prefija" Value="4" />
                <asp:ListItem Text="Complementos" Value="3" />
                <asp:ListItem Text="Devoluciones" Value="6" />
                </asp:DropDownList>

                <div class="button">
                    <asp:Button ID="btnUbicar" runat="server" Text="Ubicar" OnClick="btnUbicar_Click" />
                    <asp:Button ID="btnConfirmarImpresion" runat="server" Text="Confirmar e Imprimir" OnClick="btnConfirmarImpresion_Click" Width="210px" />
                </div>
            </div>

                <!-- Selección de tipo de empaque -->
                <asp:Label ID="lblTipoEmpaque" runat="server" Text="Tipo de Empaque:" CssClass="lbl-titulo" Font-Bold="True"></asp:Label>
                <br />

                <asp:RadioButton ID="rbRollo" runat="server" Text="Rollo"
                    GroupName="Empaque" CssClass="rb-empaque" />
                <asp:RadioButton ID="rbTubular" runat="server" Text="Tubular"
                    GroupName="Empaque" CssClass="rb-empaque" />
                <asp:RadioButton ID="rbTalego" runat="server" Text="Talego"
                    GroupName="Empaque" CssClass="rb-empaque" />

                <br />

            <div class="form-row">
                <asp:Label ID="lblResultado" runat="server" ForeColor="Black"></asp:Label>
                <asp:Label ID="lblTotalRollos" runat="server" ForeColor="Black"></asp:Label>
            </div>

            <%-- GridView para mostrar etiquetas generadas por el SP --%>

        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" BorderWidth="1px" 
        CellPadding="5" ForeColor="Black" GridLines="Both" Width="100%" 
        OnRowCommand="gvPrevisualizacion_RowCommand">
        <Columns>
        <asp:BoundField DataField="numero_pieza" HeaderText="Nº Rollo" />
        <asp:BoundField DataField="AlbRef" HeaderText="Referencia" />
        <asp:BoundField DataField="CliNom" HeaderText="Cliente" />
        <asp:BoundField DataField="zona" HeaderText="Zona" />
        <asp:BoundField DataField="celda" HeaderText="Celda" />

        <%-- Botón por fila para imprimir el ZPL por pieza --%>
        <asp:TemplateField HeaderText="Acciones">
            <ItemTemplate>
                <asp:Button ID="btnImprimirFila" runat="server" Text="Imprimir" 
                            CommandName="ImprimirFila" 
                            CommandArgument='<%# Container.DataItemIndex %>' 
                            CssClass="btn btn-primary btn-sm" />
            </ItemTemplate>
        </asp:TemplateField>
        </Columns>
        </asp:GridView>

            <%-- Botón de confirmación que manda las etiquetas a la Zebra --%>
            <div class="button" style="margin-top:15px;">
            </div>

            <div class="footer">
                # ESTA PÁGINA ESTÁ EN DESARROLLO #
            </div>
        </div>
    </form>
</body>
</html>
