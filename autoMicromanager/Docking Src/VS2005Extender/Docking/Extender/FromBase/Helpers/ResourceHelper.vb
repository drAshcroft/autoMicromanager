' *****************************************************************************
' 
'  Copyright 2004, Weifen Luo
'  All rights reserved. The software and associated documentation 
'  supplied hereunder are the proprietary information of Weifen Luo
'  and are supplied subject to licence terms.
' 
'  WinFormsUI Library Version 1.0
' *****************************************************************************
Imports System
Imports System.Drawing
Imports System.Reflection
Imports System.Resources
Imports System.Windows.Forms
Imports WeifenLuo.WinFormsUI
Namespace VS2005Style
    Friend Class ResourceHelper
        Private Shared m_resourceManager As ResourceManager
        Shared Sub New()
            m_resourceManager = New ResourceManager("VS2005Extender.Resources", GetType(Extender).Assembly)
        End Sub
        Public Shared Function LoadBitmap(ByVal name As String) As Bitmap
            Dim assembly As Assembly = GetType(DockPanel).Assembly
            Dim fullNamePrefix As String = "WeifenLuo.WinFormsUI.Resources."
            Return New Bitmap(assembly.GetManifestResourceStream(fullNamePrefix & name))
        End Function
        Public Shared Function LoadExtenderBitmap(ByVal name As String) As Bitmap
            Dim assembly As Assembly = GetType(Extender).Assembly
            Return New Bitmap(assembly.GetManifestResourceStream("VS2005Extender." & name))
        End Function
        Public Shared Function GetString(ByVal name As String) As String
            Dim assembly As Assembly = GetType(Extender).Assembly
            Return m_resourceManager.GetString(name)
        End Function
    End Class
End Namespace
