Attribute VB_Name = "modTypes"
Public Type skyFileProperties
     AppVersion As String * 128
     AppName As String * 128
     LastSaved As Date
     LinesCount As Double
     CreatorName As String * 128
     CreatorsEMail As String * 128
     CreatorsWebPage As String * 256
     Comment As String * 512
     AdditionalInfo As String * 4096
End Type

Public Type skyCDsTreeEntry
    Parent As String '* 2048
    Key As String '* 2048
    Text As String '* 255
End Type

Public Type skySettingsListFormat
    Caption As String
    Type As String
End Type
