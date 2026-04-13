<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Pocetna.aspx.cs" Inherits="KorisnickiInterfejs.Pocetna" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Početna</title>
    <style>
        body {
            font-family: Arial;
            background-color: #f4f6f8;
        }

        .container {
            width: 700px;
            margin: 80px auto;
            background-color: white;
            padding: 35px;
            border-radius: 10px;
            box-shadow: 0px 0px 10px #cccccc;
            text-align: center;
        }

        .title {
            font-size: 28px;
            font-weight: bold;
            margin-bottom: 15px;
        }

        .subtitle {
            font-size: 18px;
            margin-bottom: 30px;
        }

        .user-info {
            margin-bottom: 30px;
            font-size: 16px;
            font-weight: bold;
            color: #2c5e91;
        }

        .menu-button {
            display: inline-block;
            width: 260px;
            margin: 12px;
            padding: 16px;
            font-size: 18px;
            font-weight: bold;
            text-decoration: none;
            text-align: center;
            color: white;
            background-color: #2c5e91;
            border-radius: 6px;
        }

        .menu-button:hover {
            background-color: #1f4569;
        }

        .logout-button {
            margin-top: 30px;
            padding: 12px 30px;
            font-size: 16px;
            font-weight: bold;
            background-color: #a33d3d;
            color: white;
            border: none;
            cursor: pointer;
            border-radius: 6px;
        }

        .logout-button:hover {
            background-color: #7d2e2e;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="title">KARATE KLUB</div>
            <div class="subtitle">Sistem za evidenciju članova i zdravstvenih pregleda</div>

            <asp:Label ID="lblKorisnik" runat="server" CssClass="user-info"></asp:Label>

            <div>
                <a href="Clanovi.aspx" class="menu-button">Evidencija članova</a>
                <a href="Pregledi.aspx" class="menu-button">Pregledi i statusi</a>
            </div>

            <asp:Button ID="btnOdjava" runat="server" Text="ODJAVA" CssClass="logout-button" OnClick="btnOdjava_Click" />
        </div>
    </form>
</body>
</html>