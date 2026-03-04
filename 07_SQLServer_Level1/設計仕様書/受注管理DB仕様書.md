# 受注管理データベース 設計仕様書

## 1. 概要

受注管理業務で使用するデータベース。顧客・商品・受注情報を管理する。

## 2. 開発環境

| 項目 | 仕様 |
|------|------|
| RDBMS | SQL Server 2019 以上 |
| 管理ツール | SQL Server Management Studio (SSMS) |
| データベース名 | OrderManagementDB |
| 照合順序 | Japanese_CI_AS |

## 3. テーブル設計

### 3.1 Customers（顧客マスタ）

| カラム名 | データ型 | 制約 | 説明 |
|----------|---------|------|------|
| CustomerID | INT | PRIMARY KEY, IDENTITY(1,1) | 顧客ID（自動採番） |
| Name | NVARCHAR(100) | NOT NULL | 顧客名 |
| Phone | NVARCHAR(20) | NULL許可 | 電話番号 |
| Email | NVARCHAR(200) | NULL許可 | メールアドレス |
| CreatedAt | DATETIME | NOT NULL, DEFAULT GETDATE() | 登録日時 |

### 3.2 Products（商品マスタ）

| カラム名 | データ型 | 制約 | 説明 |
|----------|---------|------|------|
| ProductID | INT | PRIMARY KEY, IDENTITY(1,1) | 商品ID（自動採番） |
| ProductName | NVARCHAR(200) | NOT NULL | 商品名 |
| Category | NVARCHAR(50) | NOT NULL | カテゴリ |
| UnitPrice | DECIMAL(10,2) | NOT NULL | 単価 |
| StockQuantity | INT | NOT NULL, **CHECK (StockQuantity >= 0)** | 在庫数（0以上） |

### 3.3 Orders（受注ヘッダ）

| カラム名 | データ型 | 制約 | 説明 |
|----------|---------|------|------|
| OrderID | INT | PRIMARY KEY, IDENTITY(1,1) | 受注ID（自動採番） |
| CustomerID | INT | NOT NULL, **FOREIGN KEY → Customers(CustomerID)** | 顧客ID |
| OrderDate | DATE | NOT NULL, DEFAULT GETDATE() | 受注日 |
| TotalAmount | DECIMAL(12,2) | NOT NULL, DEFAULT 0 | **受注合計金額** |
| Status | NVARCHAR(20) | NOT NULL, DEFAULT N'未処理' | ステータス |

**Status の取りうる値**: 未処理 / 処理中 / 完了 / キャンセル

### 3.4 OrderDetails（受注明細）

| カラム名 | データ型 | 制約 | 説明 |
|----------|---------|------|------|
| DetailID | INT | PRIMARY KEY, IDENTITY(1,1) | 明細ID（自動採番） |
| OrderID | INT | NOT NULL, **FOREIGN KEY → Orders(OrderID)** | 受注ID |
| ProductID | INT | NOT NULL, **FOREIGN KEY → Products(ProductID)** | 商品ID |
| Quantity | INT | NOT NULL, CHECK (Quantity > 0) | 数量（1以上） |
| UnitPrice | DECIMAL(10,2) | NOT NULL | 受注時単価 |
| Subtotal | **DECIMAL(12,2)** | NOT NULL | **小計 = Quantity x UnitPrice** |

## 4. テーブル関連図

```
Customers (1) ──── (N) Orders (1) ──── (N) OrderDetails (N) ──── (1) Products
   │                      │                      │                      │
   PK: CustomerID         PK: OrderID            PK: DetailID           PK: ProductID
                          FK: CustomerID         FK: OrderID
                                                 FK: ProductID
```

## 5. ビジネスルール

### BR-001: 受注合計金額の計算
**Orders.TotalAmount = 該当OrderIDのOrderDetails.Subtotalの合計（SUM）**

受注明細が追加・変更・削除されるたびに、Ordersの TotalAmount を再計算して更新すること。

### BR-002: 在庫の減算
**受注登録時に、Products.StockQuantity を注文数量（Quantity）分だけ減算する。**

```
更新後StockQuantity = 更新前StockQuantity - Quantity
```

### BR-003: 在庫チェック
**受注登録時に在庫数が注文数量を下回る場合はエラーとする。**

```
条件: Products.StockQuantity >= OrderDetails.Quantity
違反時: エラーを返し、受注登録を行わない
```

### BR-004: キャンセル時の在庫戻し
受注ステータスを「キャンセル」に変更する際、該当受注の明細に含まれる各商品の在庫数を元に戻す。

```
更新後StockQuantity = 更新前StockQuantity + Quantity
```

### BR-005: 外部キー制約の必須設定
**CustomerID、ProductID、OrderIDの参照関係には、必ず外部キー制約（FOREIGN KEY）を設定すること。**

外部キー制約により、以下を保証する:
- 存在しない顧客への受注を防止
- 存在しない商品への受注明細を防止
- 存在しない受注への明細追加を防止

## 6. ストアドプロシージャ仕様

### 6.1 sp_CreateOrder（受注登録）

**パラメータ**:

| パラメータ | 型 | 説明 |
|-----------|----|----|
| @CustomerID | INT | 顧客ID |
| @ProductID | INT | 商品ID |
| @Quantity | INT | 注文数量 |

**処理フロー**:
1. **在庫チェック**（BR-003）: StockQuantity >= @Quantity を確認
2. Ordersテーブルに新規レコードを挿入
3. OrderDetailsテーブルに明細レコードを挿入（Subtotal = Quantity x UnitPrice）
4. **在庫を減算**（BR-002）
5. **TotalAmountを更新**（BR-001）
6. 上記の処理は**すべてトランザクション内で実行**すること

**エラー処理**:
- 在庫不足の場合: エラーメッセージを返し、処理を中断
- その他のエラー: ロールバックしてエラーメッセージを返す

## 7. 非機能要件

- テーブルの主キーには自動採番（IDENTITY）を使用すること
- 金額系カラムはすべてDECIMAL型を使用すること（INT不可）
- 日本語文字列はNVARCHAR型を使用すること
- 外部キーカラムにはインデックスを作成すること
