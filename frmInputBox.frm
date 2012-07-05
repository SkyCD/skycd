VERSION 5.00
Begin VB.Form frmInputBox 
   AutoRedraw      =   -1  'True
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "Input Box"
   ClientHeight    =   1080
   ClientLeft      =   2760
   ClientTop       =   3750
   ClientWidth     =   6030
   Icon            =   "frmInputBox.frx":0000
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   1080
   ScaleWidth      =   6030
   ShowInTaskbar   =   0   'False
   Begin VB.TextBox txtAnswer 
      Height          =   330
      Left            =   135
      TabIndex        =   0
      Top             =   600
      Width           =   4440
   End
   Begin VB.CommandButton CancelButton 
      Caption         =   "Cancel"
      Height          =   375
      Left            =   4680
      TabIndex        =   2
      Top             =   600
      Width           =   1215
   End
   Begin VB.CommandButton OKButton 
      Caption         =   "OK"
      Height          =   375
      Left            =   4680
      TabIndex        =   1
      Top             =   120
      Width           =   1215
   End
   Begin VB.Label lblCaption 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   "Label1"
      Height          =   195
      Left            =   165
      TabIndex        =   3
      Top             =   180
      Width           =   480
   End
End
Attribute VB_Name = "frmInputBox"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False

Option Explicit

Private Sub CancelButton_Click()
 Me.txtAnswer.Text = ""
 Me.Hide
End Sub

Private Sub Form_Load()
 Me.OKButton.Caption = LoadResString(151)
 Me.OKButton.Enabled = False
 Me.CancelButton.Caption = LoadResString(152)
 Me.txtAnswer.Text = GetSetting(App.Title, "Default", "InputBoxValue", "")
 Me.txtAnswer.SelStart = 0
 Me.txtAnswer.SelLength = Len(Me.txtAnswer.Text)
End Sub

Private Sub lblCaption_Click()
 Me.txtAnswer.SetFocus
End Sub

Private Sub OKButton_Click()
 Me.Hide
End Sub

Private Sub txtAnswer_KeyPress(KeyAscii As Integer)
 If KeyAscii = 13 Then
    KeyAscii = 0
    If Me.txtAnswer.Text <> "" Then _
        SaveSetting App.Title, "Default", "InputBoxValue", Me.txtAnswer.Text
    Call OKButton_Click
 End If
 If KeyAscii = 27 Then
    KeyAscii = 0
    Call CancelButton_Click
 End If
End Sub

Private Sub txtAnswer_KeyUp(KeyCode As Integer, Shift As Integer)
 EnableX Me.OKButton, (Len(Me.txtAnswer.Text) > 0)
End Sub
