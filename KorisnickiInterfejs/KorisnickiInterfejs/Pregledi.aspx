<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Pregledi.aspx.cs" Inherits="KorisnickiInterfejs.Pregledi" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Pregledi i statusi</title>
    <style>
        body {
            font-family: Arial;
            background-color: #f4f6f8;
        }

        .container {
            width: 1250px;
            margin: 30px auto;
            background-color: white;
            padding: 25px;
            border-radius: 10px;
            box-shadow: 0px 0px 10px #cccccc;
        }

        .title {
            font-size: 28px;
            font-weight: bold;
            text-align: center;
            margin-bottom: 20px;
        }

        .section {
            border: 1px solid #d0d0d0;
            border-radius: 8px;
            padding: 20px;
            margin-bottom: 25px;
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

        .grid {
            width: 100%;
            margin-top: 15px;
        }

        .status {
            display: block;
            margin-top: 15px;
            font-weight: bold;
            color: darkred;
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

        .filter-row {
            margin-bottom: 15px;
        }

        .filter-label {
            font-weight: bold;
            margin-right: 10px;
        }

        .filter-input {
            width: 250px;
            padding: 8px;
            margin-right: 10px;
        }

        .clearfix::after {
            content: "";
            display: block;
            clear: both;
        }

        .left-panel {
            width: 32%;
            float: left;
        }

        .right-panel {
            width: 65%;
            float: right;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="top-links">
                <a href="Pocetna.aspx">Početna</a>
                <a href="Clanovi.aspx">Članovi</a>
            </div>

            <div class="title">PREGLEDI I STATUSI ZDRAVSTVENIH PREGLEDA</div>

            <div class="section">
                <div class="section-title">Unos pregleda</div>

                <div class="clearfix">
                    <div class="left-panel">
                        <asp:HiddenField ID="hfZdravstveniPregledID" runat="server" />

                        <label class="label">Član</label>
                        <asp:DropDownList ID="ddlClanovi" runat="server" CssClass="input"></asp:DropDownList>

                        <label class="label">Datum pregleda</label>
                        <asp:TextBox ID="txtDatumPregleda" runat="server" CssClass="input" TextMode="Date"></asp:TextBox>

                        <label class="label">Napomena</label>
                        <asp:TextBox ID="txtNapomena" runat="server" CssClass="input"></asp:TextBox>

                        <div class="buttons">
                            <asp:Button ID="btnDodajPregled" runat="server" Text="Unesi pregled" CssClass="btn btn-primary" OnClick="btnDodajPregled_Click" />
                            <asp:Button ID="btnIzmeniPregled" runat="server" Text="Izmeni pregled" CssClass="btn btn-warning" OnClick="btnIzmeniPregled_Click" />
                            <asp:Button ID="btnObrisiPregled" runat="server" Text="Obriši pregled" CssClass="btn btn-danger" OnClick="btnObrisiPregled_Click" />
                            <asp:Button ID="btnOcistiPregled" runat="server" Text="Očisti" CssClass="btn btn-secondary" OnClick="btnOcistiPregled_Click" />
                        </div>

                        <asp:Label ID="lblStatusPregled" runat="server" CssClass="status"></asp:Label>
                    </div>

                    <div class="right-panel">
                        <div class="section-title">Lista svih pregleda</div>

                        <asp:GridView ID="gvSviPregledi" runat="server" AutoGenerateColumns="False"
                            CssClass="grid" DataKeyNames="ZdravstveniPregledID"
                            OnSelectedIndexChanged="gvSviPregledi_SelectedIndexChanged">
                            <Columns>
                                <asp:BoundField DataField="ZdravstveniPregledID" HeaderText="ID pregleda" />
                                <asp:BoundField DataField="ClanID" HeaderText="ID člana" />
                                <asp:BoundField DataField="Clan" HeaderText="Član" />
                                <asp:BoundField DataField="DatumPregleda" HeaderText="Datum pregleda" />
                                <asp:BoundField DataField="Napomena" HeaderText="Napomena" />
                                <asp:CommandField ShowSelectButton="True" SelectText="Izaberi" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>

            <div class="section">
                <div class="section-title">Pregled statusa članova</div>

                <div class="filter-row">
                    <span class="filter-label">Filter statusa:</span>
                    <asp:DropDownList ID="ddlFilterStatus" runat="server" CssClass="filter-input"></asp:DropDownList>
                    <asp:Button ID="btnPrikazi" runat="server" Text="Prikaži" CssClass="btn btn-primary" OnClick="btnPrikazi_Click" />
                </div>

                <asp:GridView ID="gvStatusi" runat="server" AutoGenerateColumns="False" CssClass="grid">
                    <Columns>
                        <asp:BoundField DataField="ClanID" HeaderText="ID" />
                        <asp:BoundField DataField="JMBG" HeaderText="JMBG" />
                        <asp:BoundField DataField="Ime" HeaderText="Ime" />
                        <asp:BoundField DataField="Prezime" HeaderText="Prezime" />
                        <asp:BoundField DataField="DatumRodjenja" HeaderText="Datum rođenja" />
                        <asp:BoundField DataField="Kategorija" HeaderText="Kategorija" />
                        <asp:BoundField DataField="DatumPregleda" HeaderText="Poslednji pregled" />
                        <asp:BoundField DataField="StatusPregleda" HeaderText="Status pregleda" />
                    </Columns>
                </asp:GridView>
            </div>
            <div class="section">
                <div class="section-title">Štampe Pregleda</div>

                <div class="filter-row">
                    <asp:Button ID="btnStampaSvihPregleda" runat="server" Text="Prikaži sve preglede"
                        CssClass="btn btn-primary" OnClick="btnStampaSvihPregleda_Click" />
                    <span class="filter-label">Član:</span>
                    <asp:DropDownList ID="ddlClanZaStampu" runat="server" CssClass="filter-input"></asp:DropDownList>
                    <asp:Button ID="btnStampaPregledaClana" runat="server" Text="Prikaži preglede člana"
                        CssClass="btn btn-secondary" OnClick="btnStampaPregledaClana_Click" />
                </div>
            </div>
        </div>
    </form>
</body>
</html>
