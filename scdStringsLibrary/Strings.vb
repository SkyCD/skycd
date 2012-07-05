Public Class Strings

    Public Shared Function FindLast(ByVal Text As String, ByVal What As String)
        Return InStrRev(Text, What)        
    End Function

    ' http://home.att.net/~codeLibrary/XML/serialization_p.htm
    Public Shared Function SerializeToText(ByVal obj As Object) As System.String
        If IsNothing(obj) Then Return ""
        Dim StringWriter As New System.IO.StringWriter
        Dim XmlSerializer As New System.Xml.Serialization.XmlSerializer(obj.GetType)
        XmlSerializer.Serialize(StringWriter, obj)
        Return StringWriter.ToString
    End Function

    Public Shared Function UnSerializeFromText(ByVal Text As System.String, ByVal type As System.Type) As Object
        Dim Sr As New System.IO.StringReader(Text)
        Dim XmlSerializer As New System.Xml.Serialization.XmlSerializer(type)
        Return XmlSerializer.Deserialize(Sr)
    End Function

    Public Shared Function AddSlashes(ByVal Text As String) As String
        Return Text.Replace("'", "''")
    End Function

    Public Shared Function ToBase64(ByVal Text As String) As String
        Dim a As New System.Text.ASCIIEncoding
        Return Convert.ToBase64String(a.GetBytes(Text))
    End Function

    Public Shared Function FromBase64(ByVal Text As String) As String
        Dim a As New System.Text.ASCIIEncoding
        Dim bufferStream As New System.IO.MemoryStream(Convert.FromBase64String(Text))
        Dim Filter As New System.IO.StreamReader(bufferStream)
        Return Filter.ReadToEnd
    End Function

    Public Shared Function UrlComp(ByVal Url As String) As String
        Dim Txt As String = ""
        For Each That As Char In Url.ToCharArray()
            Select Case That
                Case "a" To "z"
                    Txt = Txt & That.ToString
                Case "A" To "Z"
                    Txt = Txt & That.ToString
                Case Else
                    Txt = Txt & "%" & Asc(That).ToString
            End Select
        Next
        Return Txt
    End Function

End Class
