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
        public Gesture gestureQuicklyClosed;
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
            IGesturePart[] RightHandQuicklyClosed = new IGesturePart[]
            {
            Gesture.open, Gesture.closed, Gesture.open
            };

            IGesturePart[] RightHandQuicklyOpened = new IGesturePart[]
                {
                Gesture.closed, Gesture.open, Gesture.closed
                };
            IGesturePart[] RightHandOpen = new IGesturePart[]
                {
                //1x closed
                Gesture.closed,
                //15x open
                Gesture.open, Gesture.open, Gesture.open, Gesture.open, Gesture.open,
                Gesture.open, Gesture.open, Gesture.open, Gesture.open, Gesture.open,
                Gesture.open, Gesture.open, Gesture.open, Gesture.open, Gesture.open
                };
            IGesturePart[] RightHandClosed = new IGesturePart[]
                {
                //1x open
                Gesture.open,
                //15x closed
                Gesture.closed, Gesture.closed, Gesture.closed, Gesture.closed, Gesture.closed,
                Gesture.closed, Gesture.closed, Gesture.closed, Gesture.closed, Gesture.closed,
                Gesture.closed, Gesture.closed, Gesture.closed, Gesture.closed, Gesture.closed
                };

            #endregion

            //prepare gesture
            gestureClosed = new Gesture(RightHandClosed);
            gestureClosed.name = "gestureClosed";
            gestureOpen = new Gesture(RightHandOpen);
            gestureOpen.name = "gestureOpen";
            gestureQuicklyClosed = new Gesture(RightHandQuicklyClosed);
            gestureQuicklyClosed.name = "gestureQuicklyClosed";
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
            gestureQuicklyClosed.GestureRecognized += new EventHandler(methodName);
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
                gestureQuicklyClosed.Update(body);
                gestureQuicklyOpen.Update(body);
            }

        }




        
    }


}
