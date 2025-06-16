<asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
<br />
<asp:GridView ID="gvUserActivity" runat="server" AutoGenerateColumns="false" 
    GridLines="Both" BorderStyle="Solid" Width="100%">
    <Columns>
        <asp:BoundField DataField="UserId" HeaderText="User ID" />
        <asp:BoundField DataField="Username" HeaderText="Username" />
        <asp:BoundField DataField="Email" HeaderText="Email" />
        <asp:BoundField DataField="PageName" HeaderText="Page Name" />
        <asp:BoundField DataField="EntryTime" HeaderText="Entry Time" DataFormatString="{0:G}" />
        <asp:BoundField DataField="ExitTime" HeaderText="Exit Time" DataFormatString="{0:G}" />
        <asp:BoundField DataField="TimeSpentMinutes" HeaderText="Time Spent (Minutes)" />
    </Columns>
</asp:GridView>
