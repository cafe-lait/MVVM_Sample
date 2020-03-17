using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MVVM_Sample.Model
{
    [AddINotifyPropertyChangedInterface]
    public class ThemeModel
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        ThemeModel() { }

        /// <summary>
        /// 引数付きコンストラクタ
        /// </summary>
        /// <param name="_id"></param>
        /// <param name="_name"></param>
        /// <param name="_kind"></param>
        /// <param name="_input"></param>
        public ThemeModel(string _name, string _kind, DateTime _input)
        {
            ThemeId = "THEME_" + DateTime.Now.ToString(@"hhmmssfff");
            ThemeName = _name;
            ThemeKind = _kind;
            InputDate = _input;
        }

        /// <summary>
        /// テーマID
        /// </summary>
        public string ThemeId { get; set; } = "未設定";

        /// <summary>
        /// テーマ名
        /// </summary>
        public string ThemeName { get; set; } = "未設定";

        /// <summary>
        /// テーマ種別
        /// </summary>
        public string ThemeKind { get; set; } = "未設定";

        /// <summary>
        /// 登録日時
        /// </summary>
        public DateTime InputDate { get; set; } = DateTime.Now;

        /// <summary>
        /// ステータスバリュー
        /// </summary>
        public double StatusValue { get; set; } = 0.0;

        /// <summary>
        /// ステータスマーク
        /// </summary>
        public string PublishStatus { get; set; } = "";

        /// <summary>
        /// ステータス表示色
        /// </summary>
        public System.Windows.Media.SolidColorBrush PublishStatusColor { get; set; } =
            new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Transparent);

        /// <summary>
        /// ステータス設定
        /// </summary>
        /// <param name="isPublish"></param>        
        public void SetPubStatus(bool isPublish)
        {
            if (isPublish)
            {
                StatusValue = 100.0;
                PublishStatus = "〇";
                PublishStatusColor = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Green);
            }
            else
            {
                PublishStatus = "×";
                PublishStatusColor = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red);
            }
        }
        /// <summary>
        /// ステータスリセット
        /// </summary>
        /// <param name="isPublish"></param>
        public void ClearPubStatus()
        {
            StatusValue = 0.0;
            PublishStatus = "";
            PublishStatusColor = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Transparent);
        }
    }
}
