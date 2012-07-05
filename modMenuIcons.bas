Attribute VB_Name = "modMenuIcons"
    
Private Declare Function GetMenu Lib "user32" _
   (ByVal hwnd As Long) As Long

Private Declare Function GetSubMenu Lib "user32" _
   (ByVal hMenu As Long, ByVal nPos As Long) As Long

Private Declare Function SetMenuItemBitmaps Lib "user32" _
   (ByVal hMenu As Long, ByVal nPosition As Long, ByVal wFlags As Long, _
    ByVal hBitmapUnchecked As Long, ByVal hBitmapChecked As Long) As Long

Const MF_BYPOSITION = &H400&
Public Const MF_APPEND = &H100&
Public Const MF_BITMAP = &H4&
Public Const MF_OWNERDRAW = &H100&
Public Const MF_UNHILITE = &H0&

Sub SetMenuPicture(frm As Object, Index1 As Long, Index2 As Long, Picture)
  Dim mHandle As Long, lRet As Long, sHandle As Long, sHandle2 As Long
  'Dim Picture As Long
  'Picture = frm.imgMenu.ListImages.Item(PictureIndex).Picture
  mHandle = GetMenu(frm.hwnd)
  sHandle = GetSubMenu(mHandle, Index1)
  lRet = SetMenuItemBitmaps(sHandle, Index2, MF_BYPOSITION, Picture, Picture)
  Debug.Print Picture
End Sub


