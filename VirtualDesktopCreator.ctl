VERSION 5.00
Object = "{0E59F1D2-1FBE-11D0-8FF2-00A0D10038BC}#1.0#0"; "msscript.ocx"
Begin VB.UserControl VirtualDesktopCreator 
   AutoRedraw      =   -1  'True
   ClientHeight    =   3600
   ClientLeft      =   0
   ClientTop       =   0
   ClientWidth     =   4800
   ScaleHeight     =   3600
   ScaleWidth      =   4800
   Begin MSScriptControlCtl.ScriptControl scScript 
      Left            =   1890
      Top             =   2760
      _ExtentX        =   1005
      _ExtentY        =   1005
   End
   Begin VB.VScrollBar vsbScrolling 
      Height          =   2775
      Left            =   4500
      TabIndex        =   4
      Top             =   180
      Visible         =   0   'False
      Width           =   255
   End
   Begin VB.PictureBox picContainer 
      AutoRedraw      =   -1  'True
      BackColor       =   &H80000001&
      BorderStyle     =   0  'None
      Height          =   2895
      Left            =   15
      ScaleHeight     =   2895
      ScaleWidth      =   4020
      TabIndex        =   0
      Top             =   15
      Width           =   4020
      Begin VB.ListBox lstList 
         Height          =   540
         Index           =   0
         IntegralHeight  =   0   'False
         Left            =   960
         TabIndex        =   5
         Top             =   1680
         Visible         =   0   'False
         Width           =   1575
      End
      Begin VB.TextBox txtText 
         Height          =   315
         Index           =   0
         Left            =   1125
         TabIndex        =   3
         Top             =   0
         Visible         =   0   'False
         Width           =   1080
      End
      Begin VB.CommandButton cmdButton 
         Caption         =   "CommandButton"
         Height          =   390
         Index           =   0
         Left            =   870
         TabIndex        =   1
         Top             =   720
         Visible         =   0   'False
         Width           =   690
      End
      Begin VB.Label lblLabel 
         AutoSize        =   -1  'True
         Caption         =   "Label"
         Height          =   195
         Index           =   0
         Left            =   0
         TabIndex        =   2
         Top             =   135
         Visible         =   0   'False
         Width           =   390
      End
   End
End
Attribute VB_Name = "VirtualDesktopCreator"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Public LastY As Single

Enum skyVirtualDesktopCreatorObjectsTypes
    skyButton = 0
    skyLabel = 1
    skyTextBox = 2
End Enum

Enum skyVirtualDesktopCreatorEventsTypes
    skyClick = 0
    skyDoubleClick = 1
    skyKeyDown = 2
    skyKeyUp = 3
    skyKeyPress = 4
    skyGotFocus = 5
    skyLostFocus = 6
    skyMouseDown = 7
    skyMouseUp = 8
    skyMouseMove = 9
    skyChange = 10
End Enum

Event ObjectsEvent(ObjectType As skyVirtualDesktopCreatorObjectsTypes, EventType As skyVirtualDesktopCreatorEventsTypes, Index As Integer, Params() As Variant)

Sub ExecuteScript(Filename As String)
  'On Error Resume Next
  UserControl.scScript.Reset
  'UserControl.scScript.AddCode ReadTextFileToLine(Filename)
  UserControl.scScript.AddObject "Me", Me, True
  'UserControl.scScript.AddCode "Const LanguageString =" & GetLanguageAllStrings
  UserControl.scScript.ExecuteStatement ReadTextFileToLine(Filename)
  'UserControl.scScript.Run "Main"
End Sub

Private Sub cmdButton_Click(Index As Integer)
  Dim Params() As Variant
  RaiseEvent ObjectsEvent(skyButton, skyClick, Index, Params())
End Sub

Private Sub cmdButton_GotFocus(Index As Integer)
  Dim Params() As Variant
  RaiseEvent ObjectsEvent(skyButton, skyGotFocus, Index, Params())
End Sub

Private Sub cmdButton_KeyDown(Index As Integer, KeyCode As Integer, Shift As Integer)
  Dim Params(0 To 1) As Variant
  Let Params(0) = KeyCode
  Let Params(1) = Shift
  RaiseEvent ObjectsEvent(skyButton, skyKeyDown, Index, Params())
End Sub

Private Sub cmdButton_KeyPress(Index As Integer, KeyAscii As Integer)
  Dim Params(0 To 0) As Variant
  Let Params(0) = KeyAscii
  RaiseEvent ObjectsEvent(skyButton, skyKeyPress, Index, Params())
End Sub

Private Sub cmdButton_KeyUp(Index As Integer, KeyCode As Integer, Shift As Integer)
  Dim Params(0 To 1) As Variant
  Let Params(0) = KeyCode
  Let Params(1) = Shift
  RaiseEvent ObjectsEvent(skyButton, skyKeyUp, Index, Params())
End Sub

Private Sub cmdButton_LostFocus(Index As Integer)
  Dim Params() As Variant
  RaiseEvent ObjectsEvent(skyButton, skyLostFocus, Index, Params())
End Sub

Private Sub cmdButton_MouseDown(Index As Integer, Button As Integer, Shift As Integer, X As Single, Y As Single)
  Dim Params(0 To 3) As Variant
  Let Params(0) = Button
  Let Params(1) = Shift
  Let Params(2) = X
  Let Params(3) = Y
  RaiseEvent ObjectsEvent(skyButton, skyMouseDown, Index, Params())
End Sub

Private Sub cmdButton_MouseMove(Index As Integer, Button As Integer, Shift As Integer, X As Single, Y As Single)
  Dim Params(0 To 3) As Variant
  Let Params(0) = Button
  Let Params(1) = Shift
  Let Params(2) = X
  Let Params(3) = Y
  RaiseEvent ObjectsEvent(skyButton, skyMouseMove, Index, Params())
End Sub

Private Sub cmdButton_MouseUp(Index As Integer, Button As Integer, Shift As Integer, X As Single, Y As Single)
  Dim Params(0 To 3) As Variant
  Let Params(0) = Button
  Let Params(1) = Shift
  Let Params(2) = X
  Let Params(3) = Y
  RaiseEvent ObjectsEvent(skyButton, skyMouseUp, Index, Params())
End Sub

Private Sub lblLabel_Change(Index As Integer)
  Dim Params() As Variant
  RaiseEvent ObjectsEvent(skyLabel, skyChange, Index, Params())
End Sub

Private Sub lblLabel_Click(Index As Integer)
  Dim Params() As Variant
  RaiseEvent ObjectsEvent(skyLabel, skyClick, Index, Params())
End Sub

Private Sub lblLabel_DblClick(Index As Integer)
  Dim Params() As Variant
  RaiseEvent ObjectsEvent(skyLabel, skyDoubleClick, Index, Params())
End Sub

Private Sub lblLabel_MouseDown(Index As Integer, Button As Integer, Shift As Integer, X As Single, Y As Single)
  Dim Params(0 To 3) As Variant
  Let Params(0) = Button
  Let Params(1) = Shift
  Let Params(2) = X
  Let Params(3) = Y
  RaiseEvent ObjectsEvent(skyLabel, skyMouseDown, Index, Params())
End Sub

Private Sub lblLabel_MouseMove(Index As Integer, Button As Integer, Shift As Integer, X As Single, Y As Single)
  Dim Params(0 To 3) As Variant
  Let Params(0) = Button
  Let Params(1) = Shift
  Let Params(2) = X
  Let Params(3) = Y
  RaiseEvent ObjectsEvent(skyLabel, skyMouseMove, Index, Params())
End Sub

Private Sub lblLabel_MouseUp(Index As Integer, Button As Integer, Shift As Integer, X As Single, Y As Single)
  Dim Params(0 To 3) As Variant
  Let Params(0) = Button
  Let Params(1) = Shift
  Let Params(2) = X
  Let Params(3) = Y
  RaiseEvent ObjectsEvent(skyLabel, skyMouseUp, Index, Params())
End Sub

Private Sub txtText_Change(Index As Integer)
  Dim Params() As Variant
  RaiseEvent ObjectsEvent(skyTextBox, skyChange, Index, Params())
End Sub

Private Sub txtText_Click(Index As Integer)
  Dim Params() As Variant
  RaiseEvent ObjectsEvent(skyTextBox, skyClick, Index, Params())
End Sub

Private Sub txtText_DblClick(Index As Integer)
  Dim Params() As Variant
  RaiseEvent ObjectsEvent(skyTextBox, skyDoubleClick, Index, Params())
End Sub

Private Sub txtText_GotFocus(Index As Integer)
  Dim Params() As Variant
  RaiseEvent ObjectsEvent(skyTextBox, skyGotFocus, Index, Params())
End Sub

Private Sub txtText_KeyDown(Index As Integer, KeyCode As Integer, Shift As Integer)
  Dim Params(0 To 1) As Variant
  Let Params(0) = KeyCode
  Let Params(1) = Shift
  RaiseEvent ObjectsEvent(skyTextBox, skyGotFocus, Index, Params())
End Sub

Private Sub txtText_KeyPress(Index As Integer, KeyAscii As Integer)
  Dim Params(0 To 0) As Variant
  Let Params(0) = KeyAscii
  RaiseEvent ObjectsEvent(skyTextBox, skyGotFocus, Index, Params())
End Sub

Private Sub txtText_KeyUp(Index As Integer, KeyCode As Integer, Shift As Integer)
  Dim Params(0 To 1) As Variant
  Let Params(0) = KeyCode
  Let Params(1) = Shift
  RaiseEvent ObjectsEvent(skyTextBox, skyGotFocus, Index, Params())
End Sub

Private Sub txtText_LostFocus(Index As Integer)
  Dim Params() As Variant
  RaiseEvent ObjectsEvent(skyTextBox, skyLostFocus, Index, Params())
End Sub

Private Sub txtText_MouseDown(Index As Integer, Button As Integer, Shift As Integer, X As Single, Y As Single)
  Dim Params(0 To 3) As Variant
  Let Params(0) = Button
  Let Params(1) = Shift
  Let Params(2) = X
  Let Params(3) = Y
  RaiseEvent ObjectsEvent(skyTextBox, skyMouseDown, Index, Params())
End Sub

Private Sub txtText_MouseMove(Index As Integer, Button As Integer, Shift As Integer, X As Single, Y As Single)
  Dim Params(0 To 3) As Variant
  Let Params(0) = Button
  Let Params(1) = Shift
  Let Params(2) = X
  Let Params(3) = Y
  RaiseEvent ObjectsEvent(skyTextBox, skyMouseMove, Index, Params())
End Sub

Private Sub txtText_MouseUp(Index As Integer, Button As Integer, Shift As Integer, X As Single, Y As Single)
  Dim Params(0 To 3) As Variant
  Let Params(0) = Button
  Let Params(1) = Shift
  Let Params(2) = X
  Let Params(3) = Y
  RaiseEvent ObjectsEvent(skyTextBox, skyMouseUp, Index, Params())
End Sub

Private Sub UserControl_Initialize()
  UserControl.scScript.AddObject "Me", Me, True
'  UserControl.scScript.AddObject "Desktop", Me, True
End Sub

Private Sub UserControl_Resize()
On Error Resume Next
If UserControl.ScaleHeight < UserControl.picContainer.Height Then
   UserControl.vsbScrolling.Min = 0
   UserControl.vsbScrolling.Max = (UserControl.picContainer.Height - UserControl.ScaleHeight) / 15
   UserControl.vsbScrolling.Visible = True
   UserControl.picContainer.Width = UserControl.ScaleWidth - UserControl.vsbScrolling.Width - 10
Else
   UserControl.vsbScrolling.Visible = False
   UserControl.picContainer.Width = UserControl.ScaleWidth
End If
End Sub

Sub AddLine()
  LastY = LastY + UserControl.picContainer.TextHeight("SkyCD")
End Sub

Function LoadLabel(Caption As String, X As Single, Y As Single, Optional W As Single, Optional H As Single, Optional AutoSize As Boolean) As Integer
  LoadLabel = UserControl.lblLabel.Count
  Load UserControl.lblLabel(LoadLabel)
  Caption = cFormat(Caption)
  If Not IsMissing(W) Then _
     UserControl.lblLabel(LoadLabel).Width = W
  'Else
     
  'End If
  If Not IsMissing(AutoSize) Then
     'UserControl.lblLabel(LoadLabel).WordWrap = True
     UserControl.lblLabel(LoadLabel).AutoSize = AutoSize
     LastY = Y + UserControl.picContainer.TextHeight(Caption)
  Else
     UserControl.lblLabel(LoadLabel).AutoSize = False
     LastY = Y + H
  End If
  If Not IsMissing(H) Then _
     UserControl.lblLabel(LoadLabel).Height = H
  UserControl.lblLabel.Item(LoadLabel).Caption = Caption
  UserControl.lblLabel.Item(LoadLabel).Left = X
  UserControl.lblLabel.Item(LoadLabel).Top = Y
  UserControl.lblLabel.Item(LoadLabel).Visible = True
End Function

Function LoadTextBox(Text As String, X As Single, Y As Single, Optional ByRef W As Single, Optional ByRef H As Single) As Integer
  LoadTextBox = UserControl.txtText.Count
  Load UserControl.txtText(LoadTextBox)
  Text = cFormat(Text)
  If IsMissing(W) Or W = 0 Then
     UserControl.txtText(LoadTextBox).Width = UserControl.picContainer.ScaleWidth - UserControl.txtText(LoadTextBox).Left - 10
  Else
     UserControl.txtText(LoadTextBox).Width = UserControl.picContainer.ScaleWidth - UserControl.txtText(LoadTextBox).Left - 10
     UserControl.txtText(LoadTextBox).Width = W
  End If
  If Not IsMissing(H) Then _
     UserControl.txtText(LoadTextBox).Height = H
  LastY = Y + UserControl.txtText(LoadTextBox).Height
  UserControl.txtText.Item(LoadTextBox).Text = Text
  UserControl.txtText.Item(LoadTextBox).Left = X
  UserControl.txtText.Item(LoadTextBox).Top = Y
  UserControl.txtText.Item(LoadTextBox).Visible = True
End Function

Function LoadListBox(X As Single, Y As Single, Optional ByRef W As Single, Optional ByRef H As Single)
  LoadListBox = UserControl.txtText.Count
  Load UserControl.lstList(LoadListBox)
  If IsMissing(W) Or W = 0 Then
     UserControl.lstList(LoadListBox).Width = UserControl.picContainer.Width - UserControl.lstList(LoadListBox).Left - 50
  Else
     UserControl.lstList(LoadListBox).Width = UserControl.picContainer.Width - UserControl.lstList(LoadListBox).Left - 50
     UserControl.lstList(LoadListBox).Width = W
  End If
  If Not IsMissing(H) Then _
     UserControl.lstList(LoadListBox).Height = H
  LastY = Y + UserControl.lstList(LoadListBox).Height
  UserControl.lstList.Item(LoadListBox).Text = Text
  UserControl.lstList.Item(LoadListBox).Left = X
  UserControl.lstList.Item(LoadListBox).Top = Y
  UserControl.lstList.Item(LoadListBox).Visible = True
End Function

Function LoadListBoxWithLabel(Caption As String, X As Single, Y As Single) As Integer
  LoadListBoxWithLabel = LoadLabel(Caption, X, Y, , , True)
  LoadListBox X, Y + UserControl.lblLabel(LoadListBoxWithLabel).Height, , 2000
End Function

Function LoadTextBoxWithLabel(Caption As String, Text As String, X As Single, Y As Single) As Integer
  LoadTextBoxWithLabel = LoadLabel(Caption, X, Y, , , True)
  LoadTextBoxWithLabel = LoadTextBox(Text, X, Y + UserControl.lblLabel(LoadTextBoxWithLabel).Height)
'  LastY = Y + UserControl.txtText(LoadTextBoxWithLabel).Height
End Function

Function LoadButton(Caption As String, X As Single, Y As Single, Optional ByRef W As Single, Optional ByRef H As Single) As Integer
  LoadButton = UserControl.cmdButton.Count
  Load UserControl.cmdButton(LoadButton)
  Caption = cFormat(Caption)
  If Not IsMissing(W) Then
     UserControl.cmdButton(LoadButton).Width = W
     'UserControl.cmdButton(LoadButton).Width = UserControl.picContainer.ScaleWidth - cmdButton(LoadButton).Left - 50
  Else
     'UserControl.cmdButton(LoadButton).Width = UserControl.picContainer.ScaleWidth - cmdButton(LoadButton).Left - 50
  End If
  If Not IsMissing(H) Then _
     UserControl.cmdButton(LoadButton).Height = H
  LastY = Y + UserControl.txtText(LoadButton).Height
  UserControl.cmdButton(LoadButton).Caption = Caption
  UserControl.cmdButton(LoadButton).Left = X
  UserControl.cmdButton(LoadButton).Top = Y
  UserControl.cmdButton(LoadButton).Visible = True
End Function

Sub UnloadAll()
 Dim I As Integer
 For I = 1 To UserControl.lblLabel.Count - 1
    Unload UserControl.lblLabel(I)
 Next I
 For I = 1 To UserControl.txtText.Count - 1
    Unload UserControl.txtText(I)
 Next I
 For I = 1 To UserControl.cmdButton.Count - 1
    Unload UserControl.cmdButton(I)
 Next I
 For I = 1 To UserControl.lstList.Count - 1
    Unload UserControl.lstList(I)
 Next I
End Sub
