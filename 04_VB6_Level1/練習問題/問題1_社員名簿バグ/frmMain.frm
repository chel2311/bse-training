VERSION 5.00
Object = "{831FDD16-0C5C-11D2-A9FC-0000F8754DA1}#2.0#0"; "MSCOMCTL.OCX"
Begin VB.Form frmMain
   Caption         =   "社員名簿管理"
   ClientHeight    =   7200
   ClientLeft      =   120
   ClientTop       =   465
   ClientWidth     =   10200
   LinkTopic       =   "Form1"
   ScaleHeight     =   7200
   ScaleWidth      =   10200
   StartUpPosition =   3  'Windows Default
   Begin VB.TextBox txtEmpNo
      Height          =   375
      Left            =   1800
      TabIndex        =   1
      Top             =   360
      Width           =   2415
   End
   Begin VB.TextBox txtName
      Height          =   375
      Left            =   1800
      TabIndex        =   3
      Top             =   840
      Width           =   2415
   End
   Begin VB.ComboBox cmbDept
      Height          =   315
      Left            =   1800
      Style           =   2  'Dropdown List
      TabIndex        =   5
      Top             =   1320
      Width           =   2415
   End
   Begin VB.TextBox txtHireDate
      Height          =   375
      Left            =   1800
      TabIndex        =   7
      Top             =   1800
      Width           =   2415
   End
   Begin VB.CommandButton cmdAdd
      Caption         =   "追加"
      Height          =   375
      Left            =   600
      TabIndex        =   8
      Top             =   2520
      Width           =   1215
   End
   Begin VB.CommandButton cmdDelete
      Caption         =   "削除"
      Height          =   375
      Left            =   2040
      TabIndex        =   9
      Top             =   2520
      Width           =   1215
   End
   Begin VB.CommandButton cmdSearch
      Caption         =   "検索"
      Height          =   375
      Left            =   3480
      TabIndex        =   10
      Top             =   2520
      Width           =   1215
   End
   Begin MSComctlLib.ListView lstEmployees
      Height          =   3615
      Left            =   360
      TabIndex        =   11
      Top             =   3240
      Width           =   9495
      _ExtentX        =   16748
      _ExtentY        =   6376
      View            =   3
      LabelEdit       =   1
      LabelWrap       =   -1  'True
      HideSelection   =   0   'False
      FullRowSelect   =   -1  'True
      GridLines       =   -1  'True
      _Version        =   393217
      ForeColor       =   -2147483640
      BackColor       =   -2147483643
      BorderStyle     =   1
      Appearance      =   1
      NumItems        =   0
   End
   Begin VB.Label lblEmpNo
      Caption         =   "社員番号:"
      Height          =   255
      Left            =   600
      TabIndex        =   0
      Top             =   420
      Width           =   1095
   End
   Begin VB.Label lblName
      Caption         =   "氏名:"
      Height          =   255
      Left            =   600
      TabIndex        =   2
      Top             =   900
      Width           =   1095
   End
   Begin VB.Label lblDept
      Caption         =   "部署:"
      Height          =   255
      Left            =   600
      TabIndex        =   4
      Top             =   1380
      Width           =   1095
   End
   Begin VB.Label lblHireDate
      Caption         =   "入社日:"
      Height          =   255
      Left            =   600
      TabIndex        =   6
      Top             =   1860
      Width           =   1095
   End
End
Attribute VB_Name = "frmMain"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

' ====================================
' フォーム読み込み時
' ====================================
Private Sub Form_Load()
    ' コンボボックスの初期化
    With cmbDept
        .AddItem "営業部"
        .AddItem "開発部"
        .AddItem "総務部"
        .AddItem "経理部"
        .ListIndex = 0
    End With

    ' ListViewのカラム設定
    With lstEmployees
        .ColumnHeaders.Add , , "社員番号", 1200
        .ColumnHeaders.Add , , "氏名", 1800
        .ColumnHeaders.Add , , "部署", 1200
        .ColumnHeaders.Add , , "入社日", 1500
        .ColumnHeaders.Add , , "勤続年数", 1200
    End With

    ' モジュールの初期化
    Call InitializeData

    ' サンプルデータの追加
    Dim emp As Employee

    emp.EmpNo = "001"
    emp.EmpName = "田中太郎"
    emp.Department = "営業部"
    emp.HireDate = #4/1/2015#
    Call AddEmployee(emp)

    emp.EmpNo = "002"
    emp.EmpName = "鈴木花子"
    emp.Department = "開発部"
    emp.HireDate = #10/1/2020#
    Call AddEmployee(emp)

    emp.EmpNo = "003"
    emp.EmpName = "佐藤次郎"
    emp.Department = "総務部"
    emp.HireDate = #4/1/2018#
    Call AddEmployee(emp)

    ' 一覧を更新
    Call RefreshList
End Sub

' ====================================
' 追加ボタン
' ====================================
Private Sub cmdAdd_Click()
    On Error GoTo ErrorHandler

    ' --- バリデーション ---

    ' 社員番号: 必須チェック
    If Len(Trim$(txtEmpNo.Text)) = 0 Then
        MsgBox "社員番号を入力してください", vbExclamation, "入力エラー"
        txtEmpNo.SetFocus
        Exit Sub
    End If

    ' 社員番号: 数字チェック
    If Not IsValidEmpNo(Trim$(txtEmpNo.Text)) Then
        MsgBox "社員番号は数字で入力してください", vbExclamation, "入力エラー"
        txtEmpNo.SetFocus
        Exit Sub
    End If

    ' ★★★ バグ2: 社員番号の重複チェックがない ★★★
    ' 仕様(FR-001): 社員番号は重複不可。既に登録済みの場合はエラー表示
    ' 実装: 重複チェック処理が丸ごと欠落している
    ' → 同じ社員番号で複数の社員が登録できてしまう

    ' 氏名: 必須チェック
    If Len(Trim$(txtName.Text)) = 0 Then
        MsgBox "氏名を入力してください", vbExclamation, "入力エラー"
        txtName.SetFocus
        Exit Sub
    End If

    ' 入社日: 日付形式チェック
    ' ★★★ バグ3: IsDate関数を使っていない ★★★
    ' 仕様(FR-004): IsDate関数を使用して日付形式チェックを行う
    ' 実装: 文字列長チェック(Len = 10)のみで判定している
    ' → "aaaa/bb/cc" のような不正な値が通ってしまう
    If Len(txtHireDate.Text) <> 10 Then
        MsgBox "入社日を正しい日付形式(yyyy/mm/dd)で入力してください", vbExclamation, "入力エラー"
        txtHireDate.SetFocus
        Exit Sub
    End If

    ' --- 社員データの作成と追加 ---
    Dim emp As Employee
    emp.EmpNo = Trim$(txtEmpNo.Text)
    emp.EmpName = Trim$(txtName.Text)
    emp.Department = cmbDept.Text
    emp.HireDate = CDate(txtHireDate.Text)

    Call AddEmployee(emp)

    ' 入力欄のクリア
    txtEmpNo.Text = ""
    txtName.Text = ""
    cmbDept.ListIndex = 0
    txtHireDate.Text = ""

    ' 一覧を更新
    Call RefreshList

    Exit Sub

ErrorHandler:
    MsgBox "社員の追加中にエラーが発生しました。" & vbCrLf & _
           "入社日の形式を確認してください。" & vbCrLf & _
           "エラー: " & Err.Description, vbCritical, "エラー"
End Sub

' ====================================
' 削除ボタン
' ====================================
Private Sub cmdDelete_Click()
    On Error GoTo ErrorHandler

    ' 選択チェック
    If lstEmployees.SelectedItem Is Nothing Then
        MsgBox "社員を選択してください", vbExclamation, "警告"
        Exit Sub
    End If

    ' 選択された社員番号を取得
    Dim selectedEmpNo As String
    selectedEmpNo = lstEmployees.SelectedItem.Text

    ' 配列内のインデックスを検索
    Dim index As Long
    index = FindEmployeeIndex(selectedEmpNo)

    If index > 0 Then
        ' 確認ダイアログ
        Dim result As VbMsgBoxResult
        result = MsgBox(Employees(index).EmpName & "を削除しますか？", _
                        vbQuestion + vbYesNo, "削除確認")

        If result = vbYes Then
            Call DeleteEmployee(index)
            Call RefreshList
        End If
    End If

    Exit Sub

ErrorHandler:
    MsgBox "削除中にエラーが発生しました: " & Err.Description, vbCritical, "エラー"
End Sub

' ====================================
' 検索ボタン
' ====================================
Private Sub cmdSearch_Click()
    On Error GoTo ErrorHandler

    Dim keyword As String
    keyword = Trim$(txtName.Text)

    ' キーワードが空の場合は全件表示
    If Len(keyword) = 0 Then
        Call RefreshList
        Exit Sub
    End If

    ' 一覧をクリア
    lstEmployees.ListItems.Clear

    ' 部分一致検索
    Dim i As Long
    Dim matchCount As Long
    matchCount = 0

    For i = 1 To EmployeeCount
        If InStr(1, Employees(i).EmpName, keyword, vbTextCompare) > 0 Then
            Dim li As ListItem
            Set li = lstEmployees.ListItems.Add(, , Employees(i).EmpNo)
            li.SubItems(1) = Employees(i).EmpName
            li.SubItems(2) = Employees(i).Department
            li.SubItems(3) = FormatHireDate(Employees(i).HireDate)
            li.SubItems(4) = CalcYearsOfService(Employees(i).HireDate) & "年"
            matchCount = matchCount + 1
        End If
    Next i

    If matchCount = 0 Then
        MsgBox "該当する社員はいません", vbInformation, "検索結果"
    End If

    Exit Sub

ErrorHandler:
    MsgBox "検索中にエラーが発生しました: " & Err.Description, vbCritical, "エラー"
End Sub

' ====================================
' 一覧表示の更新
' ====================================
Private Sub RefreshList()
    On Error GoTo ErrorHandler

    lstEmployees.ListItems.Clear

    Dim i As Long
    For i = 1 To EmployeeCount
        Dim li As ListItem
        Set li = lstEmployees.ListItems.Add(, , Employees(i).EmpNo)
        li.SubItems(1) = Employees(i).EmpName
        li.SubItems(2) = Employees(i).Department
        li.SubItems(3) = FormatHireDate(Employees(i).HireDate)
        li.SubItems(4) = CalcYearsOfService(Employees(i).HireDate) & "年"
    Next i

    Exit Sub

ErrorHandler:
    MsgBox "一覧の更新中にエラーが発生しました: " & Err.Description, vbCritical, "エラー"
End Sub
