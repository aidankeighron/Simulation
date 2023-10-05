using System;
using System.Drawing;

namespace Simulation
{
    class InteractableObject
    {
        public static bool isPointOnLine(PointF lineStart, PointF lineEnd, PointF point)
        {
            float yt = (point.X - lineStart.X) / (lineEnd.X - lineStart.X);
            float xt = (point.Y - lineStart.Y) / (lineEnd.Y - lineStart.Y);
            if (Math.Abs(yt - xt) > 0.01) // TODO make constant
            {
                return false;
            }
            return yt >= 0 && yt <= 1;
        }

        public static bool doLinesCollide(PointF line1Start, PointF line1End, PointF line2Start, PointF line2End)
        {
            float bottom = (line1End.Y - line1Start.Y) * (line2End.X - line2Start.X) - (line1End.X - line1Start.X) * (line2End.Y - line2Start.Y);
            float x1 = (line1Start.X * line1End.Y - line1Start.X * line1Start.Y) * (line2End.X - line2Start.X);
            float x2 = (line2End.X * (line2Start.Y - line1Start.Y) - line2Start.X * (line2End.Y - line1Start.Y)) * (line1End.X - line1Start.X);
            float x = (x1 + x2) / bottom;
            float y1 = (line1Start.X * line1End.Y - line1End.X * line1Start.Y) * (line2End.Y - line2Start.Y);
            float y2 = (line2Start.X * line2End.Y - line2End.X * line2Start.Y) * (line1End.Y - line1Start.Y);
            float y = (y1 + y2) / bottom;
            return isPointOnLine(line1Start, line1End, new PointF(x, y)) && isPointOnLine(line2Start, line2End, new PointF(x, y));
        }
    }

    class Wall : InteractableObject
    {
        private PointF lineStart;
        private PointF lineEnd;

        public Wall(PointF lineStart, PointF lineEnd)
        {
            this.lineStart = lineStart;
            this.lineEnd = lineEnd;
        }

        public PointF[] getWall()
        {
            return new PointF[2] { lineStart, lineEnd };
        }

        public bool pointCollides(PointF point)
        {
            return InteractableObject.isPointOnLine(lineStart, lineEnd, point);
        }

        public bool lineCollides(PointF lineStart, PointF lineEnd)
        {
            return InteractableObject.doLinesCollide(lineStart, lineEnd, this.lineStart, this.lineEnd);
        }

        public void DrawWall(Graphics g)
        {
            g.DrawLine(new Pen(Color.Black, 4), lineStart.X, lineStart.Y, lineEnd.X, lineEnd.Y);
        }
    }
}
