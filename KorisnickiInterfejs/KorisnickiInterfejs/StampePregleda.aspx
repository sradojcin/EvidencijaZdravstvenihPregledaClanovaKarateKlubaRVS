<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StampePregleda.aspx.cs" Inherits="KorisnickiInterfejs.StampePregleda" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <title>Štampe pregleda</title>
    <style>
        @page {
            size: A4 portrait;
            margin: 14mm;
        }

        body {
            font-family: Arial;
            background-color: #f4f6f8;
            color: #222;
            margin: 0;
        }

        .container {
            width: 1100px;
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

        .report-title {
            font-size: 24px;
            font-weight: bold;
            text-align: center;
            margin-bottom: 8px;
            text-transform: uppercase;
            letter-spacing: 0.5px;
        }

        .report-subtitle {
            text-align: center;
            color: #444;
            margin-bottom: 20px;
        }

        .report-meta {
            display: flex;
            justify-content: space-between;
            gap: 20px;
            margin-bottom: 18px;
            padding: 12px 14px;
            background-color: #f7f9fb;
            border: 1px solid #dfe6ee;
            border-radius: 6px;
        }

        .report-meta-item {
            flex: 1;
        }

        .report-meta-label {
            display: block;
            font-size: 12px;
            font-weight: bold;
            color: #5d6b79;
            text-transform: uppercase;
            margin-bottom: 4px;
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
            width: 280px;
            padding: 8px;
            margin-right: 10px;
        }

        .actions {
            margin-top: 10px;
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

        .btn-secondary {
            background-color: #777;
            color: white;
        }

        .btn-print {
            background-color: #1f7a3e;
            color: white;
        }

        .grid {
            width: 100%;
            margin-top: 15px;
            border-collapse: collapse;
            font-size: 13px;
        }

        .grid th,
        .grid td {
            border: 1px solid #cfd7df;
            padding: 8px 10px;
            text-align: left;
            vertical-align: top;
        }

        .grid th {
            background-color: #e9f0f7;
            color: #1e4468;
            font-weight: bold;
        }

        .status {
            display: block;
            margin-top: 15px;
            font-weight: bold;
            color: darkred;
        }

        .print-note {
            margin-top: 15px;
            color: #5d6b79;
            font-size: 12px;
        }

        .print-only {
            display: none;
        }

        @media print {
            body {
                background: white;
                color: black;
            }

            .container {
                width: auto;
                margin: 0;
                padding: 0;
                box-shadow: none;
                border-radius: 0;
            }

            .no-print {
                display: none !important;
            }

            .print-only {
                display: block;
            }

            .section {
                border: none;
                padding: 0;
                margin: 0;
            }

            .report-meta {
                background: white;
                border: 1px solid #cfd7df;
            }

            .grid {
                margin-top: 12px;
            }

            .grid thead {
                display: table-header-group;
            }

            .grid tr {
                page-break-inside: avoid;
            }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="top-links no-print">
                <a href="Pocetna.aspx">Početna</a>
                <a href="Pregledi.aspx">Pregledi</a>
            </div>

            <div class="title no-print">ŠTAMPE PREGLEDA</div>

            <div class="section">
                <div class="section-title no-print">Filter pregleda</div>

                <div class="filter-row no-print">
                    <asp:Button ID="btnPrikaziSve" runat="server" Text="Svi pregledi"
                        CssClass="btn btn-primary" OnClick="btnPrikaziSve_Click" />
                    <span class="filter-label">Član:</span>
                    <asp:DropDownList ID="ddlClanovi" runat="server" CssClass="filter-input"></asp:DropDownList>
                    <asp:Button ID="btnFiltrirajClana" runat="server" Text="Pregledi člana"
                        CssClass="btn btn-secondary" OnClick="btnFiltrirajClana_Click" />
                </div>

                <div class="actions no-print">
                    <asp:Button ID="btnStampaj" runat="server" Text="Štampaj / Sačuvaj kao PDF"
                        CssClass="btn btn-print" UseSubmitBehavior="false"
                        OnClientClick="window.print(); return false;" />
                </div>

                <div class="report-title">Izveštaj o zdravstvenim pregledima</div>
                <div class="report-subtitle">Tabelarni prikaz spreman za štampu ili čuvanje u PDF formatu</div>

                <div class="report-meta">
                    <div class="report-meta-item">
                        <span class="report-meta-label">Tip izveštaja</span>
                        <asp:Label ID="lblNaslovPrikaza" runat="server"></asp:Label>
                    </div>
                    <div class="report-meta-item">
                        <span class="report-meta-label">Parametar</span>
                        <asp:Label ID="lblParametarPrikaza" runat="server"></asp:Label>
                    </div>
                    <div class="report-meta-item">
                        <span class="report-meta-label">Datum generisanja</span>
                        <asp:Label ID="lblDatumGenerisanja" runat="server"></asp:Label>
                    </div>
                </div>

                <asp:GridView ID="gvStampaPregleda" runat="server" AutoGenerateColumns="False" CssClass="grid">
                    <Columns>
                        <asp:BoundField DataField="ZdravstveniPregledID" HeaderText="ID pregleda" />
                        <asp:BoundField DataField="ClanID" HeaderText="ID člana" />
                        <asp:BoundField DataField="Clan" HeaderText="Član" />
                        <asp:BoundField DataField="DatumPregleda" HeaderText="Datum pregleda" />
                        <asp:BoundField DataField="Napomena" HeaderText="Napomena" />
                    </Columns>
                </asp:GridView>

                <asp:Label ID="lblStatus" runat="server" CssClass="status"></asp:Label>
                <div class="print-note no-print">
                    Koristi dugme za štampu, a zatim u prozoru pregledača izaberi opciju <strong>Save as PDF</strong> ako želiš PDF dokument.
                </div>
            </div>
        </div>
    </form>
</body>
</html>
