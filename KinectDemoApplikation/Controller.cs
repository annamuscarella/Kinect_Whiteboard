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
        /// Finger Tracking Handler
        /// </summary>
        private FingerTrackingHandler fingerTrackingHandler;

        /// <summary>
        /// Kinect sensor reference
        /// </summary>
        private KinectSensor sensor;

        IGestureHandler _gestureHandler;

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


            _gestureHandler = new APIController();

            
            //create Handler objects
            this.colorFrameHandler = new ColorFrameHandler();
            this.fingerTrackingHandler = new FingerTrackingHandler();

            //start handler
            this.colorFrameHandler.Start(this, sensor);
            this.fingerTrackingHandler.Start(this, sensor);

            //enables only the color handler
            this.colorFrameHandler.Enabled = true;
            this.fingerTrackingHandler.Enabled = false;
            DisplayStatus();

            //add gesture Handlers
            _gestureHandler.SetRightHandClosed(fingerTrackingHandler.Gesture_RightHandClosedGesture);
            _gestureHandler.SetRightHandOpen(fingerTrackingHandler.Gesture_RightHandOpenGesture);
            _gestureHandler.SetRightHandQuicklyClosed(fingerTrackingHandler.Gesture_RightHandQuicklyClosedGesture);
            _gestureHandler.setRightHandQuicklyOpen(fingerTrackingHandler.Gesture_RightHandQuicklyOpenGesture);

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

        public void Update(Body body) {
            _gestureHandler.Update(body);
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
            this.fingerTrackingHandler.Stop();
        }

        /// <summary>
        /// Enables the color handler and disable all other handlers
        /// </summary>
        public void EnableColor()
        {
            colorFrameHandler.Enabled = true;
            DisplayStatus();
        }

        public void DisableColor()
        {
            colorFrameHandler.Enabled = false;
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


    }
}
