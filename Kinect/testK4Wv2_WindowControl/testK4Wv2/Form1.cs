using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Kinect;
using Microsoft.Kinect.Input;
using Microsoft.Kinect.Face;
using Microsoft.Kinect.Fusion;
using System.IO;


namespace testK4Wv2
{
    public partial class Form1 : Form
    {
        private Body[] bodies = null;
        Bitmap bmp;
        Graphics graphics;
        float indicatorSize = 10f;
        BodyFrameReader bfReader;
        KinectSensor sensor;
        CoordinateMapper mapper;

        public Form1()
        {
            InitializeComponent();
            init();
        }


        public void init()
        {
            this.pictureBox1.BackColor = Color.LightBlue;

            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            
            this.pictureBox1.DrawToBitmap(bmp, new Rectangle(0,0, pictureBox1.Width, pictureBox1.Height));
            graphics = this.pictureBox1.CreateGraphics();
            graphics.DrawEllipse(new Pen(Color.Red), pictureBox1.Width / 2, pictureBox1.Height / 2, indicatorSize, indicatorSize);

            //Start Sensor
            sensor = KinectSensor.GetDefault();
            mapper = sensor.CoordinateMapper;
            bfReader = sensor.BodyFrameSource.OpenReader();

            if (bfReader != null)
            {
                bfReader.FrameArrived += bfReader_FrameArrived;
            }
            sensor.Open();
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

                    bodyFrame.GetAndRefreshBodyData(this.bodies);        
                }
            }

            if (this.bodies != null  && this.bodies.Length > 0)
            {
                foreach (Body body in this.bodies)
                {
                    if (body.IsTracked)
                    {
                        Joint rightThumb = body.Joints[JointType.HandTipRight];

                        CameraSpacePoint cspt = rightThumb.Position;

                        if ( rightThumb != null && rightThumb.TrackingState != TrackingState.NotTracked)
                        {
                            if ( cspt.Z < 0 )
                            {
                                cspt.Z = 0.1f; //to avoid mapping errors as it may return -ve value for Z some times
                            }

                            DepthSpacePoint dspt = mapper.MapCameraPointToDepthSpace(cspt);
                             
                            if (body.HandRightState == HandState.Closed)
                            {
                                graphics.DrawEllipse(new Pen(Color.Red), dspt.X, dspt.Y, indicatorSize, indicatorSize);
                            }

                            switch (body.HandLeftState)
                            {
                                case HandState.Open:
                                    this.WindowState = FormWindowState.Maximized;
                                    break;
                                case HandState.Closed:
                                    this.WindowState = FormWindowState.Minimized;
                                    break;
                                case HandState.Lasso:
                                    this.WindowState = FormWindowState.Normal;
                                    break;
                            }
                            
                        }
                                                
                        this.Text = "Bodies are being tracked...";
                    }
                }
               
            }
            else
            {
                this.Text = "No bodies to track..";
            }
        }
    }
}