-- ============================================
-- 受注登録ストアドプロシージャ
-- ============================================
-- 仕様:
--   顧客ID、商品ID、数量を受け取り、受注を登録する。
--   詳細は設計仕様書「6.1 sp_CreateOrder」を参照。
-- ============================================

USE OrderManagementDB;
GO

CREATE OR ALTER PROCEDURE sp_CreateOrder
    @CustomerID  INT,
    @ProductID   INT,
    @Quantity    INT
AS
BEGIN
    SET NOCOUNT ON;

    -- 変数宣言
    DECLARE @UnitPrice   DECIMAL(10,2);
    DECLARE @Subtotal    DECIMAL(12,2);
    DECLARE @NewOrderID  INT;

    -- 商品の単価を取得
    SELECT @UnitPrice = UnitPrice
    FROM Products
    WHERE ProductID = @ProductID;

    -- 商品が存在しない場合
    IF @UnitPrice IS NULL
    BEGIN
        RAISERROR(N'指定された商品が存在しません。ProductID: %d', 16, 1, @ProductID);
        RETURN;
    END

    -- 小計を計算
    SET @Subtotal = @Quantity * @UnitPrice;

    -- Ordersテーブルに受注ヘッダを挿入
    INSERT INTO Orders (CustomerID, OrderDate, TotalAmount, Status)
    VALUES (@CustomerID, GETDATE(), 0, N'未処理');

    -- 挿入した受注IDを取得
    SET @NewOrderID = SCOPE_IDENTITY();

    -- OrderDetailsテーブルに受注明細を挿入
    INSERT INTO OrderDetails (OrderID, ProductID, Quantity, UnitPrice, Subtotal)
    VALUES (@NewOrderID, @ProductID, @Quantity, @UnitPrice, @Subtotal);

    -- 在庫を減算（BR-002）
    UPDATE Products
    SET StockQuantity = StockQuantity - @Quantity
    WHERE ProductID = @ProductID;

    PRINT N'受注を登録しました。OrderID: ' + CAST(@NewOrderID AS NVARCHAR(10));
END
GO
