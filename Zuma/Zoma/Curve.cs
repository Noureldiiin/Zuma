﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Zoma
{
    class Ball
    {
        public float X, Y, T;
        public Color Cl;
    }
    class Curve
    {
        public List<PointF> LCtrPts = new List<PointF>();
        public List<Ball> LBalls = new List<Ball>();
       
        float Fact(int v)
        {
            float F = 1;
            for (int i = 2; i <= v; i++)
            {
                F *= i;
            }
            return F;
        }
        PointF CalcCurvePointAtTime(float t)
        {
            PointF P = new PointF();
            int n = LCtrPts.Count - 1;
            float C;
            for (int i = 0; i < LCtrPts.Count; i++)
            {
                C = Fact(n) / (Fact(i) * Fact(n - i));
                P.X += (float)(Math.Pow(t, i) * Math.Pow(1 - t, n - i) * C * LCtrPts[i].X);
                P.Y += (float)(Math.Pow(t, i) * Math.Pow(1 - t, n - i) * C * LCtrPts[i].Y);
                
            }

            return P;
        }
        public void DrawYourSelf(Graphics g)
        {

            for (float t = 0; t <= 1; t += 0.001f)
            {
                PointF P = CalcCurvePointAtTime(t);
                g.FillEllipse(Brushes.Black, P.X - 5, P.Y - 5, 5, 5);
            }

            for (int i = 0; i < LBalls.Count; i++)
            {
                LBalls[i].X = CalcCurvePointAtTime(LBalls[i].T).X;
                LBalls[i].Y = CalcCurvePointAtTime(LBalls[i].T).Y;
               
                SolidBrush brush = new SolidBrush(LBalls[i].Cl);
                g.FillEllipse(brush, LBalls[i].X - 20, LBalls[i].Y - 20, 35, 35);
            }
        }
    }
}
