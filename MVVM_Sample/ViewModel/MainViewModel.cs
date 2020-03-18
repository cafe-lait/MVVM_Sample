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
using MVVM_Sample.Behavior;

namespace MVVM_Sample.ViewModel
{
    [AddINotifyPropertyChangedInterface]
    public partial class MainViewModel
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
            // テストデータ
            ThemeList.Add(new ThemeModel("ガソリン急落 半年ぶり143円台", "経済", _rdm.Next(1,3), _rdm.Next(0, 3)));
            ThemeList[ThemeList.Count - 1].ContentList.Add(new ContentModel("レギュラーガソリン急落、前週比2.9円安の143.5円　半年ぶりの安値", "記事", $"{_rdm.Next(15,45)}行"));
            ThemeList[ThemeList.Count - 1].ContentList.Add(new ContentModel("［ハイオクガソリン実売価格（「e燃費」調べ）］", "画像", "1920x1080"));
            ThemeList.Add(new ThemeModel("新型コロナの影響拡大を受けて", "政治", _rdm.Next(1, 3), _rdm.Next(0, 3)));
            ThemeList[ThemeList.Count - 1].ContentList.Add(new ContentModel("公共料金の支払いを猶予　欧州から入国、待機要請　新型コロナ", "記事", $"{_rdm.Next(15, 45)}行"));
            ThemeList[ThemeList.Count - 1].ContentList.Add(new ContentModel("電気など公共料金支払い猶予要請へ　欧州など38カ国に入国制限", "記事", $"{_rdm.Next(15, 45)}行"));
            ThemeList[ThemeList.Count - 1].ContentList.Add(new ContentModel("新型コロナウイルス感染症対策本部", "画像", "355x466"));
            ThemeList.Add(new ThemeModel("新型iPad、25日に発売", "社会", _rdm.Next(1, 3), _rdm.Next(0, 3)));
            ThemeList[ThemeList.Count - 1].ContentList.Add(new ContentModel("アップル、LiDARセンサー/2カメラの新iPad Pro。トラックパッドにも初対応", "記事", $"{_rdm.Next(15, 45)}行"));
            ThemeList[ThemeList.Count - 1].ContentList.Add(new ContentModel("新型iPad、25日に発売　米アップル、超広角カメラ搭載", "記事", $"{_rdm.Next(15, 45)}行"));
            ThemeList[ThemeList.Count - 1].ContentList.Add(new ContentModel("iPad（アイパッド）プロ", "画像", "400x448"));
            ThemeList.Add(new ThemeModel("将棋の清水市代女流六段が史上初の女流七段に", "社会", _rdm.Next(1, 3), _rdm.Next(0, 3)));
            ThemeList[ThemeList.Count - 1].ContentList.Add(new ContentModel("清水が史上初の女流七段に　里見は女流六段昇段", "記事", $"{_rdm.Next(15, 45)}行"));
            ThemeList[ThemeList.Count - 1].ContentList.Add(new ContentModel("女流七段への昇段は史上初", "記事", $"{_rdm.Next(15, 45)}行"));
            ThemeList[ThemeList.Count - 1].ContentList.Add(new ContentModel("日本将棋連盟", "画像", "1280x720"));
            ThemeList.Add(new ThemeModel("　【第1部】日経平均株価", "経済", _rdm.Next(1, 3), _rdm.Next(0, 3)));
            ThemeList[ThemeList.Count - 1].ContentList.Add(new ContentModel("乱高下後に小幅続落＝不安定な値動き続く", "記事", $"{_rdm.Next(15, 45)}行"));
            ThemeList[ThemeList.Count - 1].ContentList.Add(new ContentModel("日経平均は急速に切り返し底入れを期待", "記事", $"{_rdm.Next(15, 45)}行"));
            ThemeList[ThemeList.Count - 1].ContentList.Add(new ContentModel("ダウ工業株30種平均が過去最大の下げ幅", "画像", "1920x1080"));

            ContentFreeList.Add(new ContentModel("【コロナショックで資産が半分に!?】「iDeCo・つみたてNISA」利用者が今とるべき対処法とは", "記事", $"{_rdm.Next(15, 45)}行"));
            ContentFreeList.Add(new ContentModel("東電、全社員に一時金9万円　初任給は3千円上げ", "画像", $"{_rdm.Next(600, 1980)}x{_rdm.Next(400, 1080)}"));
            ContentFreeList.Add(new ContentModel("死者64万人想定のコロナ緊急宣言は妥当なのか", "記事", $"{_rdm.Next(15, 45)}行"));
            ContentFreeList.Add(new ContentModel("自然災害リスクへの警戒高まる　川沿いの住宅地、公示地価で下落顕著", "記事", $"{_rdm.Next(15, 45)}行"));
            ContentFreeList.Add(new ContentModel("直ちに消費税率ゼロにする発想ない", "記事", $"{_rdm.Next(15, 45)}行"));
            ContentFreeList.Add(new ContentModel("ＮＹ株1700ドル超下げ　2万ドル割り込む　取引一時停止", "記事", $"{_rdm.Next(15, 45)}行"));
            ContentFreeList.Add(new ContentModel("18日東京株式市場終値　284円98銭安の1万6726円55銭", "画像", $"{_rdm.Next(600, 1980)}x{_rdm.Next(400, 1080)}"));
            ContentFreeList.Add(new ContentModel("ＵＳＪが子ども達に『サプライズプレゼント』学童保育に通う３千人超に１３００万円分", "記事", $"{_rdm.Next(15, 45)}行"));
            ContentFreeList.Add(new ContentModel("GPIF22兆円の大損失 株価暴落＆運用失敗", "記事", $"{_rdm.Next(15, 45)}行"));

            // Drag&Drop
            this.Description = new DropAcceptDescription();
            this.Description.DragOver += Description_DragOver;
            this.Description.DragDrop += Description_DragDrop;
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
        public ObservableCollection<ThemeModel> ThemeList { get; set; } = new ObservableCollection<ThemeModel>();

        /// <summary>
        /// 未使用素材一覧情報
        /// </summary>
        public ObservableCollection<ContentModel> ContentFreeList { get; set; } = new ObservableCollection<ContentModel>();

        /// <summary>
        /// ステータス情報
        /// </summary>
        public StatusModel StatusModel { get; set; } = new StatusModel();
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
            ThemeList.Add(new ThemeModel("追加テーマ_" + DateTime.Now.ToString(@"mmss_fff"), ThemeList.Count % 2 == 0 ? "社会" : "政治", 1, 2));
        }
        #endregion

        #region Method : DeleteTheme
        /// <summary>
        /// テーマ削除
        /// </summary>
        /// <param name="model"></param>
        private void DeleteTheme(ThemeModel model)
        {
            var target = ThemeList.Where(x => x.ThemeId == model.ThemeId).SingleOrDefault();
            ThemeList.Remove(target);
        }
        #endregion

        #region Method : UpdateThemeDate
        /// <summary>
        /// テーマ登録日時更新
        /// </summary>
        /// <param name="model"></param>
        private void UpdateThemeDate(ThemeModel model)
        {
            var target = ThemeList.Where(x => x.ThemeId == model.ThemeId).SingleOrDefault();
            target.InputDate = DateTime.Now;
        }
        #endregion

        #region Method : Publish
        /// <summary>
        /// 出稿処理呼び出し
        /// </summary>
        private async void Publish()
        {
            StatusModel.StatusMessage = "出稿開始";

            // TaskをまとめるListを作成
            var tasks = new List<Task<bool>>();

            // 出稿対象の素材のリストを生成
            var contents = ThemeList.SelectMany(thm => thm.ContentList).ToList();

            foreach (var item in contents)
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

            if (arrayInt.All(ret => ret)) StatusModel.StatusMessage = "出稿終了：全件成功";
            else StatusModel.StatusMessage = "出稿終了：失敗したタスクがあります";
        }

        private static long _maxWaitTime = 60000;
        private static Random _rdm = new Random();

        /// <summary>
        /// 出稿処理
        /// </summary>
        /// <param name="cm"></param>
        private bool PublishAsync(ContentModel cm)//, CancellationToken cancelToken)
        {
            bool ret = false;

            // ステータスクリア
            cm.ClearPubStatus();

            // json作成
            // ..............

            // exe呼び出し
            Process CmdExec = new Process();

            // Timeout Timer
            Stopwatch sw = Stopwatch.StartNew();

            // Sample Wait (実際はこの時間がexe待ち時間)
            long sampleWait = _rdm.Next(3000, 8200);

            Console.WriteLine($"■ {cm.ContentlId} の出稿開始（処理時間：{sampleWait / 1000}秒）");

            while (sw.ElapsedMilliseconds < _maxWaitTime)
            {
                //ダミー負荷用ウエイト ms スレッドを止める
                Thread.Sleep(30);

                //進捗報告
                cm.StatusValue = (double)(sw.ElapsedMilliseconds * 100.0 / _maxWaitTime);

                // Sample Wait到達でbreak
                if (sampleWait < sw.ElapsedMilliseconds)
                {
                    // 処理結果OK
                    if (sw.ElapsedMilliseconds < 8000)
                    {
                        ret = true;
                        Console.WriteLine($"■ {cm.ContentlId} の出稿成功（処理時間：{sw.ElapsedMilliseconds / 1000}秒）");
                        break;
                    }
                    // 処理結果NG
                    ret = false;
                    Console.WriteLine($"■ {cm.ContentlId} の出稿エラー（処理時間：{sw.ElapsedMilliseconds / 1000}秒）");
                    break;
                }

                //キャンセルリクエストの確認
                //if (cancelToken.IsCancellationRequested)
                //{
                //    ret = false;
                //    break;
                //}

            }
            cm.SetPubStatus(ret);
            Console.WriteLine($"■ {cm.ContentlId} の出稿終了（処理時間：{sw.ElapsedMilliseconds / 1000}秒）");
            //}
            return ret;
        }
        #endregion
        #endregion

        #region Drag & Drop
        //private DropAcceptDescription _description;
        //public DropAcceptDescription Description
        //{
        //    get { return this._description; }
        //    set
        //    {
        //        if (this._description == value)
        //        {
        //            return;
        //        }
        //        this._description = value;
        //        //this.RaisePropertyChanged(() => this.Description);
        //    }
        //}
        public DropAcceptDescription Description { get; set; } = new DropAcceptDescription();
        private void Description_DragDrop(System.Windows.DragEventArgs args)
        {
            if (!args.Data.GetDataPresent(typeof(MainViewModel))) return;
            var data = args.Data.GetData(typeof(MainViewModel)) as MainViewModel;
            if (data == null) return;
            var fe = args.OriginalSource as FrameworkElement;
            if (fe == null) return;
            var target = fe.DataContext as MainViewModel;
            if (target == null) return;
        }
        private void Description_DragOver(System.Windows.DragEventArgs args)
        {
            if (args.AllowedEffects.HasFlag(DragDropEffects.Copy))
            {
                if (args.Data.GetDataPresent(typeof(MainViewModel)))
                {
                    return;
                }
            }
            args.Effects = DragDropEffects.None;
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
