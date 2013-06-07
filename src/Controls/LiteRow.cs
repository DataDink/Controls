using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using Controls.Collections;
using Controls.Interface;

namespace Controls
{
    [ContentProperty("Cells")]
    public class LiteRow : FrameworkElement, IContentHost<UIElement>
    {
        public static readonly DependencyPropertyKey ItemProperty = DependencyProperty.RegisterReadOnly("Item", typeof (object), typeof (LiteRow), new PropertyMetadata());
        public object Item { get { return (object) GetValue(ItemProperty.DependencyProperty); } set { SetValue(ItemProperty, value); } }

        public static readonly DependencyPropertyKey RowProperty = DependencyProperty.RegisterAttachedReadOnly("Row", typeof (LiteRow), typeof (LiteRow), new PropertyMetadata());
        public static LiteRow GetRow(DependencyObject cell) { return cell.GetValue(RowProperty.DependencyProperty) as LiteRow; }
        private static void SetRow(DependencyObject cell, LiteRow row) { cell.SetValue(RowProperty, row); }

        public static readonly DependencyPropertyKey IndexProperty = DependencyProperty.RegisterAttachedReadOnly("Index", typeof(int), typeof(LiteRow), new PropertyMetadata());
        public static int GetIndex(DependencyObject cell) { return (int)cell.GetValue(IndexProperty.DependencyProperty); }
        private static void SetIndex(DependencyObject cell, int index) { cell.SetValue(IndexProperty, index); }

        private readonly ContentCollection<UIElement> _cells;
        public ContentCollection<UIElement> Cells { get { return _cells; } } 

        public LiteRow()
        {
            _cells = new ContentCollection<UIElement>(this);
        }

        void IContentHost<UIElement>.Add(IEnumerable<UIElement> cells)
        {
            foreach (var cell in cells) {
                AddLogicalChild(cell);
                AddVisualChild(cell);
                SetRow(cell, this);
                SetIndex(cell, _cells.IndexOf(cell));
            }
        }

        void IContentHost<UIElement>.Remove(IEnumerable<UIElement> cells)
        {
            foreach (var cell in cells) {
                RemoveLogicalChild(cell);
                RemoveVisualChild(cell);
                SetRow(cell, null);
                SetIndex(cell, 0);
            }
        }

        void IContentHost<UIElement>.Update()
        {
            InvalidateArrange();
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            if (!_cells.Any()) return finalSize;

            var grid = DataGridLite.GetGrid(this);
            var widths = grid.SpeculateColumnWidths(finalSize.Width);

            var left = 0d;
            for (var i = 0; i < widths.Length; i++) {
                var cell = _cells[i];
                var width = widths[i];
                cell.Arrange(new Rect(left, 0, width, grid.RowHeight));
                left += width;
            }
            return finalSize;
        }
        protected override Visual GetVisualChild(int index) { return _cells[index]; }
        protected override int VisualChildrenCount { get { return _cells.Count; } }
    }
}
