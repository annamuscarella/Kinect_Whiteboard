using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace KinectDemoApplikation
{
   public class Pencil
    {
        public Image GridImage, GridCoverImage;
        public readonly Image CanvasImage;
        private Color color;
        private Brush brush;
        public readonly double thickness;
        public Boolean DrawingEnabled;
        private double Offset_X = 0, Offset_Y = 0;
        /// <summary>
        /// Constructor with default thickness 8 and same Image in Canvas
        /// </summary>
        /// <param name="ImageInGrid">Image displayed in Grid</param>
        /// <param name="pencil_color">Color of Pencil Brush</param>
        public Pencil(Image ImageInGrid, Color pencil_color)
        {
            GridImage = ImageInGrid;
            CanvasImage = new Image();
            CanvasImage.Source = GridImage.Source;
            CanvasImage.Height = GridImage.ActualHeight;
            CanvasImage.Width = GridImage.ActualWidth;
            changeColor(pencil_color.R, pencil_color.G, pencil_color.B, pencil_color.A);
            thickness = 8;
            CanvasImage.Visibility = System.Windows.Visibility.Hidden;
            DrawingEnabled = true;
        }

 

        /// <summary>
        /// Costructor with custom pencil thickness
        /// </summary>
        /// <param name="ImageInGrid">Image displayed in Grid</param>
        /// <param name="pencil_color">Color of Pencil Brush</param>
        /// <param name="PencilThickness">Pencil Thickness</param>
        public Pencil(Image ImageInGrid, Color pencil_color, double PencilThickness)
        {
            GridImage = ImageInGrid;
            CanvasImage = new Image();
            CanvasImage.Source = GridImage.Source;
            CanvasImage.Height = GridImage.ActualHeight;
            CanvasImage.Width = GridImage.ActualWidth;
            changeColor(pencil_color.R, pencil_color.G, pencil_color.B, pencil_color.A);
            thickness = PencilThickness;
            CanvasImage.Visibility = System.Windows.Visibility.Hidden;
            DrawingEnabled = true;
        }
        /// <summary>
        /// Costructor for using a different Image in canvas
        /// </summary>
        /// <param name="ImageInGrid">Image displayed in Grid</param>
        /// <param name="ImageInCanvas">Image displayed in Canvas</param>
        /// <param name="pencil_color">Color of Pencil Brush</param>
        /// <param name="PencilThickness">Pencil Thickness</param>
        public Pencil(Image ImageInGrid, Image ImageInCanvas, Color pencil_color, double PencilThickness)
        {
            GridImage = ImageInGrid;
            CanvasImage = new Image();
            CanvasImage.Source = ImageInCanvas.Source;
            CanvasImage.Height = ImageInCanvas.ActualHeight;
            CanvasImage.Width = ImageInCanvas.ActualWidth;
            changeColor( pencil_color.R, pencil_color.G, pencil_color.B, pencil_color.A);
            thickness = PencilThickness;
            CanvasImage.Visibility = System.Windows.Visibility.Hidden;
            DrawingEnabled = true;
        }

        /// <summary>
        /// Change Pencil Color in ARGB-Color
        /// </summary>
        /// <param name="r">Red value</param>
        /// <param name="g">Green value</param>
        /// <param name="b">Blue Value</param>
        /// <param name="a">offset</param>
        public void changeColor(byte r, byte g, byte b, byte a) {
            color = Color.FromArgb(a, r, g, b);
            brush = new SolidColorBrush(color);
        }
        public Brush getBrush() {
            return this.brush;
        }
        public void SetOffset(double X, double Y)
        {
            Offset_X = X;
            Offset_Y = Y;
        }

        public double GetOffsetX()
        {
            return Offset_X * CanvasImage.ActualWidth;
        }

        public double GetOffsetY()
        {
            return Offset_Y * CanvasImage.ActualHeight;
        }
        /*public void ShowCanvasImage() {
            this.CanvasImage.Visibility = System.Windows.Visibility.Visible;
            this.GridImage.Visibility = System.Windows.Visibility.Hidden;
        }*/
        public void HideAllImages() {
            this.CanvasImage.Visibility = System.Windows.Visibility.Hidden;
            this.GridImage.Visibility = System.Windows.Visibility.Hidden;
        }
        /// <summary>
        /// Grab Pencil
        /// </summary>
        public void GrabPencil() {
            this.CanvasImage.Visibility = System.Windows.Visibility.Visible;
            this.GridImage.Visibility = System.Windows.Visibility.Hidden;
        }
        /// <summary>
        /// Grab pencil and display cover in grid
        /// </summary>
        /// <param name="PencilCoverPicture">Picture showing pencil's cover</param>
        public void GrabPencil(Image PencilCoverPicture)
        {
            GridCoverImage = PencilCoverPicture;
            this.CanvasImage.Visibility = System.Windows.Visibility.Visible;
            this.GridCoverImage.Visibility = System.Windows.Visibility.Visible;
            this.GridImage.Visibility = System.Windows.Visibility.Hidden;
        }

    }
}
