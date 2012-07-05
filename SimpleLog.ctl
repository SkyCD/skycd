VERSION 5.00
Begin VB.UserControl SimpleLog 
   ClientHeight    =   3600
   ClientLeft      =   0
   ClientTop       =   0
   ClientWidth     =   4800
   ScaleHeight     =   3600
   ScaleWidth      =   4800
   Begin VB.ListBox lstProgress 
      Height          =   1980
      IntegralHeight  =   0   'False
      Left            =   15
      TabIndex        =   0
      Top             =   15
      Width           =   5760
   End
End
Attribute VB_Name = "SimpleLog"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False


Private Sub UserControl_Resize()
With UserControl
    .lstProgress.Left = 0
    .lstProgress.Top = 0
    .lstProgress.Width = .ScaleWidth
    .lstProgress.Height = .ScaleHeight
End With
End Sub

Sub lClear()
   UserControl.lstProgress.Clear
End Sub

Sub lWrite(Text As String)
   With UserControl.lstProgress
        .AddItem Text
        .ListIndex = .ListCount - 1
   End With
End Sub
