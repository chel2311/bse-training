#include "ShapeManager.h"
#include <iostream>

ShapeManager::ShapeManager()
{
}

ShapeManager::~ShapeManager()
{
    // ★★★ バグ2: shapesの中身をdeleteしていない ★★★
    // 仕様: ShapeManagerのデストラクタで保持する全図形のメモリを解放（delete）すること
    // 実装: shapes vectorの中身をdeleteする処理がない
    // → ShapeManagerが破棄されても、newで確保した図形オブジェクトが解放されず
    //    メモリリークが発生する

    // 正しくは以下のようにすべての要素をdeleteする必要がある:
    // for (Shape* shape : shapes)
    // {
    //     delete shape;
    // }
    // shapes.clear();
}

void ShapeManager::AddShape(Shape* shape)
{
    if (shape != nullptr)
    {
        shapes.push_back(shape);
    }
}

bool ShapeManager::RemoveShape(int index)
{
    if (index < 0 || index >= static_cast<int>(shapes.size()))
    {
        return false;
    }

    delete shapes[index];
    shapes.erase(shapes.begin() + index);
    return true;
}

double ShapeManager::GetTotalArea()
{
    double total = 0.0;
    for (Shape* shape : shapes)
    {
        total += shape->GetArea();
    }
    return total;
}

int ShapeManager::GetShapeCount() const
{
    return static_cast<int>(shapes.size());
}

std::vector<Shape*> ShapeManager::FindByType(const std::string& type)
{
    std::vector<Shape*> result;
    for (Shape* shape : shapes)
    {
        if (shape->GetType() == type)
        {
            result.push_back(shape);
        }
    }
    return result;
}

void ShapeManager::PrintAll()
{
    std::cout << "=== 図形一覧 ===" << std::endl;
    for (int i = 0; i < static_cast<int>(shapes.size()); i++)
    {
        std::cout << "[" << i << "] " << shapes[i]->GetInfo() << std::endl;
    }
    std::cout << "=== 合計面積: " << GetTotalArea() << " ===" << std::endl;
    std::cout << "=== 図形数: " << GetShapeCount() << " ===" << std::endl;
}
