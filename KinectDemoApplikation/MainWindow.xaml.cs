using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
    /// This class is responsible for handling the UI
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// reference to controller
        /// </summary>
        private Controller controller;

        /// <summary>
        /// Canvas drawer helper class reference
        /// </summary>
        public CanvasDrawer CanvasDrawer { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public MainWindow()
        {
            //create associated objects

            InitializeComponent();
            this.WindowState = WindowState.Maximized;
            Whiteboard whiteBoard = new Whiteboard();
            this.Content = whiteBoard;
            controller = new Controller(whiteBoard);
            whiteBoard.controller = this.controller;


        }

        #region Event Handlers

        /// <summary>
        /// Starts the application when the UI is opened 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            controller.Start();
            controller.ChangeFingerTrackingMode();

        }

        /// <summary>
        /// Stops the application when the UI is closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closed(object sender, EventArgs e)
        {
            controller.Stop();
        }
        #endregion
    }
    }
