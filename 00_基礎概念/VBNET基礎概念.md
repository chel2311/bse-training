# VB.NET プログラミング基礎概念（BSE向け）

> コードを「書く」のではなく「読んでレビューできる」ことが目標
> C#との比較を交えて理解する

---

## 1. VB.NETとは

VB.NET（Visual Basic .NET）は、Microsoftが開発したプログラミング言語。
C#と同じ.NETプラットフォーム上で動作し、**できることはC#とほぼ同じ**だが、構文が英語に近く読みやすいのが特徴。

### VB6との違い（よくある誤解）

| 項目 | VB6（旧） | VB.NET（現在） |
|------|----------|---------------|
| プラットフォーム | COM / Win32 | .NET Framework / .NET |
| オブジェクト指向 | 部分的 | 完全対応（継承・インターフェース等） |
| メモリ管理 | 手動（参照カウント） | 自動（ガベージコレクション） |
| エラー処理 | On Error GoTo | Try-Catch-Finally |
| データ型 | Variant型が主流 | 強い型付け（Option Strict推奨） |
| 互換性 | VB.NETとは**非互換** | VB6コードはそのまま動かない |

**重要**: VB6とVB.NETは名前が似ているが**別の言語**。VB6の知識だけではVB.NETのコードレビューはできない。

---

## 2. 変数と型

### VB.NET の書き方

```vb
' 整数
Dim age As Integer = 30

' 文字列
Dim name As String = "田中太郎"

' 小数（金額計算に使う）
Dim price As Decimal = 1980.5D

' 真偽値（はい/いいえ）
Dim isActive As Boolean = True
```

### C# との比較

| VB.NET | C# | 説明 |
|--------|-----|------|
| `Dim age As Integer = 30` | `int age = 30;` | 整数 |
| `Dim name As String = "田中"` | `string name = "田中";` | 文字列 |
| `Dim price As Decimal = 100D` | `decimal price = 100m;` | 小数 |
| `Dim flag As Boolean = True` | `bool flag = true;` | 真偽値 |
| `Dim items As New List(Of String)` | `var items = new List<string>()` | リスト |

### レビューで見るポイント
- 金額に `Single` や `Double` を使っていないか？ → 誤差が出る。`Decimal` が正しい
- `Integer` に入りきらない大きな数を扱っていないか？ → `Long` を使うべき
- 文字列が `Nothing` になる可能性を考慮しているか？
- VB.NETでは `Nothing` がC#の `null` に相当する

---

## 3. 制御構造

### If/Else - 条件分岐

```vb
If age >= 18 Then
    ' 成人の処理
    Console.WriteLine("成人です")
Else
    ' 未成年の処理
    Console.WriteLine("未成年です")
End If
```

**C# との違い**: VB.NETは `{}`（中かっこ）を使わず `Then` / `End If` で囲む。

### For - 繰り返し

```vb
' 0から9まで10回繰り返す
For i As Integer = 0 To 9
    Console.WriteLine(i)
Next
```

**C# との違い**: C#は `for (int i = 0; i < 10; i++)` と書く。VB.NETは `To` で終了値を指定。

### For Each - コレクションの走査

```vb
Dim names As New List(Of String) From {"田中", "鈴木", "佐藤"}
For Each name As String In names
    Console.WriteLine(name)
Next
```

### レビューで見るポイント
- `If` の条件が仕様通りか？（`>=` と `>` の違い = 境界値）
- VB.NETの `To` は**終了値を含む**（C#の `<` と `<=` の違いに注意）
- `ElseIf`（C#の `else if`）の処理漏れはないか？
- `OrElse` と `Or` の違い: `OrElse` は短絡評価（推奨）、`Or` は両方評価する

---

## 4. 演算子の違い

C#と演算子が異なるため、レビュー時に注意が必要。

| 操作 | VB.NET | C# |
|------|--------|-----|
| 等しい | `=` | `==` |
| 等しくない | `<>` | `!=` |
| 論理AND（短絡） | `AndAlso` | `&&` |
| 論理OR（短絡） | `OrElse` | `||` |
| 論理NOT | `Not` | `!` |
| 文字列結合 | `&` または `+` | `+` |
| 整数除算 | `\` | `/`（整数同士の場合） |
| 型チェック | `TypeOf x Is クラス名` | `x is クラス名` |
| キャスト | `CType(x, 型)` / `DirectCast(x, 型)` | `(型)x` / `x as 型` |
| Nothing判定 | `x Is Nothing` | `x == null` |

### レビューで見るポイント
- `=` が「代入」と「比較」の両方に使われる（文脈で判断）
- `+` で文字列と数値を結合すると暗黙変換が起きる → `&` を使うべき
- `And` / `Or` ではなく `AndAlso` / `OrElse`（短絡評価）を使っているか

---

## 5. メソッド（関数とサブルーチン）

VB.NETには「戻り値あり（Function）」と「戻り値なし（Sub）」の2種類がある。

```vb
' 戻り値あり: Function
' 税込金額を計算するメソッド
Private Function CalculateTaxIncluded(price As Decimal, taxRate As Decimal) As Decimal
    Return price * (1 + taxRate)
End Function

' 戻り値なし: Sub
' メッセージを表示するメソッド
Private Sub ShowMessage(message As String)
    MessageBox.Show(message)
End Sub

' 使い方
Dim total As Decimal = CalculateTaxIncluded(1000D, 0.1D)  ' 1100
ShowMessage("計算完了")
```

### C# との比較

| VB.NET | C# |
|--------|-----|
| `Function 名前() As 型` | `型 名前()` |
| `Sub 名前()` | `void 名前()` |
| `End Function` / `End Sub` | `}` |

### レビューで見るポイント
- `Function` なのに `Return` がないパスが存在しないか？
- `Sub` で値を返すべき処理をしていないか？
- 引数の型は設計書のデータ型定義と一致しているか？

---

## 6. クラスとオブジェクト

```vb
' 商品クラス
Public Class Product
    ' プロパティ（データ）
    Public Property Id As Integer
    Public Property ProductName As String = String.Empty
    Public Property Stock As Integer
    Public Property UnitPrice As Integer

    ' 読み取り専用プロパティ（計算結果）
    Public ReadOnly Property StockValue As Integer
        Get
            Return Stock * UnitPrice
        End Get
    End Property

    ' コンストラクタ（初期化処理）
    Public Sub New(id As Integer, productName As String, stock As Integer, unitPrice As Integer)
        Me.Id = id
        Me.ProductName = productName
        Me.Stock = stock
        Me.UnitPrice = unitPrice
    End Sub
End Class

' 使い方
Dim product As New Product(1, "コーヒー豆", 50, 800)
Dim value As Integer = product.StockValue  ' 40000
```

### C# との比較

| VB.NET | C# |
|--------|-----|
| `Public Class 名前` | `public class 名前` |
| `Public Property Id As Integer` | `public int Id { get; set; }` |
| `Public Sub New(...)` | `public 名前(...)` |
| `Me.Id` | `this.Id` |
| `End Class` | `}` |

### レビューで見るポイント
- プロパティの型は設計書のデータ型定義と一致しているか？
- `ReadOnly Property` の計算ロジックは仕様通りか？（演算子の誤り等）
- 必須項目が `Nothing` になれる状態になっていないか？

---

## 7. イベント駆動プログラミング（WinForms）

GUIアプリは「ユーザーの操作（イベント）」に応じて処理が実行される。

```vb
Public Partial Class FormMain
    Inherits Form

    Public Sub New()
        InitializeComponent()
    End Sub

    ' フォームが読み込まれた時に実行される
    Private Sub FormMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadProductList()
    End Sub

    ' 「追加」ボタンがクリックされた時に実行される
    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        AddProduct()
    End Sub

    ' 「削除」ボタンがクリックされた時に実行される
    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        DeleteProduct()
    End Sub
End Class
```

### C# との違い（イベント接続方式）

| VB.NET | C# |
|--------|-----|
| `Handles btnAdd.Click`（宣言的） | `btnAdd.Click += btnAdd_Click;`（命令的） |
| `WithEvents` をフィールドに付ける | Designer.csで `+=` で接続 |

**VB.NETの特徴**: `Handles` 句でイベントとメソッドを直接紐付ける。
C#では Designer.cs 内の `+=` で接続するが、VB.NETでは `Handles` キーワードを使う。

### レビューで見るポイント
- すべてのボタンに `Handles` でイベントハンドラが接続されているか？
- `WithEvents` 修飾子がDesigner.vbの該当コントロールに付いているか？
- フォームロード時の初期化処理は適切か？

---

## 8. 例外処理

```vb
Try
    ' エラーが起きるかもしれない処理
    conn.Open()
    ' DB操作...
Catch ex As SqlException
    ' DB関連のエラー
    MessageBox.Show($"データベースエラー: {ex.Message}")
Catch ex As Exception
    ' その他のエラー
    MessageBox.Show($"エラーが発生しました: {ex.Message}")
Finally
    ' エラーが起きても起きなくても必ず実行される
    conn.Close()
End Try
```

### C# との比較

| VB.NET | C# |
|--------|-----|
| `Try` | `try {` |
| `Catch ex As Exception` | `catch (Exception ex) {` |
| `Finally` | `finally {` |
| `End Try` | `}` |

### レビューで見るポイント
- `Try-Catch` が適切な範囲で使われているか？
- `Catch` でエラーを握りつぶしていないか？（空のCatchブロック）
- VB6の `On Error GoTo` / `On Error Resume Next` が混在していないか？ → .NETではNG
- `Finally` でリソース解放が確実に行われるか？

---

## 9. VB.NET特有の注意点

### Nothing の扱い
```vb
Dim name As String = Nothing

' NGパターン: NullReferenceException が発生する
Dim length As Integer = name.Length

' OKパターン: Nothingチェックしてから使う
If name IsNot Nothing Then
    Dim length As Integer = name.Length
End If

' OKパターン: Null条件演算子（VB 14以降）
Dim length As Integer? = name?.Length
```

### 文字列比較
```vb
' 大文字小文字を区別しない比較
If String.Equals(a, b, StringComparison.OrdinalIgnoreCase) Then
    ' 一致
End If

' 部分一致検索（大文字小文字区別なし）
If productName.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0 Then
    ' 含まれている
End If
```

### 型変換
```vb
' 安全な変換（推奨）
Dim stock As Integer
If Integer.TryParse(txtStock.Text, stock) Then
    ' 変換成功
Else
    ' 変換失敗
End If

' 危険な変換（非推奨）
Dim stock As Integer = CInt(txtStock.Text)  ' 変換失敗時に例外が発生
```

---

## 10. Visual Studio デバッグの基本操作

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

## C# ↔ VB.NET 変換 早見表

レビュー時に片方の言語しか知らない場合に参照する。

| 概念 | C# | VB.NET |
|------|-----|--------|
| 名前空間 | `namespace X { }` | `Namespace X ... End Namespace` |
| クラス | `public class X { }` | `Public Class X ... End Class` |
| メソッド（戻り値あり） | `int Calc() { return 1; }` | `Function Calc() As Integer ... Return 1 ... End Function` |
| メソッド（戻り値なし） | `void DoWork() { }` | `Sub DoWork() ... End Sub` |
| プロパティ | `public int X { get; set; }` | `Public Property X As Integer` |
| コンストラクタ | `public X() { }` | `Public Sub New() ... End Sub` |
| 変数宣言 | `int x = 0;` | `Dim x As Integer = 0` |
| 条件分岐 | `if (x > 0) { } else { }` | `If x > 0 Then ... Else ... End If` |
| ループ | `foreach (var x in list) { }` | `For Each x In list ... Next` |
| 例外処理 | `try { } catch { } finally { }` | `Try ... Catch ... Finally ... End Try` |
| null | `null` | `Nothing` |
| this | `this` | `Me` |
| 文末 | `;`（セミコロン必須） | なし（改行で文の終わり） |
| ブロック | `{ }`（中かっこ） | `End If` / `End Sub` 等のキーワード |
| ラムダ式 | `x => x.Name` | `Function(x) x.Name` |
| コメント | `// 1行` / `/* 複数行 */` | `' 1行のみ` |

---

## 次のステップ

この基礎概念を理解したら、`02_VBNET_Level1/` の練習問題に進んでください。
実際のGUIアプリのコードを読み、設計仕様書と照らし合わせてバグを見つける訓練を行います。
