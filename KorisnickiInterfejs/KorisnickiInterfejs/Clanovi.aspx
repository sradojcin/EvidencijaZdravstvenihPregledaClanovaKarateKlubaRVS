<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Clanovi.aspx.cs" Inherits="KorisnickiInterfejs.Clanovi" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Članovi</title>
    <style>
        body {
            font-family: Arial;
            background-color: #f4f6f8;
        }

        .container {
            width: 1200px;
            margin: 30px auto;
            background-color: white;
            padding: 25px;
            border-radius: 10px;
            box-shadow: 0 0 10px #cccccc;
        }

        .title {
            font-size: 28px;
            font-weight: bold;
            text-align: center;
            margin-bottom: 25px;
        }

        .left-panel {
            width: 32%;
            float: left;
        }

        .right-panel {
            width: 65%;
            float: right;
        }

        .section {
            border: 1px solid #d0d0d0;
            border-radius: 8px;
            padding: 20px;
            margin-bottom: 20px;
        }

        .section-title {
            font-size: 20px;
            font-weight: bold;
            margin-bottom: 15px;
            color: #2c5e91;
        }

        .label {
            display: block;
            font-weight: bold;
            margin-top: 10px;
            margin-bottom: 5px;
        }

        .input {
            width: 100%;
            padding: 10px;
            font-size: 14px;
        }

        .buttons {
            margin-top: 20px;
        }

        .btn {
            padding: 10px 16px;
            margin-right: 8px;
            margin-top: 8px;
            font-weight: bold;
            border: none;
            cursor: pointer;
            border-radius: 5px;
        }

        .btn-primary {
            background-color: #2c5e91;
            color: white;
        }

        .btn-warning {
            background-color: #c98b2b;
            color: white;
        }

        .btn-danger {
            background-color: #a33d3d;
            color: white;
        }

        .btn-secondary {
            background-color: #777;
            color: white;
        }

        .status {
            display: block;
            margin-top: 15px;
            font-weight: bold;
            color: darkred;
        }

        .grid {
            width: 100%;
            margin-top: 10px;
        }

        .clearfix::after {
            content: "";
            display: block;
            clear: both;
        }

        .top-links {
            text-align: right;
            margin-bottom: 15px;
        }

        .top-links a {
            margin-left: 15px;
            text-decoration: none;
            font-weight: bold;
            color: #2c5e91;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="top-links">
                <a href="Pocetna.aspx">Početna</a>
                <a href="Pregledi.aspx">Pregledi</a>
            </div>

            <div class="title">EVIDENCIJA ČLANOVA</div>

            <div class="clearfix">
                <div class="left-panel">
                    <div class="section">
                        <div class="section-title">Podaci o članu</div>

                        <asp:HiddenField ID="hfClanID" runat="server" />

                        <label class="label">JMBG</label>
                        <asp:TextBox ID="txtJMBG" runat="server" CssClass="input"></asp:TextBox>

                        <label class="label">Ime</label>
                        <asp:TextBox ID="txtIme" runat="server" CssClass="input"></asp:TextBox>

                        <label class="label">Prezime</label>
                        <asp:TextBox ID="txtPrezime" runat="server" CssClass="input"></asp:TextBox>

                        <label class="label">Datum rođenja</label>
                        <asp:TextBox ID="txtDatumRodjenja" runat="server" CssClass="input" TextMode="Date"></asp:TextBox>

                        <label class="label">Kategorija</label>
                        <asp:TextBox ID="txtKategorija" runat="server" CssClass="input" ReadOnly="true"></asp:TextBox>

                        <div class="buttons">
                            <asp:Button ID="btnDodaj" runat="server" Text="Dodaj" CssClass="btn btn-primary" OnClick="btnDodaj_Click" />
                            <asp:Button ID="btnIzmeni" runat="server" Text="Izmeni" CssClass="btn btn-warning" OnClick="btnIzmeni_Click" />
                            <asp:Button ID="btnObrisi" runat="server" Text="Obriši" CssClass="btn btn-danger" OnClick="btnObrisi_Click" />
                            <asp:Button ID="btnOcisti" runat="server" Text="Očisti" CssClass="btn btn-secondary" OnClick="btnOcisti_Click" />
                        </div>

                        <asp:Label ID="lblStatus" runat="server" CssClass="status"></asp:Label>
                    </div>
                </div>

                <div class="right-panel">
                    <div class="section">
                        <div class="section-title">Spisak članova</div>

                        <asp:GridView ID="gvClanovi" runat="server" AutoGenerateColumns="False" CssClass="grid"
                            DataKeyNames="ClanID" OnSelectedIndexChanged="gvClanovi_SelectedIndexChanged">
                            <Columns>
                                <asp:BoundField DataField="ClanID" HeaderText="ID" />
                                <asp:BoundField DataField="JMBG" HeaderText="JMBG" />
                                <asp:BoundField DataField="Ime" HeaderText="Ime" />
                                <asp:BoundField DataField="Prezime" HeaderText="Prezime" />
                                <asp:BoundField DataField="DatumRodjenja" HeaderText="Datum rođenja" />
                                <asp:BoundField DataField="Kategorija" HeaderText="Kategorija" />
                                <asp:CommandField ShowSelectButton="True" SelectText="Izaberi" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
