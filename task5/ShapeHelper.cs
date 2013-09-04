using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;

namespace task5
{
    public class ShapeHelper
    {
        private Canvas canvas;
        private Point startPoint;
        private Shape myShape;

        private double startValueShapeX;
        private double startValueShapeY;

        public ShapeHelper(Canvas windowCanvas)
        {
            canvas = windowCanvas;
        }


        public void createRectangle(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(canvas);

            myShape = new Rectangle
            {
                Stroke = Brushes.Red,
                StrokeThickness = 2
            };

            Canvas.SetLeft(myShape, startPoint.X);
            Canvas.SetTop(myShape, startPoint.X);

            myShape.MouseLeftButtonDown += getPosition;
            myShape.MouseRightButtonDown += select;

            canvas.Children.Add(myShape);

        }

        public void createEllipse(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(canvas);

            myShape = new Ellipse
            {
                Stroke = Brushes.Red,
                StrokeThickness = 2
            };

            Canvas.SetLeft(myShape, startPoint.X);
            Canvas.SetTop(myShape, startPoint.X);

            myShape.MouseLeftButtonDown += getPosition;
            myShape.MouseRightButtonDown += select;

            canvas.Children.Add(myShape);

        }

        public void draw(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released || myShape == null)
                return;

            var pos = e.GetPosition(canvas);
            var x = Math.Min(pos.X, startPoint.X);
            var y = Math.Min(pos.Y, startPoint.Y);
            var w = Math.Max(pos.X, startPoint.X) - x;
            var h = Math.Max(pos.Y, startPoint.Y) - y;
            myShape.Width = w;
            myShape.Height = h;

            Canvas.SetLeft(myShape, x);
            Canvas.SetTop(myShape, y);
        }

        public void getPosition(object sender, MouseButtonEventArgs e)
        {
            if (MainWindow.selectShape)
            {
                myShape = sender as Shape;
                canvas = myShape.Parent as Canvas;
                myShape.MouseMove += move;
                myShape.CaptureMouse();
                startPoint = e.GetPosition(canvas);

                startValueShapeX = startPoint.X - Canvas.GetLeft(myShape);
                startValueShapeY = startPoint.Y - Canvas.GetTop(myShape);
            }
        }

        public void select(object sender, MouseButtonEventArgs e)
        {
            if (MainWindow.selectShape)
            {
                myShape = sender as Shape;
                try
                {
                    MainWindow.clickedShape.Stroke = MainWindow.previousColor;
                }
                catch { }
                MainWindow.previousColor = myShape.Stroke;
                myShape.Stroke = Brushes.Black;
                MainWindow.clickedShape = myShape;
                MainWindow.propertyGrid1.SelectedObject = myShape;
            }
        }


        public void move(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (MainWindow.selectShape)
            {
                myShape = sender as Shape;
                canvas = myShape.Parent as Canvas;

                Point mouseOnCanvas = e.GetPosition(canvas);
                if (myShape.IsMouseCaptured == true)
                {
                    Canvas.SetTop(myShape, mouseOnCanvas.Y - startValueShapeY);
                    Canvas.SetLeft(myShape, mouseOnCanvas.X - startValueShapeX);
                }
            }
        }

        public void finished(object sender, MouseButtonEventArgs e)
        {
            try
            {
                myShape.Fill = Brushes.Green;
 
            }
            catch { }
        }

        public void drop(object sender, MouseButtonEventArgs e)
        {
            for (int index = 0; index < VisualTreeHelper.GetChildrenCount((DependencyObject)sender); index++)
            {
                Shape s = VisualTreeHelper.GetChild((DependencyObject)sender, index) as Shape;

                if (s != null)
                {
                    if (s.IsMouseCaptured == true)
                    {
                        s.ReleaseMouseCapture();
                    }
                }
            }
        }

        public void scale(string scale)
        {
            if (MainWindow.selectShape)
            {
                TransformGroup shapeTg = new TransformGroup();
                double doubleScale;
                try
                {
                    doubleScale = double.Parse(scale);
                }
                catch
                {
                    doubleScale = 1;
                }

                ScaleTransform temporaryTransform = new ScaleTransform(doubleScale, doubleScale, MainWindow.clickedShape.Width / 2, MainWindow.clickedShape.Height / 2);

                try
                {
                    foreach (Transform t in ((TransformGroup)MainWindow.clickedShape.RenderTransform).Children)
                    {
                        if (t is ScaleTransform)
                        {
                            temporaryTransform = (ScaleTransform)t;
                            temporaryTransform.ScaleX *= doubleScale;
                            temporaryTransform.ScaleY *= doubleScale;
                        }
                        else
                        {
                            shapeTg.Children.Add(t);
                        }
                    }

                }
                catch   //could'nt cast RenderTransform to TransformGroup
                {
                }

                shapeTg.Children.Add(temporaryTransform);
                MainWindow.clickedShape.RenderTransform = shapeTg;
            }
        }


        public void rotate()
        {


            TransformGroup shapeTg = new TransformGroup();
            RotateTransform temporaryTransform = new RotateTransform(45, MainWindow.clickedShape.Width / 2, MainWindow.clickedShape.Height / 2);

            try
            {
                foreach (Transform t in ((TransformGroup)MainWindow.clickedShape.RenderTransform).Children)
                {
                    if (t is RotateTransform)
                    {
                        temporaryTransform = (RotateTransform)t;
                        temporaryTransform.Angle += 45;
                    }
                    else
                    {
                        shapeTg.Children.Add(t);
                    }
                }

            }
            catch   //could'nt cast RenderTransform to TransformGroup
            {
            }

            shapeTg.Children.Add(temporaryTransform);
            MainWindow.clickedShape.RenderTransform = shapeTg;

        }
    }
}
