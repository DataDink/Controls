using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

namespace Controls
{
    [ContentProperty("Content")]
    public class TouchContainer : FrameworkElement
    {
        public static List<string> Properties = new List<string>();
        #region dependency property setup
        static TouchContainer() // Reducing validation boiler plate setup with some good ol reflection static construction
        {
            ConfigureValidationSet(() => TranslateXProperty, () => TranslateXMinProperty, () => TranslateXMaxProperty, 0d);
            ConfigureValidationSet(() => TranslateYProperty, () => TranslateYMinProperty, () => TranslateYMaxProperty, 0d);
            ConfigureValidationSet(() => ScaleXProperty, () => ScaleXMinProperty, () => ScaleXMaxProperty, 1d);
            ConfigureValidationSet(() => ScaleYProperty, () => ScaleYMinProperty, () => ScaleYMaxProperty, 1d);
            ConfigureValidationSet(() => FlickXProperty, () => FlickXMinProperty, () => FlickXMaxProperty, 0d);
            ConfigureValidationSet(() => FlickYProperty, () => FlickYMinProperty, () => FlickYMaxProperty, 0d);
            ConfigureValidationSet(() => RotateProperty, () => RotateMinProperty, () => RotateMaxProperty, 0d);
        }

        private static string GetPropertyName(string dependencyPropertyName)
        {
            return dependencyPropertyName.Substring(0, dependencyPropertyName.Length - 8);
        }

        private static void ConfigureValidationSet(Expression<Func<DependencyProperty>> valueExpression, Expression<Func<DependencyProperty>> minExpression, Expression<Func<DependencyProperty>> maxExpression, object defaultValue)
        {
            // Ignore the blue squigglies
            var valueField = (FieldInfo)(((MemberExpression)valueExpression.Body).Member);
            DependencyProperty valueProperty = null;
            var minField = (FieldInfo)(((MemberExpression)minExpression.Body).Member);
            DependencyProperty minProperty = null;
            var maxField = (FieldInfo)(((MemberExpression)maxExpression.Body).Member);
            DependencyProperty maxProperty = null;

            valueProperty = DependencyProperty.Register(GetPropertyName(valueField.Name), typeof(double), typeof(TouchContainer), new PropertyMetadata(defaultValue, null, (sender, value) => {
                var element = sender as TouchContainer;
                if (element == null)
                    return value;
                var minValue = (double)element.GetValue(minProperty);
                var maxValue = (double)element.GetValue(maxProperty);
                return Math.Max(minValue, Math.Min(maxValue, (double)value));
            }));
            valueField.SetValue(null, valueProperty);

            minProperty = DependencyProperty.Register(GetPropertyName(minField.Name), typeof(double), typeof(TouchContainer), new PropertyMetadata(double.MinValue, null, (sender, minValue) => {
                var element = sender as TouchContainer;
                if (element == null)
                    return minValue;
                var maxValue = (double)element.GetValue(maxProperty);
                var value = (double)element.GetValue(valueProperty);
                var validated = Math.Max((double)minValue, Math.Min(maxValue, value));
                if (value != validated)
                    element.SetValue(valueProperty, validated);
                return minValue;
            }));
            minField.SetValue(null, minProperty);

            maxProperty = DependencyProperty.Register(GetPropertyName(maxField.Name), typeof(double), typeof(TouchContainer), new PropertyMetadata(double.MaxValue, null, (sender, maxValue) => {
                var element = sender as TouchContainer;
                if (element == null)
                    return maxValue;
                var minValue = (double)element.GetValue(minProperty);
                var value = (double)element.GetValue(valueProperty);
                var validated = Math.Max(minValue, Math.Min((double)maxValue, value));
                if (value != validated)
                    element.SetValue(valueProperty, validated);
                return maxValue;
            }));
            maxField.SetValue(null, maxProperty);
        }

        public static readonly DependencyProperty UseDefaultTransformsProperty = DependencyProperty.Register("UseDefaultTransforms", typeof(bool), typeof(TouchContainer), new PropertyMetadata(true, UseDefaultTransformsUpdated));
        public bool UseDefaultTransforms { get { return (bool)GetValue(UseDefaultTransformsProperty); } set { SetValue(UseDefaultTransformsProperty, value); } }

        public static readonly DependencyProperty IsTranslateEnabledProperty = DependencyProperty.Register("IsTranslateEnabled", typeof(bool), typeof(TouchContainer), new PropertyMetadata(true));
        /// <summary>
        /// Get/Set if translation guestures are tracked.
        /// </summary>
        public bool IsTranslateEnabled { get { return (bool)GetValue(IsTranslateEnabledProperty); } set { SetValue(IsTranslateEnabledProperty, value); } }

        public static readonly DependencyProperty TranslateXProperty;
        /// <summary>
        /// Get/Set the accumulated X translation.
        /// </summary>
        public double TranslateX { get { return (double)GetValue(TranslateXProperty); } set { SetValue(TranslateXProperty, value); } }

        public static readonly DependencyProperty TranslateXMinProperty;
        /// <summary>
        /// Get/Set the minimum X translation value.
        /// </summary>
        public double TranslateXMin { get { return (double)GetValue(TranslateXMinProperty); } set { SetValue(TranslateXMinProperty, value); } }

        public static readonly DependencyProperty TranslateXMaxProperty;
        /// <summary>
        /// Get/Set the maximum X translation value.
        /// </summary>
        public double TranslateXMax { get { return (double)GetValue(TranslateXMaxProperty); } set { SetValue(TranslateXMaxProperty, value); } }

        public static readonly DependencyProperty TranslateYProperty;
        /// <summary>
        /// Get/Set the accumulated Y translation.
        /// </summary>
        public double TranslateY { get { return (double)GetValue(TranslateYProperty); } set { SetValue(TranslateYProperty, value); } }

        public static readonly DependencyProperty TranslateYMinProperty;
        /// <summary>
        /// Get/Set the minimum Y translation value.
        /// </summary>
        public double TranslateYMin { get { return (double)GetValue(TranslateYMinProperty); } set { SetValue(TranslateYMinProperty, value); } }

        public static readonly DependencyProperty TranslateYMaxProperty;
        /// <summary>
        /// Get/Set the maximum Y translation value.
        /// </summary>
        public double TranslateYMax { get { return (double)GetValue(TranslateYMaxProperty); } set { SetValue(TranslateYMaxProperty, value); } }

        public static readonly DependencyProperty IsRotateEnabledProperty = DependencyProperty.Register("IsRotateEnabled", typeof(bool), typeof(TouchContainer), new PropertyMetadata(true));
        /// <summary>
        /// Get/Set if rotation guestures are tracked.
        /// </summary>
        public bool IsRotateEnabled { get { return (bool)GetValue(IsRotateEnabledProperty); } set { SetValue(IsRotateEnabledProperty, value); } }

        public static readonly DependencyProperty RotateProperty;
        /// <summary>
        /// Get/Set the accumulated rotation.
        /// </summary>
        public double Rotate { get { return (double)GetValue(RotateProperty); } set { SetValue(RotateProperty, value); } }

        public static readonly DependencyProperty RotateMinProperty;
        /// <summary>
        /// Get/Set the minimum rotation value
        /// </summary>
        public double RotateMin { get { return (double)GetValue(RotateMinProperty); } set { SetValue(RotateMinProperty, value); } }

        public static readonly DependencyProperty RotateMaxProperty;
        /// <summary>
        /// Get/Set the minimum rotation value
        /// </summary>
        public double RotateMax { get { return (double)GetValue(RotateMaxProperty); } set { SetValue(RotateMaxProperty, value); } }

        public static readonly DependencyProperty IsScaleEnabledProperty = DependencyProperty.Register("IsScaleEnabled", typeof(bool), typeof(TouchContainer), new PropertyMetadata(true));
        /// <summary>
        /// Get/Set if resize guestures are tracked.
        /// </summary>
        public bool IsScaleEnabled { get { return (bool)GetValue(IsScaleEnabledProperty); } set { SetValue(IsScaleEnabledProperty, value); } }

        public static readonly DependencyProperty ScaleXProperty;
        /// <summary>
        /// Get/Set the accumulated resize Width.
        /// </summary>
        public double ScaleX { get { return (double)GetValue(ScaleXProperty); } set { SetValue(ScaleXProperty, value); } }

        public static readonly DependencyProperty ScaleXMinProperty;
        /// <summary>
        /// Get/Set the minimum scalex value.
        /// </summary>
        public double ScaleXMin { get { return (double)GetValue(ScaleXMinProperty); } set { SetValue(ScaleXMinProperty, value); } }

        public static readonly DependencyProperty ScaleXMaxProperty;
        /// <summary>
        /// Get/Set the maximum scalex value.
        /// </summary>
        public double ScaleXMax { get { return (double)GetValue(ScaleXMaxProperty); } set { SetValue(ScaleXMaxProperty, value); } }

        public static readonly DependencyProperty ScaleYProperty;
        /// <summary>
        /// Get/Set the accumulated resize Height.
        /// </summary>
        public double ScaleY { get { return (double)GetValue(ScaleYProperty); } set { SetValue(ScaleYProperty, value); } }

        public static readonly DependencyProperty ScaleYMinProperty;
        /// <summary>
        /// Get/Set the minimum scalex value.
        /// </summary>
        public double ScaleYMin { get { return (double)GetValue(ScaleYMinProperty); } set { SetValue(ScaleYMinProperty, value); } }

        public static readonly DependencyProperty ScaleYMaxProperty;
        /// <summary>
        /// Get/Set the maximum scaley value.
        /// </summary>
        public double ScaleYMax { get { return (double)GetValue(ScaleYMaxProperty); } set { SetValue(ScaleYMaxProperty, value); } }

        public static readonly DependencyProperty IsFlickEnabledProperty = DependencyProperty.Register("IsFlickEnabled", typeof(bool), typeof(TouchContainer), new PropertyMetadata(false));
        /// <summary>
        /// Get/Set if flick guestures are tracked.
        /// </summary>
        public bool IsFlickEnabled { get { return (bool)GetValue(IsFlickEnabledProperty); } set { SetValue(IsFlickEnabledProperty, value); } }

        public static readonly DependencyProperty FlickXProperty;
        /// <summary>
        /// Get/Set the accumulated X flicks.
        /// </summary>
        public double FlickX { get { return (double)GetValue(FlickXProperty); } set { SetValue(FlickXProperty, value); } }

        public static readonly DependencyProperty FlickXMinProperty;
        /// <summary>
        /// Get/Set the minimum flickx value.
        /// </summary>
        public double FlickXMin { get { return (double)GetValue(FlickXMinProperty); } set { SetValue(FlickXMinProperty, value); } }

        public static readonly DependencyProperty FlickXMaxProperty;
        /// <summary>
        /// Get/Set the maximum flickx value.
        /// </summary>
        public double FlickXMax { get { return (double)GetValue(FlickXMaxProperty); } set { SetValue(FlickXMaxProperty, value); } }

        public static readonly DependencyProperty FlickYProperty;
        /// <summary>
        /// Get/Set the accumulated Y flicks.
        /// </summary>
        public double FlickY { get { return (double)GetValue(FlickYProperty); } set { SetValue(FlickYProperty, value); } }

        public static readonly DependencyProperty FlickYMinProperty;
        /// <summary>
        /// Get/Set the minimum flicky value.
        /// </summary>
        public double FlickYMin { get { return (double)GetValue(FlickYMinProperty); } set { SetValue(FlickYMinProperty, value); } }

        public static readonly DependencyProperty FlickYMaxProperty;
        /// <summary>
        /// Get/Set the maximum flicky value.
        /// </summary>
        public double FlickYMax { get { return (double)GetValue(FlickYMaxProperty); } set { SetValue(FlickYMaxProperty, value); } }

        public static readonly DependencyProperty WeightProperty = DependencyProperty.Register("Weight", typeof(double), typeof(TouchContainer), new PropertyMetadata(default(double)));
        /// <summary>
        /// Get/Set a value effecting deceleration of manipulations
        /// </summary>
        public double Weight { get { return (double)GetValue(WeightProperty); } set { SetValue(WeightProperty, value); } }

        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register("Content", typeof(UIElement), typeof(TouchContainer), new PropertyMetadata(OnContentSet));
        /// <summary>
        /// The child element which will be given transforms and will trigger touch manipulations
        /// </summary>
        public UIElement Content { get { return (UIElement)GetValue(ContentProperty); } set { SetValue(ContentProperty, value); } }

        public static readonly DependencyProperty FlickThresholdProperty = DependencyProperty.Register("FlickThreshold", typeof(int), typeof(TouchContainer), new PropertyMetadata(10));
        /// <summary>
        /// The threshold to detect a flick
        /// </summary>
        public int FlickThreshold { get { return (int)GetValue(FlickThresholdProperty); } set { SetValue(FlickThresholdProperty, value); } }
        #endregion

        private TranslateTransform _translation = new TranslateTransform();
        private TranslateTransform _flick = new TranslateTransform();
        private RotateTransform _rotation = new RotateTransform();
        private ScaleTransform _scale = new ScaleTransform();
        private TransformGroup _transforms = new TransformGroup();
        private int _deltaCount;
        private bool _isFlick;

        public TouchContainer()
        {
            IsManipulationEnabled = true;
            SetBinding(TranslateXProperty, new Binding("X") { Source = _translation, Mode = BindingMode.OneWayToSource });
            SetBinding(TranslateYProperty, new Binding("Y") { Source = _translation, Mode = BindingMode.OneWayToSource });
            SetBinding(FlickXProperty, new Binding("X") { Source = _flick, Mode = BindingMode.OneWayToSource });
            SetBinding(FlickYProperty, new Binding("Y") { Source = _flick, Mode = BindingMode.OneWayToSource });
            SetBinding(RotateProperty, new Binding("Angle") { Source = _rotation, Mode = BindingMode.OneWayToSource });
            SetBinding(ScaleXProperty, new Binding("ScaleX") { Source = _scale, Mode = BindingMode.OneWayToSource });
            SetBinding(ScaleYProperty, new Binding("ScaleY") { Source = _scale, Mode = BindingMode.OneWayToSource });
        }

        protected override void OnManipulationStarting(ManipulationStartingEventArgs e)
        {
            e.Handled = true;
            e.ManipulationContainer = this;
            _deltaCount = 0;
            _isFlick = false;
            base.OnManipulationStarting(e);
        }

        protected override void OnManipulationDelta(ManipulationDeltaEventArgs e)
        {
            e.Handled = true;
            _deltaCount++;
            if (IsTranslateEnabled) {
                TranslateX = Math.Max(TranslateXMin, Math.Min(TranslateXMax, TranslateX + e.DeltaManipulation.Translation.X));
                TranslateY = Math.Max(TranslateYMin, Math.Min(TranslateYMax, TranslateY + e.DeltaManipulation.Translation.Y));
            }
            if (IsFlickEnabled && e.IsInertial && _isFlick) {
                FlickX = Math.Max(FlickXMin, Math.Min(FlickXMax, FlickX + e.DeltaManipulation.Translation.X));
                FlickY = Math.Max(FlickYMin, Math.Min(FlickYMax, FlickY + e.DeltaManipulation.Translation.Y));
            }
            if (IsRotateEnabled) {
                Rotate = Math.Max(RotateMin, Math.Min(RotateMax, Rotate + e.DeltaManipulation.Rotation));
            }
            if (IsScaleEnabled) {
                ScaleX = Math.Max(ScaleXMin, Math.Min(ScaleXMax, ScaleX * e.DeltaManipulation.Scale.X));
                ScaleY = Math.Max(ScaleYMin, Math.Min(ScaleYMax, ScaleY * e.DeltaManipulation.Scale.Y));
            }
            base.OnManipulationDelta(e);
        }

        protected override void OnManipulationInertiaStarting(ManipulationInertiaStartingEventArgs e)
        {
            e.Handled = true;
            _isFlick = _deltaCount < FlickThreshold;
            const double velocityTime = 1000d * 1000d; // seconds squared
            e.TranslationBehavior = new InertiaTranslationBehavior {
                InitialVelocity = e.InitialVelocities.LinearVelocity,
                DesiredDeceleration = 10d * 96d / velocityTime * Weight
            };

            e.ExpansionBehavior = new InertiaExpansionBehavior {
                InitialVelocity = e.InitialVelocities.ExpansionVelocity,
                DesiredDeceleration = .1d * 96d / velocityTime * Weight
            };

            e.RotationBehavior = new InertiaRotationBehavior {
                InitialVelocity = e.InitialVelocities.AngularVelocity,
                DesiredDeceleration = 720d / velocityTime * Weight
            };
            base.OnManipulationInertiaStarting(e);
        }

        private static void OnContentSet(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var container = sender as TouchContainer;
            if (container == null)
                return;

            var oldelement = e.OldValue as UIElement;
            if (oldelement != null) {
                container.RemoveVisualChild(oldelement);
                container.RemoveLogicalChild(oldelement);
                container.RemoveDefaultTransforms(oldelement);
            }

            var newelement = e.NewValue as UIElement;
            if (newelement != null) {
                container.AddVisualChild(newelement);
                container.AddLogicalChild(newelement);
                container.AddDefaultTransforms(newelement);
            }
        }

        private static void UseDefaultTransformsUpdated(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var container = sender as TouchContainer;
            if (container == null || container.Content == null)
                return;

            if (container.UseDefaultTransforms) {
                container.AddDefaultTransforms(container.Content);
            } else {
                container.RemoveDefaultTransforms(container.Content);
            }
        }

        private void RemoveDefaultTransforms(UIElement target)
        {
            _transforms.Children.Remove(_translation);
            _transforms.Children.Remove(_rotation);
            _transforms.Children.Remove(_scale);
            _transforms.Children.Remove(_flick);
            var remaining = _transforms.Children.ToArray();
            _transforms.Children.Clear();
            var replacement = remaining.Length > 1
                ? new TransformGroup { Children = new TransformCollection(remaining) }
                : remaining.FirstOrDefault();
            target.RenderTransform = replacement;
        }

        private void AddDefaultTransforms(UIElement target)
        {
            _transforms.Children.Clear();
            _transforms.Children.Add(_translation);
            _transforms.Children.Add(_rotation);
            _transforms.Children.Add(_scale);
            _transforms.Children.Add(_flick);
            if (target.RenderTransform != null) {
                _transforms.Children.Add(target.RenderTransform);
            }
            target.RenderTransform = _transforms;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            if (Content != null) {
                Content.Arrange(new Rect(0, 0, finalSize.Width, finalSize.Height));
            }
            return finalSize;
        }
        protected override System.Collections.IEnumerator LogicalChildren { get { return new[] { Content }.GetEnumerator(); } }
        protected override System.Windows.Media.Visual GetVisualChild(int index) { return Content; }
        protected override int VisualChildrenCount { get { return Content == null ? 0 : 1; } }
    }
}
