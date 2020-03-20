using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MVVM_Sample.Common
{
    /// <summary>
    /// This Extended ListBox can be used for dragging multiple selected items.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The default ListBox does not allow an implementation of the Windows Explorer's default drag/drop behaviour. This derived class repairs this by hacking around with custom ListBoxItems.
    /// </para>
    /// <para>
    /// The same class can be easily adapted to ListView usage by replacing "Box" with "View".
    /// </para>
    /// </remarks>
    public class ListBoxEx : ListBox
    {
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new ListBoxItemEx();
        }

        class ListBoxItemEx : ListBoxItem
        {
            private bool _deferSelection = false;

            protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
            {
                if (e.ClickCount == 1 && IsSelected)
                {
                    // the user may start a drag by clicking into selected items
                    // delay destroying the selection to the Up event
                    _deferSelection = true;
                }
                else
                {
                    base.OnMouseLeftButtonDown(e);
                }
            }

            protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
            {
                if (_deferSelection)
                {
                    try
                    {
                        base.OnMouseLeftButtonDown(e);
                    }
                    finally
                    {
                        _deferSelection = false;
                    }
                }
                base.OnMouseLeftButtonUp(e);
            }

            protected override void OnMouseLeave(MouseEventArgs e)
            {
                // abort deferred Down
                _deferSelection = false;
                base.OnMouseLeave(e);
            }
        }
    }
}
