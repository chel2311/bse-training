# SQL Server 基礎概念（BSE向け）

> SQLを「書く」のではなく「読んでレビューできる」ことが目標

---

## 1. SQL Serverとは

SQL Serverは、マイクロソフトが開発したリレーショナルデータベース管理システム（RDBMS）。
データを「テーブル」という表形式で管理し、SQL言語を使って操作する。

```
リレーショナルデータベースの構造:

サーバー
  └── データベース（例: OrderManagementDB）
        ├── テーブル（例: Customers, Products, Orders）
        ├── ビュー
        ├── ストアドプロシージャ
        └── インデックス
```

### BSEが知っておくべきこと
- SQL Serverは.NETアプリとの組み合わせが多い（C#、VB.NET）
- 開発環境では SQL Server Express（無料版）を使うことが多い
- 管理ツールは SQL Server Management Studio（SSMS）

---

## 2. テーブル設計の基本

### 主キー（Primary Key）
テーブル内のレコードを一意に識別するカラム。重複を許さない。

```sql
CREATE TABLE Customers (
    CustomerID INT PRIMARY KEY IDENTITY(1,1),  -- 自動採番の主キー
    Name NVARCHAR(100) NOT NULL,
    Phone NVARCHAR(20)
);
```

### 外部キー（Foreign Key）
他のテーブルの主キーを参照するカラム。テーブル間の関連を定義する。

```sql
CREATE TABLE Orders (
    OrderID INT PRIMARY KEY IDENTITY(1,1),
    CustomerID INT NOT NULL,
    OrderDate DATE NOT NULL,
    -- 外部キー制約: CustomersテーブルのCustomerIDを参照
    CONSTRAINT FK_Orders_Customers
        FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID)
);
```

### 正規化
データの重複を排除し、整合性を保つための設計手法。

```
悪い設計（非正規化）:
| 注文ID | 顧客名   | 商品名    | 単価  |
|--------|---------|----------|-------|
| 1      | 田中太郎 | ノートPC  | 98000 |
| 2      | 田中太郎 | マウス    | 2500  |  ← 「田中太郎」が重複

良い設計（正規化）:
Customers: | CustomerID | Name     |
           | 1          | 田中太郎 |

Orders:    | OrderID | CustomerID |  ← IDで参照（重複なし）
           | 1       | 1          |
           | 2       | 1          |
```

### レビューで見るポイント
- すべてのテーブルに主キーが定義されているか？
- テーブル間の関連に外部キー制約が設定されているか？
- NOT NULL制約が必要なカラムに設定されているか？
- 適切なデータ型が選択されているか？（特に金額はDECIMAL）

---

## 3. CRUD操作

データベースの基本操作4つ。

### SELECT（読み取り）
```sql
-- 全件取得
SELECT * FROM Customers;

-- 条件付き取得
SELECT CustomerID, Name, Phone
FROM Customers
WHERE Name LIKE N'%田中%';
```

### INSERT（追加）
```sql
-- 1件追加
INSERT INTO Customers (Name, Phone, Email, CreatedAt)
VALUES (N'田中太郎', '090-1234-5678', 'tanaka@example.com', GETDATE());
```

### UPDATE（更新）
```sql
-- 条件に一致するレコードを更新
UPDATE Customers
SET Phone = '080-9999-8888'
WHERE CustomerID = 1;
```

### DELETE（削除）
```sql
-- 条件に一致するレコードを削除
DELETE FROM Customers
WHERE CustomerID = 5;
```

### レビューで見るポイント
- UPDATE/DELETEにWHERE句があるか？（WHERE句なし = 全件更新/削除）
- INSERT時にNOT NULLカラムに値が指定されているか？
- 文字列にN（Unicode）プレフィックスが付いているか？（日本語データ）

---

## 4. JOIN（テーブル結合）

複数のテーブルを関連付けてデータを取得する。

### INNER JOIN（内部結合）
両方のテーブルに一致するデータのみ取得。

```sql
-- 注文と顧客名を結合（注文がある顧客のみ）
SELECT o.OrderID, c.Name, o.OrderDate
FROM Orders o
INNER JOIN Customers c ON o.CustomerID = c.CustomerID;
```

### LEFT JOIN（左外部結合）
左テーブルのデータはすべて表示。右テーブルに一致がなければNULL。

```sql
-- 全顧客と注文を結合（注文がない顧客も表示）
SELECT c.Name, o.OrderID, o.OrderDate
FROM Customers c
LEFT JOIN Orders o ON c.CustomerID = o.CustomerID;
-- → 注文がない顧客は OrderID=NULL, OrderDate=NULL で表示される
```

### RIGHT JOIN（右外部結合）
右テーブルのデータはすべて表示。LEFT JOINの逆方向。

```
INNER JOIN: 注文がある顧客のみ
LEFT JOIN:  全顧客（注文なしも含む）
RIGHT JOIN: 全注文（顧客なしは通常ない）
```

### レビューで見るポイント
- 要件が「すべての○○を表示」ならLEFT JOINが必要ではないか？
- INNER JOINだと条件に一致しないレコードが消えることを理解しているか？
- JOINの結合条件（ON句）は正しいカラム同士か？

---

## 5. 集計関数

### 基本的な集計関数
```sql
-- 件数
SELECT COUNT(*) AS OrderCount FROM Orders;

-- 合計
SELECT SUM(TotalAmount) AS TotalSales FROM Orders;

-- 平均
SELECT AVG(UnitPrice) AS AvgPrice FROM Products;

-- 最大・最小
SELECT MAX(UnitPrice) AS MaxPrice, MIN(UnitPrice) AS MinPrice FROM Products;
```

### GROUP BY（グループ化）
```sql
-- カテゴリ別の商品数と平均単価
SELECT Category, COUNT(*) AS ProductCount, AVG(UnitPrice) AS AvgPrice
FROM Products
GROUP BY Category;
```

### HAVING（グループへの条件）
```sql
-- 注文合計が10万円以上の顧客
SELECT CustomerID, SUM(TotalAmount) AS Total
FROM Orders
GROUP BY CustomerID
HAVING SUM(TotalAmount) >= 100000;
```

### WHERE と HAVING の違い
```sql
-- WHERE: 集計前に行を絞り込む（個々の行に対する条件）
-- HAVING: 集計後にグループを絞り込む（集計結果に対する条件）

-- 正しい使い方
SELECT CustomerID, SUM(TotalAmount) AS Total
FROM Orders
WHERE Status <> N'キャンセル'       -- 集計前: キャンセル注文を除外
GROUP BY CustomerID
HAVING SUM(TotalAmount) >= 100000;  -- 集計後: 合計10万以上のみ
```

### レビューで見るポイント
- SELECT句に集計関数以外のカラムがある場合、GROUP BYに含まれているか？
- 集計結果への条件がWHEREに書かれていないか？（HAVINGが正しい）
- COUNT(*)とCOUNT(カラム名)の違いを理解しているか？（NULLの扱い）

---

## 6. WHERE句と条件指定

```sql
-- 等値比較
WHERE Status = N'完了'

-- 範囲指定
WHERE UnitPrice BETWEEN 1000 AND 50000

-- 部分一致（LIKE）
WHERE Name LIKE N'%田中%'

-- 複数値（IN）
WHERE Status IN (N'未処理', N'処理中')

-- NULLチェック（= ではなく IS を使う）
WHERE Phone IS NULL
WHERE Phone IS NOT NULL

-- 複合条件
WHERE Status = N'完了' AND OrderDate >= '2024-01-01'
WHERE Status = N'キャンセル' OR TotalAmount = 0
```

### レビューで見るポイント
- NULLの比較に`=`を使っていないか？（`= NULL`は常にFALSE。`IS NULL`が正しい）
- LIKE検索で先頭に`%`があるとインデックスが効かないことを理解しているか？
- AND/ORの優先順位は正しいか？（括弧で明示すべき）

---

## 7. インデックスの基本概念

インデックスは「本の索引」のようなもの。検索を高速化するが、更新時はオーバーヘッドになる。

```sql
-- インデックスの作成
CREATE INDEX IX_Customers_Name ON Customers(Name);

-- 複合インデックス
CREATE INDEX IX_Orders_CustomerID_OrderDate ON Orders(CustomerID, OrderDate);
```

### インデックスが有効な場面
- WHERE句で頻繁に検索するカラム
- JOIN条件に使うカラム（外部キー）
- ORDER BYで並び替えるカラム

### インデックスが不要/逆効果な場面
- レコード数が少ないテーブル
- INSERT/UPDATE/DELETEが頻繁なカラム
- 値のバリエーションが少ないカラム（例: フラグ列 0/1）

### レビューで見るポイント
- 外部キーカラムにインデックスがあるか？
- 検索条件に使われるカラムにインデックスがあるか？
- 不要なインデックスが大量に作成されていないか？

---

## 8. SQL Server Management Studio（SSMS）の使い方

### 基本操作

| 操作 | 方法 |
|------|------|
| サーバー接続 | オブジェクトエクスプローラー → 接続 → サーバー名入力 |
| クエリ実行 | 新しいクエリ → SQL入力 → F5（実行） |
| テーブル確認 | データベース → テーブル → 右クリック → 上位1000行の選択 |
| テーブル設計確認 | テーブル → 右クリック → デザイン |
| 実行計画確認 | Ctrl+L（推定実行プラン） / Ctrl+M（実際の実行プラン） |

### SSMSでのデバッグ手順
1. 対象のデータベースを選択
2. 新しいクエリウィンドウを開く
3. 問題のSQLを貼り付けて実行
4. 結果を確認し、期待と違う場合はクエリを分解して確認
5. 実行計画でパフォーマンス問題がないか確認

---

## 9. BSEがSQLレビューで見るポイント

### テーブル設計レビュー
1. **主キーが定義されているか**
2. **外部キー制約が設定されているか**（テーブル間の関連）
3. **データ型は適切か**（特に金額 → DECIMAL、日本語文字列 → NVARCHAR）
4. **NOT NULL制約**が必須カラムに設定されているか
5. **CHECK制約**で値の範囲が制限されているか（例: 在庫数 >= 0）
6. **デフォルト値**が必要なカラムに設定されているか

### クエリレビュー
1. **JOINの種類**は要件に合っているか（INNER vs LEFT）
2. **GROUP BY**に必要なカラムがすべて含まれているか
3. **WHERE vs HAVING**の使い分けは正しいか
4. **NULLの扱い**は正しいか（IS NULL / IS NOT NULL）
5. パフォーマンス上問題がないか（不要なSELECT *、LIKE '%xx%'等）

### ストアドプロシージャレビュー
1. **トランザクション**が適切に使われているか（BEGIN TRAN / COMMIT / ROLLBACK）
2. **エラーハンドリング**があるか（TRY-CATCH）
3. **ビジネスルール**が正しく実装されているか（在庫チェック等）
4. **データ整合性**が保たれるか（複数テーブルの更新が一貫しているか）
5. パラメータの型と入力値チェックは適切か

---

## 次のステップ

この基礎概念を理解したら、`07_SQLServer_Level1/` の練習問題に進んでください。
設計仕様書と照らし合わせて、DDL・クエリ・ストアドプロシージャのバグを見つける訓練を行います。
