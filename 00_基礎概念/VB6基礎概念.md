# VB6 プログラミング基礎概念（BSE向け）

> レガシーコードを「読んでレビューできる」ことが目標

---

## 1. VB6とは

Visual Basic 6.0（VB6）は1998年にMicrosoftがリリースした開発言語。2008年にサポートが終了したが、多くの業務システムで現役稼働している。BSEとしてレガシー保守案件に関わる際、VB6のコードを読んでレビューできることが求められる。

### 開発環境

| 項目 | 内容 |
|------|------|
| IDE | Visual Basic 6.0 IDE（VB6 IDE） |
| ファイル形式 | .frm（フォーム）、.bas（標準モジュール）、.cls（クラスモジュール）、.vbp（プロジェクト） |
| 実行環境 | Windows上でランタイム（MSVBVM60.DLL）が必要 |
| 特徴 | フォームファイルはテキスト形式で保存され、テキストエディタで読める |

---

## 2. 変数宣言（Dim）

VB6では `Dim` で変数を宣言する。型指定をしないと `Variant` 型になる。

```vb
' 整数
Dim empNo As Long

' 文字列
Dim empName As String

' 日付
Dim hireDate As Date

' 真偽値
Dim isActive As Boolean

' 型指定なし → Variant型（何でも入る。非推奨）
Dim value
```

### レビューで見るポイント
- `Option Explicit` が宣言されているか？ → ないと未宣言変数が使えてしまい、タイプミスがバグになる
- `Variant` 型が多用されていないか？ → 型安全性がなく予期しない動作の原因になる
- 数値型に `Integer`（-32768～32767）を使っていないか？ → 大きな数は `Long` を使うべき

---

## 3. プロシージャ（Sub / Function）

処理をまとめるもの。`Sub` は戻り値なし、`Function` は戻り値あり。

```vb
' Subプロシージャ（戻り値なし）
Private Sub ShowMessage(msg As String)
    MsgBox msg, vbInformation, "お知らせ"
End Sub

' Functionプロシージャ（戻り値あり）
Private Function CalcTax(price As Long) As Long
    CalcTax = price * 1.1    ' 関数名に代入 = 戻り値
End Function

' 使い方
Dim total As Long
total = CalcTax(1000)    ' 1100
```

### レビューで見るポイント
- `Function` の戻り値が関数名への代入で設定されているか？（VB6独特の書き方）
- 引数の `ByVal` / `ByRef` は適切か？（VB6のデフォルトは `ByRef`。意図せず呼び出し元の値が変わる）

---

## 4. ユーザー定義型（Type）

C言語の構造体に相当する。関連データをまとめる。

```vb
' 社員データの型定義（標準モジュール .bas に記述）
Public Type Employee
    EmpNo As String
    EmpName As String
    Department As String
    HireDate As Date
End Type

' 使い方
Dim emp As Employee
emp.EmpNo = "001"
emp.EmpName = "田中太郎"
emp.Department = "営業部"
emp.HireDate = #2015/04/01#
```

### レビューで見るポイント
- 型定義は標準モジュール（.bas）に配置されているか？
- メンバーのデータ型は適切か？（日付は `Date` 型、金額は `Currency` 型を使うべき）

---

## 5. 制御構造

### If...Then...Else

```vb
If age >= 18 Then
    MsgBox "成人です"
ElseIf age >= 0 Then
    MsgBox "未成年です"
Else
    MsgBox "不正な値です"
End If
```

### For...Next

```vb
Dim i As Long
For i = 0 To 9
    Debug.Print i
Next i
```

### Select Case

```vb
Select Case department
    Case "営業部"
        bonus = salary * 0.2
    Case "開発部"
        bonus = salary * 0.15
    Case Else
        bonus = salary * 0.1
End Select
```

### レビューで見るポイント
- `If` の条件が仕様通りか？（`>=` と `>` の境界値に注意）
- `Select Case` に `Case Else` があるか？（想定外の値への対処）
- ループの回数は正しいか？（VB6の `For i = 1 To 10` は10回実行される）

---

## 6. エラー処理（On Error GoTo）

VB6にはtry-catchがない。`On Error GoTo` でエラー処理を行う。

```vb
Private Sub LoadData()
    On Error GoTo ErrorHandler

    ' 正常処理
    Dim rs As ADODB.Recordset
    Set rs = New ADODB.Recordset
    rs.Open "SELECT * FROM Employees", cn

    ' 後始末
    rs.Close
    Set rs = Nothing
    Exit Sub

ErrorHandler:
    MsgBox "エラーが発生しました: " & Err.Description, vbCritical, "エラー"
    If Not rs Is Nothing Then
        If rs.State = adStateOpen Then rs.Close
        Set rs = Nothing
    End If
End Sub
```

### レビューで見るポイント
- `On Error GoTo` が設定されているか？ → ないとエラー時にアプリが停止する
- `On Error Resume Next`（エラー無視）が使われていないか？ → エラーの握りつぶし
- `Exit Sub` / `Exit Function` でエラーハンドラを飛び越えているか？ → ないと正常時もエラーハンドラを実行してしまう
- `Set obj = Nothing` でオブジェクト参照を解放しているか？

---

## 7. With...End With構文

同じオブジェクトに対する複数の操作をまとめる。

```vb
' Withなし（冗長）
lstEmployees.ListItems.Add , , emp.EmpNo
lstEmployees.ListItems(lstEmployees.ListItems.Count).SubItems(1) = emp.EmpName
lstEmployees.ListItems(lstEmployees.ListItems.Count).SubItems(2) = emp.Department

' Withあり（簡潔）
With lstEmployees.ListItems.Add(, , emp.EmpNo)
    .SubItems(1) = emp.EmpName
    .SubItems(2) = emp.Department
    .SubItems(3) = Format(emp.HireDate, "yyyy/mm/dd")
End With
```

### レビューで見るポイント
- `With` の対象オブジェクトは正しいか？
- ネストした `With` で対象が混同していないか？

---

## 8. フォームファイル（.frm）の構造

VB6のフォームファイルはテキスト形式で、2つの部分で構成される。

### 前半: コントロール定義（デザイナ部分）

```
VERSION 5.00
Object = "{831FDD16-0C5C-11D2-A9FC-0000F8754DA1}#2.0#0"; "MSCOMCTL.OCX"
Begin VB.Form frmMain
   Caption         =   "社員名簿管理"
   ClientHeight    =   7200
   ClientWidth     =   10200
   Begin VB.TextBox txtEmpNo
      Height          =   375
      Left            =   1800
      Top             =   360
      Width           =   2415
   End
   Begin VB.CommandButton cmdAdd
      Caption         =   "追加"
      Height          =   375
      Left            =   600
      Top             =   3000
      Width           =   1215
   End
End
```

### 後半: コード部分

```
Attribute VB_Name = "frmMain"
Option Explicit

Private Sub Form_Load()
    ' フォーム読み込み時の処理
End Sub

Private Sub cmdAdd_Click()
    ' 追加ボタンクリック時の処理
End Sub
```

### レビューで見るポイント
- コントロール名が設計書の命名規則と一致しているか？
- イベントプロシージャ名が正しいか？（コントロール名_イベント名）
- `Option Explicit` が宣言されているか？

---

## 9. VB6とVB.NETの主な違い

VB.NETに移行されたコードをレビューする際にも役立つ。

| 項目 | VB6 | VB.NET |
|------|-----|--------|
| オブジェクト代入 | `Set obj = New Class1` | `Dim obj As New Class1` |
| デフォルト引数渡し | `ByRef`（参照渡し） | `ByVal`（値渡し） |
| 汎用型 | `Variant` | `Object` |
| 文字列関数 | `Left$()`, `Mid$()`, `Right$()` | `.Substring()`, `.Length` |
| エラー処理 | `On Error GoTo` | `Try...Catch...Finally` |
| 固定長文字列 | `Dim s As String * 10` | サポートなし |
| 配列の下限 | `Option Base` で変更可能 | 常に0始まり |
| 型判定 | `TypeName()`, `VarType()` | `GetType()`, `TypeOf...Is` |
| コレクション | `Collection` | `List(Of T)`, `Dictionary` |
| 日付関数 | `DateDiff()`, `DateAdd()`, `IsDate()` | `DateTime` クラスメソッド |

---

## 10. レガシーコードレビューのポイント

VB6のレガシーコードをレビューする際、特に注意すべき点。

### よくある問題

1. **Option Explicit の欠如**
   - 宣言なしで変数が使えるため、タイプミスが実行時まで発見されない

2. **Variant型の多用**
   - 型安全性がなく、実行時の型変換エラーの原因になる

3. **On Error Resume Next の乱用**
   - エラーが無視され、データ不整合の原因になる

4. **Set ... = Nothing の漏れ**
   - オブジェクト参照の解放忘れ。メモリリークの原因

5. **DateDiff等の日付関数の引数順序**
   - 引数の順序ミスで計算結果が逆になるバグは頻出

6. **配列の境界チェック不足**
   - `ReDim` / `UBound` の取り扱いミスによる実行時エラー

### レビューの進め方

1. 仕様書（設計書）を先に読む
2. モジュール構造を把握する（.vbp → .frm → .bas の順）
3. コントロール定義と画面設計を照合する
4. イベントプロシージャ（_Click, _Load等）から処理の流れを追う
5. 仕様書の各機能要件に対応する実装を確認する
6. エラー処理とバリデーションの漏れを確認する

---

## 次のステップ

この基礎概念を理解したら、`04_VB6_Level1/` の練習問題に進んでください。
実際のVB6フォームファイルを読み、設計仕様書と照らし合わせてバグを見つける訓練を行います。
