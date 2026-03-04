namespace CustomerManagerDB
{
    public partial class FormMain : Form
    {
        private DatabaseHelper dbHelper = new DatabaseHelper();

        public FormMain()
        {
            InitializeComponent();
        }

        // フォーム読み込み時にデータを表示
        private void FormMain_Load(object sender, EventArgs e)
        {
            LoadCustomers();
        }

        // 顧客一覧を読み込み
        private void LoadCustomers()
        {
            try
            {
                var customers = dbHelper.GetAllCustomers();
                RefreshGrid(customers);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"データの読み込みに失敗しました: {ex.Message}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 追加ボタン
        private void btnAdd_Click(object sender, EventArgs e)
        {
            // バリデーション
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("氏名を入力してください", "入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 年齢チェック
            if (!int.TryParse(txtAge.Text, out int age) || age < 0 || age > 150)
            {
                MessageBox.Show("年齢は0～150の整数で入力してください", "入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                dbHelper.AddCustomer(txtName.Text, txtPhone.Text, age, txtEmail.Text);

                // 入力欄をクリア
                txtName.Text = "";
                txtPhone.Text = "";
                txtAge.Text = "";
                txtEmail.Text = "";

                LoadCustomers();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"顧客の追加に失敗しました: {ex.Message}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 編集ボタン
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvCustomers.SelectedRows.Count == 0)
            {
                MessageBox.Show("顧客を選択してください", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int selectedId = (int)dgvCustomers.SelectedRows[0].Cells["Id"].Value;

            if (!int.TryParse(txtAge.Text, out int age) || age < 0 || age > 150)
            {
                MessageBox.Show("年齢は0～150の整数で入力してください", "入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                dbHelper.UpdateCustomer(selectedId, txtName.Text, txtPhone.Text, age, txtEmail.Text);
                LoadCustomers();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"顧客の更新に失敗しました: {ex.Message}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 削除ボタン
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvCustomers.SelectedRows.Count == 0)
            {
                MessageBox.Show("顧客を選択してください", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int selectedId = (int)dgvCustomers.SelectedRows[0].Cells["Id"].Value;
            string customerName = dgvCustomers.SelectedRows[0].Cells["Name"].Value?.ToString() ?? "";

            var result = MessageBox.Show(
                $"{customerName}を削除しますか？",
                "削除確認",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    dbHelper.DeleteCustomer(selectedId);
                    LoadCustomers();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"顧客の削除に失敗しました: {ex.Message}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // 検索ボタン
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text;

            if (string.IsNullOrWhiteSpace(keyword))
            {
                LoadCustomers();
                return;
            }

            try
            {
                var results = dbHelper.SearchCustomers(keyword);

                if (results.Count == 0)
                {
                    MessageBox.Show("該当する顧客はありません", "検索結果", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                RefreshGrid(results);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"検索に失敗しました: {ex.Message}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 全件表示ボタン
        private void btnShowAll_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            LoadCustomers();
        }

        // DataGridView選択変更時に入力欄に値をセット
        private void dgvCustomers_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvCustomers.SelectedRows.Count > 0)
            {
                var row = dgvCustomers.SelectedRows[0];
                txtName.Text = row.Cells["Name"].Value?.ToString() ?? "";
                txtPhone.Text = row.Cells["Phone"].Value?.ToString() ?? "";
                txtAge.Text = row.Cells["Age"].Value?.ToString() ?? "";
                txtEmail.Text = row.Cells["Email"].Value?.ToString() ?? "";
            }
        }

        // 一覧を更新
        private void RefreshGrid(List<Customer> displayList)
        {
            dgvCustomers.DataSource = null;
            dgvCustomers.DataSource = displayList.Select(c => new
            {
                c.Id,
                c.Name,
                c.Phone,
                c.Age,
                c.Email
            }).ToList();

            UpdateStatus(displayList);
        }

        // ステータスバー更新
        private void UpdateStatus(List<Customer> displayList)
        {
            int count = displayList.Count;
            int totalAge = displayList.Sum(c => c.Age);
            double avgAge = count > 0 ? (double)totalAge / count : 0;

            lblStatus.Text = $"件数: {count}件  合計年齢: {totalAge}  平均年齢: {avgAge:F1}";
        }
    }
}
