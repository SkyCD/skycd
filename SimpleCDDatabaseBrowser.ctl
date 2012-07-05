VERSION 5.00
Object = "{831FDD16-0C5C-11D2-A9FC-0000F8754DA1}#2.0#0"; "MSCOMCTL.OCX"
Begin VB.UserControl SimpleCDDatabaseBrowser 
   ClientHeight    =   3600
   ClientLeft      =   0
   ClientTop       =   0
   ClientWidth     =   4800
   ScaleHeight     =   3600
   ScaleWidth      =   4800
   Begin MSComctlLib.TreeView lstProgress 
      Height          =   2190
      Left            =   30
      TabIndex        =   0
      Top             =   30
      Width           =   3450
      _ExtentX        =   6085
      _ExtentY        =   3863
      _Version        =   393217
      LabelEdit       =   1
      Sorted          =   -1  'True
      Style           =   7
      Appearance      =   1
      BeginProperty Font {0BE35203-8F91-11CE-9DE3-00AA004BB851} 
         Name            =   "Lucida Console"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
   End
End
Attribute VB_Name = "SimpleCDDatabaseBrowser"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Dim cNeedSave As Boolean
Dim aInfo As skyFileProperties
Dim dFileFormat As skySkyCDFileTypes
Public GCPS As Integer

Event Loading(FoundedItems As Integer)
Event CantOpen(Filename As String)

Public Enum skySkyCDFileTypes
     skyUnknownFile = -1
     skyListFile = 0
     skyBinaryFormatFile = 1
     'skyMicrosoftAccessFile = 3
End Enum

Public Property Get NeedSave() As Boolean
  NeedSave = cNeedSave
End Property

Public Property Get MyNodes(Index As Long) As Node
  Set MyNodes = UserControl.lstProgress.Nodes(Index)
End Property

Public Property Let MyNodes(Index As Long, Value As Node)
  UserControl.lstProgress.Nodes(Index) = Value
End Property

Public Property Get ListCount() As Long
  ListCount = UserControl.lstProgress.Nodes.Count
End Property

Private Sub UserControl_Resize()
With UserControl
    .lstProgress.Left = 0
    .lstProgress.Top = 0
    .lstProgress.Width = .ScaleWidth
    .lstProgress.Height = .ScaleHeight
End With
End Sub

Sub AddItem(CDCaption As String, Path As String, Filename As String, FileSize As Double)
    Let cNeedSave = True
    Path = RemoveUnused(Path, "\")
    Filename = RemoveUnused(Filename, "\")
    If Path <> "" Then Path = Path & "\"
    Me.AddToTree "[" & CDCaption & "]\" & Path & "[" & FormatFileSize(FileSize) & "] " & Filename
End Sub

Sub AddTittleOnly(CDCaption As String)
    Me.AddToTree "[" & CDCaption & "]\" & "[" & FormatToFixedLengthText(LoadResString(160), 16) & "] *.*"
    cNeedSave = True
End Sub

Property Get CurrentProgressState() As Integer
 CurrentProgressState = GCPS
End Property

Sub FileSave(Filename As String, Optional fFormat)
    'On Error Resume Next
    Dim L As Integer
    Dim I As Integer
'    MsgBox fFormat
    If IsMissing(fFormat) Then fFormat = dFileFormat
    Select Case fFormat
        Case skyListFile
            Let cNeedSave = False
            Dim Clase As New SkyCD_TextFormatSupport.Index
            Dim max As Integer
            max = UserControl.lstProgress.Nodes.Count
            Dim Masyvas() As String
            ReDim Masyvas(1 To max)
            For I = 1 To UserControl.lstProgress.Nodes.Count
               Masyvas(I) = UserControl.lstProgress.Nodes.Item(I).FullPath
            Next I
            Set Clase.Obj = Me
            Clase.SaveFile Filename, Masyvas()
        Case skyBinaryFormatFile
            Dim Header As String * 127
            Dim T2() As skyCDsTreeEntry
            ReDim T2(1 To UserControl.lstProgress.Nodes.Count)
            Let cNeedSave = False
            Let L = FreeFile(255)
            Open Filename For Binary As #L
            Let Header = "# SkyCD File" + vbCrLf
            Let aInfo.LastSaved = Now
            Let aInfo.AppName = App.ProductName
            Let aInfo.AppVersion = App.Revision & " release"
            Let aInfo.LinesCount = UserControl.lstProgress.Nodes.Count
            On Error Resume Next
            For I = 1 To UserControl.lstProgress.Nodes.Count
                T2(I).Key = UserControl.lstProgress.Nodes.Item(I).Key
                T2(I).Parent = UserControl.lstProgress.Nodes.Item(I).Parent.Key
                T2(I).Text = UserControl.lstProgress.Nodes.Item(I).Text
                Let GCPS = I
            Next I
            Put #L, 1, Header
            Put #L, , aInfo
            Put #L, , T2
            Close #L
    End Select
End Sub

Sub FileOpen(Filename As String)
    'On Error Resume Next
    Dim L As Integer
    Dim I As Integer
    Dim Text As String
'    MsgBox GetFileType(Filename)
    Select Case GetFileType(Filename)
        Case skyListFile
            Let cNeedSave = False
            Let dFileFormat = skyListFile
            Me.Clear
            Dim Clase As New SkyCD_TextFormatSupport.Index
            Set Clase.Obj = Me
            Clase.OpenFile Filename
        Case skyBinaryFormatFile
            Dim Header As String * 127
            Dim T2() As skyCDsTreeEntry
            Let cNeedSave = False
            Let dFileFormat = skyBinaryFormatFile
            Let L = FreeFile(255)
            I = 0
            Me.Clear
            Filename = RemoveUnused(Filename, """")
            Open Filename For Binary As #L
            Get #L, 1, Header
            Get #L, , aInfo
            ReDim T2(1 To aInfo.LinesCount)
            Get #L, , T2
            'On Error Resume Next
            For I = 1 To aInfo.LinesCount
                If Not (T2(I).Parent = "") Then
                    'UserControl.lstProgress.Nodes.Add , , T2(I).Parent, T2(I).Text
                    UserControl.lstProgress.Nodes.Add T2(I).Parent, tvwChild, T2(I).Key, T2(I).Text
                Else
                    UserControl.lstProgress.Nodes.Add , , T2(I).Key, T2(I).Text
                End If
                If I Mod 20 = 0 Then DoEvents
                'RaiseEvent Loading(I)
                Let GCPS = I
            Next I
            Close #L
        Case skyUnknownFile
            RaiseEvent CantOpen(Filename)
    End Select
End Sub

Function GetFileType(Filename As String) As skySkyCDFileTypes
 On Error Resume Next
   Dim L As Integer
   Dim Text As String
   L = FreeFile
   Open Filename For Input As #L
     Line Input #L, Text
   Close #L
   If EqLeftStringNC(Text, "# SkyCD File") Then
      Let GetFileType = skyBinaryFormatFile
      Exit Function
   End If
   If EqLeftStringNC(Text, "[") Or EqLeftStringNC(Text, "(") _
      Or EqLeftStringNC(Text, "{") Then
      Let GetFileType = skyListFile
      Exit Function
   End If
   Let GetFileType = skyUnknownFile
End Function

Sub AddToTree(Text As String)
  On Error Resume Next
  Dim Data() As String
  Dim I As Long, Rel As String
  Dim O As Long, Key As String
  Dim R2 As TreeRelationshipConstants
  Data = Split(Text, "\")
  Rel = ""
  Key = Rel
  R2 = tvwFirst
  With UserControl.lstProgress.Nodes
      For I = LBound(Data) To UBound(Data)
          If I > LBound(Data) Then
             Rel = Key
             R2 = tvwChild
          End If
             Key = ToXCode(Key & Data(I))
          If R2 = tvwFirst Then
             .Add , R2, Key, Data(I)
          Else
             .Add Rel, R2, Key, Data(I)
          End If
      Next I
  End With
End Sub

Sub Clear()
    UserControl.lstProgress.Nodes.Clear
End Sub

Sub DeleteGroup()
 On Error Resume Next
    Dim I As Integer, GroupName As String, Obj As Object
    Dim Data() As String
    Let cNeedSave = True
    Data = Split(UserControl.lstProgress.SelectedItem.FullPath, "\")
    GroupName = ToXCode(Data(LBound(Data)))
    UserControl.lstProgress.Nodes.Remove GroupName
End Sub
