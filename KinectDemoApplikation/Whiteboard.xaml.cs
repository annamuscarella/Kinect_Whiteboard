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

namespace KinectDemoApplikation
{
    /// <summary>
    /// Interaction logic for Whiteboard.xaml
    /// </summary>
    public partial class Whiteboard : UserControl
    {
        public Controller controller;

        /// <summary>
        /// Canvas drawer helper class reference
        /// </summary>
        public CanvasDrawer CanvasDrawer { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public Whiteboard()
        {
            //create associated objects
            CanvasDrawer = new CanvasDrawer(this);

            InitializeComponent();


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
            //controller.ChangeSkeletonMode();
            //controller.ChangeFingerTrackingMode();
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

        /*<--------- SEBIS METHODS----------------------------------->
        private void Color_Click(object sender, RoutedEventArgs e)
        {
            controller.EnableColor();
        }

        private void Depth_Click(object sender, RoutedEventArgs e)
        {
            controller.EnableDepth();
        }

        private void Infrared_Click(object sender, RoutedEventArgs e)
        {
            controller.EnableInfrared();
        }

        private void Body_Click(object sender, RoutedEventArgs e)
        {
            controller.ChangeSkeletonMode();
        }

        private void Tracking_Click(object sender, RoutedEventArgs e)
        {
            controller.ChangeFingerTrackingMode();
        }

        private void ResetCanvas_Click(object sender, RoutedEventArgs e)
        {
            CanvasDrawer.Clear();
        }*/

        /// <summary>
        /// Displays the passed status message
        /// </summary>
        /// <param name="msg">status message</param>
        public void DisplayStatus(String msg)
        {
            //this.Status.Text = msg;
        }

        /// <summary>
        /// Displays the passed debug message
        /// </summary>
        /// <param name="msg">debug message</param>
        public void DisplayDebug(String msg)
        {
            //this.Debug.Text = msg;
        }



        #endregion

        private void SwitchButton_Click(object sender, RoutedEventArgs e)
        {
            controller.EnableColor();
            controller.Ui.Papier.Visibility = System.Windows.Visibility.Hidden;
        }
    }
}

