# VBA プログラミング基礎概念（BSE向け）

> コードを「書く」のではなく「読んでレビューできる」ことが目標

---

## 1. VBAとは

VBA（Visual Basic for Applications）はMicrosoft Office製品に組み込まれたプログラミング言語。
Excel VBAでは、セル操作・シート操作・帳票作成などの業務自動化マクロを作成する。

### VBE（Visual Basic Editor）の開き方
1. Excelを開く
2. `Alt + F11` でVBEが起動
3. 左側の「プロジェクトエクスプローラー」でVBAProjectを展開
4. 「挿入」メニュー → 「標準モジュール」でコードを書く場所を追加

### マクロの実行方法
- VBE上で `F5` キー（カーソルがSubプロシージャ内にある状態で）
- Excel上で `Alt + F8` → マクロ一覧から選択して「実行」
- ボタンにマクロを割り当てて実行

### .basファイルについて
- VBAモジュールをテキスト形式でエクスポートしたファイル
- VBEの「ファイル」→「ファイルのインポート」で取り込める
- 教材ではこの形式でコードを提供する

---

## 2. 変数と型宣言

VBAでは `Dim` キーワードで変数を宣言する。

```vba
' 整数
Dim count As Long

' 文字列
Dim name As String

' 小数（金額計算に使う）
Dim price As Currency

' 日付
Dim targetDate As Date

' 真偽値
Dim isValid As Boolean

' バリアント型（何でも入る。型を指定しない場合のデフォルト）
Dim value As Variant
```

### Option Explicit
```vba
Option Explicit  ' モジュールの先頭に記述

Sub Sample()
    Dim count As Long
    count = 10
    ' coutn = 20  ← スペルミスでもエラーにならない（Option Explicitがない場合）
End Sub
```

`Option Explicit` を付けると、宣言されていない変数を使った時にコンパイルエラーになる。
付けないと変数のスペルミスが検出できず、原因不明のバグになる。

### C#/VB.NETとの違い
| 項目 | VBA | C# / VB.NET |
|------|-----|-------------|
| 型宣言 | `Dim x As Long` | `int x;` / `Dim x As Integer` |
| 整数型 | `Long`推奨（Integerは16bit） | `int`（32bit） |
| 金額型 | `Currency` | `decimal` |
| 型省略 | Variant型になる | コンパイルエラー |
| 型安全性 | 弱い（暗黙変換多い） | 強い |

### レビューで見るポイント
- `Option Explicit` がモジュールの先頭にあるか？ → ないと変数名の誤字が検出できない
- 金額に適切な型（`Currency`）を使っているか？
- `Integer` ではなく `Long` を使っているか？ → VBAのIntegerは16bitで32,767が上限
- 変数が `Variant` になっていないか？ → 型が不明確でバグの温床になる

---

## 3. Sub と Function

### Sub プロシージャ（戻り値なし）
```vba
Sub ClearInputFields()
    Range("A1").Value = ""
    Range("B1").Value = ""
    Range("C1").Value = ""
End Sub
```

### Function プロシージャ（戻り値あり）
```vba
Function CalculateTax(price As Currency, taxRate As Double) As Currency
    CalculateTax = price * (1 + taxRate)
End Function

' 使い方
Dim total As Currency
total = CalculateTax(1000, 0.1)  ' 1100
```

### C#との違い
- C#: `return` で値を返す
- VBA: 関数名に代入して値を返す（`CalculateTax = ...`）

### レビューで見るポイント
- Functionなのに戻り値が設定されていない箇所はないか？
- 引数の型は呼び出し元と一致しているか？
- `ByRef`（参照渡し）と `ByVal`（値渡し）の使い分けは適切か？
  - VBAのデフォルトは `ByRef`（C#のデフォルトは値渡し）

---

## 4. Range操作とセル参照

Excel VBAの最も基本的な操作。セルの値を読み書きする。

```vba
' セルに値を設定
Range("A1").Value = "商品名"
Cells(1, 1).Value = "商品名"   ' 同じ意味（行, 列で指定）

' セルの値を読み取る
Dim productName As String
productName = Range("A1").Value

' 範囲指定
Range("A1:E10").Value = ""     ' A1～E10をクリア
Range("A1:E1").Font.Bold = True ' A1～E1を太字にする

' 最終行の取得（データがある最後の行）
Dim lastRow As Long
lastRow = Cells(Rows.Count, 1).End(xlUp).Row

' 行ループ
Dim i As Long
For i = 2 To lastRow  ' 2行目（ヘッダの次）から最終行まで
    Debug.Print Cells(i, 1).Value
Next i
```

### RangeとCellsの使い分け
| 書き方 | 使い所 |
|--------|--------|
| `Range("A1")` | 固定セルを参照する時 |
| `Cells(i, j)` | ループで行・列を変数にする時 |
| `Range("A1:E" & lastRow)` | 可変範囲を指定する時 |

### レビューで見るポイント
- 最終行の取得方法は適切か？ → `Cells(Rows.Count, 1).End(xlUp).Row` が定石
- ループの開始行は正しいか？ → ヘッダ行を飛ばして2行目からか
- セル参照でハードコーディングされた行番号・列番号がないか？
- `UsedRange` と固定範囲の使い分けは適切か？

---

## 5. Worksheetオブジェクト

複数シートを操作する場合、対象シートを明示的に指定する必要がある。

```vba
' シートを変数に格納
Dim wsInput As Worksheet
Dim wsOutput As Worksheet
Set wsInput = ThisWorkbook.Worksheets("売上データ")
Set wsOutput = ThisWorkbook.Worksheets("月次帳票")

' シート上のセルを操作
wsInput.Cells(2, 1).Value   ' 「売上データ」シートのA2セル
wsOutput.Range("A1").Value   ' 「月次帳票」シートのA1セル

' シートの存在チェック
Dim ws As Worksheet
Dim sheetExists As Boolean
sheetExists = False
For Each ws In ThisWorkbook.Worksheets
    If ws.Name = "月次帳票" Then
        sheetExists = True
        Exit For
    End If
Next ws

' シートのデータクリア
wsOutput.UsedRange.Clear    ' 使用範囲全体をクリア
wsOutput.Cells.Clear        ' シート全体をクリア
```

### レビューで見るポイント
- シート名の指定は正しいか？ → 全角/半角、スペースの有無
- `Set` キーワードを使っているか？ → オブジェクト代入には `Set` が必須
- ActiveSheetに依存していないか？ → 明示的にシート変数を使うべき
- シートのクリア範囲は適切か？ → `UsedRange.Clear` か `Cells.Clear` で確実にクリア

---

## 6. エラーハンドリング（On Error）

VBAのエラー処理はC#のtry-catchとは異なり、`On Error` ステートメントを使う。

```vba
Sub SafeProcess()
    On Error GoTo ErrorHandler

    ' 正常処理
    Dim ws As Worksheet
    Set ws = ThisWorkbook.Worksheets("存在しないシート")
    ws.Range("A1").Value = "テスト"

    Exit Sub  ' 正常終了時はここで抜ける（重要！）

ErrorHandler:
    MsgBox "エラーが発生しました: " & Err.Description, vbExclamation, "エラー"
End Sub
```

### エラー処理の3パターン
```vba
' パターン1: エラーハンドラに飛ぶ
On Error GoTo ErrorHandler

' パターン2: エラーを無視して次の行に進む（危険！）
On Error Resume Next

' パターン3: エラー処理を解除（通常のエラーに戻る）
On Error GoTo 0
```

### C#との比較
| 項目 | VBA | C# |
|------|-----|-----|
| エラー処理 | `On Error GoTo` | `try-catch` |
| エラー情報 | `Err.Number`, `Err.Description` | `Exception` オブジェクト |
| リソース解放 | `GoTo` で手動 | `finally` / `using` |
| エラー無視 | `On Error Resume Next` | （該当なし）|

### レビューで見るポイント
- `On Error Resume Next` が広範囲に使われていないか？ → エラーの握りつぶし
- `Exit Sub` がErrorHandlerの手前にあるか？ → ないと正常時もエラー処理に入る
- `Err.Clear` でエラー情報がリセットされているか？
- エラー時にユーザーに適切なメッセージが表示されるか？

---

## 7. よく使うVBA関数・メソッド

### 文字列操作
```vba
Left("2024/01/15", 7)       ' "2024/01" （左から7文字）
Right("2024/01/15", 2)      ' "15"（右から2文字）
Mid("2024/01/15", 6, 2)     ' "01"（6文字目から2文字）
InStr("2024/01/15", "/")    ' 5（"/"の位置）
Format(Now, "yyyy/mm")      ' "2024/01"（日付の書式変換）
```

### 型変換
```vba
CLng("123")      ' 文字列→Long
CDbl("3.14")     ' 文字列→Double
CStr(123)        ' 数値→文字列
CDate("2024/01/15") ' 文字列→Date
IsDate("2024/01/15") ' True（日付として有効か判定）
IsNumeric("123")     ' True（数値として有効か判定）
```

### メッセージボックス
```vba
' 情報表示
MsgBox "処理が完了しました", vbInformation, "完了"

' 確認ダイアログ
Dim result As VbMsgBoxResult
result = MsgBox("実行しますか？", vbYesNo + vbQuestion, "確認")
If result = vbYes Then
    ' はいの処理
End If
```

### InputBox
```vba
Dim inputValue As String
inputValue = InputBox("対象月を入力してください（yyyy/mm）", "月の指定")
If inputValue = "" Then Exit Sub  ' キャンセル時
```

### レビューで見るポイント
- 日付の比較方法は適切か？ → 文字列比較と日付型比較の違い
- 型変換でエラーが起きる可能性はないか？ → 入力値の検証が先
- MsgBoxの引数（アイコン、ボタン種類）は適切か？
- InputBoxのキャンセル処理があるか？

---

## 8. VBEでのデバッグ基本操作

| 操作 | ショートカット | 用途 |
|------|--------------|------|
| ブレークポイント設置 | F9 | 実行を止めたい行に設置 |
| 実行 | F5 | マクロの実行（ブレークポイントで止まる） |
| ステップオーバー | F8 | 1行ずつ実行 |
| 変数の値確認 | マウスホバー | 変数にカーソルを合わせると値が見える |
| イミディエイトウィンドウ | Ctrl+G | `Debug.Print` の出力先 / 直接式を実行 |
| ローカルウィンドウ | - | 現在のスコープの全変数の値を表示 |

### Debug.Print の活用
```vba
Sub SampleDebug()
    Dim i As Long
    For i = 1 To 10
        Debug.Print "i = " & i & ", 値 = " & Cells(i, 1).Value
    Next i
End Sub
```
イミディエイトウィンドウ（Ctrl+G）に出力される。
MsgBoxと違い、処理を止めずに値を確認できる。

### デバッグの手順
1. 「おかしい」と思う処理の手前にブレークポイント（F9）
2. マクロを実行（F5）
3. 止まったら変数にマウスを合わせて値を確認
4. F8で1行ずつ進めながら値の変化を追う
5. イミディエイトウィンドウで `?変数名` と入力して値を確認することもできる

---

## 次のステップ

この基礎概念を理解したら、`03_VBA_Level1/` の練習問題に進んでください。
VBAマクロのコードを読み、設計仕様書と照らし合わせてバグを見つける訓練を行います。
