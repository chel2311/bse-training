using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace CustomerManager
{
    public partial class FormMain : Form
    {
        private List<Customer> customers = new List<Customer>();

        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            LoadCustomers();
            RefreshGrid();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // 入力バリデーション
            if (string.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show("名前を入力してください", "入力エラー",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtAge.Text, out int age) || age < 0 || age > 150)
            {
                MessageBox.Show("年齢は0～150の整数で入力してください", "入力エラー",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var customer = new Customer
            {
                Id = customers.Count + 1,
                Name = txtName.Text,
                Age = age,
                Phone = txtPhone.Text,
                Email = txtEmail.Text
            };

            customers.Add(customer);

            // 入力欄をクリア
            txtName.Text = "";
            txtAge.Text = "";
            txtPhone.Text = "";
            txtEmail.Text = "";

            RefreshGrid();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvCustomers.SelectedRows.Count == 0)
            {
                MessageBox.Show("削除する顧客を選択してください", "警告",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var customer = (Customer)dgvCustomers.SelectedRows[0].DataBoundItem;
            var result = MessageBox.Show(
                $"{customer.Name}を削除しますか？",
                "削除確認",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                customers.Remove(customer);
                RefreshGrid();
            }
        }

        /// <summary>
        /// 顧客一覧をCSVファイルにエクスポートする
        /// </summary>
        private void btnExportCsv_Click(object sender, EventArgs e)
        {
            if (customers.Count == 0)
            {
                MessageBox.Show("エクスポートする顧客データがありません", "警告",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // CSVファイルに出力
                string filePath = @"C:\export\customers.csv";

                StreamWriter writer = new StreamWriter(filePath);

                // ヘッダー行
                writer.WriteLine("顧客ID,顧客名,年齢,電話番号,メールアドレス");

                // データ行
                foreach (var customer in customers)
                {
                    writer.WriteLine($"{customer.Id},{customer.Name},{customer.Age},{customer.Phone},{customer.Email}");
                }

                writer.Close();

                MessageBox.Show($"CSVファイルを出力しました\n{filePath}", "完了",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"CSV出力中にエラーが発生しました: {ex.Message}", "エラー",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadCustomers()
        {
            // サンプルデータの読み込み
            customers.Add(new Customer { Id = 1, Name = "田中太郎", Age = 30, Phone = "090-1234-5678", Email = "tanaka@example.com" });
            customers.Add(new Customer { Id = 2, Name = "鈴木花子", Age = 25, Phone = "080-2345-6789", Email = "suzuki@example.com" });
            customers.Add(new Customer { Id = 3, Name = "佐藤一郎", Age = 45, Phone = "070-3456-7890", Email = "sato@example.com" });
        }

        private void RefreshGrid()
        {
            dgvCustomers.DataSource = null;
            dgvCustomers.DataSource = customers;
        }
    }
}
