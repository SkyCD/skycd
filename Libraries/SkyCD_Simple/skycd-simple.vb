Public Class skycd_simple

    Public Enum scdItemType As Byte
        scdFile = 0
        scdFolder = 1
        scdCDDisk = 2
        scdUnknown = 4
        scdNetworkRecource = 5
    End Enum

    Public Structure scdItem
        Dim Name As String
        Dim ParentID As Integer
        Dim ItemType As scdItemType
        Dim Size As Long
        Dim AdvancedInfo As scdProperties
    End Structure

    Public Items(0) As scdItem
    Private col As New Collection

    Public Function Add(ByVal Name As String, Optional ByVal ItemType As scdItemType = scdItemType.scdUnknown, Optional ByVal ParentID As Integer = -1) As Integer
        Dim Count As Integer
        Dim key As String = "/" + ParentID.ToString + "/" + Name
        Count = UBound(Me.Items) - LBound(Me.Items) + 1
        ReDim Preserve Me.Items(Count)
        With Me.Items(Count - 1)
            .ItemType = ItemType
            .Name = Name
            .ParentID = ParentID
            .AdvancedInfo = New scdProperties()
        End With
        col.Add(Count - 1, key)
        Return Count - 1
    End Function

    Public Function Remove(ByVal ID As Integer) As Boolean
        Try
            Dim Count As Integer = UBound(Me.Items) - LBound(Me.Items)
            Dim key As String = "/" + Me.Items(ID).ParentID.ToString + "/" + Me.Items(ID).Name
            Dim mf(Count - 1) As scdItem
            col.Remove(key)
            Dim I As Integer, K As Integer
            K = 0
            For I = 0 To Count
                If Not (I = ID) Then
                    mf(K) = Me.Items(I)
                    K = K + 1
                End If
            Next
            ReDim Me.Items(Count - 1)
            Me.Items = mf
            Return True
        Catch
            Return False
        End Try
    End Function

    Public Function Exists(ByVal Name As String, Optional ByVal ParentID As Integer = -1) As Integer
        Dim i As Integer
        Dim key As String = "/" + ParentID.ToString + "/" + Name
        If col.Contains(key) Then
            Return Val(col.Item(key))
        End If
        For i = 0 To (UBound(Me.Items) - LBound(Me.Items)) - 1
            If (Me.Items(i).ParentID = ParentID) And (Me.Items(i).Name = Name) Then
                col.Add(i, key)
                Return i
            End If
        Next
        Return -1
    End Function

    Public Function AddPath(ByVal Path As String, Optional ByVal ParentID As Integer = -1, Optional ByVal FirstItemType As scdItemType = scdItemType.scdFile, Optional ByVal LastItemType As scdItemType = scdItemType.scdFile) As Integer
        Dim Masyvas() As String = Split(Path, "\")
        Dim Vieta As Integer
        Dim L As Integer
        Me.Finalize()
        If Me.Exists(Masyvas(LBound(Masyvas)), -1) = -1 Then
            Me.Add(Masyvas(LBound(Masyvas)), FirstItemType)
        Else
            ParentID = Me.Exists(Masyvas(L), ParentID)
        End If
        For L = LBound(Masyvas) + 1 To UBound(Masyvas) - 1
            Vieta = Me.Exists(Masyvas(L), ParentID)
            If Vieta > -1 Then
                ParentID = Vieta
                If Me.Items(Vieta).ItemType = scdItemType.scdFile Then
                    Me.Items(Vieta).ItemType = scdItemType.scdFolder
                End If
            Else
                ParentID = Me.Add(Masyvas(L), scdItemType.scdFolder, ParentID)
            End If
        Next
        If Me.Exists(Masyvas(UBound(Masyvas)), ParentID) = -1 Then
            Return Me.Add(Masyvas(UBound(Masyvas)), LastItemType, ParentID)
        End If
        Return -1
    End Function

    Sub Clear()
        ReDim Me.Items(0)
        col.Clear()
    End Sub

End Class