using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using Controls.Commanding;
using Controls.Converters;

namespace Controls
{
    [ContentProperty("Content")]
    public class LoadingOverlay : FrameworkElement
    {
        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register("Content", typeof(Visual), typeof(LoadingOverlay), new PropertyMetadata(ContentSet));
        public Visual Content { get { return (Visual)GetValue(ContentProperty); } set { SetValue(ContentProperty, value); } }

        public static readonly DependencyProperty TemplateProperty = DependencyProperty.Register("Template", typeof(ControlTemplate), typeof(LoadingOverlay), new PropertyMetadata(OverlaySet));
        public ControlTemplate Template { get { return (ControlTemplate)GetValue(TemplateProperty); } set { SetValue(TemplateProperty, value); } }

        public static readonly DependencyProperty IsLoadingProperty = DependencyProperty.Register("IsLoading", typeof (bool), typeof (LoadingOverlay));
        public bool IsLoading { get { return (bool) GetValue(IsLoadingProperty); } set { SetValue(IsLoadingProperty, value); } }

        public static readonly DependencyProperty LeftProperty = DependencyProperty.Register("Left", typeof(double), typeof(LoadingOverlay), new PropertyMetadata(double.NaN, PositionSet));
        public double Left { get { return (double)GetValue(LeftProperty); } set { SetValue(LeftProperty, value); } }

        public static readonly DependencyProperty TopProperty = DependencyProperty.Register("Top", typeof(double), typeof(LoadingOverlay), new PropertyMetadata(double.NaN, PositionSet));
        public double Top { get { return (double)GetValue(TopProperty); } set { SetValue(TopProperty, value); } }

        public static readonly DependencyProperty RightProperty = DependencyProperty.Register("Right", typeof(double), typeof(LoadingOverlay), new PropertyMetadata(double.NaN, PositionSet));
        public double Right { get { return (double)GetValue(RightProperty); } set { SetValue(RightProperty, value); } }

        public static readonly DependencyProperty BottomProperty = DependencyProperty.Register("Bottom", typeof(double), typeof(LoadingOverlay), new PropertyMetadata(double.NaN, PositionSet));
        public double Bottom { get { return (double)GetValue(BottomProperty); } set { SetValue(BottomProperty, value); } }

        public LoadingOverlay()
        {
            _adorner = new LoaderAdorner(this);
            var template = new ControlTemplate { VisualTree = new FrameworkElementFactory(typeof(LoadingSpinner)) };
            template.Seal();
            Template = template;
            Loaded += (s, e) => AdornerLayer.GetAdornerLayer(this).Add(_adorner);
            _adorner.SetBinding(UIElement.VisibilityProperty, new Binding("IsLoading") {
                Source = this,
                Converter = new BooleanToVisibilityConverter(),
                Mode = BindingMode.OneWay
            });
        }

        private static void ContentSet(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var overlay = sender as LoadingOverlay;
            if (overlay == null) return;

            var oldcontent = e.OldValue as Visual;
            if (oldcontent != null) {
                overlay.RemoveLogicalChild(oldcontent);
                overlay.RemoveVisualChild(oldcontent);
                var oldelement = oldcontent as UIElement;
                if (oldelement != null) {
                    BindingOperations.ClearBinding(oldelement, IsEnabledProperty);
                }
            }

            var newcontent = e.NewValue as Visual;
            if (newcontent != null) {
                overlay.AddLogicalChild(newcontent);
                overlay.AddVisualChild(newcontent);
                var newelement = newcontent as UIElement;
                if (newelement != null) {
                    BindingOperations.SetBinding(newelement,
                        IsEnabledProperty,
                        new Binding("IsLoading") {
                            Source = overlay, 
                            Mode = BindingMode.OneWay,
                            Converter = new InversionConverter(),
                        });
                }
            }

            overlay.InvalidateArrange();
        }

        private static void OverlaySet(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var overlay = sender as LoadingOverlay;
            if (overlay == null) return;

            var content = overlay.Template == null ? null : overlay.Template.LoadContent() as UIElement;
            overlay._adorner.AdornerContent = content;

            overlay.InvalidateArrange();
        }

        private static void PositionSet(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var overlay = sender as LoadingOverlay;
            if (overlay == null) return;

            overlay._adorner.InvalidateArrange();
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var element = Content as UIElement;
            if (element != null) {
                element.Arrange(new Rect(0, 0, finalSize.Width, finalSize.Height));
            }
            return finalSize;
        }

        protected override Visual GetVisualChild(int index) { return Content; }
        protected override int VisualChildrenCount { get { return Content == null ? 0 : 1; } }

        private readonly LoaderAdorner _adorner;

        private class LoaderAdorner : Adorner
        {
            public static readonly DependencyProperty AdornerContentProperty = DependencyProperty.Register("AdornerContent", typeof (UIElement), typeof (LoaderAdorner), new PropertyMetadata(AdornerContentSet));
            public UIElement AdornerContent { get { return (UIElement) GetValue(AdornerContentProperty); } set { SetValue(AdornerContentProperty, value); } }

            public LoadingOverlay Owner { get; private set; }

            public LoaderAdorner(LoadingOverlay owner) : base(owner) { Owner = owner; }

            protected override HitTestResult HitTestCore(PointHitTestParameters hitTestParameters)
            {
                return new PointHitTestResult(this, hitTestParameters.HitPoint);
            }
            
            protected override Size ArrangeOverride(Size finalSize)
            {
                if (Owner == null || AdornerContent == null) return finalSize;
                var element = AdornerContent as FrameworkElement;
                if (element == null) return finalSize;

                var finalWidth = finalSize.Width.GetRealOrDefault();
                var finalHeight = finalSize.Height.GetRealOrDefault();

                var desiredWidth = element != null ? element.Width.GetRealOrDefault(finalWidth) : finalWidth;
                var calculateWidth = !double.IsNaN(Owner.Left) && !double.IsNaN(Owner.Right);
                var width = Math.Max(0d, calculateWidth
                    ? finalWidth - Owner.Left - Owner.Right
                    : desiredWidth);

                var desiredHeight = element != null ? element.Height.GetRealOrDefault(finalHeight) : finalHeight;
                var calculateHeight = !double.IsNaN(Owner.Top) && !double.IsNaN(Owner.Bottom);
                var height = Math.Max(0d, calculateHeight
                    ? finalHeight - Owner.Top - Owner.Bottom
                    : desiredHeight);

                var calculateLeft = double.IsNaN(Owner.Left);
                var left = calculateLeft
                    ? finalWidth - width - Owner.Right.GetRealOrDefault()
                    : Owner.Left.GetRealOrDefault();

                var calculateTop = double.IsNaN(Owner.Left);
                var top = calculateTop
                    ? finalHeight - height - Owner.Bottom.GetRealOrDefault()
                    : Owner.Top.GetRealOrDefault();

                AdornerContent.Arrange(new Rect(left, top, width, height));
                return finalSize;
            }

            protected override Visual GetVisualChild(int index) { return AdornerContent; }
            protected override int VisualChildrenCount { get { return AdornerContent == null ? 0 : 1; } }

            private static void AdornerContentSet(DependencyObject sender, DependencyPropertyChangedEventArgs e)
            {
                var adorner = sender as LoaderAdorner;
                if (adorner == null) return;

                var oldcontent = e.OldValue as UIElement;
                if (oldcontent != null) {
                    adorner.RemoveLogicalChild(oldcontent);
                    adorner.RemoveVisualChild(oldcontent);
                }

                var newcontent = e.NewValue as UIElement;
                if (newcontent != null) {
                    adorner.AddLogicalChild(newcontent);
                    adorner.AddVisualChild(newcontent);
                }

                adorner.InvalidateArrange();
            }
        }
    }
}
