-- ============================================
-- 受注管理データベース 帳票用クエリ
-- ============================================

USE OrderManagementDB;
GO

-- ============================================
-- 要件1: 顧客別の受注合計金額を表示
-- ============================================
-- 要件:
--   すべての顧客について、受注合計金額を表示する。
--   注文がない顧客も「0円」として表示すること。
--   顧客名の昇順で並べること。
-- ============================================

SELECT
    c.CustomerID,
    c.Name           AS CustomerName,
    SUM(o.TotalAmount) AS TotalOrderAmount
FROM
    Customers c
    INNER JOIN Orders o ON c.CustomerID = o.CustomerID
GROUP BY
    c.CustomerID, c.Name
ORDER BY
    c.Name;
GO

-- ============================================
-- 要件2: カテゴリ別の売上ランキング TOP3
-- ============================================
-- 要件:
--   商品カテゴリ別の売上合計金額を集計し、
--   売上の多い順にTOP3を表示する。
--   カテゴリ名と売上合計金額を表示すること。
-- ============================================

SELECT TOP 3
    p.Category,
    SUM(od.Subtotal) AS TotalSales
FROM
    OrderDetails od
    INNER JOIN Products p ON od.ProductID = p.ProductID
    INNER JOIN Orders o ON od.OrderID = o.OrderID
WHERE
    o.Status <> N'キャンセル'
GROUP BY
    p.ProductID
ORDER BY
    TotalSales DESC;
GO

-- ============================================
-- 要件3: 今月の受注で合計10万円以上の顧客一覧
-- ============================================
-- 要件:
--   2024年2月の受注を対象に、
--   顧客ごとの受注合計金額が10万円以上の顧客を一覧表示する。
--   キャンセル注文は除外すること。
--   顧客名と受注合計金額を表示する。
-- ============================================

SELECT
    c.Name             AS CustomerName,
    SUM(o.TotalAmount) AS MonthlyTotal
FROM
    Orders o
    INNER JOIN Customers c ON o.CustomerID = c.CustomerID
WHERE
    o.OrderDate >= '2024-02-01'
    AND o.OrderDate < '2024-03-01'
    AND o.Status <> N'キャンセル'
    AND SUM(o.TotalAmount) >= 100000
GROUP BY
    c.CustomerID, c.Name
ORDER BY
    MonthlyTotal DESC;
GO
