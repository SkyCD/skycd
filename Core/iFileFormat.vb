Imports System.Collections.Generic

Public Interface iFileFormat
    Inherits ISkyCDPlugIn

    Function IsSupported(ByVal Filename As String) As Boolean

    Function GetSupportedFileFormats() As List(Of String)

    Sub Load(ByVal FileName As String)
    Sub Save(ByVal FileName As String)

    Property Database() As Database.iConnection
    Property ApplicationGUID() As String

    ReadOnly Property CanSaveSize() As Boolean
    ReadOnly Property CanOpenSize() As Boolean
    ReadOnly Property CanSaveExtentedInfo() As Boolean
    ReadOnly Property CanReadExtendedInfo() As Boolean
    ReadOnly Property IsExactFormat() As Boolean
    ReadOnly Property CanDo() As iFileFormat.scdCanDo

    Enum scdCanDo As Byte
        scdWrite = 1
        scdRead = 2
        scdReadWrite = 0
    End Enum

    Structure scdStatus
        Enum scdProcedure As Byte
            scdLoading = 0
            scdSaving = 1
            scdDone = 2
            scdExporting = 3
            scdImporting = 4
        End Enum
        Dim scdEvent As scdStatus.scdProcedure
        Dim scdValue As Byte
    End Structure

    Enum scdError As Byte
        scdWrongFileFormat = 0
        scdCantOpen = 1
        scdCantSave = 2
        scdCantConnectToDatabase = 3
    End Enum

    Structure scdFileFormatsItem
        Dim scdExtentions As List(Of String)
        Dim scdFileVersion As String
        Dim scdName As String
    End Structure

    Event UpdateStatus(ByVal e As scdStatus)
    Event WasError(ByVal e As scdError)
    Event NeedDoEvents()
    Event DebugWrite(ByVal Text)

End Interface
