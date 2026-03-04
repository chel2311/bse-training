Attribute VB_Name = "modEmployee"
Option Explicit

' ====================================
' 社員データ型定義
' ====================================
Public Type Employee
    EmpNo       As String       ' 社員番号
    EmpName     As String       ' 氏名
    Department  As String       ' 部署
    HireDate    As Date         ' 入社日
End Type

' ====================================
' モジュールレベル変数
' ====================================
Public Employees()  As Employee     ' 社員データ配列
Public EmployeeCount As Long        ' 社員数

' ====================================
' 初期化
' ====================================
Public Sub InitializeData()
    EmployeeCount = 0
    Erase Employees
End Sub

' ====================================
' 社員追加
' ====================================
Public Sub AddEmployee(emp As Employee)
    EmployeeCount = EmployeeCount + 1
    ReDim Preserve Employees(1 To EmployeeCount)
    Employees(EmployeeCount) = emp
End Sub

' ====================================
' 社員削除（インデックス指定）
' ====================================
Public Sub DeleteEmployee(index As Long)
    Dim i As Long

    If index < 1 Or index > EmployeeCount Then
        Exit Sub
    End If

    ' 削除対象以降を前に詰める
    For i = index To EmployeeCount - 1
        Employees(i) = Employees(i + 1)
    Next i

    EmployeeCount = EmployeeCount - 1

    If EmployeeCount > 0 Then
        ReDim Preserve Employees(1 To EmployeeCount)
    Else
        Erase Employees
    End If
End Sub

' ====================================
' 社員番号から配列インデックスを検索
' ====================================
Public Function FindEmployeeIndex(empNo As String) As Long
    Dim i As Long

    FindEmployeeIndex = 0   ' 見つからない場合は0

    For i = 1 To EmployeeCount
        If Employees(i).EmpNo = empNo Then
            FindEmployeeIndex = i
            Exit Function
        End If
    Next i
End Function

' ====================================
' 勤続年数計算
' ====================================
Public Function CalcYearsOfService(dtHireDate As Date) As Long
    ' ★★★ バグ1: DateDiffの引数の順序が逆 ★★★
    ' 仕様(FR-006): DateDiff("yyyy", 入社日, Now) で年数を取得
    ' 実装: DateDiff("yyyy", Now, dtHireDate) になっている
    ' → マイナスの勤続年数が返される
    CalcYearsOfService = DateDiff("yyyy", Now, dtHireDate)
End Function

' ====================================
' 社員番号の形式チェック（数字のみかどうか）
' ====================================
Public Function IsValidEmpNo(empNo As String) As Boolean
    Dim i As Long

    IsValidEmpNo = True

    If Len(empNo) = 0 Then
        IsValidEmpNo = False
        Exit Function
    End If

    For i = 1 To Len(empNo)
        If Not IsNumeric(Mid$(empNo, i, 1)) Then
            IsValidEmpNo = False
            Exit Function
        End If
    Next i
End Function

' ====================================
' 日付フォーマット変換
' ====================================
Public Function FormatHireDate(dtDate As Date) As String
    FormatHireDate = Format(dtDate, "yyyy/mm/dd")
End Function
