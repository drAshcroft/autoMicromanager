Attribute VB_Name = "NetImageConverter"
Option Explicit
Private Type Memorybitmap
    hdc As Long
    hBM As Long
    oldhDC As Long
    wid As Long
    hgt As Long
End Type
Private Type RECT
   Left As Long
   Top As Long
   Right As Long
   Bottom As Long
End Type

Private Declare Function GetStockObject Lib "gdi32" (ByVal nIndex As Long) As Long

Private Type BITMAPINFOHEADER '40 bytes
    biSize As Long
    biWidth As Long
    biHeight As Long
    biPlanes As Integer
    biBitCount As Integer
    biCompression As Long
    biSizeImage As Long
    biXPelsPerMeter As Long
    biYPelsPerMeter As Long
    biClrUsed As Long
    biClrImportant As Long
End Type

      Private Type RGBQUAD
         rgbBlue As Byte
         rgbGreen As Byte
         rgbRed As Byte
         rgbReserved As Byte
      End Type

Private Type BITMAPINFO
    bmiHeader As BITMAPINFOHEADER
    bmiColors As RGBQUAD
End Type

   
Private Declare Function BitBlt Lib "gdi32" _
  (ByVal hDCDest As Long, ByVal XDest As Long, _
   ByVal YDest As Long, ByVal nWidth As Long, _
   ByVal nHeight As Long, ByVal hDCSrc As Long, _
   ByVal xSrc As Long, ByVal ySrc As Long, _
   ByVal dwRop As Long) As Long

Private Declare Function CreateBitmap Lib "gdi32" _
  (ByVal nWidth As Long, _
   ByVal nHeight As Long, _
   ByVal nPlanes As Long, _
   ByVal nBitCount As Long, _
   lpBits As Any) As Long

Private Declare Function SetBkColor Lib "gdi32" _
   (ByVal hdc As Long, _
    ByVal crColor As Long) As Long

Private Declare Function SelectObject Lib "gdi32" _
  (ByVal hdc As Long, _
   ByVal hObject As Long) As Long

Private Declare Function CreateCompatibleBitmap Lib "gdi32" _
  (ByVal hdc As Long, _
   ByVal nWidth As Long, _
   ByVal nHeight As Long) As Long

Private Declare Function CreateCompatibleDC Lib "gdi32" _
   (ByVal hdc As Long) As Long

Private Declare Function DeleteDC Lib "gdi32" _
   (ByVal hdc As Long) As Long

Private Declare Function DeleteObject Lib "gdi32" (ByVal hObject As Long) As Long



Private Declare Function StretchBlt Lib "gdi32" (ByVal hdc As Long, ByVal x As Long, ByVal y As Long, ByVal nWidth As Long, ByVal nHeight As Long, ByVal hSrcDC As Long, ByVal xSrc As Long, ByVal ySrc As Long, ByVal nSrcWidth As Long, ByVal nSrcHeight As Long, ByVal dwRop As Long) As Long







Type BITMAP    '24 bytes
  bmType As Long
  bmWidth As Long
  bmHeight As Long
  bmWidthBytes As Long
  bmPlanes As Integer
  bmBitsPixel As Integer
  bmBits As Long
End Type



Type COLORQUAD
  rgbB As Byte
  rgbG As Byte
  rgbR As Byte
  rgbP As Byte
End Type

Private Const BI_RGB = 0&
Private Const DIB_RGB_COLORS = 0&
Private Const LR_LOADFROMFILE = &H10
Private Const IMAGE_BITMAP = 0&

Private Declare Function GetDIBits Lib "gdi32" (ByVal hdc As Long, ByVal hBitmap As Long, ByVal nStartScan As Long, ByVal nNumScans As Long, lpBits As Any, lpBI As BITMAPINFO, ByVal wUsage As Long) As Long
Private Declare Function LoadImage Lib "user32" Alias "LoadImageA" (ByVal hInst As Long, ByVal lpsz As String, ByVal un1 As Long, ByVal n1 As Long, ByVal n2 As Long, ByVal un2 As Long) As Long
Private Declare Function CreateDIBSection Lib "gdi32" (ByVal hdc As Long, pBitmapInfo As BITMAPINFOHEADER, ByVal un As Long, ByRef lpVoid As Any, ByVal handle As Long, ByVal dw As Long) As Long



Private Declare Function GetObject Lib "gdi32" Alias "GetObjectA" (ByVal hObject As Long, ByVal nCount As Long, lpObject As Any) As Long

Private Declare Function SetDIBits Lib "gdi32" (ByVal hdc As Long, _
        ByVal hBitmap As Long, ByVal nStartScan As Long, ByVal nNumScans As Long, _
         lpBits As Any, lpBI As BITMAPINFO, ByVal wUsage As Long) As Long

  



Public Sub ConvertCoreImageToStdPicture(Image As CoreImage, PictureBox1 As PictureBox)
   Dim MemMap As Memorybitmap
  ' MemMap = MakeMemoryBitmap(Image.Width, Image.Height)
  MemMap.hBM = Image.ImageRGB.gethbitmap
  MemMap.wid = Image.Width
  MemMap.hgt = Image.Height

   Dim Data() As Byte
   Data = Image.GetRawDataArray
   Call DrawArray(MemMap, Data, PictureBox1)
   Call DeleteMemoryBitmap(MemMap)
End Sub


' Make a memory bitmap of the given size.
' Return the bitmap's DC.
Private Function MakeMemoryBitmap(ByVal wid As Long, ByVal hgt As Long) As Memorybitmap
Dim result As Memorybitmap

    ' Create the device context.
    result.hdc = CreateCompatibleDC(0)

    ' Create the bitmap.
    result.hBM = CreateBitmap(wid, hgt, 1, 32, 0) 'CreateCompatibleBitmap(result.hdc, wid,         hgt)

    ' Make the device context use the bitmap.
    result.oldhDC = SelectObject(result.hdc, result.hBM)

    ' Return the MemoryBitmap structure.
    result.wid = wid
    result.hgt = hgt
    MakeMemoryBitmap = result
End Function


' Convert a color image to gray scale.
Private Sub DrawArray(memory_bitmap As Memorybitmap, Data() As Byte, Picture1 As PictureBox)
  Dim hand As Long, oldhand As Long
  Dim bmap As BITMAP
  Dim srcewid As Long, srcehgt As Long
  Dim srcedibbmap As BITMAPINFO
  Dim BytesPerScanLine As Long
  Dim PadBytesPerScanLine As Long
  Dim icol As Integer, irow As Integer
  Dim lsuccess As Long
  Dim hdcNew As Long
  Dim srceqarr() As COLORQUAD
  Dim thiscolor As COLORQUAD

  'Load bitmap data from disk
  hand = memory_bitmap.hBM

  'Fill out the BITMAP structure.
  lsuccess = GetObject(hand, Len(bmap), bmap)

  'Create a device context compatible with the Desktop.
  hdcNew = CreateCompatibleDC(0&)

  'Select the bitmap handle into the new device context.
  oldhand = SelectObject(hdcNew, hand)

  'Get the source bitmap width and height, in pixels, from BITMAP
  'structure.
  srcewid = memory_bitmap.wid
  srcehgt = memory_bitmap.hgt  '  bmap.bmHeight

 'to differentiate it from RGBQUAD.
  ReDim srceqarr(1 To srcewid, 1 To srcehgt)


  'srcedibbmap has been dimensioned as BITMAPINFO structure so
  'fill it out to create a template.
  'Two useful equations are those for BytesPerScanLine and
  'PadBytesPerScanLine. They work for any bit depth.
  'PadBytesPerScanLine will always be zero with biBiCount = 32
  'biheight is set negative to invert the "bottom up" scanline
  'reading.

  With srcedibbmap.bmiHeader
    .biSize = 40
    .biWidth = srcewid
    .biHeight = -srcehgt
    .biPlanes = 1
    .biBitCount = 32
    .biCompression = BI_RGB
    BytesPerScanLine = ((((.biWidth * .biBitCount) + 31) \ 32) * 4)
    PadBytesPerScanLine = _
      BytesPerScanLine - (((.biWidth * .biBitCount) + 7) \ 8)
    .biSizeImage = BytesPerScanLine * Abs(.biHeight)
  End With
   

  'Get color data from the source into a dib based on the template
 ' lsuccess = GetDIBits(hdcNew, hand, 0, srcehgt, srceqarr(1, 1), _
  '           srcedibbmap, DIB_RGB_COLORS)
  'lsuccess = SetDIBits(hdcNew, hand, 0, UBound(Data), Data(0), srcedibbmap, DIB_RGB_COLORS)

  'StretchBlt to PictureBox so we can see the result
  lsuccess = StretchBlt(Picture1.hdc, _
             0, 0, Picture1.ScaleWidth, _
             Picture1.ScaleHeight, _
             hdcNew, _
             0, 0, srcewid, srcehgt, _
             vbSrcCopy)

  Picture1.Refresh
 'Set Picture1.Picture = PictureFromHandle(hbmpPic, vbPicTypeBitmap, True)
  'Clean up
  SelectObject hdcNew, oldhand
  DeleteObject hand
  DeleteDC hdcNew

  'Clean up
  SelectObject hdcNew, oldhand
  DeleteObject hand
  DeleteDC hdcNew
   
End Sub

' Delete the bitmap and free its resources.
Private Sub DeleteMemoryBitmap(memory_bitmap As _
    Memorybitmap)
    SelectObject memory_bitmap.hdc, memory_bitmap.oldhDC
    DeleteObject memory_bitmap.hBM
    DeleteDC memory_bitmap.hdc
End Sub

