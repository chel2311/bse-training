#include "Shape.h"

Shape::Shape()
    : type("Unknown")
{
}

Shape::~Shape()
{
}

std::string Shape::GetType() const
{
    return type;
}
