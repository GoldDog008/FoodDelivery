using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace Ipz_client.Design
{
    public class OutlinedTextBlock : FrameworkElement
    {
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(OutlinedTextBlock),
                new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty FillProperty =
            DependencyProperty.Register("Fill", typeof(Brush), typeof(OutlinedTextBlock),
                new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty StrokeProperty =
            DependencyProperty.Register("Stroke", typeof(Brush), typeof(OutlinedTextBlock),
                new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register("StrokeThickness", typeof(double), typeof(OutlinedTextBlock),
                new FrameworkPropertyMetadata(1.0, FrameworkPropertyMetadataOptions.AffectsRender));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public Brush Fill
        {
            get { return (Brush)GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }

        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            var formattedText = new FormattedText(
                Text,
                System.Globalization.CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface("Moul"),
                40, // Это должно быть либо жестко заданное значение, либо значение свойства FontSize
                Brushes.Black); // Этот параметр не будет использоваться для отрисовки текста, так как мы зададим кисти непосредственно

            // Создаем геометрию текста
            Geometry textGeometry = formattedText.BuildGeometry(new Point(0, 0));

            // Рисуем контур текста
            drawingContext.DrawGeometry(null, new Pen(Stroke, StrokeThickness), textGeometry);

            // Рисуем заливку текста. Установите Fill в Brushes.White, если хотите белый внутренний цвет
            drawingContext.DrawGeometry(Fill, null, textGeometry);
        }


    }
}
