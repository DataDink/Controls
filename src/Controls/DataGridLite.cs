using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using System.Windows.Media;
using Controls.Collections;
using Controls.Interface;

namespace Controls
{
    [ContentProperty("Columns")]
    public class DataGridLite : FrameworkElement, IContentHost<LiteColumn>, IContentHost<LiteRow>, IContentHost<UIElement>
    {
        public static readonly DependencyProperty HeaderHeightProperty = DependencyProperty.Register("HeaderHeight", typeof (double), typeof (DataGridLite), new PropertyMetadata(30d));
        public double HeaderHeight { get { return (double) GetValue(HeaderHeightProperty); } set { SetValue(HeaderHeightProperty, value); } }

        public static readonly DependencyProperty RowHeightProperty = DependencyProperty.Register("RowHeight", typeof (double), typeof (DataGridLite), new PropertyMetadata(30d));
        public double RowHeight { get { return (double) GetValue(RowHeightProperty); } set { SetValue(RowHeightProperty, value); } }

        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register("Items", typeof (IEnumerable<object>), typeof (DataGridLite));
        public IEnumerable<object> Items { get { return (IEnumerable<object>) GetValue(ItemsProperty); } set { SetValue(ItemsProperty, value); } }

        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof (object), typeof (DataGridLite));
        public object SelectedItem { get { return (object) GetValue(SelectedItemProperty); } set { SetValue(SelectedItemProperty, value); } }

        public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.Register("SelectedItems", typeof (IEnumerable<object>), typeof (DataGridLite));
        public IEnumerable<object> SelectedItems { get { return (IEnumerable<object>) GetValue(SelectedItemsProperty); } set { SetValue(SelectedItemsProperty, value); } }

        public static readonly DependencyProperty SelectedRowsProperty = DependencyProperty.Register("SelectedRows", typeof (IEnumerable<LiteRow>), typeof (DataGridLite));
        public IEnumerable<LiteRow> SelectedRows { get { return (IEnumerable<LiteRow>) GetValue(SelectedRowsProperty); } set { SetValue(SelectedRowsProperty, value); } }

        public static readonly DependencyProperty SelectedCellsProperty = DependencyProperty.Register("SelectedCells", typeof (IEnumerable<UIElement>), typeof (DataGridLite));
        public IEnumerable<UIElement> SelectedCells { get { return (IEnumerable<UIElement>) GetValue(SelectedCellsProperty); } set { SetValue(SelectedCellsProperty, value); } }

        public static readonly DependencyProperty SelectedColumnsProperty = DependencyProperty.Register("SelectedColumns", typeof (IEnumerable<LiteColumn>), typeof (DataGridLite));
        public IEnumerable<LiteColumn> SelectedColumns { get { return (IEnumerable<LiteColumn>) GetValue(SelectedColumnsProperty); } set { SetValue(SelectedColumnsProperty, value); } }

        public static readonly DependencyProperty SelectionModeProperty = DependencyProperty.Register("SelectionMode", typeof (DataGridLiteSelectionMode), typeof (DataGridLite), new PropertyMetadata(DataGridLiteSelectionMode.MultiRow));
        public DataGridLiteSelectionMode SelectionMode { get { return (DataGridLiteSelectionMode) GetValue(SelectionModeProperty); } set { SetValue(SelectionModeProperty, value); } }

        public static readonly DependencyPropertyKey GridProperty = DependencyProperty.RegisterAttachedReadOnly("Grid", typeof(DataGridLite), typeof(DataGridLite), new PropertyMetadata());
        public static DataGridLite GetGrid(DependencyObject child) { return child.GetValue(GridProperty.DependencyProperty) as DataGridLite; }
        private static void SetGrid(DependencyObject child, DataGridLite grid) { child.SetValue(GridProperty, grid); }

        public static readonly DependencyPropertyKey IndexProperty = DependencyProperty.RegisterAttachedReadOnly("Index", typeof(int), typeof(DataGridLite), new PropertyMetadata());
        public static int GetIndex(DependencyObject child) { return (int)child.GetValue(IndexProperty.DependencyProperty); }
        private static void SetIndex(DependencyObject child, int index) { child.SetValue(IndexProperty, index); }

        public static readonly DependencyPropertyKey IsSelectedProperty = DependencyProperty.RegisterAttachedReadOnly("IsSelected", typeof(bool), typeof(DataGridLite), new PropertyMetadata());
        public static bool GetIsSelected(DependencyObject child) { return (bool)child.GetValue(IsSelectedProperty.DependencyProperty); }
        private static void SetIsSelected(DependencyObject child, bool value) { child.SetValue(IsSelectedProperty, value); }

        public DataGridLite()
        {
            _columns = new ContentCollection<LiteColumn>(this);
            _rows = new ContentCollection<LiteRow>(this);
            _cells = new ContentCollection<UIElement>(this);
            _headerCells = new ContentCollection<UIElement>(this);
            _scrollbar = new ScrollBar();

            AddLogicalChild(_scrollbar);
            AddVisualChild(_scrollbar);
            _scrollbar.Scroll += OnScroll;
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property == HeaderHeightProperty) PositionHeaderCells(ActualWidth);
            if (e.Property == RowHeightProperty) PositionRows(ActualWidth);
            if (e.Property == ItemsProperty) {
                InvalidateRows();
                PositionRows(ActualWidth);
            }
            if (e.Property == ActualWidthProperty || e.Property == ActualHeightProperty) {
                PositionRows(ActualWidth);
                PositionHeaderCells(ActualWidth);
                PositionScroll(ActualWidth);
            }
            if (e.Property == SelectedItemProperty) SetSelection(new[] {SelectedItem}, null, null, null);
            if (e.Property == SelectedItemsProperty) SetSelection(SelectedItems.ToArray(), null, null, null);
            if (e.Property == SelectedRowsProperty) SetSelection(null, SelectedRows.ToArray(), null, null);
            if (e.Property == SelectedColumnsProperty) SetSelection(null, null, SelectedColumns.ToArray(), null);
            if (e.Property == SelectedCellsProperty) SetSelection(null, null, null, SelectedCells.ToArray());
        }

        private readonly ContentCollection<LiteColumn> _columns;
        public ContentCollection<LiteColumn> Columns { get { return _columns; } } 

        void IContentHost<LiteColumn>.Add(IEnumerable<LiteColumn> columns)
        {
            foreach (var column in columns) {
                SetGrid(column, this);
                SetIndex(column, _columns.IndexOf(column));
            }
        }

        void IContentHost<LiteColumn>.Remove(IEnumerable<LiteColumn> columns)
        {
            foreach (var column in columns) {
                SetGrid(column, null);
                SetIndex(column, 0);
            }
        }

        void IContentHost<LiteColumn>.Update()
        {
            InvalidateHeader();
            PositionHeaderCells(ActualWidth);
            InvalidateRows();
            PositionRows(ActualWidth);
        }

        private readonly ContentCollection<LiteRow> _rows;
        public IEnumerable<LiteRow> Rows { get { return _rows.ToArray(); } }

        void IContentHost<LiteRow>.Add(IEnumerable<LiteRow> rows)
        {
            foreach (var row in rows) {
                SetGrid(row, this);
                AddLogicalChild(row);
                AddVisualChild(row);
                SetIndex(row, _rows.IndexOf(row));
            }
        }

        void IContentHost<LiteRow>.Remove(IEnumerable<LiteRow> rows)
        {
            foreach (var row in rows) {
                SetGrid(row, null);
                RemoveLogicalChild(row);
                RemoveVisualChild(row);
                SetIndex(row, 0);
            }
        }

        void IContentHost<LiteRow>.Update() { }

        private readonly ContentCollection<UIElement> _cells;
        public IEnumerable<UIElement> Cells { get { return _cells.ToArray(); } }

        void IContentHost<UIElement>.Add(IEnumerable<UIElement> cells)
        {
            foreach (var cell in cells) {
                SetGrid(cell, this);
            }
        }

        void IContentHost<UIElement>.Remove(IEnumerable<UIElement> cells)
        {
            foreach (var cell in cells) {
                SetGrid(cell, null);
            }
        }

        void IContentHost<UIElement>.Update() { }

        public double[] SpeculateColumnWidths(double width)
        {
            if (!_columns.Any()) return new double[0];
            width -= _scrollbar.Width;
            var measuredTotal = _columns.Sum(c => double.IsNaN(c.Width) ? 0d : c.Width);
            var unmeasuredTotal = Math.Max(0d, width - measuredTotal);
            var unmeasuredWidth = unmeasuredTotal / _columns.Count(c => double.IsNaN(c.Width));

            return _columns.Select(c => double.IsNaN(c.Width) ? unmeasuredWidth : c.Width).ToArray();
        }

        private readonly ContentCollection<UIElement> _headerCells; 

        protected virtual void PositionHeaderCells(double width)
        {
            var widths = SpeculateColumnWidths(width);
            var left = 0d;
            for (var i = 0; i < widths.Length; i++) {
                var cellWidth = widths[i];
                var cell = _headerCells[i];
                cell.Arrange(new Rect(left, 0, cellWidth, HeaderHeight));
                left += cellWidth;
            }
        }

        public virtual void InvalidateHeader()
        {
            _headerCells.ForEach(h => {
                RemoveLogicalChild(h);
                RemoveVisualChild(h);
            });
            _headerCells.Clear();

            foreach (var column in _columns) {
                var element = column.GenerateHeader();
                AddLogicalChild(element);
                AddVisualChild(element);
                _headerCells.Add(element);
            }
        }

        private readonly ScrollBar _scrollbar;

        protected virtual void PositionScroll(double width)
        {
            _scrollbar.Arrange(new Rect(width - _scrollbar.Width, HeaderHeight, _scrollbar.Width, Math.Max(0d, ActualHeight - HeaderHeight)));
            var contentHeight = RowHeight * _rows.Count;
            var scrollDist = Math.Max(0d, contentHeight - ActualHeight + HeaderHeight);
            _scrollbar.Maximum = scrollDist;
            _scrollbar.Minimum = 0d;
        }

        protected virtual void OnScroll(object sender, ScrollEventArgs e)
        {
            PositionRows(ActualWidth);
            if (Scroll != null) Scroll(sender, e);
        }

        public event EventHandler<ScrollEventArgs> Scroll;

        protected virtual void PositionRows(double width)
        {
            var top = HeaderHeight - _scrollbar.Value;
            _rows.ForEach(r => {
                r.Arrange(new Rect(0, top, Math.Max(0d, width), RowHeight));

                var clipTop = Math.Min(RowHeight, -Math.Min(0d, top - HeaderHeight));
                var clipBottom = Math.Min(RowHeight, Math.Max(0d, top + RowHeight - ActualHeight));
                var clipHeight = Math.Max(0d, RowHeight - clipTop - clipBottom);

                if (clipHeight <= 0) {
                    r.Visibility = Visibility.Hidden;
                } else if (clipHeight < RowHeight) {
                    r.Visibility = Visibility.Visible;
                    r.Clip = new RectangleGeometry(new Rect(0, clipTop, width, clipHeight));
                } else {
                    r.Visibility = Visibility.Visible;
                    r.Clip = null;
                }
                top += RowHeight;
            });
        }

        public virtual void InvalidateRows()
        {
            _rows.ForEach(r => r.Cells.Clear());
            _rows.Clear();
            _columns.ForEach(c => c.Cells.Clear());
            _cells.Clear();
            if (Items == null) return;
            foreach (var item in Items) {
                var row = new LiteRow {Item = item};
                _rows.Add(row);

                foreach (var column in Columns) {
                    var cell = column.GenerateCell();
                    _cells.Add(cell);
                    row.Cells.Add(cell);
                    column.Cells.Add(cell);
                }
            }

        }

        private bool _configuringSelection = false;
        private void SetSelection(object[] items, LiteRow[] rows, LiteColumn[] columns, UIElement[] cells)
        {
            if (_configuringSelection) return;
            _configuringSelection = true;

            items = items ?? new object[0];
            rows = rows ?? new LiteRow[0];
            columns = columns ?? new LiteColumn[0];
            cells = cells ?? new UIElement[0];

            cells = cells
                .Concat(rows.SelectMany(r => r.Cells))
                .Concat(columns.SelectMany(c => c.Cells))
                .Concat(_rows.Where(r => items.Contains(r.Item)).SelectMany(r => r.Cells))
                .Distinct()
                .ToArray();
            rows = rows
                .Concat(cells.Select(LiteRow.GetRow))
                .Distinct()
                .ToArray();
            columns = columns
                .Concat(cells.Select(LiteColumn.GetColumn))
                .Distinct()
                .ToArray();

            var selection = new List<DependencyObject>();
            if (SelectionMode.HasFlag(DataGridLiteSelectionMode.SingleCell)) selection.AddRange(cells.Take(1));
            if (SelectionMode.HasFlag(DataGridLiteSelectionMode.MultiCell)) selection.AddRange(cells);
            if (SelectionMode.HasFlag(DataGridLiteSelectionMode.SingleRow)) selection.AddRange(rows.Take(1));
            if (SelectionMode.HasFlag(DataGridLiteSelectionMode.MultiRow)) selection.AddRange(rows);
            if (SelectionMode.HasFlag(DataGridLiteSelectionMode.SingleColumn)) selection.AddRange(columns.Take(1));
            if (SelectionMode.HasFlag(DataGridLiteSelectionMode.MultiColumn)) selection.AddRange(columns);
            selection = selection.Distinct().ToList();

            // Sync all selection types
            SelectedCells = selection.OfType<UIElement>().Where(s => cells.Contains(s)).ToArray();
            SelectedRows = selection.OfType<LiteRow>().ToArray();
            SelectedColumns = selection.OfType<LiteColumn>().ToArray();
            SelectedItems = selection.OfType<LiteRow>()
                                     .Concat(SelectedCells.Select(LiteRow.GetRow))
                                     .Select(r => r.Item)
                                     .ToArray();
            SelectedItem = SelectedItems.FirstOrDefault();

            var selected = selection.Concat(SelectedItems.OfType<DependencyObject>()).ToList();
            selected.ForEach(i => SetIsSelected(i, true));

            var deselected = Items.OfType<DependencyObject>()
                                .Concat(_cells)
                                .Concat(_rows)
                                .Concat(_columns)
                                .Where(i => !selected.Contains(i))
                                .ToList();
            deselected.ForEach(i => SetIsSelected(i, false));

            _configuringSelection = false;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            PositionHeaderCells(finalSize.Width);
            PositionRows(finalSize.Width);
            PositionScroll(finalSize.Width);
            return finalSize;
        }
        protected virtual IEnumerable<UIElement> VisualChildren { get { return _headerCells.Concat(_rows).Concat(new[] {_scrollbar}); } }
        protected override Visual GetVisualChild(int index) { return VisualChildren.ToArray()[index]; }
        protected override int VisualChildrenCount { get { return VisualChildren.Count(); } }
    }

    [Flags]
    public enum DataGridLiteSelectionMode
    {
        SingleRow = 1,
        MultiRow = 2,
        SingleColumn = 4,
        MultiColumn = 8,
        SingleCell = 16,
        MultiCell = 32,
    }
}
