#include <iostream>
#include <stdexcept>
#include "ShapeManager.h"
#include "Circle.h"
#include "Rectangle.h"
#include "Triangle.h"

int main()
{
    std::cout << "=== 図形計算ツール ===" << std::endl;
    std::cout << std::endl;

    try
    {
        ShapeManager manager;

        // 円を追加（半径 5.0）
        Circle* circle = new Circle(5.0);
        manager.AddShape(circle);
        std::cout << "円を追加しました: " << circle->GetInfo() << std::endl;

        // 矩形を追加（幅 4.0, 高さ 6.0）
        Rectangle* rect = new Rectangle(4.0, 6.0);
        manager.AddShape(rect);
        std::cout << "矩形を追加しました: " << rect->GetInfo() << std::endl;

        // 三角形を追加（底辺 3.0, 高さ 4.0, 辺 3.0, 4.0, 5.0）
        Triangle* tri = new Triangle(3.0, 4.0, 3.0, 4.0, 5.0);
        manager.AddShape(tri);
        std::cout << "三角形を追加しました: " << tri->GetInfo() << std::endl;

        std::cout << std::endl;

        // 全図形の情報を表示
        manager.PrintAll();

        std::cout << std::endl;

        // タイプ別検索テスト
        std::cout << "=== タイプ別検索 ===" << std::endl;
        auto circles = manager.FindByType("Circle");
        std::cout << "Circle の数: " << circles.size() << std::endl;
        auto rectangles = manager.FindByType("Rectangle");
        std::cout << "Rectangle の数: " << rectangles.size() << std::endl;
        auto triangles = manager.FindByType("Triangle");
        std::cout << "Triangle の数: " << triangles.size() << std::endl;

        std::cout << std::endl;

        // 図形の削除テスト
        std::cout << "=== 削除テスト ===" << std::endl;
        if (manager.RemoveShape(0))
        {
            std::cout << "インデックス0の図形を削除しました" << std::endl;
        }
        manager.PrintAll();
    }
    catch (const std::invalid_argument& e)
    {
        std::cerr << "入力エラー: " << e.what() << std::endl;
    }
    catch (const std::exception& e)
    {
        std::cerr << "エラー: " << e.what() << std::endl;
    }

    std::cout << std::endl;
    std::cout << "Press Enter to exit...";
    std::cin.get();

    return 0;
}
