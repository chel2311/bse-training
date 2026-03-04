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

<<<<<<< HEAD
        /// <summary>
        /// 除算（エラーメッセージを統一）
        /// </summary>
        public int Divide(int a, int b)
        {
            if (b == 0)
            {
                throw new ArgumentException("エラー: 除数に0は指定できません。正の整数を入力してください。");
            }
            return a / b;
        }
=======
        /// <summary>
        /// 除算（引数チェック強化）
        /// </summary>
        public int Divide(int a, int b)
        {
            if (b == 0)
                throw new DivideByZeroException("0で割ることはできません");

            // 結果が整数にならない場合の警告
            if (a % b != 0)
            {
                Console.WriteLine($"警告: {a} / {b} の結果は切り捨てられます");
            }
            return a / b;
        }
>>>>>>> feature/divide-validation

        /// <summary>
        /// 計算結果を表示用文字列で返す
        /// </summary>
        public string GetResultText(string operation, int a, int b, int result)
        {
            return $"{a} {operation} {b} = {result}";
        }
    }
}
