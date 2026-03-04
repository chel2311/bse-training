Namespace InventoryManager
    Partial Class FormMain

        Private components As System.ComponentModel.IContainer = Nothing

        Protected Overrides Sub Dispose(disposing As Boolean)
            If disposing AndAlso (components IsNot Nothing) Then
                components.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

#Region "Windows Form Designer generated code"

        Private Sub InitializeComponent()
            Me.txtProductName = New System.Windows.Forms.TextBox()
            Me.cmbCategory = New System.Windows.Forms.ComboBox()
            Me.txtStock = New System.Windows.Forms.TextBox()
            Me.txtPrice = New System.Windows.Forms.TextBox()
            Me.txtSearch = New System.Windows.Forms.TextBox()
            Me.btnAdd = New System.Windows.Forms.Button()
            Me.btnEdit = New System.Windows.Forms.Button()
            Me.btnDelete = New System.Windows.Forms.Button()
            Me.btnSearch = New System.Windows.Forms.Button()
            Me.dgvInventory = New System.Windows.Forms.DataGridView()
            Me.lblStatus = New System.Windows.Forms.Label()
            Me.lblProductName = New System.Windows.Forms.Label()
            Me.lblCategory = New System.Windows.Forms.Label()
            Me.lblStock = New System.Windows.Forms.Label()
            Me.lblPrice = New System.Windows.Forms.Label()
            Me.lblSearch = New System.Windows.Forms.Label()
            CType(Me.dgvInventory, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()

            ' lblProductName
            Me.lblProductName.AutoSize = True
            Me.lblProductName.Location = New System.Drawing.Point(20, 20)
            Me.lblProductName.Name = "lblProductName"
            Me.lblProductName.Size = New System.Drawing.Size(52, 15)
            Me.lblProductName.Text = "商品名:"

            ' txtProductName
            Me.txtProductName.Location = New System.Drawing.Point(100, 17)
            Me.txtProductName.Name = "txtProductName"
            Me.txtProductName.Size = New System.Drawing.Size(200, 23)

            ' lblCategory
            Me.lblCategory.AutoSize = True
            Me.lblCategory.Location = New System.Drawing.Point(20, 50)
            Me.lblCategory.Name = "lblCategory"
            Me.lblCategory.Size = New System.Drawing.Size(58, 15)
            Me.lblCategory.Text = "カテゴリ:"

            ' cmbCategory
            Me.cmbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbCategory.FormattingEnabled = True
            Me.cmbCategory.Location = New System.Drawing.Point(100, 47)
            Me.cmbCategory.Name = "cmbCategory"
            Me.cmbCategory.Size = New System.Drawing.Size(200, 23)

            ' lblStock
            Me.lblStock.AutoSize = True
            Me.lblStock.Location = New System.Drawing.Point(20, 80)
            Me.lblStock.Name = "lblStock"
            Me.lblStock.Size = New System.Drawing.Size(46, 15)
            Me.lblStock.Text = "在庫数:"

            ' txtStock
            Me.txtStock.Location = New System.Drawing.Point(100, 77)
            Me.txtStock.Name = "txtStock"
            Me.txtStock.Size = New System.Drawing.Size(200, 23)

            ' lblPrice
            Me.lblPrice.AutoSize = True
            Me.lblPrice.Location = New System.Drawing.Point(20, 110)
            Me.lblPrice.Name = "lblPrice"
            Me.lblPrice.Size = New System.Drawing.Size(35, 15)
            Me.lblPrice.Text = "単価:"

            ' txtPrice
            Me.txtPrice.Location = New System.Drawing.Point(100, 107)
            Me.txtPrice.Name = "txtPrice"
            Me.txtPrice.Size = New System.Drawing.Size(200, 23)

            ' btnAdd
            Me.btnAdd.Location = New System.Drawing.Point(20, 145)
            Me.btnAdd.Name = "btnAdd"
            Me.btnAdd.Size = New System.Drawing.Size(80, 30)
            Me.btnAdd.Text = "追加"
            Me.btnAdd.UseVisualStyleBackColor = True

            ' btnEdit
            Me.btnEdit.Location = New System.Drawing.Point(110, 145)
            Me.btnEdit.Name = "btnEdit"
            Me.btnEdit.Size = New System.Drawing.Size(80, 30)
            Me.btnEdit.Text = "編集"
            Me.btnEdit.UseVisualStyleBackColor = True

            ' btnDelete
            Me.btnDelete.Location = New System.Drawing.Point(200, 145)
            Me.btnDelete.Name = "btnDelete"
            Me.btnDelete.Size = New System.Drawing.Size(80, 30)
            Me.btnDelete.Text = "削除"
            Me.btnDelete.UseVisualStyleBackColor = True

            ' lblSearch
            Me.lblSearch.AutoSize = True
            Me.lblSearch.Location = New System.Drawing.Point(20, 190)
            Me.lblSearch.Name = "lblSearch"
            Me.lblSearch.Size = New System.Drawing.Size(35, 15)
            Me.lblSearch.Text = "検索:"

            ' txtSearch
            Me.txtSearch.Location = New System.Drawing.Point(100, 187)
            Me.txtSearch.Name = "txtSearch"
            Me.txtSearch.Size = New System.Drawing.Size(200, 23)

            ' btnSearch
            Me.btnSearch.Location = New System.Drawing.Point(310, 186)
            Me.btnSearch.Name = "btnSearch"
            Me.btnSearch.Size = New System.Drawing.Size(60, 25)
            Me.btnSearch.Text = "検索"
            Me.btnSearch.UseVisualStyleBackColor = True

            ' dgvInventory
            Me.dgvInventory.AllowUserToAddRows = False
            Me.dgvInventory.AllowUserToDeleteRows = False
            Me.dgvInventory.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top _
                Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right, System.Windows.Forms.AnchorStyles)
            Me.dgvInventory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.dgvInventory.Location = New System.Drawing.Point(20, 220)
            Me.dgvInventory.MultiSelect = False
            Me.dgvInventory.Name = "dgvInventory"
            Me.dgvInventory.ReadOnly = True
            Me.dgvInventory.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.dgvInventory.Size = New System.Drawing.Size(600, 200)

            ' lblStatus
            Me.lblStatus.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom _
                Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right, System.Windows.Forms.AnchorStyles)
            Me.lblStatus.Location = New System.Drawing.Point(20, 430)
            Me.lblStatus.Name = "lblStatus"
            Me.lblStatus.Size = New System.Drawing.Size(600, 23)
            Me.lblStatus.Text = "商品数: 0件  在庫金額合計: 0円"

            ' FormMain
            Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0F, 15.0F)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(640, 461)
            Me.Controls.Add(Me.lblStatus)
            Me.Controls.Add(Me.dgvInventory)
            Me.Controls.Add(Me.btnSearch)
            Me.Controls.Add(Me.txtSearch)
            Me.Controls.Add(Me.lblSearch)
            Me.Controls.Add(Me.btnDelete)
            Me.Controls.Add(Me.btnEdit)
            Me.Controls.Add(Me.btnAdd)
            Me.Controls.Add(Me.txtPrice)
            Me.Controls.Add(Me.lblPrice)
            Me.Controls.Add(Me.txtStock)
            Me.Controls.Add(Me.lblStock)
            Me.Controls.Add(Me.cmbCategory)
            Me.Controls.Add(Me.lblCategory)
            Me.Controls.Add(Me.txtProductName)
            Me.Controls.Add(Me.lblProductName)
            Me.Name = "FormMain"
            Me.Text = "在庫管理システム"
            CType(Me.dgvInventory, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()
        End Sub

#End Region

        Private WithEvents txtProductName As System.Windows.Forms.TextBox
        Private WithEvents cmbCategory As System.Windows.Forms.ComboBox
        Private WithEvents txtStock As System.Windows.Forms.TextBox
        Private WithEvents txtPrice As System.Windows.Forms.TextBox
        Private WithEvents txtSearch As System.Windows.Forms.TextBox
        Private WithEvents btnAdd As System.Windows.Forms.Button
        Private WithEvents btnEdit As System.Windows.Forms.Button
        Private WithEvents btnDelete As System.Windows.Forms.Button
        Private WithEvents btnSearch As System.Windows.Forms.Button
        Private WithEvents dgvInventory As System.Windows.Forms.DataGridView
        Private lblStatus As System.Windows.Forms.Label
        Private lblProductName As System.Windows.Forms.Label
        Private lblCategory As System.Windows.Forms.Label
        Private lblStock As System.Windows.Forms.Label
        Private lblPrice As System.Windows.Forms.Label
        Private lblSearch As System.Windows.Forms.Label
    End Class
End Namespace
