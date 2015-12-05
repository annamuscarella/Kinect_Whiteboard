using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

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

        //every pencil must be displayed in the grid (when window is initialized and user didn't use the pencil so far
        //and in large cavas (Name=Panel, same size as visual table) as well, so the pencil can be moved on the whole screen (not including header!!)

        public PencilProvider(Controller con) {
            this.controller = con;
            brush = new SolidColorBrush(Colors.Black);
            //initialize Images
            grid_pencil = new Image();
            grid_eraser = new Image();
            grid_marker = new Image();

            pencil = new Image();
            this.marker = new Image(); 
            eraser = new Image();

            //get images from grid on the right side of paper
            grid_pencil = controller.Ui.Pencil;
            grid_eraser = controller.Ui.Eraser;
            grid_marker = controller.Ui.Marker;
            hand = controller.Ui.cursor;

            //copy images from grid to canvas (for each copy source and size)
            pencil.Source = grid_pencil.Source;
            pencil.Height = grid_pencil.ActualHeight;
            pencil.Width = grid_pencil.ActualWidth;

            marker.Source = grid_marker.Source;
            marker.Height = grid_marker.ActualHeight;
            marker.Width = grid_marker.ActualWidth;

            eraser.Source = grid_eraser.Source;
            eraser.Height = grid_eraser.ActualHeight;
            eraser.Width = grid_eraser.ActualWidth;

            //hide all canvas images, as they should not be visible until user grabs the pen for the first time
            pencil.Visibility = System.Windows.Visibility.Hidden;
            marker.Visibility = System.Windows.Visibility.Hidden;
            eraser.Visibility = System.Windows.Visibility.Hidden;

            //Add all pencils to canvas Panel (large canvas) and set properties (all must be left for the MoveTo function to work)
            controller.Ui.Panel.Children.Add(pencil);
            Canvas.SetLeft(pencil, controller.Ui.Panel.ActualWidth);
            Canvas.SetTop(pencil, 1);

            controller.Ui.Panel.Children.Add(marker);
            Canvas.SetLeft(marker, controller.Ui.Panel.ActualWidth);
            Canvas.SetTop(marker, 1);

            controller.Ui.Panel.Children.Add(eraser);
            Canvas.SetLeft(eraser, controller.Ui.Panel.ActualWidth);
            Canvas.SetTop(eraser, 1);

            cursor = hand;
        }

        //function for changing the displayed cursor icon to hand image
        public void changeCursorToHand() {
            cursor = hand;
            hand.Visibility = System.Windows.Visibility.Visible;
        }

        //function for changing the displayed cursor icon to pencil
        //change pencil color to gray
        //hide hand image and pencil image in grid
        public void changeCursorToPencil()
        {
            cursor = pencil;
            brush = new SolidColorBrush(Colors.Gray);
            hand.Visibility = System.Windows.Visibility.Hidden;
            pencil.Visibility = System.Windows.Visibility.Visible;
            grid_pencil.Visibility = System.Windows.Visibility.Hidden;
        }

        //function for changing the displayed cursor icon to marker
        //change marker color to blue
        //hide hand image and marker image in grid
        public void changeCursorToMarker()
        {
            cursor = marker;
            brush = new SolidColorBrush(Colors.Blue);
            hand.Visibility = System.Windows.Visibility.Hidden;
            marker.Visibility = System.Windows.Visibility.Visible;
            grid_marker.Visibility = System.Windows.Visibility.Hidden;
        }

        //function for changing the displayed cursor icon to eraser
        //change eraser color to white
        //TODO: how to erase points instead of drawing white lines?? --> if paper is switched to ColorFrame, white lines are visible
        //hide hand image and eraser image in grid
        public void changeCursorToEraser()
        {
            cursor = eraser;
            brush = new SolidColorBrush(Colors.White);
            hand.Visibility = System.Windows.Visibility.Hidden;
            eraser.Visibility = System.Windows.Visibility.Visible;
            grid_eraser.Visibility = System.Windows.Visibility.Hidden;
        }

        public void MoveTo(System.Windows.Controls.Image target, double newX, double newY, DependencyProperty canvas_horizontal, DependencyProperty canvas_vertical)
        {
            Point oldP = new Point(newX, newY);
            if (canvas_horizontal == Canvas.LeftProperty)
            {
                oldP.X = Canvas.GetLeft(target);
            }
            if (canvas_horizontal == Canvas.RightProperty)
            {
                oldP.X = Canvas.GetRight(target);
            }
            if (canvas_vertical == Canvas.TopProperty) { oldP.Y = Canvas.GetTop(target); }
            if (canvas_vertical == Canvas.BottomProperty) { oldP.Y = Canvas.GetBottom(target); }



            DoubleAnimation anim1 = new DoubleAnimation(oldP.X, newX, TimeSpan.FromSeconds(0.05));
            DoubleAnimation anim2 = new DoubleAnimation(oldP.Y, newY, TimeSpan.FromSeconds(0.05));

            target.BeginAnimation(canvas_horizontal, anim1);
            target.BeginAnimation(canvas_vertical, anim2);
        }
    }
}
