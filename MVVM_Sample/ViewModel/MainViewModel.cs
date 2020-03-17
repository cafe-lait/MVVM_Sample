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
using PropertyChanged;

namespace MVVM_Sample.ViewModel
{
    [AddINotifyPropertyChangedInterface]
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
            ThemeModelList.Add(new ThemeModel("初期表示用テーマ", "社会", DateTime.Now));
            ThemeModelList.Add(new ThemeModel("初期表示用テーマ2", "政治", DateTime.Now));
            ThemeModelList.Add(new ThemeModel("初期表示用テーマ3", "社会", DateTime.Now));
            ThemeModelList.Add(new ThemeModel("初期表示用テーマ4", "政治", DateTime.Now));
            ThemeModelList.Add(new ThemeModel("初期表示用テーマ5", "社会", DateTime.Now));
        }
        #endregion

        #region ViewModel : Binding Properties
        /// <summary>
        /// 選択テーマ情報
        /// </summary>
        public SelectedInfoModel SelectedInfoModel { get; set; } = new SelectedInfoModel();

        /// <summary>
        /// Theme一覧情報
        /// </summary>
        public ObservableCollection<ThemeModel> ThemeModelList { get; set; } = new ObservableCollection<ThemeModel>();

        /// <summary>
        /// ステータス情報
        /// </summary>
        public StatusModel StatusModel { get; set; } = new StatusModel();
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
                    case "AddTheme":
                        AddTheme();
                        break;
                    case "DeleteTheme":
                        DeleteTheme(param1 as ThemeModel);
                        break;
                    case "UpdateThemeDate":
                        UpdateThemeDate(param1 as ThemeModel);
                        break;
                    case "Publish":
                        Publish();
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

        #region Command Interface
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

        #endregion コマンド処理

        #region ViewModel : Command Method
        #region Method : AddTheme
        /// <summary>
        /// テーマ追加
        /// </summary>
        private void AddTheme()
        {
            ThemeModelList.Add(new ThemeModel("追加テーマ_" + DateTime.Now.ToString(@"mmss_fff"), ThemeModelList.Count % 2 == 0 ? "社会" : "政治", DateTime.Now));
        }
        #endregion

        #region Method : DeleteTheme
        /// <summary>
        /// テーマ削除
        /// </summary>
        /// <param name="model"></param>
        private void DeleteTheme(ThemeModel model)
        {
            var target = ThemeModelList.Where(x => x.ThemeId == model.ThemeId).SingleOrDefault();
            ThemeModelList.Remove(target);
        }
        #endregion

        #region Method : UpdateThemeDate
        /// <summary>
        /// テーマ登録日時更新
        /// </summary>
        /// <param name="model"></param>
        private void UpdateThemeDate(ThemeModel model)
        {
            var target = ThemeModelList.Where(x => x.ThemeId == model.ThemeId).SingleOrDefault();
            target.InputDate = DateTime.Now;
        }
        #endregion

        #region Method : Publish
        /// <summary>
        /// 出稿処理呼び出し
        /// </summary>
        private async void Publish()
        {
            var tasks = new List<Task<bool>>(); // TaskをまとめるListを作成
            StatusModel.StatusMessage = "出稿開始";

            foreach (var item in ThemeModelList)
            {
                //var cancelTokensource1 = new CancellationTokenSource();
                //var cToken = cancelTokensource1.Token;

                var task = Task.Run(() => PublishAsync(item));
                tasks.Add(task); // を、Listにまとめる

                //cancelTokensource1.Dispose();
                //cancelTokensource1 = null;
            }
            
            // Task配列の処理を実行し、全ての処理が終了するまで待機
            var arrayInt = await Task.WhenAll(tasks);

            if (arrayInt.All(ret=>ret)) StatusModel.StatusMessage = "出稿終了：全件成功";
            else StatusModel.StatusMessage = "出稿終了：失敗したタスクがあります";
        }

        private static long MaxWaitTime=60000;
        /// <summary>
        /// 出稿処理
        /// </summary>
        /// <param name="tm"></param>
        private bool PublishAsync(ThemeModel tm)//, CancellationToken cancelToken)
        {
            bool ret =  false;

            // ステータスクリア
            tm.ClearPubStatus();

            // json作成
            // ..............

            // exe呼び出し
            Process CmdExec = new Process();

            // Timeout Timer
            Stopwatch sw = Stopwatch.StartNew();

            // Sample Wait (実際はこの時間がexe待ち時間)
            long sampleWait = 5000;
            if (tm.ThemeName == "初期表示用テーマ2") sampleWait = 7000;
            if (tm.ThemeName == "初期表示用テーマ3") sampleWait = 9000;
            if (tm.ThemeName == "初期表示用テーマ4") sampleWait = 4000;
            if (tm.ThemeName == "初期表示用テーマ5") sampleWait = 7800;

            Console.WriteLine($"■ {tm.ThemeName} の出稿開始（処理時間：{sampleWait/1000}秒）");

            while (sw.ElapsedMilliseconds < MaxWaitTime)
            {
                //ダミー負荷用ウエイト ms スレッドを止める
                Thread.Sleep(30);

                //進捗報告
                tm.StatusValue = (double)(sw.ElapsedMilliseconds * 100.0 / MaxWaitTime);

                // Sample Wait到達でbreak
                if (sampleWait < sw.ElapsedMilliseconds)
                {
                    // 処理結果OK
                    if(sw.ElapsedMilliseconds < 8000)
                    {
                        ret = true;
                        Console.WriteLine($"■ {tm.ThemeName} の出稿成功（処理時間：{sw.ElapsedMilliseconds / 1000}秒）");
                        break;
                    }
                    // 処理結果NG
                    ret = false;
                    Console.WriteLine($"■ {tm.ThemeName} の出稿エラー（処理時間：{sw.ElapsedMilliseconds / 1000}秒）");
                    break;
                }

                //キャンセルリクエストの確認
                //if (cancelToken.IsCancellationRequested)
                //{
                //    ret = false;
                //    break;
                //}

            }
            tm.SetPubStatus(ret);
            Console.WriteLine($"■ {tm.ThemeName} の出稿終了（処理時間：{sw.ElapsedMilliseconds / 1000}秒）");
            //}
            return ret;
        }
        public long CreateLong()
        {
            return new Random().Next(3, 20) * 1000;
        }
        #endregion
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
