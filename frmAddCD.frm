VERSION 5.00
Begin VB.Form frmAddCD 
   AutoRedraw      =   -1  'True
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "Add CD To Database"
   ClientHeight    =   975
   ClientLeft      =   2760
   ClientTop       =   3750
   ClientWidth     =   6000
   Icon            =   "frmAddCD.frx":0000
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   975
   ScaleWidth      =   6000
   ShowInTaskbar   =   0   'False
   Begin VB.DriveListBox drvDrive 
      Height          =   315
      Left            =   2640
      TabIndex        =   3
      Top             =   120
      Width           =   3255
   End
   Begin VB.CommandButton CancelButton 
      Cancel          =   -1  'True
      Caption         =   "Close"
      Default         =   -1  'True
      Height          =   375
      Left            =   4680
      TabIndex        =   1
      Top             =   525
      Width           =   1215
   End
   Begin VB.CommandButton OKButton 
      Caption         =   "Add It!"
      CausesValidation=   0   'False
      Height          =   375
      Left            =   3420
      TabIndex        =   0
      Top             =   525
      Width           =   1215
   End
   Begin VB.Label lblSelectDrive 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   "Select CD Drive, where is your CD:"
      Height          =   195
      Left            =   105
      TabIndex        =   2
      Top             =   165
      Width           =   2475
   End
End
Attribute VB_Name = "frmAddCD"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Private Sub CancelButton_Click()
 Unload Me
End Sub

Private Sub Form_Load()
On Error Resume Next
With Me
    .lblSelectDrive.Caption = LoadResString(134)
    .OKButton.Caption = LoadResString(136)
    .CancelButton.Caption = LoadResString(137)
    .Caption = LoadResString(138)
    .drvDrive.Drive = GetSetting(App.Title, "Default", "Drive", "D:")
End With
End Sub

Private Sub lblSelectDrive_Click()
 Me.drvDrive.SetFocus
End Sub

Private Sub OKButton_Click()
 SaveSetting App.Title, "Default", "Drive", Me.drvDrive.Drive
 frmProgress.Show , Me
 frmProgress.DoCommand "addtodatabase " & Left(Me.drvDrive.Drive, Len("c:")) & "\"
End Sub

