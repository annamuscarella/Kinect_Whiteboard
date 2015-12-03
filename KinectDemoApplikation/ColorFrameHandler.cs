using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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
namespace KinectDemoApplikation
{

    /// <summary>
    /// This is class is responsible for receiving, interpreting and displaying incoming color frames which are caputered by the built-in Kinect color camera.
    /// </summary>
    public class ColorFrameHandler : KinectHandler
    {
        /// <summary>
        /// Reader instance which reads incoming frames from the kinect (like an input stream)
        /// </summary>
        private ColorFrameReader reader;
        

        /// <summary>
        /// Starts this handler
        /// </summary>
        /// <param name="c">controller class</param>
        /// <param name="sensor">kinect sensor</param>
        public override void Start(Controller c, KinectSensor sensor)
        {

            controller = c;
            if(sensor!= null)
            {
                //load reader and register frame arrived delegate (listener)
                reader = sensor.ColorFrameSource.OpenReader();
                reader.FrameArrived += Reader_FrameArrived;
            }
            
           

            
        }

        /// <summary>
        /// Stops this handler
        /// </summary>
        public override void Stop()
        {
            if (reader != null)
            {
                reader.Dispose();
            }
        }

        /// <summary>
        /// Delegate which will be called, if a frame arrived
        /// </summary>
        /// <param name="sender">source which has fired this event</param>
        /// <param name="e">event arguments containing the incoming frame</param>
        private void Reader_FrameArrived(object sender, ColorFrameArrivedEventArgs e)
        {
           

            //load color frame
            using (ColorFrame frame = e.FrameReference.AcquireFrame())
            {
                //pass color frame for interpretation, only if this handler is enabled
                if (frame != null && Enabled)
                {
                    controller.Ui.color_background.Source = ToBitmap(frame);
                }
            }
        }

        /// <summary>
        /// Interprets the passed color frame and returns an image source (bitmap) which can be displayed in the UI
        /// Code by Vangos Pterneas
        /// </summary>
        /// <param name="frame">color frame</param>
        /// <returns>bitmap</returns>
        private ImageSource ToBitmap(ColorFrame frame)
        {
            //width and height of the color frame (should be 1920 x 1080)
            int width = frame.FrameDescription.Width;
            int height = frame.FrameDescription.Height;

            //create an array which will hold the color information for each pixel (width * height) which should be converted to the bitmap (--> this array is for the output). 
            //Each pixel consists of a couple of bytes for storing rgb values (e.g. 1 Byte for each red green blue color) and aditional information like the alpha value
            //The amounts of bytes for each pixel depends on the PixelFormat. (PixelFormats.TYPE.BitsPerPixel + 7)/8 calculates the minimal number of required bytes.

            byte[] pixels = new byte[width * height * ((PixelFormats.Bgr32.BitsPerPixel + 7) / 8)];

            //convert frame and store data into array
            if (frame.RawColorImageFormat == ColorImageFormat.Bgra)
            {
                frame.CopyRawFrameDataToArray(pixels);
            }
            else
            {
                frame.CopyConvertedFrameDataToArray(pixels, ColorImageFormat.Bgra);
            }

            //?
            int stride = width * PixelFormats.Bgra32.BitsPerPixel / 8;

            //create and return bitmap
            return BitmapSource.Create(width, height, 96, 96, PixelFormats.Bgra32, null, pixels, stride);

        }

    }
}
