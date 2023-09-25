using System.Drawing;

namespace Simulation
{
    class PhysicsObject
    {
        // Position
        protected PointF StartPosition = new PointF(100, 100);
        protected PointF Position = new PointF(0, 0);

        // Velocity
        protected PointF StartVelocity = new PointF(0, 0);
        protected PointF Velocity = new PointF(0, 0);

        // Acceleration
        protected PointF Acceleration = new PointF(0, 0);

        // Coefficients
        protected float CoefficientOfRestitution = 0.8f;

        // State
        protected float Timestamp = 0;

        virtual
        public void tick(float dt)
        {
            Timestamp += dt;
            HandleGravity();
        }

        public void HandleGravity()
        {
            Position.X = StartPosition.X + StartVelocity.X * Timestamp;
            Position.Y = StartPosition.Y + StartVelocity.Y * Timestamp + 0.5f * Physics.Gravity * Timestamp * Timestamp;
            Velocity.X = StartVelocity.X;
            Velocity.Y = StartVelocity.Y + Physics.Gravity * Timestamp;
            Acceleration.Y = Physics.Gravity;
        }

        public void Restart(float x, float y, float vx, float vy)
        {
            StartPosition.X = (float)x;
            StartPosition.Y = (float)y;
            Position = StartPosition;
            StartVelocity.X = (float)vx;
            StartVelocity.Y = (float)vy;
            Velocity = StartVelocity;
            Timestamp = 0;
        }

        public PointF getPosition() { return Position; }
        public PointF getStartPosition() { return StartPosition; }
        public PointF getVelocity() { return Velocity; }
        public PointF getStartVelocity() { return StartVelocity; }
        public PointF getAcceleration() { return Acceleration; }
    }

    class CubePhysics : PhysicsObject
    {
        private float CubeSize;

        public CubePhysics(float cubeSize)
        {
            this.CubeSize = cubeSize;
        }

        override
        public void tick(float dt)
        {
            this.Timestamp += dt;
            this.HandleGravity();
            BoundValues();
        }

        public void DrawCube(Graphics g)
        {
            float xPos = this.Position.X - CubeSize / 2f;
            float yPos = this.Position.Y - CubeSize / 2f;
            g.DrawRectangle(new Pen(Color.Black, 1), xPos, yPos, CubeSize, CubeSize);
        }

        public void BoundValues()
        {
            bool rightWall = Position.X <= 0 + CubeSize / 2.0f;
            bool leftWall = Position.X >= Physics.ScreenWidth - CubeSize / 2.0f;
            bool topWall = Position.Y <= 0 + CubeSize / 2.0f;
            bool bottomWall = Position.Y >= Physics.ScreenHeight - CubeSize / 2.0f;
            float posX = rightWall ? 0 + CubeSize / 2.0f : leftWall ? Physics.ScreenWidth - CubeSize / 2.0f : Position.X;
            float velX = Velocity.X * this.CoefficientOfRestitution * ((rightWall || leftWall) ? -1 : 1);
            float posY = topWall ? 0 + CubeSize / 2.0f : bottomWall ? Physics.ScreenHeight - CubeSize / 2.0f : Position.Y;
            float velY = Velocity.Y * this.CoefficientOfRestitution * ((topWall || bottomWall) ? -1 : 1);
            if (rightWall || leftWall || topWall || bottomWall)
                this.Restart(posX, posY, velX, velY);
        }
    }
}
