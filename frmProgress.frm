VERSION 5.00
Object = "{831FDD16-0C5C-11D2-A9FC-0000F8754DA1}#2.0#0"; "MSCOMCTL.OCX"
Begin VB.Form frmProgress 
   AutoRedraw      =   -1  'True
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "Progress"
   ClientHeight    =   3240
   ClientLeft      =   2760
   ClientTop       =   3750
   ClientWidth     =   6075
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   ScaleHeight     =   3240
   ScaleWidth      =   6075
   Begin VB.Timer tmrProgressBarUpdate 
      Enabled         =   0   'False
      Interval        =   150
      Left            =   4425
      Top             =   675
   End
   Begin VB.PictureBox picProgress 
      AutoRedraw      =   -1  'True
      BorderStyle     =   0  'None
      Height          =   1395
      Left            =   210
      ScaleHeight     =   1395
      ScaleWidth      =   5715
      TabIndex        =   2
      Top             =   1680
      Width           =   5715
      Begin MSComctlLib.ProgressBar pbrAllWork 
         Height          =   300
         Left            =   120
         TabIndex        =   4
         Top             =   345
         Width           =   5475
         _ExtentX        =   9657
         _ExtentY        =   529
         _Version        =   393216
         Appearance      =   1
         Scrolling       =   1
      End
      Begin MSComctlLib.ProgressBar pbrCurrentWork 
         Height          =   300
         Left            =   120
         TabIndex        =   5
         Top             =   945
         Width           =   5475
         _ExtentX        =   9657
         _ExtentY        =   529
         _Version        =   393216
         Appearance      =   1
         Scrolling       =   1
      End
      Begin VB.Label lblProgress 
         AutoSize        =   -1  'True
         Caption         =   "Current Operation:"
         Height          =   195
         Index           =   1
         Left            =   105
         TabIndex        =   6
         Top             =   705
         Width           =   1290
      End
      Begin VB.Label lblProgress 
         AutoSize        =   -1  'True
         Caption         =   "All Work:"
         Height          =   195
         Index           =   0
         Left            =   105
         TabIndex        =   3
         Top             =   75
         Width           =   645
      End
   End
   Begin SkyCD.SimpleLog silLog 
      Height          =   1425
      Left            =   195
      TabIndex        =   1
      Top             =   1665
      Visible         =   0   'False
      Width           =   5745
      _ExtentX        =   10134
      _ExtentY        =   2514
   End
   Begin MSComctlLib.TabStrip tsCategories 
      Height          =   1890
      Left            =   105
      TabIndex        =   0
      Top             =   1275
      Width           =   5910
      _ExtentX        =   10425
      _ExtentY        =   3334
      HotTracking     =   -1  'True
      _Version        =   393216
      BeginProperty Tabs {1EFB6598-857C-11D1-B16A-00C0F0283628} 
         NumTabs         =   2
         BeginProperty Tab1 {1EFB659A-857C-11D1-B16A-00C0F0283628} 
            Caption         =   "Progress Bar"
            Key             =   "progress"
            ImageVarType    =   2
         EndProperty
         BeginProperty Tab2 {1EFB659A-857C-11D1-B16A-00C0F0283628} 
            Caption         =   "Log"
            Key             =   "log"
            ImageVarType    =   2
         EndProperty
      EndProperty
   End
   Begin VB.Image imgAnimation 
      Height          =   1155
      Left            =   90
      Stretch         =   -1  'True
      Top             =   45
      Width           =   5865
   End
End
Attribute VB_Name = "frmProgress"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Private Sub tmrProgressBarUpdate_Timer()
    Dim I As Integer
    I = frmIndex.lstBrowser.CurrentProgressState
    If Me.pbrAllWork.Max < I Then _
        I = I \ Me.pbrAllWork.Max
    Me.pbrAllWork.Value = I
    Me.pbrCurrentWork.Value = Me.pbrAllWork.Value
End Sub

Private Sub tsCategories_Click()
 Select Case Me.tsCategories.SelectedItem.Key
    Case "log"
        Me.silLog.Visible = True
        Me.picProgress.Visible = False
        Me.silLog.SetFocus
    Case "progress"
        Me.silLog.Visible = False
        Me.picProgress.Visible = True
        Me.picProgress.SetFocus
 End Select
End Sub

Sub DoCommand(Command As String)
Dim AllData() As String
Dim URL As String
AllData() = Split(Command, " ")
Let URL = Right(Command, Len(Command) - Len(AllData(0)) - 1)
Select Case LCase(AllData(0))
    Case "addtodatabase"
'        MsgBox URL
        Me.ReadCDData URL
    Case "open"
        Me.pbrAllWork.Min = 0
        Me.pbrAllWork.Max = 100
        Me.pbrAllWork.Value = 0
        Me.pbrCurrentWork.Min = Me.pbrAllWork.Min
        Me.pbrCurrentWork.Max = Me.pbrAllWork.Max
        Me.pbrCurrentWork.Value = Me.pbrAllWork.Value
        Me.silLog.lWrite "Opening..."
        Me.tmrProgressBarUpdate.Enabled = True
        frmIndex.lstBrowser.FileOpen URL
        Me.tmrProgressBarUpdate.Enabled = False
        Unload Me
    Case "save"
        Me.pbrAllWork.Min = 0
        Me.pbrAllWork.Max = frmIndex.lstBrowser.ListCount
        Me.pbrAllWork.Value = 0
        Me.pbrCurrentWork.Min = Me.pbrAllWork.Min
        Me.pbrCurrentWork.Max = Me.pbrAllWork.Max
        Me.pbrCurrentWork.Value = Me.pbrAllWork.Value
        Me.silLog.lWrite "Saving..."
        Me.tmrProgressBarUpdate.Enabled = True
        frmIndex.lstBrowser.FileSave URL
        Me.tmrProgressBarUpdate.Enabled = False
        Unload Me
End Select
End Sub

Function GetDirectories(MyPath As String) As Collection
   Dim Col As New Collection
   Dim MyName As String
   MyName = Dir(MyPath, vbDirectory)
   Do While MyName <> ""
    If MyName <> "." And MyName <> ".." Then
        If (GetAttr(MyPath & MyName) And vbDirectory) = vbDirectory Then
            Col.Add MyName
        End If
    End If
    MyName = Dir
    Loop
    Set GetDirectories = Col
End Function

Function GetFiles(MyPath As String, Ext As String) As Collection
   Dim Col As New Collection
   Dim MyName As String
   MyPath = MyPath
   MyName = Dir(MyPath & Ext)
   Do While MyName <> ""
    If MyName <> "." And MyName <> ".." Then
        If (GetAttr(MyPath & MyName) And vbDirectory) <> vbDirectory Then
            Col.Add MyName
        End If
    End If
    MyName = Dir
    Loop
    Set GetFiles = Col
End Function

Sub ReadCDData(CDurl As String)
   On Error Resume Next
   Dim Tracks As New Collection
   Dim Col As New Collection
   Dim Files As New Collection
   Dim MyName As String, MyPath As String
   Dim I As Integer, O As Integer, K As Integer
   Dim Types() As String, TMP() As String
   Dim Pav As String, Lauk(1 To 4) As String
   Pav = nsInputBox(LoadResString(145))
   Const CDID = 3
   Const ItemDefaultType = 1
   Tracks.Add CDurl
   Me.silLog.lWrite LoadResString(146)
   O = 1
   Me.pbrAllWork.Min = 0
   Me.pbrAllWork.Value = 0
   Me.pbrAllWork.Max = 3
   Me.pbrCurrentWork.Min = 0
   Me.pbrCurrentWork.Max = 1
   Me.pbrCurrentWork.Value = 0
   Do
      MyPath = Tracks.Item(O) & "\"
      Set Col = GetDirectories(MyPath)
      For I = 1 To Col.Count
        Tracks.Add MyPath & Col.Item(I)
        Me.silLog.lWrite xReplace(LoadResString(147), MyPath & Col.Item(I))
        DoEvents
      Next I
      O = O + 1
      Me.pbrCurrentWork.Max = Tracks.Count
      Me.pbrCurrentWork.Value = O
   Loop Until O > Tracks.Count
   Me.pbrAllWork.Value = 1
   Types = Split(skyFileTypes, ";")
   Me.pbrCurrentWork.Min = 0
   Me.pbrCurrentWork.Max = UBound(Types) - LBound(Types)
   Me.pbrCurrentWork.Value = 0
   For K = LBound(Types) To UBound(Types)
    For O = 1 To Tracks.Count
      MyPath = Tracks.Item(O) & "\"
      Set Col = GetFiles(MyPath, Types(K))
      For I = 1 To Col.Count
        Files.Add MyPath & Col.Item(I)
        Me.silLog.lWrite xReplace(LoadResString(147), MyPath & Col.Item(I))
        DoEvents
      Next I
    Next O
    Me.pbrCurrentWork.Value = K
   Next K
   Me.pbrAllWork.Value = 2
   Me.silLog.lWrite LoadResString(148)
   Me.pbrCurrentWork.Min = 0
   Me.pbrCurrentWork.Max = Files.Count
   Me.pbrCurrentWork.Value = 0
   For O = 1 To Files.Count
    TMP = Split(Files.Item(O), "\")
    Lauk(1) = TMP(UBound(TMP))
    Lauk(3) = Mid(Files.Item(O), Len("E:\") + 1, Len(Files.Item(O)) - Len("E:\") - Len(Lauk(1)) - 1)
    frmIndex.lstBrowser.AddItem Pav, Lauk(3), Lauk(1), FileLen(Files.Item(O))
    If O Mod 11 = 0 Then DoEvents
    Me.pbrCurrentWork.Value = O
   Next O
   Me.pbrAllWork.Value = 3
   Me.silLog.lWrite LoadResString(149)
   Beep
   Unload Me
'   MsgBox LoadResString(150), vbInformation Or vbOKOnly
End Sub
