namespace OrderManager
{
    public partial class FormMain : Form
    {
        private List<Order> orders = new List<Order>();
        private int nextId = 1;

        public FormMain()
        {
            InitializeComponent();
        }

        // フォーム読み込み時
        private void FormMain_Load(object sender, EventArgs e)
        {
            // 税率コンボボックスの初期設定
            cmbTaxRate.Items.Add("10%（標準）");
            cmbTaxRate.Items.Add("8%（軽減税率）");
            cmbTaxRate.SelectedIndex = 0;

            // サンプルデータ
            orders.Add(new Order(nextId++, "コピー用紙A4", 500, 10, 0.10m,
                CalculateSubtotal(500, 10, 0.10m)));
            orders.Add(new Order(nextId++, "お茶（500ml）", 150, 24, 0.08m,
                CalculateSubtotal(150, 24, 0.08m)));

            RefreshGrid();
            UpdateTotal();
        }

        // 追加ボタン
        private void btnAdd_Click(object sender, EventArgs e)
        {
            // バリデーション: 商品名
            if (string.IsNullOrWhiteSpace(txtProductName.Text))
            {
                MessageBox.Show("商品名を入力してください", "入力エラー",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // バリデーション: 単価
            if (!int.TryParse(txtUnitPrice.Text, out int unitPrice) || unitPrice < 1)
            {
                MessageBox.Show("単価は1以上の整数で入力してください", "入力エラー",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // バリデーション: 数量
            // ★★★ バグ3: 数量0でも注文可能 ★★★
            // 仕様: 数量は1以上100以下の整数
            // 実装: 0以上100以下でチェックしているため、数量0の注文が入る
            if (!int.TryParse(txtQuantity.Text, out int quantity) || quantity < 0 || quantity > 100)
            {
                MessageBox.Show("数量は1～100の整数で入力してください", "入力エラー",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 税率の取得
            decimal taxRate = GetSelectedTaxRate();

            // 小計計算
            int subtotal = CalculateSubtotal(unitPrice, quantity, taxRate);

            // 注文追加
            var order = new Order(nextId++, txtProductName.Text, unitPrice, quantity, taxRate, subtotal);
            orders.Add(order);

            // 入力欄クリア
            txtProductName.Text = "";
            txtUnitPrice.Text = "";
            txtQuantity.Text = "";
            cmbTaxRate.SelectedIndex = 0;

            RefreshGrid();
            UpdateTotal();
        }

        // 削除ボタン
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvOrders.SelectedRows.Count == 0)
            {
                MessageBox.Show("削除する注文を選択してください", "警告",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int selectedId = (int)dgvOrders.SelectedRows[0].Cells["Id"].Value;
            var order = orders.Find(o => o.Id == selectedId);

            if (order != null)
            {
                var result = MessageBox.Show(
                    $"{order.ProductName}を削除しますか？",
                    "削除確認",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    orders.Remove(order);
                    RefreshGrid();
                    UpdateTotal();
                }
            }
        }

        // 税率取得
        // ★★★ バグ1: 税率の適用ミス ★★★
        // 仕様: コンボボックスの選択に応じて税率を返す
        //       「10%（標準）」→ 0.10、「8%（軽減税率）」→ 0.08
        // 実装: if文の条件が間違っており、常に8%が返される
        private decimal GetSelectedTaxRate()
        {
            if (cmbTaxRate.SelectedIndex == 0)
            {
                // 「10%（標準）」が選択されている場合
                return 0.08m;  // ← ここが間違い。0.10m であるべき
            }
            else
            {
                // 「8%（軽減税率）」が選択されている場合
                return 0.08m;
            }
        }

        // 小計計算
        private int CalculateSubtotal(int unitPrice, int quantity, decimal taxRate)
        {
            // 小計 = 単価 × 数量 × (1 + 税率)
            decimal subtotal = (decimal)unitPrice * quantity * (1 + taxRate);
            return (int)Math.Floor(subtotal);
        }

        // 合計金額更新
        // ★★★ バグ2: 合計金額が最後の注文の小計のみ ★★★
        // 仕様: 全注文の小計の合計を表示
        // 実装: forループ内で total += ではなく total = で代入しているため、
        //       最後の注文の小計だけが表示される
        private void UpdateTotal()
        {
            int total = 0;

            for (int i = 0; i < orders.Count; i++)
            {
                total = orders[i].Subtotal;  // ← += であるべきところが = になっている
            }

            lblTotal.Text = $"合計金額（税込）: {total:#,0}円";
        }

        // 一覧を更新
        private void RefreshGrid()
        {
            dgvOrders.DataSource = null;
            dgvOrders.DataSource = orders.Select(o => new
            {
                o.Id,
                o.ProductName,
                o.UnitPrice,
                o.Quantity,
                TaxRateDisplay = o.TaxRate == 0.10m ? "10%" : "8%",
                o.Subtotal
            }).ToList();

            // 列ヘッダーの表示名を設定
            if (dgvOrders.Columns.Count > 0)
            {
                dgvOrders.Columns["Id"].Visible = false;
                dgvOrders.Columns["ProductName"].HeaderText = "商品名";
                dgvOrders.Columns["UnitPrice"].HeaderText = "単価";
                dgvOrders.Columns["Quantity"].HeaderText = "数量";
                dgvOrders.Columns["TaxRateDisplay"].HeaderText = "税率";
                dgvOrders.Columns["Subtotal"].HeaderText = "小計（税込）";
            }
        }
    }
}
