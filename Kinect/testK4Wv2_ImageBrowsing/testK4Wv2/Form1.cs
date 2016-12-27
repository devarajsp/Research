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
        /// Array for the bodies
        /// </summary>
        private Body[] bodies = null;

        //configure the app
        bool CanDoGestureActions = true;
        bool _debug = false;
        bool CanDoGestureZoom = true;

        ColorFrameReader colFrameReader;
        
        public Form1()
        {
            InitializeComponent();
         
        }

     
        string[] files;

        Bitmap bmp;
        Graphics graphics;
        float indicatorSize = 10f;

        public void init()
        {
            this.pictureBox1.Left = 0;
            this.pictureBox1.Top = button1.Height;
            this.pictureBox1.Width = this.Width;
           this.pictureBox1.Height = this.Height - button1.Height;
           this.pictureBox1.BackColor = Color.LightBlue;
           this.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;

           this.panel1.Width = pictureBox1.Width - 50;
           this.panel1.Height = pictureBox1.Height - 100;

            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            
           // bmp.SetPixel(pictureBox1.Height / 2, pictureBox1.Width / 2, Color.Red);
            this.pictureBox1.DrawToBitmap(bmp, new Rectangle(0,0, pictureBox1.Width, pictureBox1.Height));
            graphics = this.pictureBox1.CreateGraphics();
            graphics.DrawEllipse(new Pen(Color.Red), pictureBox1.Width / 2, pictureBox1.Height / 2, indicatorSize, indicatorSize);

            files = Directory.GetFiles(@"D:\devFolder\05.Archive\Pix\Pix_2014", "*.jpg");


            if ( !_debug )
            {
                lblRighthand.Visible = false;
                lblLefthand.Visible = false;
                lblGestureCommand.Visible = false;
            }
            else
            {
                lblRighthand.Visible = true;
                lblLefthand.Visible = true;
                lblGestureCommand.Visible = true;
            }
        }
        BodyFrameReader bfReader;
        KinectSensor sensor;
        CoordinateMapper mapper;

        private void button1_Click(object sender, EventArgs e)
        {
            init();
            sensor = KinectSensor.GetDefault();
            mapper = sensor.CoordinateMapper;
            bfReader = sensor.BodyFrameSource.OpenReader();
           
            if ( bfReader != null )
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
        int swipeLeftCountLimit = 30;
        int swipeRightCountLimit = 30;
        int thumbsUpCountLimit = 10;
        int swipeYlimit = 100;
        int swipeXlimit = 100;
        bool swipeLeftCommand = false;
        bool swipeRightCommand = false;

        public void CalculateSwipeData(Body body)
        {

            if ( body.IsTracked )
            {
                
            }
        }

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
                SwipeCommandReset();
                resetSwipeCounts();
            }

            if ( thumbsUpRightCount >= thumbsUpCountLimit)
            {
                lblGestureCommand.Text = " ^^^^^ THUMBS UP ^^^^^";
                resetSwipeCounts();
            } 
            else if ( swipeLeftCount == swipeLeftCountLimit  )
            {
                lblGestureCommand.Text = "" + ++SwipeCounter;
                swipeLeftCommand = true;
                if ( SwipeCounter >= files.Length)
                {
                    SwipeCounter = 0;
                }

                //Do a call back
                DisplayPicture(files[SwipeCounter]);
        
                resetSwipeCounts();
            }
            else if (swipeRightCount == swipeRightCountLimit)
            {
                lblGestureCommand.Text = "" + --SwipeCounter;
                swipeRightCommand = true;

                if ( SwipeCounter < 0 )
                {
                    SwipeCounter = files.Length-1;
                }

                DisplayPicture(files[SwipeCounter]);
                
                // Do a call back
                resetSwipeCounts();
            }
                       
          
        }

        public void DisplayPicture(string jpgFilename)
        {
            if ( jpgFilename != null && jpgFilename.Length > 0 && File.Exists(jpgFilename))
            {
                Image img = Image.FromFile(jpgFilename);
                pictureBox1.Width = img.Width;
                pictureBox1.Height = img.Height;
                pictureBox1.Image = img;
                
                pictureBox1.Load(jpgFilename);
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

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

        public void SwipeCommandReset()
        {
            swipeRightCommand = false;
            swipeLeftCommand = false;
        }

        void bfReader_FrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
            bool dataReceived = false;

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
                    
                    dataReceived = true;
                }
            }

            if (this.bodies != null  && this.bodies.Length > 0)
            {
                int penIndex = 0;
                
                
                foreach (Body body in this.bodies)
                {
                    if (body.IsTracked)
                    {
                        
                        lblLefthand.Text = "Left Hand : " + GetHandStateString(body.HandLeftState);
                        lblRighthand.Text = "Right Hand : " + GetHandStateString(body.HandRightState);
                        //IReadOnlyDictionary<JointType, Joint> joints = body.Joints;

                        //// convert the joint points to depth (display) space
                        //Dictionary<JointType, Point> jointPoints = new Dictionary<JointType, Point>();

                        //foreach (JointType jointType in joints.Keys)
                        //{
                        //    // sometimes the depth(Z) of an inferred joint may show as negative
                        //    // clamp down to 0.1f to prevent coordinatemapper from returning (-Infinity, -Infinity)
                        //    CameraSpacePoint position = joints[jointType].Position;
                        //    if (position.Z < 0)
                        //    {
                        //        position.Z = InferredZPositionClamp;
                        //    }

                        //    DepthSpacePoint depthSpacePoint = this.coordinateMapper.MapCameraPointToDepthSpace(position);
                        //    jointPoints[jointType] = new Point(depthSpacePoint.X, depthSpacePoint.Y);
                        //}

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
                            if (body.HandRightState == HandState.Closed)
                            {
                                //graphics.DrawEllipse(new Pen(Color.Red), dspt.X, dspt.Y, indicatorSize, indicatorSize);
                            }
                            CalculateSwipeData(dsptT, dsptS, dsptW, dspt);
                            if ( CanDoGestureActions) // SPD BGFIX stupid check to enable or disable this test
                            {
                                switch (body.HandLeftState)
                                {

                                    case HandState.Open:
                                       zoomOut();
                                        
                                        // this.WindowState = FormWindowState.Maximized;
                                        break;
                                    case HandState.Closed:
                                        zoomIn();
                                        // zoomPicture(50);
                                        // this.WindowState = FormWindowState.Minimized;
                                        break;

                                    case HandState.Lasso:
                                       // this.WindowState = FormWindowState.Normal;
                                        break;
                                       
                                }
                            }
                            
                        }
                        else
                        {
                            rightThumbPosition += "NOT tracked.. ";
                        }
                        

                        lblRighthand.Text += rightThumbPosition;

                        lblStatus.Text = "Bodies are being tracked...";

                        if (CanDoGestureZoom) // SPD BGFIX stupid check to enable or disable this test
                        {
                            switch (body.HandRightState)
                            {

                                case HandState.Open:
                                    
                                    break;
                                case HandState.Closed:
                                    
                                    break;
                                case HandState.Lasso:
                                    
                                    break;
                            }
                        }


                    }
                }
               
            }
            else
            {
                lblStatus.Text = "No bodies to track..";
            }


        }

        public void zoomPicture(int percent)
        {
            float zoomFactor = 1;
            if ( percent < 0 || percent > 100 )
            {
                percent = 0;
            }

            zoomFactor = percent>0?(percent / 100):1;
            pictureBox1.Width = (int) (pictureBox1.Width * zoomFactor);
            pictureBox1.Height = (int)(pictureBox1.Height * zoomFactor);
        }

        public void zoomIn()
        {
            pictureBox1.Width += (int)(pictureBox1.Width/100);
            pictureBox1.Height += (int)(pictureBox1.Height/100);

            pictureBox1.Left = 0;
            pictureBox1.Top = panel1.ClientRectangle.Top;
        }

        public void zoomOut()
        {
            pictureBox1.Width -= (int)(pictureBox1.Width / 100);
            pictureBox1.Height -= (int)(pictureBox1.Height / 100);

            pictureBox1.Left = 0;
            pictureBox1.Top = panel1.ClientRectangle.Top;
        }

        //Presentation usage
        PowerPoint._Application pApp = null;
        PowerPoint.Presentation pPre = null;
        public void StartPresentation()
        {
            string presExisting = @"D:/Sample_DryRun_Collections.pptx";
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
                pPre.SlideShowWindow.View.EndNamedShow();
                pPre.Close();
                pApp.Quit();
                pApp = null;
                pPre = null;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int iter = 0;
            while (iter++ <= pPre.Slides.Count)
            {
                Thread.Sleep(1000);
                
            }
        }
    }
}
