using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace KinectDemoApplikation
{
    
    class PencilProvider
    {
        Image pencil;
        Image marker;
        Image hand;
        Image eraser;
        Image grid_pencil;
        Image grid_eraser;
        Image grid_marker;

        public Brush brush;

        public Image cursor;

        Controller controller;

        public PencilProvider(Controller con) {
            this.controller = con;
            brush = new SolidColorBrush(Colors.Black);
            //initialize Images
            grid_pencil = new Image();
            grid_eraser = new Image();
            grid_marker = new Image();

            pencil = new Image();
            marker = new Image(); 
            eraser = new Image();

            //get images from grid
            grid_pencil = controller.Ui.Pencil;
            grid_eraser = controller.Ui.Eraser;
            grid_marker = controller.Ui.Marker;
            hand = controller.Ui.cursor;

            //copy images from grid to canvas
            pencil.Source = grid_pencil.Source;
            pencil.Height = grid_pencil.ActualHeight;
            pencil.Width = grid_pencil.ActualWidth;

            marker.Source = grid_marker.Source;
            marker.Height = grid_marker.ActualHeight;
            marker.Width = grid_marker.ActualWidth;

            eraser.Source = grid_eraser.Source;
            eraser.Height = grid_eraser.ActualHeight;
            eraser.Width = grid_eraser.ActualWidth;

            //hide all canvas images
            pencil.Visibility = System.Windows.Visibility.Hidden;
            marker.Visibility = System.Windows.Visibility.Hidden;
            eraser.Visibility = System.Windows.Visibility.Hidden;

            //Add all pencils to canvas and set properties
            controller.Ui.Panel.Children.Add(pencil);
            Canvas.SetLeft(pencil, controller.Ui.Panel.ActualWidth);
            Canvas.SetTop(pencil, 1);

            controller.Ui.Panel.Children.Add(marker);
            Canvas.SetLeft(marker, controller.Ui.Panel.ActualWidth);
            Canvas.SetTop(marker, 1);

            controller.Ui.Panel.Children.Add(eraser);
            Canvas.SetLeft(eraser, controller.Ui.Panel.ActualWidth);
            Canvas.SetTop(eraser, 1);
        }

        public void changeCursorToHand() {
            cursor = hand;
            hand.Visibility = System.Windows.Visibility.Visible;
        }

        public void changeCursorToPencil()
        {
            cursor = pencil;
            brush = new SolidColorBrush(Colors.Gray);
            hand.Visibility = System.Windows.Visibility.Hidden;
            pencil.Visibility = System.Windows.Visibility.Visible;
            grid_pencil.Visibility = System.Windows.Visibility.Hidden;
        }
        public void changeCursorToMarker()
        {
            cursor = marker;
            brush = new SolidColorBrush(Colors.Blue);
            hand.Visibility = System.Windows.Visibility.Hidden;
            marker.Visibility = System.Windows.Visibility.Visible;
            grid_marker.Visibility = System.Windows.Visibility.Hidden;
        }
        public void changeCursorToEraser()
        {
            cursor = eraser;
            brush = new SolidColorBrush(Colors.White);
            hand.Visibility = System.Windows.Visibility.Hidden;
            eraser.Visibility = System.Windows.Visibility.Visible;
            grid_eraser.Visibility = System.Windows.Visibility.Hidden;
        }
    }
}
