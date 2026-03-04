#pragma once
#include <string>

// 図形の基底クラス（抽象クラス）
class Shape
{
public:
    Shape();
    virtual ~Shape();

    // 純粋仮想関数（派生クラスで必ず実装する）
    virtual double GetArea() = 0;
    virtual double GetPerimeter() = 0;
    virtual std::string GetInfo() = 0;

    // 図形タイプ名を返す
    std::string GetType() const;

protected:
    std::string type;
};
