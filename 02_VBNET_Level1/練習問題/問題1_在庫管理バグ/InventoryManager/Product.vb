Namespace InventoryManager
    Public Class Product
        Public Property Id As Integer
        Public Property ProductName As String = String.Empty
        Public Property Category As String = String.Empty
        Public Property Stock As Integer
        Public Property UnitPrice As Integer

        ' ★★★ バグ1: 在庫金額の計算が加算になっている ★★★
        ' 仕様 FR-004: 在庫金額 = 在庫数 × 単価
        ' 実装: 在庫金額 = 在庫数 + 単価 になっている
        Public ReadOnly Property StockValue As Integer
            Get
                Return Stock + UnitPrice
            End Get
        End Property

        Public Sub New(id As Integer, productName As String, category As String, stock As Integer, unitPrice As Integer)
            Me.Id = id
            Me.ProductName = productName
            Me.Category = category
            Me.Stock = stock
            Me.UnitPrice = unitPrice
        End Sub
    End Class
End Namespace
