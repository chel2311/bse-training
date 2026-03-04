#pragma once
#include "Shape.h"

// 矩形クラス
class Rectangle : public Shape
{
public:
    Rectangle(double width, double height);
    ~Rectangle();

    double GetArea() override;
    double GetPerimeter() override;
    std::string GetInfo() override;

private:
    double width;
    double height;
};
