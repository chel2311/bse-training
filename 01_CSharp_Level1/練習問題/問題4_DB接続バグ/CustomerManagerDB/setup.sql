-- =============================================
-- 顧客管理アプリ（DB版） テーブル作成スクリプト
-- =============================================

-- データベース作成（存在しない場合）
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'CustomerDB')
BEGIN
    CREATE DATABASE CustomerDB;
END
GO

USE CustomerDB;
GO

-- テーブル作成（存在しない場合）
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'Customers') AND type = N'U')
BEGIN
    CREATE TABLE Customers (
        Id      INT            PRIMARY KEY IDENTITY(1,1),
        Name    NVARCHAR(100)  NOT NULL,
        Phone   NVARCHAR(20)   NULL,
        Age     INT            NOT NULL,
        Email   NVARCHAR(200)  NULL
    );
END
GO

-- サンプルデータ挿入
INSERT INTO Customers (Name, Phone, Age, Email) VALUES (N'田中太郎', N'090-1234-5678', 35, N'tanaka@example.com');
INSERT INTO Customers (Name, Phone, Age, Email) VALUES (N'鈴木花子', N'080-9876-5432', 28, N'suzuki@example.com');
INSERT INTO Customers (Name, Phone, Age, Email) VALUES (N'佐藤次郎', N'070-1111-2222', 42, N'sato@example.com');
GO

-- 確認
SELECT * FROM Customers;
GO
