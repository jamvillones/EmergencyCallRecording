using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Signature {
    /// <summary>
    /// Interaction logic for SignatureCaptureView.xaml
    /// </summary>
    public partial class SignatureCaptureView : UserControl {
        public SignatureCaptureView() {
            InitializeComponent();
            Canvas.DefaultDrawingAttributes = PenAttributes;
        }
        private readonly DrawingAttributes PenAttributes = new() {
            Color = Colors.Black,
            Height = 2,
            Width = 2
        };

        private readonly DrawingAttributes HighlighterAttributes = new() {
            Color = Colors.Yellow,
            Height = 10,
            Width = 2,
            IgnorePressure = true,
            IsHighlighter = true,
            StylusTip = StylusTip.Rectangle
        };



        #region Editing Mode

        private void SelectBtn_Click(object sender, RoutedEventArgs e) {
            SetEditingMode(EditingMode.Select);
        }

        private void PenBtn_Click(object sender, RoutedEventArgs e) {
            SetEditingMode(EditingMode.Pen);
        }

        private void HighlighterBtn_Click(object sender, RoutedEventArgs e) {
            SetEditingMode(EditingMode.Highlighter);
        }

        private void EraserBtn_Click(object sender, RoutedEventArgs e) {
            SetEditingMode(EditingMode.Eraser);
        }

        private void SetEditingMode(EditingMode mode) {
            SelectBtn.IsChecked = false;
            PenBtn.IsChecked = false;
            //HighlighterBtn.IsChecked = false;
            EraserBtn.IsChecked = false;

            switch (mode) {
                case EditingMode.Select:
                    SelectBtn.IsChecked = true;
                    Canvas.EditingMode = InkCanvasEditingMode.Select;
                    break;

                case EditingMode.Pen:
                    PenBtn.IsChecked = true;
                    Canvas.EditingMode = InkCanvasEditingMode.Ink;
                    Canvas.DefaultDrawingAttributes = PenAttributes;
                    break;

                //case EditingMode.Highlighter:
                //    HighlighterBtn.IsChecked = true;
                //    Canvas.EditingMode = InkCanvasEditingMode.Ink;
                //    Canvas.DefaultDrawingAttributes = HighlighterAttributes;
                //    break;

                case EditingMode.Eraser:
                    EraserBtn.IsChecked = true;
                    if (PartialStrokeRadio.IsChecked == true)
                        Canvas.EditingMode = InkCanvasEditingMode.EraseByPoint;
                    else
                        Canvas.EditingMode = InkCanvasEditingMode.EraseByStroke;
                    break;

                default:
                    break;
            }
        }

        #endregion
        #region Pen

        private void PenColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e) {
            if (IsLoaded)
                PenAttributes.Color = Colors.Black;
        }

        private void ThicknessSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
            if (IsLoaded) {
                PenAttributes.Width = ThicknessSlider.Value;
                PenAttributes.Height = ThicknessSlider.Value;
                sliderText.Text = ThicknessSlider.Value.ToString();
            }
        }

        #endregion
        #region Highlighter

        private void YellowRadio_Click(object sender, RoutedEventArgs e) {
            HighlighterAttributes.Color = Colors.Yellow;
        }

        private void CyanRadio_Click(object sender, RoutedEventArgs e) {
            HighlighterAttributes.Color = Colors.Cyan;
        }

        private void MagentaRadio_Click(object sender, RoutedEventArgs e) {
            HighlighterAttributes.Color = Colors.Magenta;
        }

        #endregion
        #region Eraser Type

        private void PartialStrokeRadio_Click(object sender, RoutedEventArgs e) {
            if (EraserBtn.IsChecked == true)
                Canvas.EditingMode = InkCanvasEditingMode.EraseByPoint;
        }

        private void FullStrokeRadio_Click(object sender, RoutedEventArgs e) {
            if (EraserBtn.IsChecked == true)
                Canvas.EditingMode = InkCanvasEditingMode.EraseByStroke;
        }

        #endregion

        private void RadioButton_Black_Checked(object sender, RoutedEventArgs e) {
            PenAttributes.Color = Colors.Black;
        }
        private void RadioButton_Red_Checked(object sender, RoutedEventArgs e) {
            PenAttributes.Color = Colors.DarkRed;
        }
        private void RadioButton_Blue_Checked(object sender, RoutedEventArgs e) {
            PenAttributes.Color = Colors.Blue;
        }

        private void Button_Clear_Click(object sender, RoutedEventArgs e) {
            if (MessageBox.Show("Are you sure you want to discard?", "", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.Cancel) return;
            Canvas.Strokes.Clear();
        }

        private void Canvas_StrokeCollected(object sender, InkCanvasStrokeCollectedEventArgs e) {
        }
    }

    public enum EditingMode {
        Select, Pen, Highlighter, Eraser
    }

    //int diameter = (int)PenSize.small;
    //Brush penBrush = Brushes.Black;
    //bool isWriting = false;
    //bool isErasing = false;

    //public enum PenSize {
    //    small = 10,
    //    medium = 15,
    //    large = 20
    //}

    //public static void PaintCircle(int diameter, Point position, InkCanvas canvas, Brush brush) {
    //    Ellipse newEllipse = new() {
    //        Fill = brush,
    //        Width = diameter,
    //        Height = diameter
    //    };

    //    Canvas.SetTop(newEllipse, position.Y);
    //    Canvas.SetLeft(newEllipse, position.X);

    //    canvas.Children.Add(newEllipse);
    //}

    //private void Canvas_MouseMove(object sender, MouseEventArgs e) {

    //}

    //private void paintCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
    //    isWriting = true;
    //}

    //private void paintCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
    //    isWriting = false;
    //}

    //private void Button_Click(object sender, RoutedEventArgs e) {
    //    paintCanvas.Children.Clear();
    //}

    //private void paintCanvas_MouseLeave(object sender, MouseEventArgs e) {
    //    //isWriting = false;
    //}

    //private void UserControl_MouseMove(object sender, MouseEventArgs e) {
    //    //if (isWriting) {
    //    //    Point mousePosition = e.GetPosition(paintCanvas);
    //    //    PaintCircle(diameter, mousePosition, paintCanvas, penBrush);
    //    //}
    //}
}

