Imports System.Windows.Forms

Namespace InventoryManager
    Public Partial Class FormMain
        Inherits Form

        Private products As New List(Of Product)()
        Private nextId As Integer = 1

        Public Sub New()
            InitializeComponent()
        End Sub

        ' フォーム読み込み時にサンプルデータを追加
        Private Sub FormMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            ' カテゴリのドロップダウン設定
            cmbCategory.Items.AddRange(New String() {"食品", "飲料", "日用品"})
            cmbCategory.SelectedIndex = 0

            ' サンプルデータ
            products.Add(New Product(nextId, "コーヒー豆", "食品", 50, 800))
            nextId += 1
            products.Add(New Product(nextId, "緑茶ペットボトル", "飲料", 3, 150))
            nextId += 1
            products.Add(New Product(nextId, "食器用洗剤", "日用品", 20, 350))
            nextId += 1
            products.Add(New Product(nextId, "カップラーメン", "食品", 100, 200))
            nextId += 1
            products.Add(New Product(nextId, "ミネラルウォーター", "飲料", 5, 100))
            nextId += 1

            RefreshGrid(products)
        End Sub

        ' 追加ボタン
        Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
            ' バリデーション
            If String.IsNullOrWhiteSpace(txtProductName.Text) Then
                MessageBox.Show("商品名を入力してください", "入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' 在庫数チェック
            Dim stock As Integer
            If Not Integer.TryParse(txtStock.Text, stock) OrElse stock < 0 Then
                MessageBox.Show("在庫数は0以上の整数で入力してください", "入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' 単価チェック
            ' ★★★ バグ3: 単価のチェックが >= 0 になっている ★★★
            ' 仕様 FR-007: 単価は1以上の整数
            ' 実装: 0以上のチェックになっており、単価0円が登録できてしまう
            Dim price As Integer
            If Not Integer.TryParse(txtPrice.Text, price) OrElse price < 0 Then
                MessageBox.Show("単価は0以上の整数で入力してください", "入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim product As New Product(nextId, txtProductName.Text, cmbCategory.SelectedItem.ToString(), stock, price)
            nextId += 1
            products.Add(product)

            ' 入力欄クリア
            txtProductName.Text = ""
            cmbCategory.SelectedIndex = 0
            txtStock.Text = ""
            txtPrice.Text = ""

            RefreshGrid(products)
        End Sub

        ' 編集ボタン
        Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
            If dgvInventory.SelectedRows.Count = 0 Then
                MessageBox.Show("商品を選択してください", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' バリデーション
            If String.IsNullOrWhiteSpace(txtProductName.Text) Then
                MessageBox.Show("商品名を入力してください", "入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim stock As Integer
            If Not Integer.TryParse(txtStock.Text, stock) OrElse stock < 0 Then
                MessageBox.Show("在庫数は0以上の整数で入力してください", "入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim price As Integer
            If Not Integer.TryParse(txtPrice.Text, price) OrElse price < 0 Then
                MessageBox.Show("単価は0以上の整数で入力してください", "入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim selectedId As Integer = CInt(dgvInventory.SelectedRows(0).Cells("Id").Value)
            Dim product As Product = products.Find(Function(p) p.Id = selectedId)

            If product IsNot Nothing Then
                product.ProductName = txtProductName.Text
                product.Category = cmbCategory.SelectedItem.ToString()
                product.Stock = stock
                product.UnitPrice = price

                RefreshGrid(products)
            End If
        End Sub

        ' 削除ボタン
        Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
            If dgvInventory.SelectedRows.Count = 0 Then
                MessageBox.Show("商品を選択してください", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim selectedId As Integer = CInt(dgvInventory.SelectedRows(0).Cells("Id").Value)
            Dim product As Product = products.Find(Function(p) p.Id = selectedId)

            If product IsNot Nothing Then
                Dim result As DialogResult = MessageBox.Show(
                    product.ProductName & "を削除しますか？",
                    "削除確認",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question)

                If result = DialogResult.Yes Then
                    products.Remove(product)
                    RefreshGrid(products)
                End If
            End If
        End Sub

        ' 検索ボタン
        Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
            Dim keyword As String = txtSearch.Text

            If String.IsNullOrWhiteSpace(keyword) Then
                RefreshGrid(products)
                Return
            End If

            Dim results As List(Of Product) = products.FindAll(
                Function(p) p.ProductName.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)

            If results.Count = 0 Then
                MessageBox.Show("該当する商品はありません", "検索結果", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

            RefreshGrid(results)
        End Sub

        ' DataGridView選択変更時に入力欄に値をセット
        Private Sub dgvInventory_SelectionChanged(sender As Object, e As EventArgs) Handles dgvInventory.SelectionChanged
            If dgvInventory.SelectedRows.Count > 0 Then
                Dim row As DataGridViewRow = dgvInventory.SelectedRows(0)
                txtProductName.Text = If(row.Cells("ProductName").Value?.ToString(), "")
                Dim categoryValue As String = If(row.Cells("Category").Value?.ToString(), "")
                Dim categoryIndex As Integer = cmbCategory.Items.IndexOf(categoryValue)
                If categoryIndex >= 0 Then
                    cmbCategory.SelectedIndex = categoryIndex
                End If
                txtStock.Text = If(row.Cells("Stock").Value?.ToString(), "")
                txtPrice.Text = If(row.Cells("UnitPrice").Value?.ToString(), "")
            End If
        End Sub

        ' 一覧を更新
        Private Sub RefreshGrid(displayList As List(Of Product))
            dgvInventory.DataSource = Nothing
            dgvInventory.DataSource = displayList.Select(
                Function(p) New With {
                    p.Id,
                    p.ProductName,
                    p.Category,
                    p.Stock,
                    p.UnitPrice,
                    p.StockValue
                }).ToList()

            ' 列ヘッダーの表示名を設定
            If dgvInventory.Columns.Count > 0 Then
                dgvInventory.Columns("Id").HeaderText = "商品ID"
                dgvInventory.Columns("ProductName").HeaderText = "商品名"
                dgvInventory.Columns("Category").HeaderText = "カテゴリ"
                dgvInventory.Columns("Stock").HeaderText = "在庫数"
                dgvInventory.Columns("UnitPrice").HeaderText = "単価"
                dgvInventory.Columns("StockValue").HeaderText = "在庫金額"
            End If

            ' ★★★ バグ2: 在庫警告の表示処理がない ★★★
            ' 仕様 FR-008: 在庫数が5以下の商品は行を黄色で表示する
            ' 実装: 黄色表示の処理が完全に欠落している
            ' → 在庫が少ない商品が視覚的に分からない

            UpdateStatus(displayList)
        End Sub

        ' ステータスバー更新
        Private Sub UpdateStatus(displayList As List(Of Product))
            Dim count As Integer = displayList.Count
            Dim totalStockValue As Integer = displayList.Sum(Function(p) p.StockValue)

            lblStatus.Text = $"商品数: {count}件  在庫金額合計: {totalStockValue:N0}円"
        End Sub
    End Class
End Namespace
