-- ============================================
-- 受注管理データベース テーブル作成スクリプト
-- ============================================

-- データベース作成
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'OrderManagementDB')
BEGIN
    CREATE DATABASE OrderManagementDB;
END
GO

USE OrderManagementDB;
GO

-- ============================================
-- 1. Customers（顧客マスタ）
-- ============================================
CREATE TABLE Customers (
    CustomerID   INT            PRIMARY KEY IDENTITY(1,1),
    Name         NVARCHAR(100)  NOT NULL,
    Phone        NVARCHAR(20)   NULL,
    Email        NVARCHAR(200)  NULL,
    CreatedAt    DATETIME       NOT NULL DEFAULT GETDATE()
);
GO

-- ============================================
-- 2. Products（商品マスタ）
-- ============================================
CREATE TABLE Products (
    ProductID      INT            PRIMARY KEY IDENTITY(1,1),
    ProductName    NVARCHAR(200)  NOT NULL,
    Category       NVARCHAR(50)   NOT NULL,
    UnitPrice      DECIMAL(10,2)  NOT NULL,
    StockQuantity  INT            NOT NULL
);
GO

-- ============================================
-- 3. Orders（受注ヘッダ）
-- ============================================
CREATE TABLE Orders (
    OrderID      INT            PRIMARY KEY IDENTITY(1,1),
    CustomerID   INT            NOT NULL,
    OrderDate    DATE           NOT NULL DEFAULT GETDATE(),
    TotalAmount  DECIMAL(12,2)  NOT NULL DEFAULT 0,
    Status       NVARCHAR(20)   NOT NULL DEFAULT N'未処理'
);
GO

-- ============================================
-- 4. OrderDetails（受注明細）
-- ============================================
CREATE TABLE OrderDetails (
    DetailID    INT          PRIMARY KEY IDENTITY(1,1),
    OrderID     INT          NOT NULL,
    ProductID   INT          NOT NULL,
    Quantity    INT          NOT NULL CHECK (Quantity > 0),
    UnitPrice   DECIMAL(10,2) NOT NULL,
    Subtotal    INT          NOT NULL,
    CONSTRAINT FK_OrderDetails_Orders
        FOREIGN KEY (OrderID) REFERENCES Orders(OrderID),
    CONSTRAINT FK_OrderDetails_Products
        FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);
GO

-- インデックス作成
CREATE INDEX IX_Orders_CustomerID ON Orders(CustomerID);
CREATE INDEX IX_OrderDetails_OrderID ON OrderDetails(OrderID);
CREATE INDEX IX_OrderDetails_ProductID ON OrderDetails(ProductID);
GO

PRINT N'テーブル作成が完了しました。';
GO
