using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualBasic;
using System.IO;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Input;
using System.Windows.Shapes;

namespace task5
{
    class SaveNLoad
    {
        private Canvas canvas;
        private ShapeHelper shapeHelper;
        private PolygonHelper polygonHelper;

        public SaveNLoad(Canvas windowCanvas)
        {
            canvas = windowCanvas;
            
        }

        public void save()
        {
            string inputBox = Interaction.InputBox("Save file as..", "Save your canvas");
            FileStream fs = File.Open(inputBox + ".xaml", FileMode.Create);
            XamlWriter.Save(canvas, fs);
            fs.Close();
        }
        public void load()
        {
            try
            {
                shapeHelper = new ShapeHelper(canvas);
                polygonHelper = new PolygonHelper(canvas);
                canvas.Children.Clear();
                OpenFileDialog Fd = new OpenFileDialog();
                Fd.ShowDialog();
                string LoadedFileName = Fd.FileName;

                //Load the file
                FileStream Fs = new FileStream(@LoadedFileName, FileMode.Open);

                canvas.Children.Clear();
                Canvas newCanvas = System.Windows.Markup.XamlReader.Load(Fs) as Canvas;
                UIElement[] children = new UIElement[newCanvas.Children.Capacity];
                newCanvas.Children.CopyTo(children, 0);
                foreach (UIElement child in children)
                {
                    if (child != null)
                    {
                        newCanvas.Children.Remove(child);
                        canvas.Children.Add(child);
                        if (child is Rectangle || child is Ellipse)
                        {
                            child.MouseLeftButtonDown += shapeHelper.getPosition;
                            child.MouseRightButtonDown += shapeHelper.select;
                        }
                        else if (child is Polygon)
                        {
                            child.MouseLeftButtonDown += polygonHelper.getPosition;
                            child.MouseRightButtonDown += polygonHelper.select;
                        }
                    }
                }



                Fs.Close();
            }
            catch { }
        }
    }
}
