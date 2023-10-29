<%@ Page 
 Title="Barkochba"
 Language="C#"
 Debug="true"
 CodeBehind="MainWebForm.aspx.cs"
 Inherits="BARKOCHBA.WebForm"
%>

<!DOCTYPE html>
<html>
<head>
 <title>Barkochba</title>
 <link rel="stylesheet" href="StyleSheet.css">
</head>
<body>
 <header>
 </header>
 <main>
  <form runat="server">
   <asp:Panel                    ID="panelTitle"         runat="server" CssClass="title-panel" >
    <asp:Button Text="BARKOCHBA" ID="btnTitle"           runat="server" CssClass="two-buttons-row btn-Title" OnClick="btnTitle_Click" Width="808px" ></asp:Button>
   </asp:Panel>
   <asp:TextBox                  ID="current_question"   runat="server" CssClass="question"                  ReadOnly="true" Rows="4" TextMode="MultiLine" ></asp:TextBox>
   <asp:Panel                    ID="panelY_IDK_N"       runat="server" CssClass="button-panel" >
    <asp:Button Text="Yes"       ID="btnY"               runat="server" CssClass="three-buttons-row btn-Y"   OnClick="btnY_Click" >    </asp:Button>
    <asp:Button Text="IDK"       ID="btnIDK"             runat="server" CssClass="three-buttons-row btn-IDK" OnClick="btnIDK_Click" >  </asp:Button>
    <asp:Button Text="No"        ID="btnN"               runat="server" CssClass="three-buttons-row btn-N"   OnClick="btnN_Click" >    </asp:Button>
   </asp:Panel>
   <asp:Panel                    ID="paneltMY_MN"        runat="server" CssClass="button-panel" >
    <asp:Button Text="Maybe Yes" ID="btnMY"              runat="server" CssClass="two-buttons-row btn-MY"    OnClick="btnMY_Click" >   </asp:Button>
    <asp:Button Text="Maybe No"  ID="btnMN"              runat="server" CssClass="two-buttons-row btn-MN"    OnClick="btnMN_Click" >   </asp:Button>
   </asp:Panel>
   <asp:TextBox                  ID="candidate_solution" runat="server" CssClass="solution"                  ReadOnly="true"  Rows="1" TextMode="SingleLine" ></asp:TextBox>
  </form>
 </main>
</body>
</html>
