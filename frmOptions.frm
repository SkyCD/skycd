VERSION 5.00
Object = "{831FDD16-0C5C-11D2-A9FC-0000F8754DA1}#2.0#0"; "MSCOMCTL.OCX"
Begin VB.Form frmOptions 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "Options"
   ClientHeight    =   5460
   ClientLeft      =   2910
   ClientTop       =   2130
   ClientWidth     =   6600
   Icon            =   "frmOptions.frx":0000
   KeyPreview      =   -1  'True
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   5460
   ScaleWidth      =   6600
   ShowInTaskbar   =   0   'False
   Begin VB.FileListBox lstFiles 
      Height          =   480
      Hidden          =   -1  'True
      Left            =   3045
      System          =   -1  'True
      TabIndex        =   11
      Top             =   4065
      Visible         =   0   'False
      Width           =   1860
   End
   Begin VB.Frame fraCoolLine 
      Height          =   30
      Left            =   30
      TabIndex        =   9
      Top             =   4920
      Width           =   6525
   End
   Begin VB.ListBox lstPlugIns 
      Height          =   945
      IntegralHeight  =   0   'False
      Left            =   2715
      Sorted          =   -1  'True
      Style           =   1  'Checkbox
      TabIndex        =   8
      Top             =   2835
      Width           =   3690
   End
   Begin VB.ListBox lstAssociations 
      Height          =   945
      IntegralHeight  =   0   'False
      Left            =   2715
      Sorted          =   -1  'True
      Style           =   1  'Checkbox
      TabIndex        =   6
      Top             =   1470
      Width           =   3690
   End
   Begin MSComctlLib.ImageList imlIcons 
      Left            =   5865
      Top             =   1200
      _ExtentX        =   1005
      _ExtentY        =   1005
      BackColor       =   -2147483643
      ImageWidth      =   16
      ImageHeight     =   16
      MaskColor       =   12632256
      _Version        =   393216
   End
   Begin MSComctlLib.ImageCombo imcLanguage 
      Height          =   330
      Left            =   2685
      TabIndex        =   4
      Top             =   720
      Width           =   3750
      _ExtentX        =   6615
      _ExtentY        =   582
      _Version        =   393216
      ForeColor       =   -2147483640
      BackColor       =   -2147483643
      Indentation     =   1
      Locked          =   -1  'True
   End
   Begin VB.CommandButton cmdCancel 
      Cancel          =   -1  'True
      Caption         =   "Cancel"
      Height          =   375
      Left            =   5445
      TabIndex        =   1
      Top             =   5025
      Width           =   1095
   End
   Begin VB.CommandButton cmdOK 
      Caption         =   "OK"
      Height          =   375
      Left            =   4290
      TabIndex        =   0
      Top             =   5025
      Width           =   1095
   End
   Begin VB.Label lblCaption 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   "SkyCD"
      BeginProperty Font 
         Name            =   "Palatino Linotype"
         Size            =   38.25
         Charset         =   186
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H00004000&
      Height          =   1035
      Left            =   105
      TabIndex        =   10
      Top             =   150
      UseMnemonic     =   0   'False
      Width           =   2370
   End
   Begin VB.Label lblWhat 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   "Select plug-ins:"
      Height          =   195
      Index           =   3
      Left            =   2730
      TabIndex        =   7
      Top             =   2595
      Width           =   1080
   End
   Begin VB.Label lblWhat 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   "Select associations:"
      Height          =   195
      Index           =   2
      Left            =   2700
      TabIndex        =   5
      Top             =   1230
      Width           =   1410
   End
   Begin VB.Label lblWhat 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   "Select language:"
      Height          =   195
      Index           =   1
      Left            =   2685
      TabIndex        =   3
      Top             =   465
      Width           =   1200
   End
   Begin VB.Label lblWhat 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   "Here you can set/view some SkyCD preferences."
      Height          =   195
      Index           =   0
      Left            =   2670
      TabIndex        =   2
      Top             =   75
      Width           =   3510
   End
   Begin VB.Image imgPic 
      BorderStyle     =   1  'Fixed Single
      Height          =   4755
      Left            =   30
      Picture         =   "frmOptions.frx":000C
      Stretch         =   -1  'True
      Top             =   75
      Width           =   2580
   End
End
Attribute VB_Name = "frmOptions"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Private Sub cmdApply_Click()
    MsgBox "Place code here to set options w/o closing dialog!"
End Sub

Private Sub cmdCancel_Click()
    Unload Me
End Sub

Private Sub cmdOK_Click()
    MsgBox "Place code here to set options and close dialog!"
    Unload Me
End Sub

Private Sub Form_Load()
With Me
   'With .plbList
   '    .AddOption "Your Name", skyText
   '    .AddOption "Your eMail", skyText
   '    .AddOption "Your URL", skyText
   '    .AddOption "Your Comments", skyText
   '    .AddOption "Language", skySelectFile
   'End With
End With
End Sub

Private Sub trvMenu_Click()
On Error Resume Next
Me.vdcOptions.UnloadAll
Me.vdcOptions.ExecuteScript App.Path & "\Scripts\Options\" & trvMenu.SelectedItem.Key & ".vbs"
End Sub

Function CalculateNewTop(Text As String, Optional ByRef NewValue As Single) As Single
Static CRX
If Not IsEmpty(NewValue) Then CRX = NewValue
CRX = CRX + TextWidth(Text)
End Function

Private Sub Form_Resize()
'If Me.plbList.Height > Me.picBox.Height Then
'   Me.vsscroll.Max = (Me.plbList.Height - Me.picBox.Height) \ 100
'   Me.vsscroll.Min = 0
'Else
'   Me.vsscroll.Enabled = False
'End If
End Sub

Private Sub vsscroll_Change()
 Me.plbList.Top = -Me.vsscroll.Value * 100
End Sub
