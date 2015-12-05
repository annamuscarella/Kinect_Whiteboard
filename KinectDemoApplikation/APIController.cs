using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectDemoApplikation
{
    class APIController : IGestureHandler
    {
        //gestures
        public Gesture gestureClosed;
        public Gesture gestureOpen;
        public Gesture gestureQuicklyClosed1;
        public Gesture gestureQuicklyClosed2;
        public Gesture gestureQuicklyClosed3;
        public Gesture gestureQuicklyClosed4;
        public Gesture gestureQuicklyClosed5;
        public Gesture gestureQuicklyClosed6;
        public Gesture gestureQuicklyClosed7;
        public Gesture gestureQuicklyOpen;

        public delegate void EventHandlerMethod(object sender, EventArgs e);

        public APIController()

        {
            Gesture.open = new GesturePart(Microsoft.Kinect.HandState.Open);
            Gesture.closed = new GesturePart(Microsoft.Kinect.HandState.Closed);
            this.Start();
      
        }

        public void Start() {

            #region IGesturePartEnums

            /// <summary>
            /// enums for storing the gesture patterns
            /// </summary>
            IGesturePart[] RightHandQuicklyClosed1 = new IGesturePart[]
            {
            Gesture.open, Gesture.closed, Gesture.open
            };
            IGesturePart[] RightHandQuicklyClosed2 = new IGesturePart[]
            {
            Gesture.open, Gesture.closed,Gesture.closed, Gesture.open
            };
            IGesturePart[] RightHandQuicklyClosed3 = new IGesturePart[]
            {
            Gesture.open, Gesture.closed,Gesture.closed, Gesture.closed, Gesture.open
            };
            IGesturePart[] RightHandQuicklyClosed4 = new IGesturePart[]
            {
            Gesture.open, Gesture.closed,Gesture.closed, Gesture.closed, Gesture.closed, Gesture.open
            };
            IGesturePart[] RightHandQuicklyClosed5 = new IGesturePart[]
            {
            Gesture.open, Gesture.closed,Gesture.closed, Gesture.closed, Gesture.closed, Gesture.closed, Gesture.open
            };
            IGesturePart[] RightHandQuicklyClosed6 = new IGesturePart[]
            {
            Gesture.open, Gesture.closed,Gesture.closed, Gesture.closed, Gesture.closed,Gesture.closed,Gesture.closed, Gesture.open
            };
            IGesturePart[] RightHandQuicklyClosed7 = new IGesturePart[]
            {
            Gesture.open, Gesture.closed,Gesture.closed, Gesture.closed, Gesture.closed,Gesture.closed,Gesture.closed,Gesture.closed, Gesture.open
            };

            IGesturePart[] RightHandQuicklyOpened = new IGesturePart[]
                {
                Gesture.closed, Gesture.open, Gesture.closed
                };
            IGesturePart[] RightHandOpen = new IGesturePart[]
                {
                //1x closed
                //Gesture.closed,
                //15x open
                Gesture.open, Gesture.open, Gesture.open, Gesture.open, Gesture.open,
                Gesture.open, Gesture.open, //Gesture.open, Gesture.open, Gesture.open,
               // Gesture.open, Gesture.open, Gesture.open, Gesture.open, Gesture.open
                };
            IGesturePart[] RightHandClosed = new IGesturePart[]
                {
                //1x open
                //Gesture.open,
                //15x closed
                Gesture.open, Gesture.closed, Gesture.closed, Gesture.closed, Gesture.closed, Gesture.closed,
                Gesture.closed, Gesture.closed, //Gesture.closed, Gesture.closed, Gesture.closed,
                //Gesture.closed, Gesture.closed, Gesture.closed, Gesture.closed, Gesture.closed
                //Gesture.closed, Gesture.closed, Gesture.closed, Gesture.closed, Gesture.closed
                };

            #endregion

            //prepare gesture
            gestureClosed = new Gesture(RightHandClosed);
            gestureClosed.name = "gestureClosed";
            gestureOpen = new Gesture(RightHandOpen);
            gestureOpen.name = "gestureOpen";
            gestureQuicklyClosed1 = new Gesture(RightHandQuicklyClosed1);
            gestureQuicklyClosed2 = new Gesture(RightHandQuicklyClosed2);
            gestureQuicklyClosed3 = new Gesture(RightHandQuicklyClosed3);
            gestureQuicklyClosed4 = new Gesture(RightHandQuicklyClosed4);
            gestureQuicklyClosed5 = new Gesture(RightHandQuicklyClosed5);
            gestureQuicklyClosed6 = new Gesture(RightHandQuicklyClosed6);
            gestureQuicklyClosed7 = new Gesture(RightHandQuicklyClosed7);
            gestureQuicklyClosed1.name = "gestureQuicklyClosed1";
            gestureQuicklyClosed2.name = "gestureQuicklyClosed2";
            gestureQuicklyClosed3.name = "gestureQuicklyClosed3";
            gestureQuicklyClosed4.name = "gestureQuicklyClosed4";
            gestureQuicklyClosed2.name = "gestureQuicklyClosed5";
            gestureQuicklyClosed3.name = "gestureQuicklyClosed6";
            gestureQuicklyClosed4.name = "gestureQuicklyClosed7";
            gestureQuicklyOpen = new Gesture(RightHandQuicklyOpened);
            gestureQuicklyOpen.name = "gestureQuicklyOpen";
        }



        public void SetRightHandClosed(EventHandlerMethod methodName)
        {
            //throw new NotImplementedException();
            //add listener
            gestureClosed.GestureRecognized += new EventHandler(methodName);
        }

        public void SetRightHandOpen(EventHandlerMethod methodName)
        {
            //throw new NotImplementedException();
            gestureOpen.GestureRecognized += new EventHandler(methodName);
        }

        public void SetRightHandQuicklyClosed(EventHandlerMethod methodName)
        {
            //throw new NotImplementedException();
            gestureQuicklyClosed1.GestureRecognized += new EventHandler(methodName);
            gestureQuicklyClosed2.GestureRecognized += new EventHandler(methodName);
            gestureQuicklyClosed3.GestureRecognized += new EventHandler(methodName);
            gestureQuicklyClosed4.GestureRecognized += new EventHandler(methodName);
        }

        public void setRightHandQuicklyOpen(EventHandlerMethod methodName)
        {
            //throw new NotImplementedException();
            gestureQuicklyOpen.GestureRecognized += new EventHandler(methodName);
        }
        public void Update(Body body) {
            //update all gesture handlers
            if (body.TrackingId != 0)
            {
                //fire update
                gestureClosed.Update(body);
                gestureOpen.Update(body);
                gestureQuicklyClosed1.Update(body);
                gestureQuicklyClosed2.Update(body);
                gestureQuicklyClosed3.Update(body);
                gestureQuicklyClosed4.Update(body);
                gestureQuicklyClosed5.Update(body);
                gestureQuicklyClosed6.Update(body);
                gestureQuicklyClosed7.Update(body);
                gestureQuicklyOpen.Update(body);
            }

        }




        
    }


}
