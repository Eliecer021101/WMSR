<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Prefijar.aspx.cs" Inherits="WMSR.Prefijar" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Prefijar</title>
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

        .button_1 {
            background-color: #F0E68C;
            border: 1px solid black;
            padding: 6px 12px;
            font-weight: bold;
            cursor: pointer;
        }

        .button_2 {
            background-color: #87CEFA;
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
    <form id="form1" runat="server">
        <div class="container">
            <div class="header">MODULO DE MOVIMIENTO PREFIJAR</div>

        <asp:Button ID="btnInicio" runat="server" Text="Inicio" CssClass="btn-inicio" OnClick="btnInicio_Click" />

            <div class="form-row">
                <label for="txtAlbaran">Albaran:</label>
                <asp:TextBox ID="txtAlbaran" runat="server" CssClass="asp-textbox" />
            </div>


            <div class="form-row">

                <label for="txtLote">
                Lote:
                <asp:TextBox ID="txtLote" runat="server" CssClass="asp-textbox" />
            &nbsp;<asp:TextBox ID="txtReopera" runat="server" CssClass="asp-textboxR" Width="42px" >0</asp:TextBox>


                </label>
                <br />
                </div>

                <div>
                    <asp:Button ID="btnPrefijarAlbaran" runat="server" CssClass="button_1" Text="Prefijar Albaran" OnClick="btnPrefijarAlbaran_Click" />
                &nbsp;&nbsp;
                    <asp:Button ID="btnPrefijarLote" runat="server" CssClass="button_2" Text="Prefijar Lote" OnClick="btnPrefijarLote_Click" />
                    <br />
                </div>
        

            <div class="form-row">
                <asp:Label ID="lblResultado" runat="server" ForeColor="Black"></asp:Label>
                <asp:Label ID="lblTotalRollos" runat="server" ForeColor="Black"></asp:Label>
            </div>



            <div class="footer">
                # ESTA PÁGINA ESTÁ EN DESARROLLO #

            </div>
        </div>
 
    </form>
</body>
</html>
