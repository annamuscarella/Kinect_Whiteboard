using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectDemoApplikation
{
    interface IGestureHandler
    {
        //public delegate void EventHandlerMethod(object sender, EventArgs e);

        void SetRightHandClosed(APIController.EventHandlerMethod methodName);
        void SetRightHandOpen(APIController.EventHandlerMethod methodName);
        void SetRightHandQuicklyClosed(APIController.EventHandlerMethod methodName);
        void setRightHandQuicklyOpen(APIController.EventHandlerMethod methodName);
        void Update(Body body);
    }
}
