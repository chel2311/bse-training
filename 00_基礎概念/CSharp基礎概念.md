# C# プログラミング基礎概念（BSE向け）

> コードを「書く」のではなく「読んでレビューできる」ことが目標

---

## 1. 変数と型

プログラムはデータを「変数」という箱に入れて処理する。箱には種類（型）がある。

```csharp
// 整数
int age = 30;

// 文字列
string name = "田中太郎";

// 小数（金額計算に使う）
decimal price = 1980.50m;

// 真偽値（はい/いいえ）
bool isActive = true;
```

### レビューで見るポイント
- 金額に `float` や `double` を使っていないか？ → 誤差が出る。`decimal` が正しい
- `int` に入りきらない大きな数を扱っていないか？ → `long` を使うべき
- 文字列が `null` になる可能性を考慮しているか？

---

## 2. 制御構造

### if/else - 条件分岐
```csharp
if (age >= 18)
{
    // 成人の処理
    Console.WriteLine("成人です");
}
else
{
    // 未成年の処理
    Console.WriteLine("未成年です");
}
```

### for - 繰り返し
```csharp
// 0から9まで10回繰り返す
for (int i = 0; i < 10; i++)
{
    Console.WriteLine(i);
}
```

### foreach - コレクションの走査
```csharp
List<string> names = new List<string> { "田中", "鈴木", "佐藤" };
foreach (string name in names)
{
    Console.WriteLine(name);
}
```

### レビューで見るポイント
- `if` の条件が仕様通りか？（`>=` と `>` の違い = 境界値）
- ループの回数は正しいか？（Off-by-oneエラー）
- `else` の処理漏れはないか？

---

## 3. メソッド（関数）

処理をまとめて名前を付けたもの。入力（引数）を受け取り、結果（戻り値）を返す。

```csharp
// 税込金額を計算するメソッド
// 引数: price（税抜価格）, taxRate（税率）
// 戻り値: decimal（税込価格）
private decimal CalculateTaxIncluded(decimal price, decimal taxRate)
{
    return price * (1 + taxRate);
}

// 使い方
decimal total = CalculateTaxIncluded(1000m, 0.10m); // 1100
```

### レビューで見るポイント
- 引数の型は適切か？
- 戻り値の型は呼び出し元で期待する型と一致しているか？
- メソッド名が処理内容を正しく表しているか？

---

## 4. クラスとオブジェクト

関連するデータと処理をまとめたもの。設計書の「エンティティ」に対応する。

```csharp
// 顧客クラス
public class Customer
{
    // プロパティ（データ）
    public int Id { get; set; }
    public string Name { get; set; }
    public string Phone { get; set; }
    public DateTime RegisterDate { get; set; }

    // コンストラクタ（初期化処理）
    public Customer(int id, string name, string phone)
    {
        Id = id;
        Name = name;
        Phone = phone;
        RegisterDate = DateTime.Now;
    }

    // メソッド（処理）
    public string GetDisplayName()
    {
        return $"[{Id}] {Name}";
    }
}

// 使い方
Customer customer = new Customer(1, "田中太郎", "090-1234-5678");
string display = customer.GetDisplayName(); // "[1] 田中太郎"
```

### レビューで見るポイント
- プロパティの型は設計書のデータ型定義と一致しているか？
- 必須項目が `null` になれる状態になっていないか？

---

## 5. イベント駆動プログラミング（WinForms）

GUIアプリは「ユーザーの操作（イベント）」に応じて処理が実行される。

```csharp
public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
    }

    // フォームが読み込まれた時に実行される
    private void Form1_Load(object sender, EventArgs e)
    {
        LoadCustomerList();
    }

    // 「追加」ボタンがクリックされた時に実行される
    private void btnAdd_Click(object sender, EventArgs e)
    {
        AddCustomer();
    }

    // 「削除」ボタンがクリックされた時に実行される
    private void btnDelete_Click(object sender, EventArgs e)
    {
        DeleteCustomer();
    }
}
```

### イベントの仕組み
1. ユーザーがボタンをクリック
2. Windowsが「クリックイベント」を発火
3. 対応するイベントハンドラ（メソッド）が実行される

### レビューで見るポイント
- ボタンにイベントハンドラが正しく接続されているか？（Designer.csを確認）
- フォームロード時の初期化処理は適切か？
- イベントの実行順序は正しいか？

---

## 6. DB接続基礎（SQL Server）

C#からSQL Serverに接続してデータを読み書きする。

```csharp
// 接続文字列
string connectionString = "Server=localhost;Database=CustomerDB;Trusted_Connection=True;";

// データの取得（SELECT）
using (SqlConnection conn = new SqlConnection(connectionString))
{
    conn.Open();
    string sql = "SELECT Id, Name, Phone FROM Customers WHERE IsActive = @IsActive";

    using (SqlCommand cmd = new SqlCommand(sql, conn))
    {
        cmd.Parameters.AddWithValue("@IsActive", true);

        using (SqlDataReader reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string name = reader.GetString(1);
                string phone = reader.GetString(2);
            }
        }
    }
}
```

### レビューで見るポイント
- `using` でリソースが正しく解放されているか？（接続の閉じ忘れ）
- SQLにパラメータを使っているか？（文字列結合はSQLインジェクションの危険）
- 接続文字列のサーバー名やDB名は正しいか？
- `conn.Open()` 周辺にtry-catchがあるか？

---

## 7. 例外処理

想定外のエラーが発生した時の対処。

```csharp
try
{
    // エラーが起きるかもしれない処理
    conn.Open();
    // DB操作...
}
catch (SqlException ex)
{
    // DB関連のエラー
    MessageBox.Show($"データベースエラー: {ex.Message}");
}
catch (Exception ex)
{
    // その他のエラー
    MessageBox.Show($"エラーが発生しました: {ex.Message}");
}
finally
{
    // エラーが起きても起きなくても必ず実行される
    conn.Close();
}
```

### レビューで見るポイント
- `try-catch` が適切な範囲で使われているか？（広すぎ/狭すぎ）
- `catch` でエラーを握りつぶしていないか？（空のcatchブロック）
- ユーザーに適切なエラーメッセージが表示されるか？
- `finally` でリソース解放が確実に行われるか？

---

## 8. Visual Studio デバッグの基本操作

| 操作 | ショートカット | 用途 |
|------|--------------|------|
| ブレークポイント設置 | F9 | 実行を止めたい行に設置 |
| デバッグ実行 | F5 | ブレークポイントで止まる |
| ステップオーバー | F10 | 次の行へ（メソッド内に入らない） |
| ステップイン | F11 | メソッドの中に入る |
| 変数の値確認 | マウスホバー | 変数にカーソルを合わせると値が見える |
| ウォッチウィンドウ | - | 監視したい変数を登録して値を追跡 |

### デバッグの手順
1. 「おかしい」と思う処理の手前にブレークポイント（F9）
2. デバッグ実行（F5）
3. 止まったら変数の値を確認
4. F10で1行ずつ進めながら、値の変化を追う
5. 期待と違う値になる行がバグの原因

---

## 次のステップ

この基礎概念を理解したら、`01_CSharp_Level1/` の練習問題に進んでください。
実際のGUIアプリのコードを読み、設計仕様書と照らし合わせてバグを見つける訓練を行います。
