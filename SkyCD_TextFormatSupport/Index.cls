VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "Index"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Public Obj As Object

Enum skyPluginType
    skyUnknown = 0
    skyFormatSupport = 1
End Enum

Type skyPlugInInfo
    PlugInType As skyPluginType
    PlugInName As String
    SupportedExtentions() As String
End Type

Property Get Info() As skyPlugInInfo
  Info.PlugInType = skyFormatSupport
  Info.PlugInName = "Text Format Support for SkyCD"
  ReDim Info.SupportedExtentions(0 To 0)
  Info.SupportedExtentions(0) = "scd"
End Property

Function CanOpen(Filename As String) As Boolean
Dim L As Integer
   Dim Text As String
   L = FreeFile
   Open Filename For Input As #L
     Line Input #L, Text
   Close #L
   CanOpen = EqLeftStringNC(Text, "[") Or EqLeftStringNC(Text, "(") _
             Or EqLeftStringNC(Text, "{")
End Function

Sub OpenFile(Filename As String)
    Dim L As Integer
    Dim I As Integer
    Dim Text As String
    Let L = 255
    I = 0
    Filename = RemoveUnused(Filename, """")
    Open Filename For Input As #L
    Do
       Line Input #L, Text
       Me.Obj.AddToTree Text
       I = I + 1
       Let Obj.GCPS = I
       If I Mod 20 = 0 Then DoEvents
    Loop Until EOF(L)
    Close #L
End Sub

Sub SaveFile(Filename As String, Mas() As String)
 Dim L As Integer
 Dim I As Integer
 Dim Text As String
    Let L = 255
    Open Filename For Output As #L
    For I = LBound(Mas) To UBound(Mas)
        Print #L, Mas(I)
        Let Obj.GCPS = I
    Next I
    Close #L
End Sub
