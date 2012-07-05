VERSION 5.00
Begin VB.Form frmAddFolder 
   AutoRedraw      =   -1  'True
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "Add Folder as CD"
   ClientHeight    =   4365
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   6090
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   4365
   ScaleWidth      =   6090
   ShowInTaskbar   =   0   'False
   StartUpPosition =   3  'Windows Default
   Begin VB.DirListBox dirFolder 
      Height          =   3015
      Left            =   135
      TabIndex        =   5
      Top             =   780
      Width           =   5820
   End
   Begin VB.CommandButton OKButton 
      Caption         =   "Add It!"
      CausesValidation=   0   'False
      Height          =   375
      Left            =   3495
      TabIndex        =   2
      Top             =   3900
      Width           =   1215
   End
   Begin VB.CommandButton CancelButton 
      Cancel          =   -1  'True
      Caption         =   "Close"
      Default         =   -1  'True
      Height          =   375
      Left            =   4755
      TabIndex        =   1
      Top             =   3915
      Width           =   1215
   End
   Begin VB.DriveListBox drvDrive 
      Height          =   315
      Left            =   2670
      TabIndex        =   0
      Top             =   105
      Width           =   3255
   End
   Begin VB.Label lblSelectDrive 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   "Select CD Drive, where is your CD:"
      Height          =   195
      Left            =   135
      TabIndex        =   4
      Top             =   150
      Width           =   2475
   End
   Begin VB.Label lblAdding 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   "Select Folder:"
      Height          =   195
      Left            =   135
      TabIndex        =   3
      Top             =   480
      Width           =   975
   End
End
Attribute VB_Name = "frmAddFolder"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Private Sub CancelButton_Click()
 Unload Me
End Sub

Private Sub drvDrive_Change()
 On Error Resume Next
 Me.dirFolder.Path = Me.drvDrive.Drive
End Sub

Private Sub Form_Load()
On Error Resume Next
With Me
    .lblSelectDrive.Caption = LoadResString(134)
    .lblAdding.Caption = LoadResString(135)
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
 frmProgress.Show , Me
 frmProgress.DoCommand "addtodatabase " & Me.dirFolder.Path & "\"
End Sub
