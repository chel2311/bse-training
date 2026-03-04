#include "Circle.h"
#include <sstream>
#include <stdexcept>

const double PI = 3.14159265358979;

Circle::Circle(double radius)
{
    if (radius <= 0)
    {
        throw std::invalid_argument("半径は正の値である必要があります");
    }
    this->radius = radius;
    type = "Circle";
}

Circle::~Circle()
{
}

double Circle::GetArea()
{
    // ★★★ バグ1: 面積の計算が間違っている ★★★
    // 仕様: 面積 = π × r × r（π × r の2乗）
    // 実装: π × r × 2 になっている（2乗ではなく×2）
    // → 半径5の場合、正しくは78.54だが、31.42になってしまう
    return PI * radius * 2;
}

double Circle::GetPerimeter()
{
    return 2 * PI * radius;
}

std::string Circle::GetInfo()
{
    std::ostringstream oss;
    oss << "Circle [radius=" << radius
        << ", area=" << GetArea()
        << ", perimeter=" << GetPerimeter() << "]";
    return oss.str();
}
