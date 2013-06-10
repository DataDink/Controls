using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using Controls.Commanding;

namespace Controls
{
    [ContentProperty("Content")]
    public class LoadingOverlay : FrameworkElement
    {
        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register("Content", typeof(Visual), typeof(LoadingOverlay), new PropertyMetadata(ContentSet));
        public Visual Content { get { return (Visual)GetValue(ContentProperty); } set { SetValue(ContentProperty, value); } }

        public static readonly DependencyProperty OverlayTemplateProperty = DependencyProperty.Register("OverlayTemplate", typeof (ControlTemplate), typeof (LoadingOverlay), new PropertyMetadata(OverlaySet));
        public ControlTemplate OverlayTemplate { get { return (ControlTemplate) GetValue(OverlayTemplateProperty); } set { SetValue(OverlayTemplateProperty, value); } }

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
        }

        private readonly LoaderAdorner _adorner;

        private static void ContentSet(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var parent = sender as LoadingOverlay;
            if (parent == null) return;

            var oldchild = e.OldValue as UIElement;
            if (oldchild != null) {
                var layer = AdornerLayer.GetAdornerLayer(oldchild);
                if (layer != null) {
                    layer.Remove(parent._adorner);
                }
            }
        }

        private class LoaderAdorner : Adorner
        {
            public static readonly DependencyProperty AdornerContentProperty = DependencyProperty.Register("AdornerContent", typeof (UIElement), typeof (LoaderAdorner), new PropertyMetadata(AdornerContentSet));
            public UIElement AdornerContent { get { return (UIElement) GetValue(AdornerContentProperty); } set { SetValue(AdornerContentProperty, value); } }

            public LoadingOverlay Owner { get; private set; }

            public LoaderAdorner(LoadingOverlay owner) : base(owner) { Owner = owner; }
            
            protected override Size ArrangeOverride(Size finalSize)
            {
                if (Owner == null || AdornerContent == null) return finalSize;
                var element = AdornerContent as FrameworkElement;

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
            }
        }
    }
}
