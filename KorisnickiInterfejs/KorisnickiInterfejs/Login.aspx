<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="KorisnickiInterfejs.Login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Prijava</title>
    <style>
        body {
            font-family: Arial;
            background-color: #f4f6f8;
        }

        .login-container {
            width: 420px;
            margin: 100px auto;
            background-color: white;
            padding: 30px;
            border-radius: 10px;
            box-shadow: 0px 0px 10px #cccccc;
        }

        .title {
            text-align: center;
            font-size: 24px;
            font-weight: bold;
            margin-bottom: 10px;
        }

        .subtitle {
            text-align: center;
            font-size: 16px;
            margin-bottom: 30px;
        }

        .label {
            display: block;
            margin-top: 15px;
            margin-bottom: 5px;
            font-weight: bold;
        }

        .input {
            width: 100%;
            padding: 10px;
            font-size: 14px;
        }

        .button {
            margin-top: 25px;
            width: 100%;
            padding: 12px;
            font-size: 16px;
            font-weight: bold;
            background-color: #2c5e91;
            color: white;
            border: none;
            cursor: pointer;
        }

        .button:hover {
            background-color: #1f4569;
        }

        .status {
            margin-top: 20px;
            color: red;
            font-weight: bold;
            text-align: center;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="login-container">
            <div class="title">EVIDENCIJA ČLANOVA</div>
            <div class="subtitle">i zdravstvenih pregleda karate kluba</div>

            <label class="label">Korisničko ime</label>
            <asp:TextBox ID="txtKorisnickoIme" runat="server" CssClass="input"></asp:TextBox>

            <label class="label">Lozinka</label>
            <asp:TextBox ID="txtLozinka" runat="server" CssClass="input" TextMode="Password"></asp:TextBox>

            <asp:Button ID="btnPrijava" runat="server" Text="PRIJAVA" CssClass="button" OnClick="btnPrijava_Click" />

            <asp:Label ID="lblStatus" runat="server" CssClass="status"></asp:Label>
        </div>
    </form>
</body>
</html>