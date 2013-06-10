using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace Controls
{
    public class LoadingSpinner : Border
    {
        public static readonly DependencyProperty SegmentWidthProperty = DependencyProperty.Register("SegmentWidth", typeof (double), typeof (LoadingSpinner), new PropertyMetadata(10d));
        public double SegmentWidth { get { return (double) GetValue(SegmentWidthProperty); } set { SetValue(SegmentWidthProperty, value); } }

        public static readonly DependencyProperty SegmentCountProperty = DependencyProperty.Register("SegmentCount", typeof (double), typeof (LoadingSpinner), new PropertyMetadata(15d));
        public double SegmentCount { get { return (double) GetValue(SegmentCountProperty); } set { SetValue(SegmentCountProperty, value); } }

        public static readonly DependencyProperty SegmentLengthProperty = DependencyProperty.Register("SegmentLength", typeof (float), typeof (LoadingSpinner), new PropertyMetadata(.3f));
        public float SegmentLength { get { return (float) GetValue(SegmentLengthProperty); } set { SetValue(SegmentLengthProperty, value); } }

        public LoadingSpinner()
        {
            Background = new SolidColorBrush(Color.FromArgb(128, 128, 128, 128));
            Width = 50;
            Height = 50;
            Loaded += (s, e) => {
                var rotate = new RotateTransform();
                var animation = new DoubleAnimation(0, 360, new Duration(TimeSpan.FromSeconds(2))) {
                    IsCumulative = true,
                    RepeatBehavior = RepeatBehavior.Forever,
                };
                rotate.BeginAnimation(RotateTransform.AngleProperty, animation);
                LayoutTransform = rotate;
            };
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property == ActualWidthProperty || e.Property == ActualHeightProperty
                || e.Property == SegmentWidthProperty || e.Property == SegmentCountProperty) {

                var step = 360d / SegmentCount;
                var top = Math.Max(0d, (ActualHeight - ActualWidth) / 2);
                var left = Math.Max(0d, (ActualWidth - ActualHeight) / 2);
                var radius = Math.Min(ActualWidth, ActualHeight);
                var innerRatio = Math.Max(0f, Math.Min(1f, 1f - SegmentLength));
                var center = new Point(ActualWidth / 2d, ActualHeight / 2d);

                var points = new List<Point>();
                for (var a = 0d; a < 360d; a += step) {
                    points.Add(center.Plot(a, radius));
                    points.Add(center.Plot(a + SegmentWidth, radius));
                    points.Add(new Point(center.X, center.Y));
                }
                var anglesFigure = new PathFigure(center, new[] { new PolyLineSegment(points, true) }, true);
                var angles = new PathGeometry(new[] { anglesFigure });


                var outerEllipse = new EllipseGeometry(new Rect(left, top, radius, radius));
                var innerleft = left + (radius - radius * innerRatio) / 2;
                var innerTop = top + (radius - radius * innerRatio) / 2;
                var innerRadius = radius * innerRatio;
                var innerEllipse = new EllipseGeometry(new Rect(innerleft, innerTop, innerRadius, innerRadius));
                var geometry = new CombinedGeometry(GeometryCombineMode.Intersect, outerEllipse, angles);
                geometry = new CombinedGeometry(GeometryCombineMode.Exclude, geometry, innerEllipse);
                Clip = geometry;
            }
        }
    }
}
