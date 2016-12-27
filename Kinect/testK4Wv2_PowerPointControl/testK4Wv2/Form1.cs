using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Kinect;
using Microsoft.Kinect.Input;
using Microsoft.Kinect.Face;
using Microsoft.Kinect.Fusion;
using System.IO;


// for ppt
using PowerPoint = Microsoft.Office.Interop.PowerPoint;
using System.Threading;
using Office = Microsoft.Office.Core;

namespace testK4Wv2
{
    public partial class Form1 : Form
    {


        /// <summary>
        /// Array for the bodies, frame reader, sensor and mapper (to map coordinates of 3D sensor to 2D plane)
        /// </summary>
        Body[]              bodies = null;
        BodyFrameReader     bfReader;
        KinectSensor        sensor;
        CoordinateMapper    mapper;
   
        public Form1()
        {
            InitializeComponent();
            StartSensor();
        }

        /// <summary>
        /// Start the sensor and to assign the call back for reading frames
        /// </summary>
        public void StartSensor()
        {
            sensor = KinectSensor.GetDefault();
            mapper = sensor.CoordinateMapper;
            bfReader = sensor.BodyFrameSource.OpenReader();

            if (bfReader != null)
            {
                bfReader.FrameArrived += bfReader_FrameArrived;
            }

            sensor.Open();
        }

        string GetHandStateString(HandState handState)
        {
            string handStateString = "";

            switch (handState)
            {
                case HandState.Open:
                    handStateString = " Open";
                    break;
                case HandState.Closed:
                    handStateString = " Closed";
                    break;
                case HandState.Lasso:
                    handStateString = " Lasso";
                    break;
                case HandState.NotTracked:
                    handStateString = " Not Tracked";
                    break;
                case HandState.Unknown:
                    handStateString = " Unknown";
                    break;
            }

            return handStateString;
        }


        DepthSpacePoint prevPoint = new DepthSpacePoint();
        int swipeLeftCount = 0;
        int swipeRightCount = 0;
        int thumbsUpRightCount = 0;
        int swipeLeftCountLimit = 10;
        int swipeRightCountLimit = 10;

         static int SwipeCounter = 0;

        public void CalculateSwipeData(DepthSpacePoint currDeptPointT, DepthSpacePoint currDeptPointS, DepthSpacePoint currDeptPointW, DepthSpacePoint currDeptPoint)
        {
            if ( currDeptPointS.Y > currDeptPointW.Y && currDeptPoint.X > currDeptPointW.X) 
            {
                swipeLeftCount++;
            }

            if (currDeptPointS.Y > currDeptPointW.Y && currDeptPoint.X < currDeptPointW.X)
            {
                swipeRightCount++;
            }

            if (currDeptPointS.Y > currDeptPointW.Y && currDeptPointT.Y < currDeptPoint.Y && currDeptPointT.Y < currDeptPointW.Y)
            {
                thumbsUpRightCount++;
            }

            if (currDeptPointS.Y < currDeptPointW.Y)
            {
                resetSwipeCounts();
            }

            if ( swipeLeftCount == swipeLeftCountLimit  )
            {
                lblGestureCommand.Text = "" + ++SwipeCounter;

                //Do a call back
                if (pPre != null)
                {
                    NextSlide();
                }
                resetSwipeCounts();
            }
            else if (swipeRightCount == swipeRightCountLimit)
            {
                lblGestureCommand.Text = "" + --SwipeCounter;

                if (pPre != null)
                {
                    PrevSlide();
                }
                // Do a call back
                resetSwipeCounts();
            }
                       
          
        }

        public void resetSwipeCounts()
        {
            swipeLeftCount = 0;
            swipeRightCount = 0;
            thumbsUpRightCount = 0;
            prevPoint.X = 0;
            prevPoint.Y = 0;
            
        }


        void bfReader_FrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
            using (BodyFrame bodyFrame = e.FrameReference.AcquireFrame())
            {
                if (bodyFrame != null)
                {
                    if (this.bodies == null)
                    {
                        this.bodies = new Body[bodyFrame.BodyCount];
                    }

                    // The first time GetAndRefreshBodyData is called, Kinect will allocate each Body in the array.
                    // As long as those body objects are not disposed and not set to null in the array,
                    // those body objects will be re-used.
                    bodyFrame.GetAndRefreshBodyData(this.bodies);
                    
                }
            }

            if (this.bodies != null  && this.bodies.Length > 0)
            {
                               
                foreach (Body body in this.bodies)
                {
                    if (body.IsTracked)
                    {
                        
                        lblLefthand.Text = "Left Hand : " + GetHandStateString(body.HandLeftState);
                        lblRighthand.Text = "Right Hand : " + GetHandStateString(body.HandRightState);
                        
                        Joint rightThumb = body.Joints[JointType.HandTipRight];
                        Joint rightWrist = body.Joints[JointType.WristRight];
                        Joint rightShoulder = body.Joints[JointType.ShoulderRight];
                        Joint rightTb = body.Joints[JointType.ThumbRight];

                        CameraSpacePoint cspt = rightThumb.Position;
                        CameraSpacePoint csptW = rightWrist.Position;
                        CameraSpacePoint csptS = rightShoulder.Position;
                        CameraSpacePoint csptT = rightTb.Position;

                        string rightThumbPosition = "\n Right Thumb : ";
                        if ( rightThumb != null && rightThumb.TrackingState != TrackingState.NotTracked)
                        {
                            rightThumbPosition += " tracked/infered.. ";

                            if ( cspt.Z < 0 )
                            {
                                cspt.Z = 0.1f; //to avoid mapping errors as it may return -ve value for Z some times
                            }

                            DepthSpacePoint dspt = mapper.MapCameraPointToDepthSpace(cspt);
                            DepthSpacePoint dsptW = mapper.MapCameraPointToDepthSpace(csptW);
                            DepthSpacePoint dsptS = mapper.MapCameraPointToDepthSpace(csptS);
                            DepthSpacePoint dsptT = mapper.MapCameraPointToDepthSpace(csptT);
                            
                            rightThumbPosition += "Depth space --  X : "+dspt.X + "  Y : "+ dspt.Y +  "\n";
                            rightThumbPosition += "Depth spaceW -- X : " + dsptW.X + "  Y : " + dsptW.Y + "\n";
                            rightThumbPosition += "Depth spaceS -- Y : " + dsptS.Y ;
                            
                            CalculateSwipeData(dsptT, dsptS, dsptW, dspt);
                            
                            switch (body.HandLeftState)
                            {
                                case HandState.Open:
                                    break;

                                case HandState.Closed:
                                    break;

                                case HandState.Lasso:
                                    if (pPre == null)
                                        StartPresentation();
                                    break;
                            }
                            
                        }
                        else
                        {
                            rightThumbPosition += "NOT tracked.. ";
                        }
                        

                        lblRighthand.Text += rightThumbPosition;

                        lblStatus.Text = "Bodies are being tracked...";

                    }
                }
               
            }
            else
            {
                lblStatus.Text = "No bodies to track..";
            }


        }


        //Presentation usage
        PowerPoint._Application pApp = null;
        PowerPoint.Presentation pPre = null;
        public void StartPresentation()
        {
            string presExisting = @"D:/Health_Environment_Cancer_v0.1.pptx";
            if (pApp == null && pPre == null)
            {
                pApp = new PowerPoint.Application();

                pPre = pApp.Presentations.Open(
                    presExisting, Office.MsoTriState.msoFalse,
                    Office.MsoTriState.msoFalse, Office.MsoTriState.msoTrue
                    );
                //Start playing presentation.
                pPre.SlideShowSettings.Run();
            }
        }

        public void NextSlide()
        {
            if ( pPre != null )
            {
                pPre.SlideShowWindow.View.Next();
            }
        }

        public void PrevSlide()
        {
            if (pPre != null)
            {
                pPre.SlideShowWindow.View.Previous();
            }
        }

        public void EndPresentation()
        {
            if (pPre != null  && pPre.SlideShowWindow.View.CurrentShowPosition > 1)
            {
                pPre.SlideShowWindow.View.Exit();
                pPre = null;
                //pPre.SlideShowWindow.View.EndNamedShow();
                //pPre.Close();
                //pApp.Quit();

                //SPD BGFIX: killing the app for ease
                Application.Exit();
                
            }
        }

   
    }
}
