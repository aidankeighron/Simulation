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
        public const float Dt = Simulation.Interval / 1000.0f;

        // Cube
        private int CubeSize = 50;
        private PointF CubeStartPosition = new PointF(150, 150);
        private PointF CubePosition = new PointF(0, 0);
        private PointF CubeStartVelocity = new PointF(0, 0);
        private PointF CubeVelocity = new PointF(0, 0);

        // 
        private float ReferenceTimestamp = 0;
        private int TextY = 0;

        // Mouse
        private PointF MousePosition = new PointF(0, 0);
        private PointF LastMousePosition = new PointF(0, 0);
        private PointF MouseVelocity = new PointF(0, 0);

        public Physics()
        {
            Action<Graphics> drawPhysics = (g) =>
            {
                MouseControls();
                HandleGravity();
                BoundValues();
                Pen p = new Pen(Color.Black, 1);
                g.DrawRectangle(p, CubePosition.X - CubeSize / 2.0f, CubePosition.Y - CubeSize / 2.0f, CubeSize, CubeSize);
                DisplayText("Position: ", CubePosition, g);
                DisplayText("Position 0: ", CubeStartPosition, g);
                DisplayText("Velocity: ", CubeVelocity, g);
                DisplayText("Velocity 0: ", CubeStartVelocity, g);
                DisplayText("MPos: ", MousePosition, g);
                DisplayText("MVel 0: ", MouseVelocity, g);
                TextY = 0;
            };

            Simulation.RegesterDrawer(drawPhysics);
        }

        public void HandleGravity()
        {
            CubePosition.X = CubeStartPosition.X + CubeStartVelocity.X * Timestamp();
            CubePosition.Y = CubeStartPosition.Y + CubeStartVelocity.Y * Timestamp() + 0.5f * Gravity * Timestamp() * Timestamp();
            CubeVelocity.X = CubeStartVelocity.X;
            CubeVelocity.Y = CubeStartVelocity.Y + Gravity * Timestamp();
        }

        public void MouseControls()
        {
            MousePosition = Cursor.Position;
            MouseVelocity = new PointF((MousePosition.X - LastMousePosition.X) / Dt, (MousePosition.Y - LastMousePosition.Y) / Dt);
            LastMousePosition = MousePosition;
            if (GetLeftMousePressed())
            {
                ReferenceTimestamp = Simulation.TimestampSeconds;
                CubePosition = MousePosition;
                CubeStartPosition = MousePosition;
                CubeStartVelocity = MouseVelocity;
                CubeVelocity = MouseVelocity;
            }
        }

        public void BoundValues()
        {
            if (CubePosition.X <= 0 + CubeSize / 2.0f)
            {
                CubePosition.X = 0 + CubeSize / 2.0f;
            }
            if (CubePosition.X >= 1280 - CubeSize / 2.0f)
            {
                CubePosition.X = 1280 - CubeSize / 2.0f;
            }
            if (CubePosition.Y <= 0 + CubeSize / 2.0f)
            {
                CubePosition.Y = 0 + CubeSize / 2.0f;
            }
            if (CubePosition.Y >= 720 - CubeSize / 2.0f)
            {
                CubePosition.Y = 720 - CubeSize / 2.0f;
            }
        }

        public float Timestamp()
        {
            return Simulation.TimestampSeconds - ReferenceTimestamp;
        }

        private void DisplayText(String text, object value, Graphics g)
        {
            g.DrawString(text + value.ToString(), new Font("Arial", 10), new SolidBrush(Color.Black), new PointF(5, 5 + TextY));
            TextY += 20;
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
