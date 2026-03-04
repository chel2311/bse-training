#pragma once
#include "Shape.h"

// 円クラス
class Circle : public Shape
{
public:
    Circle(double radius);
    ~Circle();

    double GetArea() override;
    double GetPerimeter() override;
    std::string GetInfo() override;

private:
    double radius;
};
