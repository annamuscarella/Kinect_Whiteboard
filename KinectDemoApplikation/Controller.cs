using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
    /// Central class for coordinating all functions of the application considering the MVC pattern
    /// </summary>
    public class Controller
    {

        /// <summary>
        /// Property for the main UI window (View)
        /// </summary>
        public Whiteboard Ui { get; set; }

        /// <summary>
        /// Color Frame Handler
        /// </summary>
        private KinectHandler colorFrameHandler;


        /// <summary>
        /// Paper UIElements List
        /// </summary>
        public List<UIElement> PaperElements;

        /// <summary>
        /// Infrared Frame Handler
        /// </summary>
        private KinectHandler infraredFrameHandler;

        /// <summary>
        /// Skeleton Frame Handler
        /// </summary>
        private SkeletonHandler skeletonHandler;

        /// <summary>
        /// Finger Tracking Handler
        /// </summary>
        private FingerTrackingHandler fingerTrackingHandler;

        /// <summary>
        /// Kinect sensor reference
        /// </summary>
        private KinectSensor sensor;

        IGestureHandler _gestureHandler = new APIController();

        /// <summary>
        /// Default construtor
        /// </summary>
        /// <param name="ui">back reference to UI</param>
        public Controller(Whiteboard ui)
        {
            Ui = ui;

            
        }

        /// <summary>
        /// Starts the controller and the main part of the application
        /// </summary>
        public void Start()
        {
            //load sensor
            sensor = KinectSensor.GetDefault();

            if (sensor != null)
            {
                sensor.Open();
            }

            //add gesture Handlers
            _gestureHandler.SetRightHandClosed(Gesture_RightHandClosedGesture);
            _gestureHandler.SetRightHandOpen(Gesture_RightHandOpenGesture);
            _gestureHandler.SetRightHandQuicklyClosed(Gesture_RightHandQuicklyClosedGesture);
            _gestureHandler.setRightHandQuicklyOpen(Gesture_RightHandQuicklyOpenGesture);

            //create Handler objects
            this.colorFrameHandler = new ColorFrameHandler();
            this.infraredFrameHandler = new InfraredFrameHandler();
            this.skeletonHandler = new SkeletonHandler();
            this.fingerTrackingHandler = new FingerTrackingHandler();

            //start handler
            this.colorFrameHandler.Start(this, sensor);
            this.infraredFrameHandler.Start(this, sensor);
            this.skeletonHandler.Start(this, sensor);
            this.fingerTrackingHandler.Start(this, sensor);

            //enables only the color handler
            this.colorFrameHandler.Enabled = true;
            this.skeletonHandler.Enabled = false;
            this.fingerTrackingHandler.Enabled = false;
            DisplayStatus();

            //Add all elements for drwaing paper to a list
            PaperElements = new List<UIElement>();
            PaperElements.Add(this.Ui.Papier);
            PaperElements.Add(this.Ui.Spirale);
            PaperElements.Add(this.Ui.Ecke_rechts_oben);
            PaperElements.Add(this.Ui.Seite_rechts);
            PaperElements.Add(this.Ui.Ecke_rechts_unten);
            PaperElements.Add(this.Ui.Kante_unten);

            this.Ui.color_background.Visibility = System.Windows.Visibility.Hidden;

        }

        /// <summary>
        /// Stops the controller and all associated handlers
        /// </summary>
        public void Stop()
        {
            if (sensor != null)
            {
                sensor.Close();
            }

            this.colorFrameHandler.Stop();

            this.infraredFrameHandler.Stop();
            this.skeletonHandler.Stop();
            this.fingerTrackingHandler.Stop();
        }

        /// <summary>
        /// Enables the color handler and disable all other handlers
        /// </summary>
        public void EnableColor()
        {
            colorFrameHandler.Enabled = true;
            this.infraredFrameHandler.Enabled = false;  
            DisplayStatus();
        }

        public void DisableColor()
        {
            colorFrameHandler.Enabled = false;
            this.infraredFrameHandler.Enabled = false;
            DisplayStatus();
        }




        /// <summary>
        /// Enables the infrared handler and disable all other handlers
        /// </summary>
        public void EnableInfrared()
        {
            this.colorFrameHandler.Enabled = false;
            this.infraredFrameHandler.Enabled = true;
            DisplayStatus();
        }

        /// <summary>
        /// Changes into skeleton mode
        /// </summary>
        public void ChangeSkeletonMode()
        {
            this.skeletonHandler.Enabled = !this.skeletonHandler.Enabled;
            DisplayStatus();
        }

        /// <summary>
        /// Changes into finger tracking mode
        /// </summary>
        public void ChangeFingerTrackingMode()
        {
            this.fingerTrackingHandler.Enabled = !this.fingerTrackingHandler.Enabled;
            DisplayStatus();
        }

        /// <summary>
        /// Displays the current modes
        /// </summary>
        private void DisplayStatus(){
            String msg = "";

            if (this.colorFrameHandler.Enabled)
            {
                msg += "Color Mode ";
            }
            if (this.infraredFrameHandler.Enabled)
            {
                msg += "Infrared Mode ";
            }
            if (this.skeletonHandler.Enabled)
            {
                msg += "| Skeleton Tracking ON ";
            }
            else
            {
                msg += "| Skeleton Tracking OFF ";
            }
            if (this.fingerTrackingHandler.Enabled)
            {
                msg += "| Finger Tracking ON ";
            }
            else
            {
                msg += "| Finger Tracking OFF ";
            }

            Ui.DisplayStatus(msg);

        }

        /// <summary>
        /// methods are called when gesture is recognized (fired)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void Gesture_RightHandClosedGesture(object sender, EventArgs e)
        {
            Console.WriteLine("Right Hand Closed");
        }

        private void Gesture_RightHandOpenGesture(object sender, EventArgs e)
        {
            Console.WriteLine("Right Hand Opened");
        }

        private void Gesture_RightHandQuicklyClosedGesture(object sender, EventArgs e)
        {
            Console.WriteLine("Right Hand Quickly Closed");
        }

        private void Gesture_RightHandQuicklyOpenGesture(object sender, EventArgs e)
        {
            Console.WriteLine("Right Hand Quickly Openend");
        }



        

    }
}
