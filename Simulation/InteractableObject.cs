using System;
using System.Drawing;

namespace Simulation
{
    class InteractableObject
    {
        public static bool isPointOnLine(Vector lineStart, Vector lineEnd, Vector point)
        {
            double yt = (point.X - lineStart.X) / (lineEnd.X - lineStart.X);
            double xt = (point.Y - lineStart.Y) / (lineEnd.Y - lineStart.Y);
            if (Math.Abs(yt - xt) > 0.01) // TODO make constant
            {
                return false;
            }
            return yt >= 0 && yt <= 1;
        }

        public static bool doLinesCollide(Vector line1Start, Vector line1End, Vector line2Start, Vector line2End)
        {
            double bottom = (line1End.Y - line1Start.Y) * (line2End.X - line2Start.X) - (line1End.X - line1Start.X) * (line2End.Y - line2Start.Y);
            double x1 = (line1Start.X * line1End.Y - line1Start.X * line1Start.Y) * (line2End.X - line2Start.X);
            double x2 = (line2End.X * (line2Start.Y - line1Start.Y) - line2Start.X * (line2End.Y - line1Start.Y)) * (line1End.X - line1Start.X);
            double x = (x1 + x2) / bottom;
            double y1 = (line1Start.X * line1End.Y - line1End.X * line1Start.Y) * (line2End.Y - line2Start.Y);
            double y2 = (line2Start.X * line2End.Y - line2End.X * line2Start.Y) * (line1End.Y - line1Start.Y);
            double y = (y1 + y2) / bottom;
            return isPointOnLine(line1Start, line1End, new Vector(x, y)) && isPointOnLine(line2Start, line2End, new Vector(x, y));
        }
    }

    class Wall : InteractableObject
    {
        private Vector lineStart;
        private Vector lineEnd;

        public Wall(Vector lineStart, Vector lineEnd)
        {
            this.lineStart = lineStart;
            this.lineEnd = lineEnd;
        }

        public Vector[] getWall()
        {
            return new Vector[2] { lineStart, lineEnd };
        }

        public bool pointCollides(Vector point)
        {
            return InteractableObject.isPointOnLine(lineStart, lineEnd, point);
        }

        public bool lineCollides(Vector lineStart, Vector lineEnd)
        {
            return InteractableObject.doLinesCollide(lineStart, lineEnd, this.lineStart, this.lineEnd);
        }

        public void DrawWall(Graphics g)
        {
            g.DrawLine(new Pen(Color.Black, 4), (float)lineStart.X, (float)lineStart.Y, (float)lineEnd.X, (float)lineEnd.Y);
        }
    }
}
