using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

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
    public class SkeletonHandler : KinectHandler
    {
        /// <summary>
        /// Reader instance which reads incoming frames from the kinect (like an input stream)
        /// </summary>
        private BodyFrameReader reader;

        /// <summary>
        /// The coordinate mapper is used (in this context) to transform metrical body joint positions (x,y,z) in color pixel (x,y)
        /// </summary>
        private CoordinateMapper coordinateMapper;

        //reference to drawer helper class
        private CanvasDrawer drawer;

        //list of detected bodies
        private Body[] bodies;

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
                reader = sensor.BodyFrameSource.OpenReader();
                reader.FrameArrived += Reader_FrameArrived;

                coordinateMapper = sensor.CoordinateMapper;

                drawer = controller.Ui.CanvasDrawer;

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
        private void Reader_FrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
            bool dataReceived = false;

            //load body frame
            using (BodyFrame bodyFrame = e.FrameReference.AcquireFrame())
            {
                //TODO: Check whether this step is required or might be lead to malfunction, maybe the GetAndRefreshBodyData is already performing this job
                if (bodyFrame != null )
                {
                    //if bodies have been arrived, the array will be resetted and adapted to the size of captured bodies 
                    if (this.bodies == null)
                    {
                        this.bodies = new Body[bodyFrame.BodyCount];
                    }

                    // The first time GetAndRefreshBodyData is called, Kinect will allocate each Body in the array.
                    // As long as those body objects are not disposed and not set to null in the array,
                    // those body objects will be re-used.
                    //load body information to array and refresh the contained inforamtion
                    bodyFrame.GetAndRefreshBodyData(this.bodies);
                    dataReceived = true;
                }
            }
            //only if a frame has arrived
            if (dataReceived)
            {
                //clear screen
                //drawer.Clear();

                //for all bodies
                //for (int i = 0; i < bodies.Length; i++)
                //{
                    
                    //store all joints
                    IReadOnlyDictionary<JointType, Joint> joints = bodies[0].Joints;
                    //prepare dictionary for translated points
                    Dictionary<JointType, Point> jointPoints = new Dictionary<JointType,Point>();

                    //iterate over all joints
                    //foreach(JointType jointType in joints.Keys)
                    //{
                        // sometimes the depth(Z) of an inferred joint may show as negative
                        // clamp down to 0.1f to prevent coordinatemapper from returning (-Infinity, -Infinity)
                        CameraSpacePoint position = joints[JointType.HandRight].Position;
                        if (position.Z < 0)
                        {
                            position.Z = 0.1f;
                        }

                        //calculate point and add it
                        ColorSpacePoint mappedSpacePoint = this.coordinateMapper.MapCameraPointToColorSpace(position);

                        //adapt camera resolution to actual canvas resolution
                        double canvasWidth = controller.Ui.canvas.ActualWidth;
                        double canvasHeight = controller.Ui.canvas.ActualHeight;

                        double cameraWidth = 1920;
                        double cameraHeight = 1080;
                        
                        //store point into set of joint points
                        jointPoints[JointType.HandRight] = new Point((canvasWidth / cameraWidth ) * mappedSpacePoint.X,  (canvasHeight / cameraHeight ) * mappedSpacePoint.Y);


                //debug stuff
                // if ( == JointType.Head && joints[jointType].TrackingState == TrackingState.Tracked)
                //{
                //  controller.Ui.DisplayDebug(String.Format("[Camera: X: {0} | Y: {1} | Z: {2} ][Color: X: {3} | Y: {4} ] ",position.X, position.Y, position.Z, mappedSpacePoint.X, mappedSpacePoint.Y));
                //}

                // }

                //draw all joint points
                // foreach(Point point in jointPoints.Values)
                //{
                Image img = controller.Ui.cursor;
                try { MoveTo(img, (canvasWidth / cameraWidth) * mappedSpacePoint.X, (canvasHeight / cameraHeight) * mappedSpacePoint.Y); }
                catch (Exception d){  }

                //controller.Ui.cursor.PointFromScreen(jointPoints[JointType.HandRight]);
                
                    //}

                    
                    //draw lines between body joints
                    /*drawer.DrawLine(jointPoints[JointType.Head], jointPoints[JointType.Neck],drawer.Colors[i%6]);
                    drawer.DrawLine(jointPoints[JointType.Neck], jointPoints[JointType.SpineShoulder],drawer.Colors[i%6]);
                    drawer.DrawLine(jointPoints[JointType.SpineShoulder], jointPoints[JointType.ShoulderLeft],drawer.Colors[i%6]);
                    drawer.DrawLine(jointPoints[JointType.SpineShoulder], jointPoints[JointType.ShoulderRight],drawer.Colors[i%6]);
                    drawer.DrawLine(jointPoints[JointType.SpineShoulder], jointPoints[JointType.SpineMid],drawer.Colors[i%6]);
                    drawer.DrawLine(jointPoints[JointType.ShoulderLeft], jointPoints[JointType.ElbowLeft],drawer.Colors[i%6]);
                    drawer.DrawLine(jointPoints[JointType.ShoulderRight], jointPoints[JointType.ElbowRight],drawer.Colors[i%6]);
                    drawer.DrawLine(jointPoints[JointType.ElbowLeft], jointPoints[JointType.WristLeft],drawer.Colors[i%6]);
                    drawer.DrawLine(jointPoints[JointType.ElbowRight], jointPoints[JointType.WristRight],drawer.Colors[i%6]);
                    drawer.DrawLine(jointPoints[JointType.WristLeft], jointPoints[JointType.HandLeft],drawer.Colors[i%6]);
                    drawer.DrawLine(jointPoints[JointType.WristRight], jointPoints[JointType.HandRight],drawer.Colors[i%6]);
                    drawer.DrawLine(jointPoints[JointType.HandLeft], jointPoints[JointType.HandTipLeft],drawer.Colors[i%6]);
                    drawer.DrawLine(jointPoints[JointType.HandRight], jointPoints[JointType.HandTipRight],drawer.Colors[i%6]);
                    drawer.DrawLine(jointPoints[JointType.HandTipLeft], jointPoints[JointType.ThumbLeft],drawer.Colors[i%6]);
                    drawer.DrawLine(jointPoints[JointType.HandTipRight], jointPoints[JointType.ThumbRight],drawer.Colors[i%6]);
                    drawer.DrawLine(jointPoints[JointType.SpineMid], jointPoints[JointType.SpineBase],drawer.Colors[i%6]);
                    drawer.DrawLine(jointPoints[JointType.SpineBase], jointPoints[JointType.HipLeft],drawer.Colors[i%6]);
                    drawer.DrawLine(jointPoints[JointType.SpineBase], jointPoints[JointType.HipRight],drawer.Colors[i%6]);
                    drawer.DrawLine(jointPoints[JointType.HipLeft], jointPoints[JointType.KneeLeft],drawer.Colors[i%6]);
                    drawer.DrawLine(jointPoints[JointType.HipRight], jointPoints[JointType.KneeRight],drawer.Colors[i%6]);
                    drawer.DrawLine(jointPoints[JointType.KneeLeft], jointPoints[JointType.AnkleLeft],drawer.Colors[i%6]);
                    drawer.DrawLine(jointPoints[JointType.KneeRight], jointPoints[JointType.AnkleRight],drawer.Colors[i%6]);
                    drawer.DrawLine(jointPoints[JointType.AnkleLeft], jointPoints[JointType.FootLeft],drawer.Colors[i%6]);
                    drawer.DrawLine(jointPoints[JointType.AnkleRight], jointPoints[JointType.FootRight],drawer.Colors[i%6]);*/
                //}
            }
        }
        public static void MoveTo(System.Windows.Controls.Image target, double newX, double newY)
        {
            Point oldP = new Point(newX, newY);
            oldP.X = Canvas.GetLeft(target);
            oldP.Y = Canvas.GetTop(target);

            DoubleAnimation anim1 = new DoubleAnimation(oldP.X, newX, TimeSpan.FromSeconds(0.2));
            DoubleAnimation anim2 = new DoubleAnimation(oldP.Y, newY, TimeSpan.FromSeconds(0.2));

            target.BeginAnimation(Canvas.RightProperty, anim1);
            target.BeginAnimation(Canvas.TopProperty, anim2);

            /*Vector offset = VisualTreeHelper.GetOffset(target);
            var top = offset.Y;
            var left = offset.X;
            TranslateTransform trans = new TranslateTransform();
            target.RenderTransform = trans;
            DoubleAnimation anim1 = new DoubleAnimation(0, newY - top, TimeSpan.FromSeconds(10));
            DoubleAnimation anim2 = new DoubleAnimation(0, newX - left, TimeSpan.FromSeconds(10));
            trans.BeginAnimation(TranslateTransform.YProperty, anim1);
            trans.BeginAnimation(TranslateTransform.XProperty, anim2);*/
        }


    }
}
