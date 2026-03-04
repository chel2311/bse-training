using System;

namespace GitTraining
{
    /// <summary>
    /// 基本的な計算機能を提供するクラス
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
        /// 除算（エラーメッセージを統一）
        /// ★ mainブランチでの変更: エラーメッセージを詳細化 ★
        /// </summary>
        public int Divide(int a, int b)
        {
            if (b == 0)
            {
                throw new ArgumentException("エラー: 除数に0は指定できません。正の整数を入力してください。");
            }
            return a / b;
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
