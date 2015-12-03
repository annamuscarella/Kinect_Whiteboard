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
    public class InfraredFrameHandler : KinectHandler
    {
        /// <summary>
        /// Reader instance which reads incoming frames from the kinect (like an input stream)
        /// </summary>
        private InfraredFrameReader reader;

        /// <summary>
        /// Starts this handler
        /// </summary>
        /// <param name="c">controller class</param>
        /// <param name="sensor">kinect sensor</param>
        public override void Start(Controller c, KinectSensor sensor)
        {
            controller = c;
            if (sensor != null)
            {
                //load reader and register frame arrived delegate (listener)
                reader = sensor.InfraredFrameSource.OpenReader();
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
        private void Reader_FrameArrived(object sender, InfraredFrameArrivedEventArgs e)
        {
            //load infrared frame
            using (InfraredFrame frame = e.FrameReference.AcquireFrame())
            {
                //pass infrared frame for interpretation, only if this handler is enabled
                if (frame != null && Enabled)
                {
                    //controller.Ui.canvas.Source = ToBitmap(frame);
                }
            }
        }

        /// <summary>
        /// Interprets the passed infrared frame and returns an image source (bitmap) which can be displayed in the UI
        /// Code by Vangos Pterneas
        /// </summary>
        /// <param name="frame">depth frame</param>
        /// <returns>bitmap</returns>
        private ImageSource ToBitmap(InfraredFrame frame)
        {
            //width and height of the frame
            int width = frame.FrameDescription.Width;
            int height = frame.FrameDescription.Height;

            //create an array for the infrared data
            ushort[] infraredData = new ushort[width * height];

            //create an array which will hold the color information for each pixel (width * height) which should be converted to the bitmap (--> this array is for the output). 
            //Each pixel consists of a couple of bytes for storing rgb values (e.g. 1 Byte for each red green blue color) and aditional information like the alpha value
            //The amounts of bytes for each pixel depends on the PixelFormat. (PixelFormats.TYPE.BitsPerPixel + 7)/8 calculates the minimal number of required bytes.
            byte[] pixels = new byte[width * height * (PixelFormats.Bgr32.BitsPerPixel + 7) / 8];

            //convert the infrared information into the infrared data array
            frame.CopyFrameDataToArray(infraredData);

            //transform the depth data into a visible bitmap:
            int colorIndex = 0; //start at index 0 of the pixelData array
            //iterate over the whole infrared data
            for (int infraredIndex = 0; infraredIndex < infraredData.Length; ++infraredIndex)
            {
                //load infrared information for
                ushort ir = infraredData[infraredIndex];
                //transform infrared information into a visbile intensity (by bit shifting???)
                byte intensity = (byte)(ir >> 7);

                //store the intensity as rgb value in the color animated pixel data
                //since red, green and blue have the same values, there will be a grayscale
                pixels[colorIndex++] = (byte)(intensity / 0.2); // Blue
                pixels[colorIndex++] = (byte)(intensity / 0.2); // Green   
                pixels[colorIndex++] = (byte)(intensity / 0.2); // Red

                ++colorIndex; //skip alpha value (???)
            }

            //?
            int stride = width * PixelFormats.Bgr32.BitsPerPixel / 8;

            //create and return bitmap
            return BitmapSource.Create(width, height, 96, 96, PixelFormats.Bgr32, null, pixels, stride);
        }
    }
}
