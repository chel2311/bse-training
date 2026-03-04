using System;

namespace GitTraining
{
    /// <summary>
    /// 顧客情報を表すクラス
    /// </summary>
    public class Customer
    {
        /// <summary>顧客ID</summary>
        public int Id { get; set; }

        /// <summary>顧客名</summary>
        public string Name { get; set; }

        /// <summary>年齢</summary>
        public int Age { get; set; }

        /// <summary>電話番号</summary>
        public string Phone { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Customer(int id, string name, int age, string phone)
        {
            Id = id;
            Name = name;
            Age = age;
            Phone = phone;
        }

        /// <summary>
        /// 表示用の情報を取得する
        /// </summary>
        public string GetDisplayInfo()
        {
            return $"[{Id}] {Name}（{Age}歳）";
        }

        /// <summary>
        /// 顧客情報の妥当性を検証する
        /// </summary>
        public bool IsValid()
        {
            if (string.IsNullOrEmpty(Name))
                return false;

            if (Age < 0 || Age > 150)
                return false;

            return true;
        }
    }
}
