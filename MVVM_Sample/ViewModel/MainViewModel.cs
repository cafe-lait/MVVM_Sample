using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;
using System.Xml;
using System.Xml.Xsl;
using System.Runtime.CompilerServices;
using System.IO.Compression;
using MVVM_Sample.Common;
using System.Diagnostics;
using System.Collections.ObjectModel;
using MVVM_Sample.Model;

namespace MVVM_Sample.ViewModel
{
    public partial class MainViewModel : INotifyPropertyChanged
    {
        #region ViewModel : インスタンス
        private static MainViewModel _mvm;
        public static MainViewModel MVM
        {
            get
            {
                if (_mvm == null)
                    _mvm = new MainViewModel();
                return _mvm;

            }
            set { _mvm = value; }
        }
        #endregion

        #region ViewModel : コンストラクタ
        public MainViewModel()
        {
            // 初期化処理など
            SampleModels.Add(new SampleModel("初期表示用テーマ", "社会", DateTime.Now));
        }
        #endregion

        #region ViewModel : Properties
        /// <summary>
        /// TextBlock表示文字列
        /// </summary>
        private string _textBlockString = "初期表示テキスト";
        public string TextBlockString
        {
            get { return _textBlockString; }
            set
            {
                _textBlockString = value;
                NotifyPropertyChanged(nameof(TextBlockString));
            }
        }

        /// <summary>
        /// TextBlock表示色
        /// </summary>
        private SolidColorBrush _textBlockForeground = new SolidColorBrush(Colors.White);
        public SolidColorBrush TextBlockForeground
        {
            get { return _textBlockForeground; }
            set
            {
                _textBlockForeground = value;
                NotifyPropertyChanged(nameof(TextBlockForeground));
            }
        }

        /// <summary>
        /// SampleModelのコレクション
        /// </summary>
        private ObservableCollection<SampleModel> _sampleModels = new ObservableCollection<SampleModel>();
        public ObservableCollection<SampleModel> SampleModels
        {
            get { return _sampleModels; }
            set
            {
                _sampleModels = value;
                NotifyPropertyChanged(nameof(SampleModels));
            }
        }
        #endregion

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

        #region ViewModel : Command DoCommand コマンド振り分け
        /// <summary>  
        /// Button (Add) : Command
        /// </summary> 
        private void DoCommandExecute(object parameter = null)
        {
            var cmd = string.Empty;
            object param1 = null;
            object param2 = null;
            try
            {
                // マルチパラメタ
                if (parameter.GetType().IsArray)
                {
                    var values = (object[])parameter;
                    cmd = values[0].ToString();
                    param1 = values[1];
                    if (3 <= values.Count()) param2 = values[2];
                }
                // シングルパラメタ
                else
                {
                    cmd = parameter.ToString();
                }

               　Debug.WriteLine($"■ DoCommand：{cmd.ToString()}　" +
                    $"Param1：{(param1 == null ? "null" : param1)}　" +
                    $"Param2：{(param2 == null ? "null" : param2)}");

                switch (cmd)
                {
                    case "SetDate":
                        SetDate(param1 as SampleModel);
                        break;
                    case "AddTheme":
                        AddTheme(param1 as SampleModel);
                        break;
                    case "DeleteTheme":
                        DeleteTheme(param1 as SampleModel);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                // 異常系
            }
        }

        public bool DoCommandCanExecute(object parameter)
        {
            return true;
        }
        private static ICommand _DoCommand;
        public ICommand DoCommand
        {
            get
            {
                if (_DoCommand == null) _DoCommand = new DelegateCommand
                {
                    ExecuteHandler = DoCommandExecute,
                    CanExecuteHandler = DoCommandCanExecute,
                }; return _DoCommand;
            }
        }
        #endregion

        #region ViewModel : Method SingleCommand
        private void SetDate(SampleModel model)
        {
            var target = SampleModels.Where(x => x.ThemeId == model.ThemeId).SingleOrDefault();
            target.InputDate = DateTime.Now;
        }
        private void DeleteTheme(SampleModel model)
        {
            var target = SampleModels.Where(x => x.ThemeId == model.ThemeId).SingleOrDefault();
            SampleModels.Remove(target);
        }
        private void AddTheme(SampleModel model)
        {
            SampleModels.Add(new SampleModel("追加テーマ_"+DateTime.Now.ToString(@"mmss_fff"), SampleModels.Count%2==0?"社会":"政治",DateTime.Now));
        }
        #endregion
    }

    #region Converter

    public class MultiParamConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            //string cmd = values[0].ToString();
            //Window wnd = (Window)values[1];
            return values.Clone();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return new object[] { };
        }
    }
    // MultiParamConv
    public class BoolVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((bool?)value == true) ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public static BoolVisibilityConverter Converter = new BoolVisibilityConverter(); // 値コンバーターの実体
    }
    #endregion
}
