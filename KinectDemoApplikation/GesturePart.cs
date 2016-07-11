using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectDemoApplikation
{
    public class GesturePart : IGesturePart
    {
        HandState success; //added private instance which is initialized in constructor

        /// <summary>
        /// constructor is used to define success handstate
        /// </summary>
        /// <param name="successHandState"></param>
        public GesturePart(HandState successHandState) {
            success = successHandState;

        }

        public GesturePartResult CheckGesturePart(Body body)
        {
            if (body == null)
            {
                return GesturePartResult.Undetermined;
            }
            else
            {
                //changed handstate to success
                if (body.HandRightState == success) 
                {
                    return GesturePartResult.Succeded;

                }
                else if (body.HandRightState == HandState.NotTracked || body.HandRightState == HandState.Unknown)
                {
                    return GesturePartResult.Undetermined;
                }
                else
                {
                    return GesturePartResult.Failed;
                }
            }
        }
    }
}
