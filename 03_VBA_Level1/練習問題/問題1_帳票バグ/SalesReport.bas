Attribute VB_Name = "SalesReport"
Option Explicit

'===============================================================================
' 月次売上帳票生成マクロ
' 概要: 売上データシートから指定月のデータを抽出し、月次帳票シートに転記する
'===============================================================================

Sub GenerateMonthlyReport()

    On Error GoTo ErrorHandler

    ' --- 変数宣言 ---
    Dim wsData As Worksheet         ' 入力シート（売上データ）
    Dim wsReport As Worksheet       ' 出力シート（月次帳票）
    Dim targetMonth As String       ' 対象月（yyyy/mm）
    Dim lastRowData As Long         ' 売上データの最終行
    Dim outputRow As Long           ' 帳票の書き込み行
    Dim i As Long                   ' ループカウンタ
    Dim dataCount As Long           ' 抽出件数
    Dim totalQty As Long            ' 数量合計
    Dim totalAmount As Currency     ' 売上金額合計
    Dim cellDate As Date            ' セルの日付値
    Dim cellMonth As String         ' セルの年月文字列

    ' --- FR-008: 対象月の入力 ---
    targetMonth = InputBox("対象月を入力してください（yyyy/mm）", "月次帳票生成")
    If targetMonth = "" Then
        Exit Sub  ' キャンセル時は処理中断
    End If

    ' --- シート参照の取得 ---
    Set wsData = ThisWorkbook.Worksheets("売上データ")
    Set wsReport = ThisWorkbook.Worksheets("月次帳票")

    ' --- 画面更新の停止（処理速度向上） ---
    Application.ScreenUpdating = False

    ' --- FR-007: 既存データのクリア ---
    wsReport.Range("A1:E50").Clear

    ' --- ヘッダ行の設定 ---
    wsReport.Cells(1, 1).Value = "日付"
    wsReport.Cells(1, 2).Value = "商品名"
    wsReport.Cells(1, 3).Value = "数量"
    wsReport.Cells(1, 4).Value = "単価"
    wsReport.Cells(1, 5).Value = "売上金額"

    ' --- 売上データの最終行を取得 ---
    lastRowData = wsData.Cells(wsData.Rows.Count, 1).End(xlUp).Row

    ' --- 初期化 ---
    outputRow = 2       ' 帳票の2行目から書き込み開始
    dataCount = 0       ' 抽出件数
    totalQty = 0        ' 数量合計
    totalAmount = 0     ' 金額合計

    ' --- FR-001: 売上データをループして対象月を抽出 ---
    For i = 2 To lastRowData

        ' 日付セルの値を取得
        If IsDate(wsData.Cells(i, 1).Value) Then
            cellDate = wsData.Cells(i, 1).Value
            cellMonth = Format(cellDate, "yyyy/mm")

            ' 対象月と一致するか判定
            If cellMonth = targetMonth Then

                ' --- FR-003: 月次帳票シートに転記 ---
                wsReport.Cells(outputRow, 1).Value = cellDate                       ' 日付
                wsReport.Cells(outputRow, 2).Value = wsData.Cells(i, 2).Value       ' 商品名
                wsReport.Cells(outputRow, 3).Value = wsData.Cells(i, 3).Value       ' 数量
                wsReport.Cells(outputRow, 4).Value = wsData.Cells(i, 4).Value       ' 単価
                wsReport.Cells(outputRow, 5).Value = wsData.Cells(i, 5).Value       ' 売上金額

                ' --- 合計用に加算 ---
                totalQty = totalQty + wsData.Cells(i, 3).Value
                totalAmount = totalAmount + wsData.Cells(i, 5).Value

                dataCount = dataCount + 1
                outputRow = outputRow + 1

            End If
        End If
    Next i

    ' --- FR-006: データなし時の処理 ---
    If dataCount = 0 Then
        MsgBox "該当月のデータはありません", vbInformation, "月次帳票生成"
        Application.ScreenUpdating = True
        Exit Sub
    End If

    ' --- FR-004: 合計行の追加 ---
    wsReport.Cells(outputRow, 2).Value = "合計"
    wsReport.Cells(outputRow, 3).Value = totalQty
    wsReport.Cells(outputRow, 5).Value = totalAmount

    ' --- 完了メッセージ ---
    Application.ScreenUpdating = True
    MsgBox targetMonth & " の月次帳票を作成しました。" & vbCrLf & _
           "データ件数: " & dataCount & "件", vbInformation, "月次帳票生成"

    Exit Sub

ErrorHandler:
    Application.ScreenUpdating = True
    MsgBox "エラーが発生しました: " & Err.Description, vbExclamation, "エラー"

End Sub
