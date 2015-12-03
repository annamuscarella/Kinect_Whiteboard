using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    /// Abstract super class for all handler classes. This class contains a controller reference and an Enabling/Disabling flag for the underlying handler
    /// </summary>
    public abstract class KinectHandler
    {

       
        /// <summary>
        /// Flag for enabling/disabling the handler. The handler should only process and incoming frame, if this flag is enabled.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// controller reference
        /// </summary>
        protected Controller controller;
        
        /// <summary>
        /// Starts the handler
        /// </summary>
        /// <param name="c"></param>
        /// <param name="sensor"></param>
        public abstract void Start(Controller c, KinectSensor sensor);

        /// <summary>
        /// Strops the handler
        /// </summary>
        public abstract void Stop();



    }
}
