Imports System.Windows.Forms

Namespace InventoryManager
    Friend Module Program
        <STAThread>
        Sub Main()
            ApplicationConfiguration.Initialize()
            Application.Run(New FormMain())
        End Sub
    End Module
End Namespace
