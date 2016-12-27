using System;
using System.Collections;
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
        BodyFrameReader bfReader;
        KinectSensor sensor;
        CoordinateMapper mapper;

        public Form1()
        {
            InitializeComponent();
            this.Resize += Form1_Resize;
            StartSensor();
        }

        void Form1_Resize(object sender, EventArgs e)
        {
            reSizeControls();
        }

        public void reSizeControls()
        {
            //set size of the browser control
            webBrowser1.Top = lblLefthand.Top + lblLefthand.Height + 40;
            webBrowser1.Left = 0;
            webBrowser1.Height = this.Height - lblLefthand.Height;
            webBrowser1.Width = this.Width;
            
        }
        

        void StartSensor()
        {
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

        public void CalculateSwipeData(Body body)
        {

            if ( body.IsTracked )
            {
                
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

                        Joint leftTip = body.Joints[JointType.HandTipLeft];
                        Joint leftShoulder = body.Joints[JointType.ShoulderLeft];

                        CameraSpacePoint cspt = rightThumb.Position;
                        CameraSpacePoint csptW = rightWrist.Position;
                        CameraSpacePoint csptS = rightShoulder.Position;
                        CameraSpacePoint csptT = rightTb.Position;

                        CameraSpacePoint csptLTip = leftTip.Position;
                        CameraSpacePoint csptLShoulder = leftShoulder.Position;



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

                            DepthSpacePoint dsptLTip = mapper.MapCameraPointToDepthSpace(csptLTip);
                            DepthSpacePoint dsptLShoulder = mapper.MapCameraPointToDepthSpace(csptLShoulder);

                            rightThumbPosition += "Depth space --  X : "+dspt.X + "  Y : "+ dspt.Y +  "\n";
                           // rightThumbPosition += "Depth spaceW -- X : " + dsptW.X + "  Y : " + dsptW.Y + "\n";
                            //srightThumbPosition += "Depth spaceS -- Y : " + dsptS.Y ;


                            lock (this)
                            {
                                if (DoAutoHelp && !IsProcessing && !CanRecord)
                                {
                                    IsProcessing = true;
                                    CheckAutoHelpCoordinates(dspt, dsptLTip);
                                    IsProcessing = false;
                                }
                            }
    
                            switch (body.HandLeftState)
                            {
                                case HandState.Open:
                                   
                                    break;
                                case HandState.Closed:
                                   
                                    break;
                                case HandState.Lasso:
                                    lock(this)
                                    {
                                        if (CanRecord && !IsProcessing && dsptLShoulder.Y > dsptLTip.Y)
                                        {
                                            IsProcessing = true;
                                            RecordCoordinates(dspt, dsptW, dsptS, dsptT);
                                            IsProcessing = false;
                                        }
                                    }
                                    break;
                            }
                            
                        }
                        else
                        {
                            rightThumbPosition += "NOT tracked.. ";
                        }
                        
                        lblRighthand.Text += rightThumbPosition;

                        lblStatus.Text = "Bodies are being tracked...";

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
            else
            {
                lblStatus.Text = "No bodies to track..";
            }


        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnRecord_Click(object sender, EventArgs e)
        {
            StartRecording();
        }

        string coordinatesFilename = "coordinates.txt";
        bool CanRecord = false;
        void StartRecording()
        {
            CanRecord = true;
            btnRecord.Enabled = false;
            tmpName = btnRecord.Text;
        }

        static int ctr = 0;
        static int max_coordinates = 4;
        List<string> coordinates = new List<string>();
        static bool IsProcessing = false;
        static string tmpName = "";
        string coords = "";
        void RecordCoordinates(DepthSpacePoint dspt, DepthSpacePoint dsptW, DepthSpacePoint dsptS, DepthSpacePoint dsptT)
        {
             
            if ( ctr < max_coordinates && CanRecord )
            {
                ctr++;
                coords = dspt.X.ToString() + "," + dspt.Y.ToString() + ", d:/"+ctr.ToString()+".html";
                coordinates.Add(coords);
                File.WriteAllLines(coordinatesFilename, coordinates);
                btnRecord.Text = tmpName + ctr;

                CanRecord = false;
                btnRecord.Enabled = true;
               // btnRecord.Text = tmpName;
            }
        }

        string tmpAutoHelpTxt = "";
        private void btnHelp_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(FetchCoordinates());
            AutoHelp();
        }

        bool DoAutoHelp = false;
        void AutoHelp()
        {
            if ( DoAutoHelp )
            {
                btnHelp.Text = tmpAutoHelpTxt;
                btnRecord.Enabled = true;
                DoAutoHelp = false;
            }
            else
            {
                LoadCoordinates();
                tmpAutoHelpTxt = btnHelp.Text;
                btnHelp.Text = "STOP AutoHelp";
                btnRecord.Enabled = false;
                DoAutoHelp = true;
            }
        }

        List<Coordinate> cList = new List<Coordinate>();
        void LoadCoordinates()
        {
            string[]    lines = null;
            string[]    tokens = null;
            char[]      delims = {','};

            cList.Clear();

            if (File.Exists(coordinatesFilename))
            {
                lines = File.ReadAllLines(coordinatesFilename);
                if ( lines != null && lines.Length > 0 )
                {
                    foreach(string line in lines)
                    {
                        try
                        {
                            Coordinate c = new Coordinate();
                            tokens = line.Split(delims);
                            c.X = (float)Convert.ToDouble(tokens[0]);
                            c.Y = (float)Convert.ToDouble(tokens[1]);
                            c.filepath = tokens[2].ToString();
                            cList.Add(c);
                        }
                        catch(Exception ex)
                        {
                            MessageBox.Show(ex.ToString());//SPD BGFIX -- dont swallow the exception
                        }
                    }
                }
            }
        }

        string FetchCoordinates()
        {
            LoadCoordinates();
            string retVal = "";
            foreach (Coordinate c in cList)
            {
                retVal += c.X.ToString() + ", " + c.Y.ToString() + ", " + c.filepath.ToString() + "\n\r";
            }
            return retVal;
        }

        static string currFilePath = "";
        static int path_ctr = 0;
        static int path_max_ctr = 1;

        float radius = 10;
        void CheckAutoHelpCoordinates(DepthSpacePoint dsptRTip, DepthSpacePoint dsptLTip)
        {
            foreach(Coordinate c in cList)
            {
                if ( c.IsInsideArea(dsptRTip, radius )  || c.IsInsideArea(dsptLTip, radius ) )
                {
                    if ( currFilePath.Equals(c.filepath))
                    {
                        path_ctr++;
                    }
                    else
                    {
                        path_ctr = 0;
                        currFilePath = c.filepath;
                    }

                    if (path_ctr == path_max_ctr)
                    {
                        ShowFile(c.filepath);
                    }
                }
            }
            
        }

        void ShowFile(string filename)
        {
            
            //MessageBox.Show(filename);
            webBrowser1.Navigate(filename);
        }
    }

    class Coordinate
    {
        public float X;
        public float Y;
        public string filepath;
        public string filetype;

        public bool IsInsideArea(DepthSpacePoint dspt, float radius)
        {
            bool retval = false;

            float xLL, xUL, yLL, yUL;

            xLL = ((this.X - radius) < 0) ? 0 : (this.X - radius);
            xUL = (this.X + radius);

            yLL = ((this.Y + radius) < 0) ? 0 : (this.Y + radius);
            yUL = (this.Y - radius);

            if ( xLL <= dspt.X && xUL >= dspt.X && yLL >= dspt.Y && yUL <= dspt.Y )
            {
                retval = true;
            }

            return retval;
        }
    }
}


