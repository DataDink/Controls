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
    public class LoadingSpinner : FrameworkElement
    {
        public static readonly DependencyProperty SegmentWidthProperty = DependencyProperty.Register("SegmentWidth", typeof (double), typeof (LoadingSpinner), new PropertyMetadata(10d));
        public double SegmentWidth { get { return (double) GetValue(SegmentWidthProperty); } set { SetValue(SegmentWidthProperty, value); } }

        public static readonly DependencyProperty SegmentCountProperty = DependencyProperty.Register("SegmentCount", typeof (double), typeof (LoadingSpinner), new PropertyMetadata(10d));
        public double SegmentCount { get { return (double) GetValue(SegmentCountProperty); } set { SetValue(SegmentCountProperty, value); } }

        public static readonly DependencyProperty SegmentLengthProperty = DependencyProperty.Register("SegmentLength", typeof (float), typeof (LoadingSpinner), new PropertyMetadata(.3f));
        public float SegmentLength { get { return (float) GetValue(SegmentLengthProperty); } set { SetValue(SegmentLengthProperty, value); } }

        public static readonly DependencyProperty SpinnerRadiusProperty = DependencyProperty.Register("SpinnerRadius", typeof(double), typeof(LoadingSpinner), new PropertyMetadata(100d));
        public double SpinnerRadius { get { return (double)GetValue(SpinnerRadiusProperty); } set { SetValue(SpinnerRadiusProperty, value); } }

        public static readonly DependencyProperty FillProperty = DependencyProperty.Register("Fill", typeof (Brush), typeof (LoadingSpinner), new PropertyMetadata(Brushes.Gray));
        public Brush Fill { get { return (Brush) GetValue(FillProperty); } set { SetValue(FillProperty, value); } }

        public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register("Stroke", typeof(Brush), typeof(LoadingSpinner), new PropertyMetadata(Brushes.Black));
        public Brush Stroke { get { return (Brush)GetValue(StrokeProperty); } set { SetValue(StrokeProperty, value); } }

        public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register("StrokeThickness", typeof (double), typeof (LoadingSpinner), new PropertyMetadata(2d));
        public double StrokeThickness { get { return (double) GetValue(StrokeThicknessProperty); } set { SetValue(StrokeThicknessProperty, value); } }

        public static readonly DependencyProperty SpinnerSpeedProperty = DependencyProperty.Register("SpinnerSpeed", typeof (TimeSpan), typeof (LoadingSpinner), new PropertyMetadata(TimeSpan.FromSeconds(2)));
        public TimeSpan SpinnerSpeed { get { return (TimeSpan) GetValue(SpinnerSpeedProperty); } set { SetValue(SpinnerSpeedProperty, value); } }

        private Image _image;
        public LoadingSpinner()
        {
            RenderShape();
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var top = finalSize.Height / 2 - SpinnerRadius / 2;
            var left = finalSize.Width / 2 - SpinnerRadius / 2;
            _image.Arrange(new Rect(left, top, SpinnerRadius, SpinnerRadius));
            return finalSize;
        }
        protected override Visual GetVisualChild(int index) { return _image; }
        protected override int VisualChildrenCount { get { return 1; } }

        private void RenderShape()
        {
            var geometry = LoadGeometry();

            if (_image != null) {
                RemoveVisualChild(_image);
            }
            _image = new Image();
            AddVisualChild(_image);
            _image.Source = new DrawingImage(new GeometryDrawing(
                Fill, new Pen(Stroke, StrokeThickness), geometry));

            var rotation = new RotateTransform(0d, SpinnerRadius / 2, SpinnerRadius / 2);
            _image.RenderTransform = rotation;
            rotation.BeginAnimation(RotateTransform.AngleProperty, new DoubleAnimation(
                0d, 360d, new Duration(SpinnerSpeed)) {
                    RepeatBehavior = RepeatBehavior.Forever,
                    IsCumulative = true,
                });

            InvalidateArrange();
        }

        private Geometry LoadGeometry()
        {
            var step = 360d / SegmentCount;
            var radius = SpinnerRadius;
            var innerRatio = Math.Max(0f, Math.Min(1f, 1f - SegmentLength));
            var center = new Point(radius / 2d, radius / 2d);

            var points = new List<Point>();
            for (var a = 0d; a < 360d; a += step) {
                points.Add(center.Plot(a, radius));
                points.Add(center.Plot(a + SegmentWidth, radius));
                points.Add(new Point(center.X, center.Y));
            }
            var anglesFigure = new PathFigure(center, new[] { new PolyLineSegment(points, true) }, true);
            var angles = new PathGeometry(new[] { anglesFigure });

            var outerEllipse = new EllipseGeometry(new Rect(0, 0, radius, radius));
            var innerRadius = radius * innerRatio;
            var innerleft = (radius - innerRadius) / 2;
            var innerTop = (radius - innerRadius) / 2;
            var innerEllipse = new EllipseGeometry(new Rect(innerleft, innerTop, innerRadius, innerRadius));
            var geometry = new CombinedGeometry(GeometryCombineMode.Intersect, outerEllipse, angles);
            geometry = new CombinedGeometry(GeometryCombineMode.Exclude, geometry, innerEllipse);
            return geometry;
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property == SpinnerRadiusProperty || e.Property == SegmentWidthProperty || e.Property == SegmentCountProperty
                || e.Property == FillProperty || e.Property == StrokeProperty || e.Property == SegmentLengthProperty
                || e.Property == StrokeThicknessProperty || e.Property == SpinnerSpeedProperty) {
                    RenderShape();
            }
        }
    }
}
