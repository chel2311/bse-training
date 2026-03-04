#include "Rectangle.h"
#include <sstream>
#include <stdexcept>

Rectangle::Rectangle(double width, double height)
{
    if (width <= 0 || height <= 0)
    {
        throw std::invalid_argument("幅と高さは正の値である必要があります");
    }
    this->width = width;
    this->height = height;
    type = "Rectangle";
}

Rectangle::~Rectangle()
{
}

double Rectangle::GetArea()
{
    return width * height;
}

double Rectangle::GetPerimeter()
{
    return 2 * (width + height);
}

std::string Rectangle::GetInfo()
{
    std::ostringstream oss;
    oss << "Rectangle [width=" << width
        << ", height=" << height
        << ", area=" << GetArea()
        << ", perimeter=" << GetPerimeter() << "]";
    return oss.str();
}
