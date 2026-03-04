#pragma once
#include "Shape.h"

// 三角形クラス
class Triangle : public Shape
{
public:
    Triangle(double base_len, double height, double a, double b, double c);
    ~Triangle();

    double GetArea() override;
    double GetPerimeter() override;
    std::string GetInfo() override;

private:
    double base_len;
    double height;
    double side_a;
    double side_b;
    double side_c;
};
