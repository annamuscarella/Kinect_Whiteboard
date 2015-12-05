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
            //prepare gesture
            gestureClosed = new Gesture(RightHandClosed);
            gestureOpen = new Gesture(RightHandOpen);
            gestureQuicklyClosed = new Gesture(RightHandQuicklyClosed);
            gestureQuicklyOpen = new Gesture(RightHandQuicklyOpened);

            
            
            
            
        }

        public void SetRightHandClosed(EventHandlerMethod methodName)
        {
            throw new NotImplementedException();
            //add listener
            gestureClosed.GestureRecognized += new EventHandler(methodName);
        }

        public void SetRightHandOpen(EventHandlerMethod methodName)
        {
            throw new NotImplementedException();
            gestureOpen.GestureRecognized += new EventHandler(methodName);
        }

        public void SetRightHandQuicklyClosed(EventHandlerMethod methodName)
        {
            throw new NotImplementedException();
            gestureQuicklyClosed.GestureRecognized += new EventHandler(methodName);
        }

        public void setRightHandQuicklyOpen(EventHandlerMethod methodName)
        {
            throw new NotImplementedException();
            gestureQuicklyOpen.GestureRecognized += new EventHandler(methodName);
        }




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
    }


}
