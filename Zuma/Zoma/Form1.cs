using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Zoma
{

    public partial class Form1 : Form
    {
        public class CActor
        {
            public float X, Y, W, H, M, inM, XS, YS, XE, YE, dx, dy;
            public Color ClUser;
        }
            
        Bitmap off;
        Curve crv = new Curve();
        CActor actor = new CActor();
        CActor actor2 = new CActor();
        List<LineSeg> Lne = new List<LineSeg>();
        public Color[] colors = new Color[] { Color.Blue, Color.Yellow, Color.Red };
        Timer TT = new Timer();
        float pos = 700f;
        float pos2 = 700f;
        int ct = 0;
        int ctGameOver = 0;
        bool GameOver = false;
        bool curveStop = false;
        bool ballMove = false;
        
        float XS, YS, XE, YE;
       
        int speed = 15;
        int ctMove = 0;
        public Form1()
        {
            this.WindowState = FormWindowState.Maximized;
            this.Paint += Form1_Paint;
            this.Load += Form1_Load;
            this.MouseDown += Form1_MouseDown;
            this.KeyDown += Form1_KeyDown;
            this.MouseMove += Form1_MouseMove;
            TT.Tick += TT_Tick;
            TT.Start();
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            PointF Center = new PointF();
            Center.X = actor2.X;
            Center.Y = actor2.Y;
            float thLine = (float)Math.Atan2(e.Y - Lne[4].PE.Y, e.X - Lne[4].PE.X);
            float thLine2 = (float)Math.Atan2(Lne[5].PE.Y - Lne[4].PS.Y, Lne[5].PE.X - Lne[4].PS.X);
            float thdif = thLine - thLine2;

            for (int i = 0; i < Lne.Count; i++)
            {
                RotateLine(Lne[i], Center, thdif + 0.1f);
            }

            XS = Lne[0].PS.X;
            YS = Lne[0].PS.Y;

            XE = e.X;
            YE = e.Y;
        }

        private void TT_Tick(object sender, EventArgs e)
        {
            if (GameOver == false)
            {
                if (ct % 8 == 0)
                {
                    if (curveStop == true)
                    {
                        Ball ball = new Ball();
                        ball.T = 0;
                        ball.X = crv.LCtrPts[0].X;
                        ball.Y = crv.LCtrPts[0].Y;
                        Random RR = new Random();
                        int Rnd = RR.Next(0, colors.Length);
                        ball.Cl = colors[Rnd];
                        crv.LBalls.Add(ball);
                    }
                }

                if (curveStop == true)
                {
                    for (int i = 0; i < crv.LBalls.Count; i++)
                    {

                        if (crv.LBalls[i].T < 1)
                        {
                            crv.LBalls[i].T += 0.005f;
                        }
                        else
                        {
                            crv.LBalls.Remove(crv.LBalls[i]);
                            ctGameOver++;
                        }

                        //if (ctGameOver == 5)
                        //{
                        //    GameOver = true;
                        //}

                        if (Math.Pow(actor.X - crv.LBalls[i].X, 2) + Math.Pow(actor.Y - crv.LBalls[i].Y, 2) - Math.Pow(30, 2) <= 0)
                        {
                            if (actor.ClUser == crv.LBalls[i].Cl)
                            {
                                crv.LBalls.Remove(crv.LBalls[i]);
                                ctMove = 0;
                                ballMove = false;
                                actor.X = pos + 23;
                                actor.Y = pos2 - 250;
                                actor.W = 35;
                                actor.H = 35;
                                Random RR = new Random();
                                int Rnd = RR.Next(0, colors.Length);
                                actor.ClUser = colors[Rnd];

                            }
                            else
                            {
                                crv.LBalls[i].Cl = actor.ClUser;
                                for(int k = crv.LBalls.Count - 1; k > i + 1; k--)
                                {
                                    crv.LBalls[k].Cl = crv.LBalls[k - 1].Cl;
                                }
                                ctMove = 0;
                                ballMove = false;
                                actor.X = pos + 23;
                                actor.Y = pos2 - 250;
                                actor.W = 35;
                                actor.H = 35;
                                Random RR = new Random();
                                int Rnd = RR.Next(0, colors.Length);
                                actor.ClUser = colors[Rnd];
                            }
                        }
                    }

                    if (ballMove == true)
                    {
                        if (Math.Abs(actor.dx) > Math.Abs(actor.dy))
                        {
                            if (actor.XE > actor.XS)
                            {
                                actor.X += speed;
                                actor.Y += actor.M * speed;
                            }
                            else
                            {
                                actor.X -= speed;
                                actor.Y -= actor.M * speed;
                            }
                            
                        }
                        else
                        {
                            if (actor.YE > actor.YS)
                            {
                                actor.Y += speed;
                                actor.X += actor.inM * speed;
                            }
                            else
                            {
                                actor.Y -= speed;
                                actor.X -= actor.inM * speed;
                            }
                        }
                        ctMove++;
                    }
                    if(actor.Y < 0 || actor.Y > ClientSize.Height || actor.X < 0 || actor.X > ClientSize.Width)
                    {
                        ctMove = 0;
                        ballMove = false;

                        actor.X = pos + 23;
                        actor.Y = pos2 - 250;
                        actor.W = 35;
                        actor.H = 35;
                        Random RR = new Random();
                        int Rnd = RR.Next(0, colors.Length);
                        actor.ClUser = colors[Rnd];
                    }
                }
                ct++;

                DrawDubb(this.CreateGraphics());
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                curveStop = true;
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (curveStop == false)
                {
                    crv.LCtrPts.Add(new PointF(e.X, e.Y));
                }
                
                if (curveStop == true)
                {
                    if (ctMove < 5)
                    {
                        actor.XE = e.X;
                        actor.YE = e.Y;
                        actor.XS = actor.X;
                        actor.YS = actor.Y;
                        actor.dx = actor.XE - actor.XS;
                        actor.dy = actor.YE - actor.YS;

                        actor.M = actor.dy / actor.dx;
                        actor.inM = actor.dx / actor.dy;
                        //this.Text = "XE" + actor.XE + "         YE" + actor.YE + "      XS" + actor.XS;
                        ballMove = true;
                    }
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            off = new Bitmap(ClientSize.Width, ClientSize.Height);

            LineSeg pnn = new LineSeg();
            pnn.PS.X = pos + 40f;
            pnn.PS.Y = pos2 - 340f;
            pnn.PE.X = pos + 40f;
            pnn.PE.Y = pos2 - 300f;
            Lne.Add(pnn);

            LineSeg pnn2 = new LineSeg();
            pnn2.PS.X = Lne[0].PE.X;
            pnn2.PS.Y = Lne[0].PE.Y;
            pnn2.PE.X = Lne[0].PE.X + 60f;
            pnn2.PE.Y = Lne[0].PE.Y + 50;
            Lne.Add(pnn2);

            LineSeg pnn3 = new LineSeg();
            pnn3.PS.X = Lne[1].PE.X;
            pnn3.PS.Y = Lne[1].PE.Y;
            pnn3.PE.X = Lne[1].PE.X - 20f ;
            pnn3.PE.Y = Lne[1].PE.Y + 50;
            Lne.Add(pnn3);

            LineSeg pnn4 = new LineSeg();
            pnn4.PS.X = Lne[2].PE.X;
            pnn4.PS.Y = Lne[2].PE.Y;
            pnn4.PE.X = Lne[2].PE.X - 80f;
            pnn4.PE.Y = Lne[2].PE.Y ;
            Lne.Add(pnn4);

            LineSeg pnn5 = new LineSeg();
            pnn5.PS.X = Lne[3].PE.X;
            pnn5.PS.Y = Lne[3].PE.Y;
            pnn5.PE.X = Lne[3].PE.X - 20f;
            pnn5.PE.Y = Lne[3].PE.Y - 50f;
            Lne.Add(pnn5);

            LineSeg pnn6 = new LineSeg();
            pnn6.PS.X = Lne[4].PE.X;
            pnn6.PS.Y = Lne[4].PE.Y;
            pnn6.PE.X = Lne[4].PE.X + 60f;
            pnn6.PE.Y = Lne[4].PE.Y - 50f;
            Lne.Add(pnn6);

           
            actor.X = pos + 23;
            actor.Y = pos2 - 250;
            actor.W = 35;
            actor.H = 35;
            Random RR = new Random();
            int Rnd = RR.Next(0, colors.Length);
            actor.ClUser = colors[Rnd];

            actor2.X = pos + 40;
            actor2.Y = pos2 - 250;
            actor2.W = 5;
            actor2.H = 5;

           

        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            DrawDubb(e.Graphics);
        }

        void DrawScene(Graphics g)
        {
            g.Clear(Color.DarkGray);
            if (GameOver == true)
                MessageBox.Show("     Game Over");
            crv.DrawYourSelf(g);
            g.DrawLine(Pens.Black, XS, YS, XE, YE);
            for (int i = 0; i < Lne.Count; i++)
            {
                Lne[i].DrawYourSelf(g);
            }
            g.FillEllipse(Brushes.Red, actor2.X, actor2.Y, actor2.W, actor2.H);

            SolidBrush brush = new SolidBrush(actor.ClUser);
            g.FillEllipse(brush, actor.X, actor.Y, actor.W, actor.H);
            
        }

        void DrawDubb(Graphics g)
        {
            Graphics g2 = Graphics.FromImage(off);
            DrawScene(g2);
            g.DrawImage(off, 0, 0);
        }

        private void RotateLine(LineSeg line, PointF referencePoint, float theta)
        {
            PointF tempStart = new PointF(line.PS.X - referencePoint.X, line.PS.Y - referencePoint.Y);
            PointF tempEnd = new PointF(line.PE.X - referencePoint.X, line.PE.Y - referencePoint.Y);

            PointF temp2Start = new PointF();
            PointF temp2End = new PointF();
            temp2Start.X = (float)(tempStart.X * Math.Cos(theta) - tempStart.Y * Math.Sin(theta));
            temp2Start.Y = (float)(tempStart.X * Math.Sin(theta) + tempStart.Y * Math.Cos(theta));
            temp2End.X = (float)(tempEnd.X * Math.Cos(theta) - tempEnd.Y * Math.Sin(theta));
            temp2End.Y = (float)(tempEnd.X * Math.Sin(theta) + tempEnd.Y * Math.Cos(theta));

            tempStart = new PointF(temp2Start.X + referencePoint.X, temp2Start.Y + referencePoint.Y);
            tempEnd = new PointF(temp2End.X + referencePoint.X, temp2End.Y + referencePoint.Y);
            line.PS = tempStart;
            line.PE = tempEnd;
        }

    }
}
