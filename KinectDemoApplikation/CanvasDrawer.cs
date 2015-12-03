using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

///
/// Program: Kinect Demo Application
/// Version: 1.0
/// Author: Sebastian Kotstein
/// Email: sebastian.kotstein@hpe.com
/// This demo application has been developed for the Luneta project sponsored by Hewlett Packard Enterprise and can be considered as a prototype.
/// This application contains code framgements which were originally developed by Vangos Pterneas and provided as an example. 
/// This example code has been modified or directly used within this application. 
/// The example code provided by Vangos Pterneas is licensed under The Code Project Open License (CPOL).
/// 
/// 
///
namespace KinectDemoApplikation
{
 
    /// <summary>
    /// Helper class which provides methods for drawing on the canvas of the main UI window
    /// </summary>
    public class CanvasDrawer
    {
        #region members

        /// <summary>
        /// reference to the main UI
        /// </summary>
        private Whiteboard ui;

        /// <summary>
        /// poperty container containing preselected colors which can be used for the skeleton and finger tracking drawing
        /// </summary>
        public List<Brush> Colors 
        { 
            get
            {
                return new List<Brush>()
                {
                    Brushes.Orange,
                    Brushes.Red,
                    Brushes.Green,
                    Brushes.Yellow,
                    Brushes.Blue,
                    Brushes.Coral
                };
            }
        }


        #endregion 

        /// <summary>
        /// default constructor with passed UI reference
        /// </summary>
        /// <param name="ui"></param>
        public CanvasDrawer(Whiteboard ui)
        {
            this.ui = ui;

        }

        /// <summary>
        /// Draws a point at the passed position in the passed color
        /// </summary>
        /// <param name="point">Position of the point</param>
        /// <param name="color">Color of the point</param>
        public void DrawPoint(Point point, Brush color)
        {

            if (point == null || double.IsInfinity(point.X) || double.IsInfinity(point.X)) { return; }

            //create circle
            Ellipse ellipse = new Ellipse
            {
                Width = 20,
                Height = 20,
                Fill = color

            };

            //set position of ellipse (point is the central point of the circle)
            Canvas.SetLeft(ellipse, point.X - ellipse.Width / 2);
            Canvas.SetTop(ellipse, point.Y - ellipse.Height / 2);

            //add point to canvas
            ui.canvas.Children.Add(ellipse);
        }

        /// <summary>
        /// Draws a line between the two passed points in the passed color
        /// </summary>
        /// <param name="a">Position of point A</param>
        /// <param name="b">Position of point B</param>
        /// <param name="color">Color of the line</param>
        public void DrawLine(Point a, Point b, Brush color)
        {

            if (b == null || b == null || double.IsInfinity(a.X) || double.IsInfinity(b.X) || double.IsInfinity(a.Y) || double.IsInfinity(b.Y)) { return; }

            //create line
            Line line = new Line
            {
                X1 = a.X,
                Y1 = a.Y,
                X2 = b.X,
                Y2 = b.Y,
                StrokeThickness = 8,
                Stroke = color
            };

            //add line to canvas
            ui.canvas.Children.Add(line);
        }

        /// <summary>
        /// Removes all elements from canvas
        /// </summary>
        public void Clear()
        {
            ui.canvas.Children.Clear();
        }

    }



}
