VERSION 5.00
Begin VB.UserControl PropertiesListBox 
   ClientHeight    =   3600
   ClientLeft      =   0
   ClientTop       =   0
   ClientWidth     =   3750
   ScaleHeight     =   3600
   ScaleWidth      =   3750
   Begin VB.TextBox txtValue 
      Appearance      =   0  'Flat
      Height          =   285
      Index           =   0
      Left            =   1395
      TabIndex        =   1
      Top             =   -200
      Visible         =   0   'False
      Width           =   735
   End
   Begin VB.Label lblKey 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      BorderStyle     =   1  'Fixed Single
      Caption         =   "Label1"
      ForeColor       =   &H80000008&
      Height          =   240
      Index           =   0
      Left            =   45
      TabIndex        =   0
      Top             =   -195
      UseMnemonic     =   0   'False
      Visible         =   0   'False
      Width           =   735
   End
End
Attribute VB_Name = "PropertiesListBox"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Dim ValuesTypes As New Collection

Enum skyValueTypes
    skyText = 0
    skySelectFile = 1
End Enum

Sub AddOption(Key As String, ValueType As skyValueTypes, Optional Value As String)
 Dim I As Integer
 Let I = UserControl.lblKey.Count
 Load UserControl.lblKey(I)
 UserControl.lblKey(I).Visible = True
 UserControl.lblKey(I).Left = UserControl.lblKey(I - 1).Left
 UserControl.lblKey(I).Top = UserControl.lblKey(I - 1).Top + UserControl.lblKey(I).Height + 50
 UserControl.lblKey(I).Caption = Key
 UserControl.lblKey(I).Width = UserControl.ScaleWidth * 0.4 - 40
 UserControl.lblKey(I).WordWrap = False
 Load UserControl.txtValue(I)
 UserControl.txtValue(I).Visible = True
 UserControl.txtValue(I).Left = UserControl.ScaleWidth * 0.4 + 20
 UserControl.txtValue(I).Top = UserControl.lblKey(I).Top
 UserControl.txtValue(I).Width = UserControl.ScaleWidth * 0.6 - 40
 If Not (IsMissing(Value)) Then _
   UserControl.txtValue(I).Text = Value
 UserControl.lblKey(I).Height = UserControl.txtValue(I).Height
 If I < 2 Then UserControl.txtValue(I).Top = 10
 If I < 2 Then UserControl.lblKey(I).Top = 10
 UserControl.Height = UserControl.txtValue(I).Top + UserControl.txtValue(I).Height
 ValuesTypes.Add ValueType
End Sub

