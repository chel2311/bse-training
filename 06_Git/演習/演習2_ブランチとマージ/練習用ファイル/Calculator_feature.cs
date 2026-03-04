using System;

namespace GitTraining
{
    /// <summary>
    /// 基本的な計算機能を提供するクラス
    /// （feature/add-modulo ブランチでの変更後の状態）
    /// </summary>
    public class Calculator
    {
        /// <summary>
        /// 加算
        /// </summary>
        public int Add(int a, int b)
        {
            return a + b;
        }

        /// <summary>
        /// 減算
        /// </summary>
        public int Subtract(int a, int b)
        {
            return a - b;
        }

        /// <summary>
        /// 乗算
        /// </summary>
        public int Multiply(int a, int b)
        {
            return a * b;
        }

        /// <summary>
        /// 除算
        /// </summary>
        public int Divide(int a, int b)
        {
            if (b == 0)
                throw new DivideByZeroException("0で割ることはできません");
            return a / b;
        }

        /// <summary>
        /// 剰余（余り）を計算する
        /// ★ featureブランチで追加したメソッド ★
        /// </summary>
        public int Modulo(int a, int b)
        {
            if (b == 0)
                throw new DivideByZeroException("0で割ることはできません");
            return a % b;
        }

        /// <summary>
        /// 計算結果を表示用文字列で返す
        /// </summary>
        public string GetResultText(string operation, int a, int b, int result)
        {
            return $"{a} {operation} {b} = {result}";
        }
    }
}
