VERSION 5.00
Object = "{831FDD16-0C5C-11D2-A9FC-0000F8754DA1}#2.0#0"; "mscomctl.ocx"
Object = "{F9043C88-F6F2-101A-A3C9-08002B2F49FB}#1.2#0"; "Comdlg32.ocx"
Begin VB.Form frmIndex 
   Caption         =   "SkyCD"
   ClientHeight    =   3195
   ClientLeft      =   165
   ClientTop       =   735
   ClientWidth     =   5910
   Icon            =   "frmIndex.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   3195
   ScaleWidth      =   5910
   StartUpPosition =   3  'Windows Default
   Begin SkyCD.SimpleCDDatabaseBrowser lstBrowser 
      Height          =   1725
      Left            =   120
      TabIndex        =   2
      Top             =   645
      Width           =   2070
      _ExtentX        =   3651
      _ExtentY        =   3043
   End
   Begin MSComDlg.CommonDialog cdDialog 
      Left            =   5130
      Top             =   750
      _ExtentX        =   847
      _ExtentY        =   847
      _Version        =   393216
   End
   Begin MSComctlLib.Toolbar tbrToolBar 
      Align           =   1  'Align Top
      Height          =   540
      Left            =   0
      TabIndex        =   0
      Top             =   0
      Width           =   5910
      _ExtentX        =   10425
      _ExtentY        =   953
      ButtonWidth     =   1138
      ButtonHeight    =   953
      AllowCustomize  =   0   'False
      Wrappable       =   0   'False
      Style           =   1
      ImageList       =   "imgToolBar"
      _Version        =   393216
      BeginProperty Buttons {66833FE8-8583-11D1-B16A-00C0F0283628} 
         NumButtons      =   9
         BeginProperty Button1 {66833FEA-8583-11D1-B16A-00C0F0283628} 
            Caption         =   "Open"
            Key             =   "open"
            ImageKey        =   "open"
         EndProperty
         BeginProperty Button2 {66833FEA-8583-11D1-B16A-00C0F0283628} 
            Caption         =   "Save"
            Key             =   "save"
            ImageKey        =   "save"
         EndProperty
         BeginProperty Button3 {66833FEA-8583-11D1-B16A-00C0F0283628} 
            Style           =   3
         EndProperty
         BeginProperty Button4 {66833FEA-8583-11D1-B16A-00C0F0283628} 
            Caption         =   "Add CD"
            Key             =   "addcd"
            ImageKey        =   "addcd"
         EndProperty
         BeginProperty Button5 {66833FEA-8583-11D1-B16A-00C0F0283628} 
            Caption         =   "Del CD"
            Key             =   "deletecd"
            ImageKey        =   "delcd"
         EndProperty
         BeginProperty Button6 {66833FEA-8583-11D1-B16A-00C0F0283628} 
            Style           =   3
         EndProperty
         BeginProperty Button7 {66833FEA-8583-11D1-B16A-00C0F0283628} 
            Caption         =   "Find"
            Key             =   "find"
            ImageKey        =   "find"
         EndProperty
         BeginProperty Button8 {66833FEA-8583-11D1-B16A-00C0F0283628} 
            Style           =   3
         EndProperty
         BeginProperty Button9 {66833FEA-8583-11D1-B16A-00C0F0283628} 
            Caption         =   "About"
            Key             =   "about"
            ImageKey        =   "about"
         EndProperty
      EndProperty
   End
   Begin MSComctlLib.ImageList imgToolBar 
      Left            =   3150
      Top             =   1665
      _ExtentX        =   1005
      _ExtentY        =   1005
      BackColor       =   -2147483643
      ImageWidth      =   16
      ImageHeight     =   16
      MaskColor       =   12632256
      _Version        =   393216
      BeginProperty Images {2C247F25-8591-11D1-B16A-00C0F0283628} 
         NumListImages   =   6
         BeginProperty ListImage1 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "frmIndex.frx":0CCA
            Key             =   "open"
         EndProperty
         BeginProperty ListImage2 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "frmIndex.frx":1064
            Key             =   "addcd"
         EndProperty
         BeginProperty ListImage3 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "frmIndex.frx":15FE
            Key             =   "delcd"
         EndProperty
         BeginProperty ListImage4 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "frmIndex.frx":1B98
            Key             =   "save"
         EndProperty
         BeginProperty ListImage5 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "frmIndex.frx":2132
            Key             =   "find"
         EndProperty
         BeginProperty ListImage6 {2C247F27-8591-11D1-B16A-00C0F0283628} 
            Picture         =   "frmIndex.frx":9634
            Key             =   "about"
         EndProperty
      EndProperty
   End
   Begin MSComctlLib.StatusBar stbStatus 
      Align           =   2  'Align Bottom
      Height          =   285
      Left            =   0
      TabIndex        =   1
      Top             =   2910
      Width           =   5910
      _ExtentX        =   10425
      _ExtentY        =   503
      Style           =   1
      _Version        =   393216
      BeginProperty Panels {8E3867A5-8586-11D1-B16A-00C0F0283628} 
         NumPanels       =   1
         BeginProperty Panel1 {8E3867AB-8586-11D1-B16A-00C0F0283628} 
         EndProperty
      EndProperty
   End
   Begin VB.Menu mnuFile 
      Caption         =   "&File"
      Begin VB.Menu mnuFileNew 
         Caption         =   "&New"
         Shortcut        =   ^N
      End
      Begin VB.Menu mnuFileSk0 
         Caption         =   "-"
      End
      Begin VB.Menu mnuFileOpen 
         Caption         =   "&Open..."
         Shortcut        =   ^O
      End
      Begin VB.Menu mnuFileSave 
         Caption         =   "&Save"
         Shortcut        =   ^S
      End
      Begin VB.Menu mnuFileSaveAs 
         Caption         =   "Save &As..."
         Shortcut        =   {F12}
      End
      Begin VB.Menu mnuFileSk1 
         Caption         =   "-"
      End
      Begin VB.Menu mnuFileExit 
         Caption         =   "E&xit"
         Shortcut        =   ^{F4}
      End
   End
   Begin VB.Menu mnuEdit 
      Caption         =   "&Edit"
      Begin VB.Menu mnuEditAddCD 
         Caption         =   "Add CD..."
         Shortcut        =   {F2}
      End
      Begin VB.Menu mnuEditAddCDTittleOnly 
         Caption         =   "Add CD Tittle only..."
         Shortcut        =   +{F2}
      End
      Begin VB.Menu mnuEditAddFolderAsCD 
         Caption         =   "Add Folder as CD..."
         Shortcut        =   ^{F3}
      End
      Begin VB.Menu mnuEditRemoveCD 
         Caption         =   "Remove CD..."
         Shortcut        =   {DEL}
      End
      Begin VB.Menu mnuEditSk0 
         Caption         =   "-"
      End
      Begin VB.Menu mnuEditFind 
         Caption         =   "&Find..."
         Shortcut        =   {F3}
      End
   End
   Begin VB.Menu mnuTools 
      Caption         =   "&Tools"
      Begin VB.Menu mnuToolsOptions 
         Caption         =   "&Options..."
      End
   End
   Begin VB.Menu mnuHelp 
      Caption         =   "&Help"
      Begin VB.Menu mnuHelpAbout 
         Caption         =   "&About..."
         Shortcut        =   +^{F1}
      End
   End
End
Attribute VB_Name = "frmIndex"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False

Private Sub Form_Load()
   With Me
        .mnuEdit.Caption = LoadResString(103)
        .mnuEditAddCD.Caption = LoadResString(104)
        .mnuEditAddCDTittleOnly.Caption = LoadResString(164)
        .mnuEditFind.Caption = LoadResString(105)
        .mnuEditRemoveCD.Caption = LoadResString(106)
        .mnuFile.Caption = LoadResString(107)
        .mnuFileExit.Caption = LoadResString(108)
        .mnuFileNew.Caption = LoadResString(109)
        .mnuFileOpen.Caption = LoadResString(110)
        .mnuFileSave.Caption = LoadResString(111)
        .mnuFileSaveAs.Caption = LoadResString(112)
        .mnuHelp.Caption = LoadResString(113)
        .mnuHelpAbout.Caption = LoadResString(114)
        With Me.tbrToolBar.Buttons
            .Item("open") = LoadResString(115)
            .Item("save") = LoadResString(116)
            .Item("addcd") = LoadResString(117)
            .Item("deletecd") = LoadResString(118)
            .Item("find") = LoadResString(119)
            .Item("about") = LoadResString(120)
        End With
   End With
   Me.stbStatus.SimpleText = LoadResString(102)
   Me.Show
   DoEvents
   If Command = "" Then
      Me.Tag = App.Path & "\Index.scd"
   Else
      Me.Tag = Command
   End If
   frmProgress.Show , Me
   DoEvents
   frmProgress.DoCommand "open " & Me.Tag
   Me.stbStatus.SimpleText = LoadResString(101)
   EnableX Me.tbrToolBar.Buttons.Item("deletecd"), Me.lstBrowser.ListCount > 0
   EnableX Me.tbrToolBar.Buttons.Item("find"), Me.lstBrowser.ListCount > 0
   EnableX Me.tbrToolBar.Buttons.Item("save"), Me.lstBrowser.NeedSave
End Sub

Private Sub Form_Resize()
On Error Resume Next
With Me
    .lstBrowser.Left = 1
    .lstBrowser.Top = .tbrToolBar.Height
    .lstBrowser.Width = .ScaleWidth
    .lstBrowser.Height = .ScaleHeight - (.tbrToolBar.Height + .stbStatus.Height) - 30
End With
End Sub

Private Sub Form_Unload(Cancel As Integer)
 Dim Rez As VbMsgBoxResult
 If Me.lstBrowser.NeedSave Then
    Rez = MsgBox(LoadResString(161), vbQuestion Or vbYesNoCancel Or vbDefaultButton1)
    Select Case Rez
        Case vbCancel
            Cancel = Not Cancel
        Case vbYes
            Me.DoCommand "save"
            If Not Me.lstBrowser.NeedSave Then
                End
            Else
                Cancel = Not Cancel
            End If
        Case vbNo
            End
    End Select
 End If
End Sub

Private Sub lstBrowser_CantOpen(Filename As String)
 MsgBox LoadResString(162), vbCritical, LoadResString(163)
End Sub

Private Sub lstBrowser_Loading(State As Integer)
  Static N As Long
  If N Mod 210 = 0 Then Me.stbStatus.SimpleText = LoadResString(102) & String(N \ 210, ".")
  N = N + 1
  If N > 20000 Then N = 0
End Sub

Private Sub mnuEdit_Click()
  EnableX Me.mnuEditRemoveCD, Me.lstBrowser.ListCount > 0
End Sub

Private Sub mnuEditAddCD_Click()
  Call Me.DoCommand("addcd")
End Sub

Private Sub mnuEditAddCDTittleOnly_Click()
 Call Me.DoCommand("addcdtittle")
End Sub

Private Sub mnuEditAddFolderAsCD_Click()
 Call Me.DoCommand("addfolder")
End Sub

Private Sub mnuEditFind_Click()
 Call Me.DoCommand("find")
End Sub

Private Sub mnuEditRemoveCD_Click()
  Call Me.DoCommand("deletecd")
End Sub

Private Sub mnuFile_Click()
 If Me.mnuFileSave.Enabled <> Me.lstBrowser.NeedSave Then _
    Me.mnuFileSave.Enabled = Me.lstBrowser.NeedSave
End Sub

Private Sub mnuFileExit_Click()
  Call Me.DoCommand("exit")
End Sub

Private Sub mnuFileNew_Click()
  Call Me.DoCommand("new")
End Sub

Private Sub mnuFileOpen_Click()
  Call Me.DoCommand("open")
End Sub

Private Sub mnuFileSave_Click()
  Call Me.DoCommand("save")
End Sub

Private Sub mnuFileSaveAs_Click()
  Call Me.DoCommand("saveas")
End Sub

Private Sub mnuHelpAbout_Click()
  Call Me.DoCommand("about")
End Sub

Sub DoCommand(CommandX As String)
Dim Klausimas As VbMsgBoxResult
Dim Text As String
Select Case CommandX
     Case "addcd"
        frmAddCD.Show , Me
     Case "addcdtittle"
        Text = nsInputBox(LoadResString(153))
        If Text <> "" Then
            Me.lstBrowser.AddTittleOnly Text
        End If
     Case "addfolder"
        frmAddFolder.Show , Me
     Case "about"
        frmAbout.Show 1, Me
     Case "exit"
        Unload Me
     Case "new"
        Me.stbStatus.SimpleText = LoadResString(121): DoEvents
        Me.Tag = skyNewFile
        Me.lstBrowser.Clear
        Me.stbStatus.SimpleText = LoadResString(101): DoEvents
     Case "open"
        Me.cdDialog.DialogTitle = LoadResString(122)
        Me.cdDialog.Filter = LoadResString(123) & "|*.scd|" & LoadResString(124) & "|*.*"
        Me.cdDialog.FilterIndex = 1
        Me.cdDialog.Filename = ""
        Me.cdDialog.Flags = cdlOFNHideReadOnly
        Me.cdDialog.ShowOpen
        If Me.cdDialog.Filename <> "" Then
            Me.stbStatus.SimpleText = LoadResString(125): DoEvents
            Me.Tag = Me.cdDialog.Filename
            frmProgress.Show , Me
            frmProgress.DoCommand "open " & Me.cdDialog.Filename
            'Me.lstBrowser.FileOpen Me.cdDialog.Filename
            Me.stbStatus.SimpleText = LoadResString(101): DoEvents
        End If
     Case "save"
        If Me.Tag = skyNewFile Then
            Call Me.DoCommand("saveas")
            Exit Sub
        End If
        Me.stbStatus.SimpleText = LoadResString(126): DoEvents
        frmProgress.Show , Me
        frmProgress.DoCommand "save " & Me.Tag
'        Me.lstBrowser.FileSave
        Me.stbStatus.SimpleText = LoadResString(101): DoEvents
     Case "saveas"
        Me.cdDialog.DialogTitle = LoadResString(127)
        Me.cdDialog.Filter = LoadResString(165) & "|*.scd|" & LoadResString(166) & "|*.scd"
        Me.cdDialog.FilterIndex = 2
        Me.cdDialog.Filename = ""
        Me.cdDialog.Flags = cdlOFNHideReadOnly
        Me.cdDialog.ShowSave
        If Me.cdDialog.Filename <> "" Then
            Me.Tag = Me.cdDialog.Filename
            Me.stbStatus.SimpleText = LoadResString(126): DoEvents
            Me.lstBrowser.FileSave Me.cdDialog.Filename, Me.cdDialog.FilterIndex - 1
            Me.stbStatus.SimpleText = LoadResString(101): DoEvents
        End If
     Case "find"
        frmFind.Show 1, Me
     Case "deletecd"
        Klausimas = MsgBox(LoadResString(128), vbQuestion Or vbYesNo)
        If Klausimas = vbYes Then
            Me.stbStatus.SimpleText = LoadResString(129): DoEvents
            Me.lstBrowser.DeleteGroup
            Me.stbStatus.SimpleText = LoadResString(101): DoEvents
        End If
     Case "options"
        frmOptions.Show 1, Me
End Select
EnableX Me.tbrToolBar.Buttons.Item("deletecd"), Me.lstBrowser.ListCount > 0
EnableX Me.tbrToolBar.Buttons.Item("find"), Me.lstBrowser.ListCount > 0
EnableX Me.tbrToolBar.Buttons.Item("save"), Me.lstBrowser.NeedSave
End Sub

Private Sub mnuToolsOptions_Click()
  Call Me.DoCommand("options")
End Sub

Private Sub tbrToolBar_ButtonClick(ByVal Button As MSComctlLib.Button)
  Call Me.DoCommand(Button.Key)
End Sub
