using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MVVM_Sample.Common
{
    static class GuiUtils
    {
            /// <summary>
            /// 指定された FrameworkElement に対するテンプレートのルート要素を取得します。
            /// </summary>
            /// <param name="element">FrameworkElement を指定します。</param>
            /// <returns>TemplatedParent を辿った先のルート要素を返します。</returns>
            public static FrameworkElement GetTemplatedRootElement(FrameworkElement element)
            {
                var parent = element.TemplatedParent as FrameworkElement;
                while (parent.TemplatedParent != null)
                {
                    parent = parent.TemplatedParent as FrameworkElement;
                }
                return parent;
            }

            /// <summary>
            /// 指定されたDependencyObjectからビジュアルツリーをさかのぼり、
            /// Genericで指定された型のオブジェクトを検索します。
            /// 【CAUTION!!】DataGridColumnを探す事はできません!)<br/>
            /// 検索できなかった時はnullを返します。<br/>
            /// </summary>
            /// <remarks>
            /// DataGridColumnは、VisualTree、LogicalTree両方に属さない特殊なクラスなので、
            /// このメソッドを使ってDataGridやDaraGridRowオブジェクトを取得しようとしても失敗します。<br/>
            /// </remarks>
            /// <typeparam name="T">検索対象となるクラスの型宣言</typeparam>
            /// <param name="depObj">DependencyObjectインスタンス</param>
            /// <returns>検索対象オブジェクト</returns>
            public static T FindAncestor<T>(this DependencyObject depObj) where T : class
            {
            if (depObj == null) return null;
                var target = depObj;
                try
                {
                    do
                    {
                        //ビジュアルツリー上の親を探します。
                        //T型のクラスにヒットするまでさかのぼり続けます。
                        target = System.Windows.Media.VisualTreeHelper.GetParent(target);
                    } while (target != null && !(target is T));

                    return target as T;
                }
                finally
                {
                    target = null;
                    depObj = null;
                }
            }
    }
}
