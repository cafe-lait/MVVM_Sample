using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;

namespace MVVM_Sample.Model
{
    [AddINotifyPropertyChangedInterface]
    public class StatusModel
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public StatusModel() { }

        /// <summary>
        /// ステータスメッセージ
        /// </summary>
        public string StatusMessage { get; set; } = "準備完了";

        /// <summary>
        /// ステータスバリュー
        /// </summary>
        public double StatusValue { get; set; } = 0.0;

        /// <summary>
        /// アニメーション
        /// </summary>
        public bool IsIndeterminate { get; set; } = false;
    }
}
