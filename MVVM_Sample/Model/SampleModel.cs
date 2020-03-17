using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MVVM_Sample.Model
{
    public class SampleModel : INotifyPropertyChanged
    {
        #region ViewModel : PropertyChanged Event
        /// <summary>  
        /// PropertyChanged : イベントハンドラ
        /// </summary>  
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>  
        /// PropertyChanged : 通知View側*2にプロパティの変更を通知
        /// </summary>  
        private void NotifyPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        /// <summary>  
        /// PropertyChanged : 反映
        /// </summary>  
        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        SampleModel() { }

        /// <summary>
        /// 引数付きコンストラクタ
        /// </summary>
        /// <param name="_id"></param>
        /// <param name="_name"></param>
        /// <param name="_kind"></param>
        /// <param name="_input"></param>
        public SampleModel(string _name, string _kind, DateTime _input)
        {
            ThemeId = "THEME_"+DateTime.Now.ToString(@"hhmmssfff");
            ThemeName = _name;
            ThemeKind = _kind;
            InputDate = _input;
        }

        /// <summary>
        /// テーマID
        /// </summary>
        private string _themeId = "未設定";
        public string ThemeId
        {
            get { return _themeId; }
            set
            {
                _themeId = value;
                NotifyPropertyChanged(nameof(ThemeId));
            }
        }

        /// <summary>
        /// テーマ名
        /// </summary>
        private string _themeName = "未設定";
        public string ThemeName
        {
            get { return _themeName; }
            set
            {
                _themeName = value;
                NotifyPropertyChanged(nameof(ThemeName));
            }
        }

        /// <summary>
        /// テーマ種別
        /// </summary>
        private string _themeKind = "未設定";
        public string ThemeKind
        {
            get { return _themeKind; }
            set
            {
                _themeKind = value;
                NotifyPropertyChanged(nameof(ThemeKind));
            }
        }

        /// <summary>
        /// 登録日時
        /// </summary>
        private DateTime _inputDate = DateTime.Now;
        public DateTime InputDate
        {
            get { return _inputDate; }
            set
            {
                _inputDate = value;
                NotifyPropertyChanged(nameof(InputDate));
            }
        }
    }
}
