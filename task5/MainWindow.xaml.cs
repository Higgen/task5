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
using System.Windows.Markup;
using System.Xml;
using System.IO;
using Microsoft.VisualBasic;


namespace task5
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        public static Boolean drawRectangle;
        public static Boolean drawPolygon;
        public static Boolean drawEllipse;
        
        public static Boolean selectShape;
        public static Shape clickedShape;
        public static Brush previousColor;
        public static PropertyGrid propertyGrid1 = new PropertyGrid();

        private System.Windows.Forms.Integration.WindowsFormsHost host = new System.Windows.Forms.Integration.WindowsFormsHost();
        private ShapeHelper shapeHelper;
        private PolygonHelper polygonHelper;
        private SaveNLoad saveNLoad;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            propertyGrid1.CommandsVisibleIfAvailable = true;
            host.Child = propertyGrid1;
            grid1.Children.Add(host);
            shapeHelper = new ShapeHelper(canvas);
            polygonHelper = new PolygonHelper(canvas);
            saveNLoad = new SaveNLoad(canvas);
        }

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (drawRectangle)
            {
               shapeHelper.createRectangle(sender, e);
            }
            else if (drawEllipse)
            {
                shapeHelper.createEllipse(sender, e);
            }
            else if (drawPolygon)
            {
                polygonHelper.draw(sender, e);
            }
        }

        private void Canvas_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (drawRectangle)
            {
                shapeHelper.draw(sender, e); // TODO samma funktionsanrop iaf ? skippa if sats?
            }
            else if (drawEllipse)
            {
                shapeHelper.draw(sender, e);
            }
        }

        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (drawRectangle || drawEllipse)
            {
                shapeHelper.finished(sender, e); 
            }

            else if (selectShape)
            {
                shapeHelper.drop(sender, e);   
            }
        }

        private void rectangle_Click(object sender, RoutedEventArgs e)
        {
            drawEllipse = drawPolygon = selectShape = false;
            drawRectangle = true;
        }

        private void ellipse_Click(object sender, RoutedEventArgs e)
        {
            drawPolygon = selectShape = drawRectangle = false;
            drawEllipse = true;
        }

        private void polygon_Click(object sender, RoutedEventArgs e)
        {
            drawEllipse = selectShape = drawRectangle = false;
            drawPolygon = true;

            polygonHelper.createPolygon();
        }

        private void noShape_Click(object sender, RoutedEventArgs e)
        {
            drawPolygon = drawEllipse = drawRectangle = false;
            selectShape = true;
        }

        private void load_Click(object sender, RoutedEventArgs e)
        {
            saveNLoad.load();
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            saveNLoad.save();
        }

        private void rotate_Click(object sender, RoutedEventArgs e)
        {
            if (clickedShape != null)
            {
                if (clickedShape is Polygon)
                {
                    polygonHelper.rotate();
                }

                else if (clickedShape is Rectangle || clickedShape is Ellipse)
                {
                    shapeHelper.rotate();
                }
            }
        }

        private void scale_Click(object sender, RoutedEventArgs e)
        {
            if (clickedShape is Ellipse || clickedShape is Rectangle)
            {
                shapeHelper.scale(scaleNumber.Text);
            }
            if (clickedShape is Polygon)
            {
                polygonHelper.scale(scaleNumber.Text);
            }
        }
    }
}
