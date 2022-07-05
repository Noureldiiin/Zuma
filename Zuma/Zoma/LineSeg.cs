using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Zoma
{
    class LineSeg
    {
        public PointF PS, PE;
        
        public void DrawYourSelf(Graphics g)
        {
            Pen Pn = new Pen(Color.Red, 4);

            g.DrawLine(Pn, PS, PE);
            g.FillEllipse(Brushes.DarkRed, PS.X - 5, PS.Y - 5, 10,10);
            g.FillEllipse(Brushes.DarkRed, PE.X - 5, PE.Y - 5, 10,10);

        }

    }
}
