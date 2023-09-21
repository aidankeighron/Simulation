using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Simulation
{
    class Physics
    {
        // Constants
        public const float Gravity = 9.8f;
        public const float Dt = Simulation.Interval / 100.0f;
        public const int ScreenWidth = 1536; // TODO pull from system
        public const int ScreenHeight = 960;

        // Cube
        private CubePhysics Cube = new CubePhysics(50);

        // Other
        private int TextY = 0;

        // Mouse
        private PointF MousePosition = new PointF(0, 0);
        private PointF LastMousePosition = new PointF(0, 0);
        private PointF MouseVelocity = new PointF(0, 0);

        public void DrawPhysics(Graphics g)
        {
            MouseControls();
            Cube.tick(Dt);
            Cube.DrawCube(g);
            DisplayText("Position", Cube.getPosition(), g);
            DisplayText("Start Position", Cube.getStartPosition(), g);
            DisplayText("Velocity", Cube.getVelocity(), g);
            DrawVector(g, Color.Green, Cube.getPosition(), Cube.getVelocity());
            DisplayText("Start Velocity", Cube.getStartVelocity(), g);
            DisplayText("Acceleration", Cube.getAcceleration(), g);
            DrawVector(g, Color.Red, Cube.getPosition(), Cube.getAcceleration());
            DisplayText("MPos: ", MousePosition, g);
            DisplayText("MVel: ", MouseVelocity, g);
            TextY = 0;
        }

        public void MouseControls()
        {
            MousePosition = Cursor.Position;
            MouseVelocity = new PointF((MousePosition.X - LastMousePosition.X) / (Dt * 4), (MousePosition.Y - LastMousePosition.Y) / (Dt * 4));
            LastMousePosition = MousePosition;
            if (GetLeftMousePressed())
            {
                Cube.Restart(MousePosition.X, MousePosition.Y, MouseVelocity.X, MouseVelocity.Y);
            }
        }

        private void DisplayText(String name, object value, Graphics g)
        {
            g.DrawString($"{name}: {value.ToString()}", new Font("Arial", 10), new SolidBrush(Color.Black), new PointF(5, 5 + TextY));
            TextY += 20;
        }

        public void DrawVector(Graphics g, Color color, PointF start, PointF vector)
        {
            g.DrawLine(new Pen(color, 2), start.X, start.Y, vector.X + start.X, vector.Y + start.Y);
        }

        [DllImport("user32.dll")]
        static extern short GetAsyncKeyState(int VirtualKeyPressed);

        public static bool GetLeftMousePressed()
        {
            if (GetAsyncKeyState(0x01) == 0)
                return false;
            else
                return true;
        }


    }
}
