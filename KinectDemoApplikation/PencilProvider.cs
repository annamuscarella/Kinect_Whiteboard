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
    
   public class PencilProvider
    {
        /*Image pencil;
        Image marker;
        Image hand;
        Image eraser;
        Image grid_pencil;
        Image grid_eraser;
        Image grid_marker;*/

        public Brush brush;
        private Boolean DrawingEnabled;
        public Pencil cursor;
        private Pencil pencil, marker, hand, eraser;
        private Pencil[] pencils;
        Controller controller;

        //every pencil must be displayed in the grid (when window is initialized and user didn't use the pencil so far
        //and in large cavas (Name=Panel, same size as visual table) as well, so the pencil can be moved on the whole screen (not including header!!)

        public PencilProvider(Controller con) {
            this.controller = con;
            brush = new SolidColorBrush(Colors.Black);
            //initialize Images

            pencil = new Pencil(controller.Ui.Pencil, Colors.Gray, 2);
            pencil.SetOffset(0.5, -0.5);
            marker = new Pencil(controller.Ui.Marker, controller.Ui.Marker_Open, Color.FromArgb(100, 0, 0, 255), 10);
            marker.SetOffset(0.5, 0);
            eraser = new Pencil(controller.Ui.Eraser, Colors.White, 20);
            eraser.SetOffset(0.5, -0.5);
            hand = new Pencil(controller.Ui.handcursor, Colors.White);
            hand.DrawingEnabled = false;

            pencils = new Pencil[] {pencil, marker, eraser};
            int i = 0;
            foreach (Pencil p in pencils) {
                controller.Ui.Panel.Children.Add(p.CanvasImage);
                Canvas.SetLeft(p.CanvasImage, controller.Ui.Panel.ActualWidth / 9 * 8);
                Canvas.SetTop(p.CanvasImage, controller.Ui.Panel.ActualHeight / 3 * i);
                i++;
            }

            controller.Ui.Panel.Children.Add(hand.CanvasImage);
            Canvas.SetLeft(hand.CanvasImage, controller.Ui.Panel.ActualWidth / 9 * 2);
            Canvas.SetTop(hand.CanvasImage, 50);

            cursor = hand;
        }

        //function for changing the displayed cursor icon to hand image
        public void changeCursorToHand() {
            cursor = hand;
            hand.GrabPencil();
        }

        //function for changing the displayed cursor icon to pencil
        //change pencil color to gray
        //hide hand image and pencil image in grid
        public void changeCursorToPencil()
        {
            cursor = pencil;
            pencil.GrabPencil();
            hand.HideAllImages();
        }

        //function for changing the displayed cursor icon to marker
        //change marker color to blue
        //hide hand image and marker image in grid
        public void changeCursorToMarker()
        {
            cursor = marker;
            marker.GrabPencil(controller.Ui.Marker_Cover);
            hand.HideAllImages();
        }

        //function for changing the displayed cursor icon to eraser
        //change eraser color to white
        //TODO: how to erase points instead of drawing white lines?? --> if paper is switched to ColorFrame, white lines are visible
        //hide hand image and eraser image in grid
        public void changeCursorToEraser()
        {
            cursor = eraser;
            eraser.GrabPencil();
            hand.HideAllImages();
        }

        public void ChangeDrawingStateTo(Boolean DrawingEnabled)
        {
            if (cursor.DrawingEnabled)
            {
                this.DrawingEnabled = DrawingEnabled;
            }
            else {
                this.DrawingEnabled = false;
            }

        }

        public Boolean GetDrawingState() {
            return DrawingEnabled;
        }

        public void MoveCursorTo(Point newPoint, DependencyProperty canvas_horizontal, DependencyProperty canvas_vertical)
        {
            Point oldP = newPoint;
            if (canvas_horizontal == Canvas.LeftProperty)
            {
                oldP.X = Canvas.GetLeft(cursor.CanvasImage);
            }
            if (canvas_horizontal == Canvas.RightProperty)
            {
                oldP.X = Canvas.GetRight(cursor.CanvasImage);
            }
            if (canvas_vertical == Canvas.TopProperty) { oldP.Y = Canvas.GetTop(cursor.CanvasImage); }
            if (canvas_vertical == Canvas.BottomProperty) { oldP.Y = Canvas.GetBottom(cursor.CanvasImage); }



            DoubleAnimation anim1 = new DoubleAnimation(oldP.X, newPoint.X + cursor.GetOffsetX(), TimeSpan.FromSeconds(0.05));
            DoubleAnimation anim2 = new DoubleAnimation(oldP.Y, newPoint.Y + cursor.GetOffsetY(), TimeSpan.FromSeconds(0.05));

            cursor.CanvasImage.BeginAnimation(canvas_horizontal, anim1);
            cursor.CanvasImage.BeginAnimation(canvas_vertical, anim2);
        }

        /// <summary>
        /// Method for changing the Pencil to the nearest Pencil next to Cursor
        /// </summary>
        /// <param name="Max_Distance">Maximum Range where pencil is grabbed</param>

        public void ChangeCursorToNearestPencil(double Max_Distance) {

            Pencil nearest = null;
            double nearest_distance = Max_Distance;

            foreach (Pencil p in pencils)
            {
                if (cursor != p)
                {
                    //get difference between two pictures
                    double distance_x = Canvas.GetLeft(p.CanvasImage) - Canvas.GetLeft(cursor.CanvasImage);
                    distance_x = Math.Sqrt(distance_x * distance_x);

                    double distance_y = Canvas.GetTop(p.CanvasImage) - Canvas.GetTop(cursor.CanvasImage);
                    distance_y = Math.Sqrt(distance_y * distance_y);
                    double distance = Math.Sqrt(distance_x * distance_x + distance_y * distance_y);

                    if (nearest_distance >= distance)
                    {
                        nearest_distance = distance;
                        nearest = p;
                    }
                }
            }

            if (nearest == marker) {
                changeCursorToMarker();
            }
            if (nearest == pencil) {
                changeCursorToPencil();
            }
            if (nearest == eraser)
            {
                changeCursorToEraser();
            }
            if (nearest == null) { changeCursorToHand(); }
            

        }
    }
}
