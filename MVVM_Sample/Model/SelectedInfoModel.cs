using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using PropertyChanged;

namespace MVVM_Sample.Model
{
    [AddINotifyPropertyChangedInterface]
    public class SelectedInfoModel
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SelectedInfoModel() { }

        /// <summary>
        /// TextBlock表示文字列
        /// </summary>
        public string TextBlockString { get; set; } = "初期表示テキスト";

        /// <summary>
        /// TextBlock表示色
        /// </summary>
        public SolidColorBrush TextBlockForeground { get; set; } = new SolidColorBrush(Colors.LightSkyBlue);
    }
}
