<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8" />
    <title>BCIT Searchstar</title>
    <link rel="stylesheet" type="text/css" href="css/style.css" />
    <script type="text/javascript">

        function printTextBox() {
            var printContent = document.getElementById('<%= tbResult.ClientID %>').value;

            if (printContent) {
                var windowUrl = 'about:blank';
                var windowName = 'Print' + new Date().getTime();
                var printWindow = window.open(windowUrl, windowName, 'left=50000,top=50000,width=0,height=0');
            
                var rex = /\n/g;
                printContent = printContent.replace(rex, "<br /><br />");

                printWindow.document.write(printContent);
                printWindow.document.close();
                printWindow.focus();
                printWindow.print();
                printWindow.close();
            }
            else {
                window.alert("File not Found. Unable to print.");
            }
        }

    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div id="main" role="main">
    
        <div id="header">
            <asp:Image ID="Logo" AlternateText="BCIT Searchstar" ImageUrl="image/logo.png" runat="server" />
        </div>
        <div id="searchInput">
            <asp:TextBox ID="tbSearch" runat="server"></asp:TextBox>
            <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click"/>
            <asp:ImageButton runat="server" AlternateText="First" CssClass="auto-style1" ImageUrl="image/Actions-go-first-view-icon.png" OnClick="goFirst_Click" ID="btn_first" />
            <asp:ImageButton runat="server" AlternateText="Previous" CssClass="auto-style1" ImageUrl="image/Actions-arrow-left-icon.png" OnClick="previous_click" ID="btn_previous" />
            <asp:ImageButton runat="server" AlternateText="Next" CssClass="auto-style1" ImageUrl="image/Actions-arrow-right-icon.png" OnClick="next_click" ID="btn_next"/>
            <asp:ImageButton runat="server" AlternateText="Last" CssClass="auto-style1" ImageUrl="image/Actions-go-last-view-icon.png" OnClick ="goLast_Click" ID="btn_last"/>

        </div>

        <div id="searchResult">
            <div id="resultinfo">
                <asp:TextBox ID="tbResultFileName" runat="server" Width="20%"></asp:TextBox>
                <asp:TextBox ID="tbResultFileNumber" runat="server" Width="20%"></asp:TextBox>
                
            </div>
            <div id="resultdisplay">
                <div id="resulttextbox">
                    <asp:TextBox ID="tbResult" TextMode="multiline" style="resize:none" runat="server" rows="25" Width="100%"></asp:TextBox>
                </div>
                <div id="resultprintsave">
                    <asp:ImageButton runat="server" AlternateText="Print" CssClass="auto-style1" ImageUrl="image/print.png" ID="btnPrint" OnClientClick="printTextBox();" />
                    <asp:ImageButton runat="server" AlternateText="Save" CssClass="auto-style1" ImageUrl="image/Save_black-128.png" ID="btnSave" OnClick="btnSave_Click"  />
                </div>
            </div>

        </div>

    </div>

    </form>
</body>
</html>
