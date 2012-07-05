VERSION 5.00
Begin VB.Form frmFind 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "Find"
   ClientHeight    =   3195
   ClientLeft      =   2760
   ClientTop       =   3750
   ClientWidth     =   6030
   Icon            =   "frmFind.frx":0000
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   3195
   ScaleWidth      =   6030
   ShowInTaskbar   =   0   'False
   Begin SkyCD.SimpleCDDatabaseBrowser scdRez 
      Height          =   1890
      Left            =   75
      TabIndex        =   4
      Top             =   825
      Width           =   5910
      _ExtentX        =   10425
      _ExtentY        =   3334
   End
   Begin VB.Frame Frame1 
      Caption         =   "Phase to find"
      Height          =   720
      Left            =   60
      TabIndex        =   2
      Top             =   60
      Width           =   5925
      Begin VB.TextBox txtWhatToFind 
         Height          =   300
         Left            =   120
         TabIndex        =   3
         Top             =   255
         Width           =   5685
      End
   End
   Begin VB.CommandButton CancelButton 
      Caption         =   "&Close"
      Height          =   375
      Left            =   4770
      TabIndex        =   1
      Top             =   2760
      Width           =   1215
   End
   Begin VB.CommandButton OKButton 
      Caption         =   "&Find !"
      Height          =   375
      Left            =   3495
      TabIndex        =   0
      Top             =   2775
      Width           =   1215
   End
End
Attribute VB_Name = "frmFind"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False

Sub FindIt(Text As String)
Dim I As Long
 With Me.scdRez
    .Clear
    For I = 1 To frmIndex.lstBrowser.ListCount
       If InStr(1, LCase(frmIndex.lstBrowser.MyNodes(I).FullPath), LCase(Text)) > 0 Then _
          .AddToTree frmIndex.lstBrowser.MyNodes(I).FullPath
       If I Mod 32 Then DoEvents
    Next I
 End With
End Sub

Private Sub CancelButton_Click()
 Unload Me
End Sub

Private Sub Form_Load()
With Me
    .Frame1.Caption = LoadResString(130)
    .OKButton.Caption = LoadResString(131)
    .CancelButton.Caption = LoadResString(132)
    .Caption = LoadResString(133)
End With
End Sub

Private Sub OKButton_Click()
 Me.FindIt Me.txtWhatToFind.Text
End Sub

Private Sub txtWhatToFind_KeyPress(KeyAscii As Integer)
 If KeyAscii = 13 Then Call OKButton_Click
End Sub
