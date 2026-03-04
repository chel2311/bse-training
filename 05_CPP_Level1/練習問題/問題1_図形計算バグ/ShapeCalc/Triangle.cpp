#include "Triangle.h"
#include <sstream>
#include <stdexcept>

Triangle::Triangle(double base_len, double height, double a, double b, double c)
{
    // ★★★ バグ3: 3辺(a, b, c)のバリデーションが不足している ★★★
    // 仕様: 底辺、高さ、3辺（a, b, c）はすべて正の値（> 0）であること
    // 実装: base_len と height のチェックはあるが、a, b, c のチェックがない
    // → マイナスの辺の長さでも三角形が作成できてしまう
    if (base_len <= 0 || height <= 0)
    {
        throw std::invalid_argument("底辺と高さは正の値である必要があります");
    }

    this->base_len = base_len;
    this->height = height;
    this->side_a = a;
    this->side_b = b;
    this->side_c = c;
    type = "Triangle";
}

Triangle::~Triangle()
{
}

double Triangle::GetArea()
{
    return base_len * height / 2.0;
}

double Triangle::GetPerimeter()
{
    return side_a + side_b + side_c;
}

std::string Triangle::GetInfo()
{
    std::ostringstream oss;
    oss << "Triangle [base=" << base_len
        << ", height=" << height
        << ", sides=(" << side_a << ", " << side_b << ", " << side_c << ")"
        << ", area=" << GetArea()
        << ", perimeter=" << GetPerimeter() << "]";
    return oss.str();
}
