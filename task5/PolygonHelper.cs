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
    class PolygonHelper
    {
        private double startValueShapeX;
        private double startValueShapeY;

        private Canvas canvas;
        private Point startPoint;
        private Polygon myShape;

        private double topValue = Double.NaN;
        private double leftValue = Double.NaN;
        private double alot = 10000;

        public PolygonHelper(Canvas windowCanvas)
        {
            canvas = windowCanvas;
        }

        public void createPolygon()
        {
            myShape = new Polygon
            {
                Stroke = Brushes.DarkOrange,
                StrokeThickness = 2
            };
            myShape.MouseLeftButtonDown += getPosition;
            myShape.MouseRightButtonDown += select;
            canvas.Children.Add(myShape);

        }

        public void draw(object sender, System.Windows.Input.MouseEventArgs e)
        {
            startPoint = e.GetPosition(canvas);
            Point polygonLine = new System.Windows.Point(startPoint.X, startPoint.Y);
            myShape.Points.Add(polygonLine);
            myShape.Fill = Brushes.BurlyWood;

            findPolygonValues(myShape);
        }

        public void getPosition(object sender, MouseButtonEventArgs e)
        {
            if (MainWindow.selectShape)
            {
                Shape myShape = sender as Shape;
                if (Double.IsNaN(Canvas.GetLeft(myShape)))    //if SetLeft and SetTop haven't been set
                {
                    myShape.MouseMove += move;
                    myShape.CaptureMouse();
                    startPoint = e.GetPosition(canvas);
                    Canvas.SetLeft(myShape, e.GetPosition(canvas).X - startPoint.X);
                    Canvas.SetTop(myShape, e.GetPosition(canvas).Y - startPoint.Y);
                    startValueShapeX = startPoint.X - Canvas.GetLeft(myShape);
                    startValueShapeY = startPoint.Y - Canvas.GetTop(myShape);
                }
                else
                {
                    myShape.MouseMove += move;
                    myShape.CaptureMouse();
                    startPoint = e.GetPosition(canvas);
                    startValueShapeX = startPoint.X - Canvas.GetLeft(myShape);
                    startValueShapeY = startPoint.Y - Canvas.GetTop(myShape);
                }
            }
        }

        public void move(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (MainWindow.selectShape)
            {
                Shape myShape = sender as Shape;
                Canvas canvas = myShape.Parent as Canvas;

                Point mouseOnCanvas = e.GetPosition(canvas);

                if (myShape.IsMouseCaptured == true)
                {
                    Canvas.SetTop(myShape, mouseOnCanvas.Y - startValueShapeY);
                    Canvas.SetLeft(myShape, mouseOnCanvas.X - startValueShapeX);
                }
            }
        }

        public void select(object sender, MouseButtonEventArgs e)
        {
            Shape shape = sender as Shape;
            try
            {
                MainWindow.clickedShape.Stroke = MainWindow.previousColor;

            }
            catch { }
            MainWindow.previousColor = shape.Stroke;
            shape.Stroke = Brushes.Black;
            MainWindow.clickedShape = shape;
            MainWindow.propertyGrid1.SelectedObject = shape;
        }


        public void scale(string scale)
        {
            if (MainWindow.selectShape)
            {
                TransformGroup shapeTg = new TransformGroup();
                double doubleScale;
                findPolygonValues((Polygon)MainWindow.clickedShape);
                try
                {
                    doubleScale = double.Parse(scale);
                }
                catch
                {
                    doubleScale = 1; //if the user is bad at typing scales
                }


                ScaleTransform temporaryTransform = new ScaleTransform(doubleScale, doubleScale, (MainWindow.clickedShape.ActualWidth + leftValue) / 2, (MainWindow.clickedShape.ActualHeight + topValue) / 2);

                try
                {
                    foreach (Transform t in ((TransformGroup)MainWindow.clickedShape.RenderTransform).Children)
                    {
                        if (t is ScaleTransform)
                        {
                            temporaryTransform = (ScaleTransform)t;
                            temporaryTransform.ScaleX *= doubleScale;
                            temporaryTransform.ScaleY *= doubleScale;
                            temporaryTransform.CenterX = (MainWindow.clickedShape.ActualWidth + leftValue) / 2;
                            temporaryTransform.CenterY = (MainWindow.clickedShape.ActualHeight + topValue) / 2;
                        }
                        else
                        {
                            shapeTg.Children.Add(t);
                        }
                    }

                }
                catch   //couldn't cast RenderTransform to TransformGroup
                {
                }

                shapeTg.Children.Add(temporaryTransform);
                MainWindow.clickedShape.RenderTransform = shapeTg;
            }
        }


        public void rotate()
        {
            TransformGroup shapeTg = new TransformGroup();
            findPolygonValues((Polygon)MainWindow.clickedShape);
            RotateTransform temporaryTransform = new RotateTransform(45, (MainWindow.clickedShape.ActualWidth + leftValue) / 2, (MainWindow.clickedShape.ActualHeight + topValue) / 2);
            

            try
            {
                foreach (Transform t in ((TransformGroup)MainWindow.clickedShape.RenderTransform).Children)
                {
                    if (t is RotateTransform)
                    {
                        temporaryTransform = (RotateTransform)t;
                        temporaryTransform.Angle += 45;
                        temporaryTransform.CenterX = (MainWindow.clickedShape.ActualWidth + leftValue) / 2;
                        temporaryTransform.CenterY = (MainWindow.clickedShape.ActualHeight + topValue) / 2;
                    }
                    else
                    {
                        shapeTg.Children.Add(t);
                    }
                }

            }
            catch   //couldn't cast RenderTransform to TransformGroup
            {
            }

            shapeTg.Children.Add(temporaryTransform);
            MainWindow.clickedShape.RenderTransform = shapeTg;
        }



        private void findPolygonValues(Polygon shape)
        {
            leftValue = alot;
            topValue = alot;

            foreach (Point p in shape.Points)
            {
                if (topValue > p.Y)
                {
                    topValue = p.Y;
                }
                if (leftValue > p.X)
                {
                    leftValue = p.X;
                }
            }
        }

    }
}
