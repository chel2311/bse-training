namespace CustomerManager
{
    public partial class FormMain : Form
    {
        private List<Customer> customers = new List<Customer>();
        private int nextId = 1;

        public FormMain()
        {
            InitializeComponent();
        }

        // フォーム読み込み時にサンプルデータを追加
        private void FormMain_Load(object sender, EventArgs e)
        {
            // サンプルデータ
            customers.Add(new Customer(nextId++, "田中太郎", "090-1234-5678", 35, "tanaka@example.com"));
            customers.Add(new Customer(nextId++, "鈴木花子", "080-9876-5432", 28, "suzuki@example.com"));
            customers.Add(new Customer(nextId++, "佐藤次郎", "070-1111-2222", 42, "sato@example.com"));

            RefreshGrid(customers);
        }

        // 追加ボタン
        private void btnAdd_Click(object sender, EventArgs e)
        {
            // バリデーション: 氏名必須チェック
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("氏名を入力してください", "入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 年齢チェック: 整数かつ0-150の範囲
            if (!int.TryParse(txtAge.Text, out int age) || age < 0 || age > 150)
            {
                MessageBox.Show("年齢は0～150の整数で入力してください", "入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var customer = new Customer(nextId++, txtName.Text, txtPhone.Text, age, txtEmail.Text);
            customers.Add(customer);

            // 追加後、入力欄をクリア
            txtName.Text = "";
            txtPhone.Text = "";
            txtAge.Text = "";
            txtEmail.Text = "";

            RefreshGrid(customers);
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
            var customer = customers.Find(c => c.Id == selectedId);

            if (customer != null)
            {
                customer.Name = txtName.Text;
                customer.Phone = txtPhone.Text;
                customer.Email = txtEmail.Text;

                if (int.TryParse(txtAge.Text, out int age))
                {
                    customer.Age = age;
                }

                RefreshGrid(customers);
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
            var customer = customers.Find(c => c.Id == selectedId);

            if (customer != null)
            {
                var result = MessageBox.Show(
                    $"{customer.Name}を削除しますか？",
                    "削除確認",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    customers.Remove(customer);
                    RefreshGrid(customers);
                }
            }
        }

        // 検索ボタン
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text;

            if (string.IsNullOrWhiteSpace(keyword))
            {
                RefreshGrid(customers);
                return;
            }

            // ★★★ バグ1: 大文字・小文字を区別してしまう ★★★
            // 仕様 FR-004: 大文字・小文字を区別しない
            // 実装: StringComparison.OrdinalIgnoreCase が指定されていない
            // → "Tanaka" と "tanaka" が別扱いになる
            var results = customers.FindAll(c =>
                c.Name.Contains(keyword) ||
                c.Email.Contains(keyword));

            // ★★★ バグ2: 検索結果0件のメッセージがない ★★★
            // 仕様 FR-004: 検索結果が0件の場合「該当する顧客はありません」と表示
            // 実装: 0件チェックとメッセージ表示が抜けている
            // → ユーザーは結果がないことに気づきにくい

            RefreshGrid(results);
        }

        // 全件表示ボタン
        private void btnShowAll_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            RefreshGrid(customers);
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

            // ★★★ バグ3: 整数除算で小数が切り捨てられる ★★★
            // 仕様 FR-006: 平均年齢は小数第1位まで表示
            // 実装: int同士の除算のため小数部分が切り捨てられる
            // → 例: 63/2=31 (本来は31.5)
            int avgAge = count > 0 ? totalAge / count : 0;

            lblStatus.Text = $"件数: {count}件  合計年齢: {totalAge}  平均年齢: {avgAge}";
        }
    }
}
