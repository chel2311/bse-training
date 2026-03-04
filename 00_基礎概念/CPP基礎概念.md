# C++ プログラミング基礎概念（BSE向け）

> コードを「書く」のではなく「読んでレビューできる」ことが目標

---

## 1. C++の基本構造

C++はC言語を拡張したオブジェクト指向言語。CADや組み込みシステムなど、高速処理が必要な分野で広く使われる。

```cpp
#include <iostream>    // 標準入出力のヘッダファイル
#include <string>      // 文字列のヘッダファイル

int main()
{
    // 整数
    int age = 30;

    // 小数（double: 倍精度浮動小数点）
    double price = 1980.50;

    // 文字列
    std::string name = "田中太郎";

    // 真偽値
    bool isActive = true;

    // コンソール出力
    std::cout << name << " (" << age << "歳)" << std::endl;

    return 0;
}
```

### C#との違い
| 項目 | C# | C++ |
|------|-----|-----|
| 文字列 | `string name = "...";` | `std::string name = "...";` |
| 出力 | `Console.WriteLine(...)` | `std::cout << ... << std::endl;` |
| 名前空間 | `using System;` | `#include <iostream>` + `using namespace std;` |
| メイン関数 | `static void Main()` | `int main()` |

### レビューで見るポイント
- `#include` が必要なヘッダを漏れなくインクルードしているか
- `using namespace std;` をヘッダファイルで使っていないか（名前衝突の原因）
- `main()` の戻り値が `int` になっているか

---

## 2. ポインタと参照

C++特有の概念。メモリを直接扱えるため強力だが、バグの温床にもなる。

### ポインタ（*）
```cpp
int value = 42;
int* ptr = &value;   // &でアドレスを取得し、ポインタに格納

std::cout << *ptr;    // *で値にアクセス（デリファレンス） → 42
std::cout << ptr;     // アドレスが表示される → 0x7ffd...
```

### 参照（&）
```cpp
int value = 42;
int& ref = value;    // 参照 = 元の変数の別名

ref = 100;           // refを変更するとvalueも変わる
std::cout << value;  // 100
```

### new と delete（動的メモリ確保）
```cpp
// newでヒープにメモリ確保
int* p = new int(42);

// 使い終わったらdeleteで解放
delete p;
p = nullptr;  // 解放後はnullptrにする

// 配列の場合
int* arr = new int[10];
delete[] arr;  // 配列はdelete[]で解放
```

### C#との違い
| 項目 | C# | C++ |
|------|-----|-----|
| メモリ管理 | ガベージコレクション（自動） | 手動（new/delete） |
| ポインタ | 通常使わない | 頻繁に使う |
| null | `null` | `nullptr` |
| メモリリーク | ほぼ発生しない | delete忘れで発生する |

### レビューで見るポイント
- **`new` に対応する `delete` があるか** （最頻出のレビュー項目）
- ポインタが `nullptr` でないことを確認してからアクセスしているか
- `delete` した後のポインタを使っていないか（ダングリングポインタ）
- 配列の `new[]` に `delete[]` を使っているか（`delete` ではダメ）

---

## 3. ヘッダファイルとソースファイル

C++では、宣言（.h）と実装（.cpp）を分けるのが基本。

### ヘッダファイル（.h）: 設計図
```cpp
// Calculator.h
#pragma once  // 二重インクルード防止

class Calculator
{
public:
    int Add(int a, int b);       // 宣言のみ
    int Subtract(int a, int b);  // 宣言のみ
};
```

### ソースファイル（.cpp）: 実装
```cpp
// Calculator.cpp
#include "Calculator.h"  // 自分のヘッダをインクルード

int Calculator::Add(int a, int b)
{
    return a + b;
}

int Calculator::Subtract(int a, int b)
{
    return a - b;
}
```

### レビューで見るポイント
- ヘッダファイルに `#pragma once` があるか
- `.h` の宣言と `.cpp` の実装が一致しているか（引数の型・数、戻り値の型）
- ヘッダファイルに実装コードを書いていないか（小さなインライン関数を除く）

---

## 4. クラスと継承

### クラスの基本
```cpp
// Shape.h
#pragma once
#include <string>

class Shape
{
public:
    Shape();                      // コンストラクタ
    virtual ~Shape();             // 仮想デストラクタ

    virtual double GetArea() = 0;      // 純粋仮想関数（= 0で抽象メソッド）
    virtual double GetPerimeter() = 0; // 派生クラスで必ず実装する
    virtual std::string GetInfo() = 0;

protected:
    std::string type;  // 派生クラスからアクセス可能
};
```

### 継承
```cpp
// Circle.h
#pragma once
#include "Shape.h"

class Circle : public Shape  // Shapeを継承
{
public:
    Circle(double radius);
    ~Circle();

    double GetArea() override;      // 親クラスの仮想関数を実装
    double GetPerimeter() override;
    std::string GetInfo() override;

private:
    double radius;
};
```

### C#との対応
| 概念 | C# | C++ |
|------|-----|-----|
| 継承 | `class Circle : Shape` | `class Circle : public Shape` |
| 抽象メソッド | `abstract double GetArea();` | `virtual double GetArea() = 0;` |
| オーバーライド | `override` キーワード | `override` キーワード（C++11以降） |
| インターフェース | `interface` | 純粋仮想関数のみのクラス |
| デストラクタ | `~Shape()` (ファイナライザ、通常不要) | `virtual ~Shape()` (ポインタ使用時に必須) |

### レビューで見るポイント
- 基底クラスのデストラクタが `virtual` になっているか（ポリモーフィズム使用時に必須）
- 純粋仮想関数（= 0）がすべての派生クラスで実装されているか
- `override` キーワードが付いているか（付け忘れると別のメソッドになる）
- アクセス修飾子（`public`, `protected`, `private`）が適切か

---

## 5. CAD関連で頻出するC++パターン

### 図形クラスの継承階層
```
Shape（基底クラス）
  ├── Circle（円）
  ├── Rectangle（矩形）
  ├── Triangle（三角形）
  ├── Line（線分）
  └── Polygon（多角形）
```

### 座標と寸法の計算
```cpp
// 座標点
struct Point2D
{
    double x;
    double y;
};

// 距離計算
double Distance(Point2D p1, Point2D p2)
{
    double dx = p2.x - p1.x;
    double dy = p2.y - p1.y;
    return sqrt(dx * dx + dy * dy);  // #include <cmath> が必要
}
```

### レビューで見るポイント
- 数学関数（`sqrt`, `sin`, `cos` 等）の `#include <cmath>` が漏れていないか
- 円周率は定数で定義しているか（マジックナンバーになっていないか）
- 座標計算の精度（`float` ではなく `double` を使っているか）

---

## 6. コンパイルとビルドの基本（Visual Studio）

### ソリューションとプロジェクト
```
ShapeCalc.sln              ← ソリューションファイル（プロジェクトの集合）
  └── ShapeCalc.vcxproj    ← プロジェクトファイル（C++固有）
        ├── Shape.h         ← ヘッダファイル
        ├── Shape.cpp       ← ソースファイル
        ├── Circle.h
        ├── Circle.cpp
        └── main.cpp        ← エントリポイント
```

### C#プロジェクト（.csproj）との違い
| 項目 | C#（.csproj） | C++（.vcxproj） |
|------|-------------|---------------|
| ヘッダファイル | なし | `ClInclude` で管理 |
| ソースファイル | `Compile` で管理 | `ClCompile` で管理 |
| NuGet参照 | `PackageReference` | `vcpkg` 等で管理 |
| ビルド結果 | .dll / .exe | .obj → .exe |

### ビルドエラーの読み方
```
error C2065: 'radius' : 宣言されていない識別子です。
→ 変数名のスペルミス or #include漏れ

error LNK2019: 未解決の外部シンボル
→ .cppに関数の実装がない or .cppがプロジェクトに含まれていない
```

---

## 7. BSEがC++レビューで注意すべきポイント

### メモリリーク
```cpp
// NG: deleteされない
void BadFunction()
{
    Shape* shape = new Circle(5.0);
    // ... 処理 ...
    // deleteが呼ばれずに関数終了 → メモリリーク
}

// OK: 使い終わったらdelete
void GoodFunction()
{
    Shape* shape = new Circle(5.0);
    // ... 処理 ...
    delete shape;  // 確実に解放
}
```

### バッファオーバーフロー
```cpp
// NG: 配列の範囲外アクセス
char buffer[10];
strcpy(buffer, "これは10文字を超える文字列です");  // はみ出す

// OK: サイズチェック付き
char buffer[10];
strncpy(buffer, input, sizeof(buffer) - 1);
buffer[sizeof(buffer) - 1] = '\0';
```

### NULLポインタアクセス
```cpp
// NG: NULLチェックなし
Shape* shape = FindShape(id);
double area = shape->GetArea();  // shapeがnullptrだとクラッシュ

// OK: NULLチェックあり
Shape* shape = FindShape(id);
if (shape != nullptr)
{
    double area = shape->GetArea();
}
```

### コンストラクタでのバリデーション
```cpp
// NG: 不正な値をそのまま格納
Circle::Circle(double r) : radius(r) { }
// radius = -5.0 でも作成できてしまう

// OK: 値を検証
Circle::Circle(double r)
{
    if (r <= 0)
    {
        throw std::invalid_argument("半径は正の値である必要があります");
    }
    radius = r;
}
```

---

## 8. C++レビューの優先チェック項目

| 優先度 | チェック項目 | 理由 |
|--------|------------|------|
| 最高 | `new` に対応する `delete` があるか | メモリリーク防止 |
| 最高 | ポインタの `nullptr` チェック | クラッシュ防止 |
| 高 | 配列の範囲外アクセスがないか | バッファオーバーフロー防止 |
| 高 | 仮想デストラクタが定義されているか | メモリリーク防止 |
| 高 | 計算式が仕様書通りか | 計算バグ防止 |
| 中 | コンストラクタで入力値を検証しているか | 不正データ防止 |
| 中 | `#include` が適切か | コンパイルエラー防止 |
| 低 | `override` キーワードが付いているか | 意図しないオーバーロード防止 |

---

## 次のステップ

この基礎概念を理解したら、`05_CPP_Level1/` の練習問題に進んでください。
CADモジュールの図形計算コードを読み、設計仕様書と照らし合わせてバグを見つける訓練を行います。
