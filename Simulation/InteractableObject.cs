namespace Simulation
{
    class InteractableObject
    {
        public static bool isPointOnLine(PointF lineStart, PointF lineEnd, PointF point)
        {
            return point.Y == ((lineEnd.Y - lineStart.Y) / (lineEnd.X - lineStart.Y)) * (point.X - lineStart.X) + lineStart.Y;
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
            return false;
        }
    }

    class Wall : InteractableObject
    {

    }
}
