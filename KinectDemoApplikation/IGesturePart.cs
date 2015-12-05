using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectDemoApplikation
{
    public interface IGesturePart
    {

        GesturePartResult CheckGesturePart(Body body);

    }

    public enum GesturePartResult
    {
        Failed,
        Succeded,
        Undetermined
    }


}
