using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using Controls.Collections;
using Controls.Interface;

namespace Controls
{
    [ContentProperty("Cells")]
    public class LiteColumn : DependencyObject, IContentHost<UIElement>
    {
        public static readonly DependencyPropertyKey ColumnProperty = DependencyProperty.RegisterAttachedReadOnly("Column", typeof(LiteColumn), typeof(LiteColumn), new PropertyMetadata());
        public static LiteColumn GetColumn(DependencyObject cell) { return cell.GetValue(ColumnProperty.DependencyProperty) as LiteColumn; }
        private static void SetColumn(DependencyObject cell, LiteColumn column) { cell.SetValue(ColumnProperty, column); }

        public static readonly DependencyPropertyKey IndexProperty = DependencyProperty.RegisterAttachedReadOnly("Index", typeof(int), typeof(LiteColumn), new PropertyMetadata());
        public static int GetIndex(DependencyObject cell) { return (int)cell.GetValue(IndexProperty.DependencyProperty); }
        private static void SetIndex(DependencyObject cell, int index) { cell.SetValue(IndexProperty, index); }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof (string), typeof (LiteColumn));
        public string Title { get { return (string) GetValue(TitleProperty); } set { SetValue(TitleProperty, value); } }

        public static readonly DependencyProperty WidthProperty = DependencyProperty.Register("Width", typeof (double), typeof (LiteColumn), new PropertyMetadata(double.NaN));
        public double Width { get { return (double) GetValue(WidthProperty); } set { SetValue(WidthProperty, value); } }

        public static readonly DependencyProperty DataMemberProperty = DependencyProperty.Register("DataMember", typeof (string), typeof (LiteColumn));
        public string DataMember { get { return (string) GetValue(DataMemberProperty); } set { SetValue(DataMemberProperty, value); } }

        public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register("HeaderTemplate", typeof (ControlTemplate), typeof (LiteColumn));
        public ControlTemplate HeaderTemplate { get { return (ControlTemplate) GetValue(HeaderTemplateProperty); } set { SetValue(HeaderTemplateProperty, value); } }

        public static readonly DependencyProperty CellTemplateProperty = DependencyProperty.Register("CellTemplate", typeof (ControlTemplate), typeof (LiteColumn));
        public ControlTemplate CellTemplate { get { return (ControlTemplate) GetValue(CellTemplateProperty); } set { SetValue(CellTemplateProperty, value); } }

        private readonly ContentCollection<UIElement> _cells;
        public ContentCollection<UIElement> Cells { get { return _cells; } }

        public LiteColumn()
        {
            _cells = new ContentCollection<UIElement>(this);
        }

        void IContentHost<UIElement>.Add(IEnumerable<UIElement> cells)
        {
            foreach (var cell in cells) {
                SetColumn(cell, this);
                SetIndex(cell, _cells.IndexOf(cell));
            }
        }

        void IContentHost<UIElement>.Remove(IEnumerable<UIElement> cells)
        {
            foreach (var cell in cells) {
                SetColumn(cell, null);
                SetIndex(cell, 0);
            }
        }

        void IContentHost<UIElement>.Update()
        {
            var rows = _cells.Select(LiteRow.GetRow).ToList();
            rows.ForEach(r => r.InvalidateArrange());
        }

        public virtual UIElement GenerateHeader()
        {
            var header = HeaderTemplate == null ? null : HeaderTemplate.LoadContent() as UIElement;
            if (header == null) {
                var text = new TextBlock();
                text.SetBinding(TextBlock.TextProperty, new Binding {Path = new PropertyPath("Title"), Mode = BindingMode.OneWay});
                header = text;
            }
            SetColumn(header, this);

            var element = header as FrameworkElement;
            if (element != null) {
                element.SetBinding(FrameworkElement.DataContextProperty, new Binding { Path = new PropertyPath(ColumnProperty.DependencyProperty), Source = element, Mode = BindingMode.OneWay });
            }

            return header;
        }

        public virtual UIElement GenerateCell()
        {
            var cell = CellTemplate == null ? null : CellTemplate.LoadContent() as UIElement;
            cell = cell ?? new TextBlock();
            SetColumn(cell, this);

            var element = cell as FrameworkElement;
            if (element != null) {
                var dataPath = string.IsNullOrWhiteSpace(DataMember) ? "(0).Item" : "(0).Item." + DataMember;
                element.SetBinding(FrameworkElement.DataContextProperty, new Binding { Path = new PropertyPath(dataPath, LiteRow.RowProperty.DependencyProperty), Source = element, Mode = BindingMode.OneWay });
            }

            var text = cell as TextBlock;
            if (text != null) {
                text.SetBinding(TextBlock.TextProperty, new Binding("DataContext") {Source = text, Mode = BindingMode.OneWay});
            }

            return cell;
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.Property == WidthProperty) {
                var rows = _cells.Select(LiteRow.GetRow).ToList();
                rows.ForEach(r => r.InvalidateArrange());
            }
            if (e.Property == HeaderTemplateProperty) {
                var grid = DataGridLite.GetGrid(this);
                if (grid == null) return;
                grid.InvalidateHeader();
            }
            if (e.Property == CellTemplateProperty) {
                var grid = DataGridLite.GetGrid(this);
                if (grid == null) return;
                grid.InvalidateRows();
            }
            if (e.Property == DataMemberProperty) {
                var dataPath = string.IsNullOrWhiteSpace(DataMember) ? "(0).Item" : "(0).Item." + DataMember;
                foreach (var element in _cells.OfType<FrameworkElement>()) {
                    element.SetBinding(FrameworkElement.DataContextProperty, new Binding { Path = new PropertyPath(dataPath, LiteRow.RowProperty.DependencyProperty), Source = element, Mode = BindingMode.OneWay });
                }
            }
        }
    }
}
