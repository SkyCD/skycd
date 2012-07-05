Public Class Vars

    Public Shared Function String2Enum(ByRef Str As String, ByRef Enu As System.Type) As Object
        Dim Rez As Object = Nothing
        For Each member As System.Reflection.FieldInfo In Enu.BaseType.GetFields
            If member.Name = Str Then Return member.GetValue(Nothing)
        Next
        Return Rez
    End Function

End Class
