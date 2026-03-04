using System.Configuration;
using System.Data.SqlClient;

namespace CustomerManagerDB
{
    public class DatabaseHelper
    {
        private readonly string connectionString;

        public DatabaseHelper()
        {
            connectionString = ConfigurationManager.ConnectionStrings["CustomerDB"].ConnectionString;
        }

        /// <summary>
        /// 顧客を全件取得する
        /// </summary>
        public List<Customer> GetAllCustomers()
        {
            var customers = new List<Customer>();

            // ★★★ バグ2: usingを使っていない → DB接続が閉じられない ★★★
            // 仕様: SqlConnection, SqlCommand, SqlDataReader は必ず using で囲むこと
            // 実装: usingなしで new しており、Close() も Dispose() もない
            // → 接続プールが枯渇し、やがてDB接続ができなくなる

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT Id, Name, Phone, Age, Email FROM Customers", conn);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                var customer = new Customer(
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.IsDBNull(2) ? "" : reader.GetString(2),
                    reader.GetInt32(3),
                    reader.IsDBNull(4) ? "" : reader.GetString(4)
                );
                customers.Add(customer);
            }

            return customers;
        }

        /// <summary>
        /// 顧客を追加する
        /// </summary>
        public void AddCustomer(string name, string phone, int age, string email)
        {
            // ★★★ バグ1: SQLインジェクション脆弱性 ★★★
            // 仕様: SQLはパラメータ化クエリを使用すること（SQLインジェクション対策）
            // 実装: 文字列結合でSQLを構築しており、パラメータ化されていない
            // → 悪意ある入力でDBが破壊・情報漏洩の危険がある

            // ★★★ バグ2(続き): ここもusingを使っていない ★★★

            // ★★★ バグ3: try-catchなし ★★★
            // 仕様: DB接続時は必ずtry-catchで例外処理を行い、ユーザーにエラーメッセージを表示すること
            // 実装: try-catchが一切なく、DB接続失敗やSQL実行エラーで例外がそのまま飛ぶ
            // → アプリケーションがクラッシュする

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string sql = "INSERT INTO Customers (Name, Phone, Age, Email) VALUES ('" + name + "', '" + phone + "', " + age + ", '" + email + "')";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 顧客を更新する
        /// </summary>
        public void UpdateCustomer(int id, string name, string phone, int age, string email)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sql = "UPDATE Customers SET Name = @Name, Phone = @Phone, Age = @Age, Email = @Email WHERE Id = @Id";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Phone", (object?)phone ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Age", age);
                    cmd.Parameters.AddWithValue("@Email", (object?)email ?? DBNull.Value);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// 顧客を削除する
        /// </summary>
        public void DeleteCustomer(int id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sql = "DELETE FROM Customers WHERE Id = @Id";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// 氏名またはメールアドレスで部分一致検索する
        /// </summary>
        public List<Customer> SearchCustomers(string keyword)
        {
            var customers = new List<Customer>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sql = "SELECT Id, Name, Phone, Age, Email FROM Customers WHERE Name LIKE @Keyword OR Email LIKE @Keyword";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Keyword", "%" + keyword + "%");

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var customer = new Customer(
                                reader.GetInt32(0),
                                reader.GetString(1),
                                reader.IsDBNull(2) ? "" : reader.GetString(2),
                                reader.GetInt32(3),
                                reader.IsDBNull(4) ? "" : reader.GetString(4)
                            );
                            customers.Add(customer);
                        }
                    }
                }
            }

            return customers;
        }
    }
}
