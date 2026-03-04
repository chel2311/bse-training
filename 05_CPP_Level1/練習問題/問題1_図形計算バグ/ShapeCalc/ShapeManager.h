#pragma once
#include "Shape.h"
#include <vector>
#include <string>

// 図形管理クラス
class ShapeManager
{
public:
    ShapeManager();
    ~ShapeManager();

    // 図形を追加
    void AddShape(Shape* shape);

    // 指定インデックスの図形を削除
    bool RemoveShape(int index);

    // 全図形の面積合計を返す
    double GetTotalArea();

    // 図形の総数を返す
    int GetShapeCount() const;

    // 指定タイプの図形を検索
    std::vector<Shape*> FindByType(const std::string& type);

    // 全図形の情報を表示
    void PrintAll();

private:
    std::vector<Shape*> shapes;
};
