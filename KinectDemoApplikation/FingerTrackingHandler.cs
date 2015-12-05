using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
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
    public class FingerTrackingHandler : KinectHandler
    {
        /// <summary>
        /// Reader instance which reads incoming frames from the kinect (like an input stream)
        /// </summary>
        private BodyFrameReader reader;

        /// <summary>
        /// Controller instance used for storing the current controller
        /// </summary>
        Controller controller;

        /// <summary>
        /// The coordinate mapper is used (in this context) to transform metrical body joint positions (x,y,z) in color pixel (x,y)
        /// </summary>
        private CoordinateMapper coordinateMapper;

        //reference to drawer helper class
        private CanvasDrawer drawer;

        //reference to PencilProvider class
        PencilProvider provider;

        Boolean DrawingEnabled;

        //list of detected bodies
        private Body[] bodies;

        private double offset_x, offset_y;


        //Dictionary (HashMap) which associates the last detected right finger joint (value) with the tracking ID of the body (key)
        private Dictionary<ulong, Joint> lastFingerJoint = new Dictionary<ulong, Joint>();

        /// <summary>
        /// Starts this handler
        /// </summary>
        /// <param name="c">controller class</param>
        /// <param name="sensor">kinect sensor</param>
        public override void Start(Controller c, KinectSensor sensor)
        {
            controller = c;
            provider = new PencilProvider(controller);
            if (sensor != null)
            {
                //load reader and register frame arrived delegate (listener)
                reader = sensor.BodyFrameSource.OpenReader();
                reader.FrameArrived += Reader_FrameArrived;

                coordinateMapper = sensor.CoordinateMapper;

                drawer = controller.Ui.CanvasDrawer;

                DrawingEnabled = false;
                /*controller.Ui.Pencil_holder.Children.Add(controller.Ui.Pencil);
                controller.Ui.Pencil.SetCurrentValue(Grid.ColumnProperty, 0);
                controller.Ui.Pencil.SetCurrentValue(Grid.RowProperty, 1);*/

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
                if (bodyFrame != null && Enabled)
                {
                    //TODO: Check whether this step is required or might be lead to malfunction, maybe the GetAndRefreshBodyData is already performing this job
                    if (this.bodies == null)
                    {
                        //if bodies have been arrived, the array will be resetted and adapted to the size of captured bodies 
                        this.bodies = new Body[bodyFrame.BodyCount];
                    }

                    //load body information to array and refresh the contained inforamtion
                    bodyFrame.GetAndRefreshBodyData(this.bodies);
                    dataReceived = true;
                }
            }
            //only if a frame has arrived
            if (dataReceived)
            {
                //for each stored body
                for (int i = 0; i < bodies.Length; i++)
                {
                    //load body
                    Body body = bodies[i];

                    //update all gesture handlers
                    if (body.TrackingId != 0)
                    {
                        //fire update
                        controller.Update(body);
                    }


                    //Remove old finger joint position, only if the hand is not closed!!! TODO: consider confidence, there are more states besides HandState.Closed
                    if (body.HandRightState != HandState.Closed && body.HandRightState != HandState.Open)
                    {
                        //only if the finger was already tracked
                        if (lastFingerJoint.ContainsKey(body.TrackingId))
                        {
                            //remove...
                            lastFingerJoint.Remove(body.TrackingId);
                            //DoStuff(body);
                            //continue; //go to next iteration
                        }

                    }

                    //if an old finger joint has been tracked before....
                    if (lastFingerJoint.ContainsKey(body.TrackingId))
                    {
                        //Draw Line between the old and the new Finger joint:

                        //load old and new Joint position
                        Joint oldJoint = lastFingerJoint[body.TrackingId];
                        Joint newJoint = body.Joints[JointType.HandTipRight];

                        // sometimes the depth(Z) of an inferred joint may show as negative
                        // clamp down to 0.1f to prevent coordinatemapper from returning (-Infinity, -Infinity)
                        CameraSpacePoint oldPosition = oldJoint.Position;
                        if (oldPosition.Z < 0)
                        {
                            oldPosition.Z = 0.1f;
                        }

                        //calculate point and add it
                        ColorSpacePoint mappedSpacePoint = this.coordinateMapper.MapCameraPointToColorSpace(oldPosition);

                        //adapt camera resolution to actual canvas resolution
                        double canvasWidth = controller.Ui.Panel.ActualWidth;
                        double canvasHeight = controller.Ui.Panel.ActualHeight;

                        double cameraWidth = 1920;
                        double cameraHeight = 1080;

                        //adapt old finger point
                        Point oldPoint = new Point((canvasWidth / cameraWidth) * mappedSpacePoint.X, (canvasHeight / cameraHeight) * mappedSpacePoint.Y);





                        // sometimes the depth(Z) of an inferred joint may show as negative
                        // clamp down to 0.1f to prevent coordinatemapper from returning (-Infinity, -Infinity)
                        CameraSpacePoint newPosition = newJoint.Position;
                        if (newPosition.Z < 0)
                        {
                            newPosition.Z = 0.1f;
                        }

                        //calculate point and add it
                        mappedSpacePoint = this.coordinateMapper.MapCameraPointToColorSpace(newPosition);
                        Point newPoint = new Point((canvasWidth / cameraWidth) * mappedSpacePoint.X, (canvasHeight / cameraHeight) * mappedSpacePoint.Y);
                        try { provider.MoveTo(provider.cursor, (canvasWidth / cameraWidth) * mappedSpacePoint.X + offset_x, (canvasHeight / cameraHeight) * mappedSpacePoint.Y - offset_y, Canvas.LeftProperty, Canvas.TopProperty); }
                        catch (Exception d) { Debug.WriteLine("Exception thrown because not handled"); }


                        if (DrawingEnabled) {
                            if (newPoint.X > controller.Ui.canvas.ActualWidth / 100 && newPoint.X < controller.Ui.canvas.ActualWidth - controller.Ui.canvas.ActualWidth / 100
                                && newPoint.Y > controller.Ui.canvas.ActualHeight / 100 && newPoint.Y < controller.Ui.canvas.ActualHeight - controller.Ui.canvas.ActualHeight / 100)
                            {
                                drawer.DrawLine(oldPoint, newPoint, provider.brush);
                            }
                        }
                        //adapt new finger point
                        /*if (body.HandRightState != HandState.Closed) {

                            provider.changeCursorToHand();
                            try { MoveTo(provider.cursor, (canvasWidth / cameraWidth) * mappedSpacePoint.X, (canvasHeight / cameraHeight) * mappedSpacePoint.Y, Canvas.LeftProperty, Canvas.TopProperty); }
                            catch (Exception d) { }
                        }
                    if (body.HandRightState == HandState.Closed)
                    {
                            provider.changeCursorToMarker();
                            

                            //img.SetCurrentValue(Canvas., 1);
                            //img.SetCurrentValue(Canvas.TopProperty, 1);


                            double offset_x = 1.7 * provider.cursor.ActualWidth ;
                            double offset_y = 0.25 * provider.cursor.ActualHeight ;

                            try { MoveTo(provider.cursor, (canvasWidth / cameraWidth) * mappedSpacePoint.X + offset_x, (canvasHeight / cameraHeight) * mappedSpacePoint.Y - offset_y, Canvas.LeftProperty, Canvas.TopProperty); }
                            catch (Exception d) { }
                            if (newPoint.X > controller.Ui.canvas.ActualWidth / 100 && newPoint.X < controller.Ui.canvas.ActualWidth - controller.Ui.canvas.ActualWidth/100
                                && newPoint.Y > controller.Ui.canvas.ActualHeight / 100 && newPoint.Y < controller.Ui.canvas.ActualHeight - controller.Ui.canvas.ActualHeight / 100)
                            {
                                drawer.DrawLine(oldPoint, newPoint, provider.brush);
                            }
                    }*/
                        //debug stuff
                        controller.Ui.DisplayDebug(String.Format("[Camera: X: {0} | Y: {1} | Z: {2} ][Color: X: {3} | Y: {4} ] ", oldPosition.X, oldPosition.Y, oldPosition.Z, mappedSpacePoint.X, mappedSpacePoint.Y));

                        //draw line between those two adapted poits (Color should be dependend on tracking id)
                        //drawer.DrawLine(oldPoint, newPoint, drawer.Colors[(int)body.TrackingId % 6]);

                    }
                    //lastFingerJoint.Add(body.TrackingId, body.Joints[JointType.HandTipRight]);
                    lastFingerJoint[body.TrackingId] = body.Joints[JointType.HandTipRight];
                }
            }
        }

        public void Gesture_RightHandClosedGesture(object sender, EventArgs e)
        {
            Console.WriteLine("Right Hand Closed");
            provider.changeCursorToMarker();
            //offset_x = 0.5* provider.cursor.ActualWidth;
            offset_x = 0;
            //offset_y = -0.5 * provider.cursor.ActualHeight;
            offset_y = 0;
            DrawingEnabled = true;
        }

        public void Gesture_RightHandOpenGesture(object sender, EventArgs e)
        {
            Console.WriteLine("Right Hand Opened");
            provider.changeCursorToHand();
            DrawingEnabled = false;
            offset_x =0;
            offset_y =0;
        }

        public void Gesture_RightHandQuicklyClosedGesture(object sender, EventArgs e)
        {
            Console.WriteLine("Right Hand Quickly Closed");

        }

        public void Gesture_RightHandQuicklyOpenGesture(object sender, EventArgs e)
        {
            Console.WriteLine("Right Hand Quickly Openend");

        }


        


    }
}
