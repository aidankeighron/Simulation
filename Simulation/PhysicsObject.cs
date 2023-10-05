using System;
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
        public void tick(float dt, Wall wall)
        {
            Timestamp += dt;
            HandleGravity(wall);
        }

        public void HandleGravity(Wall wall)
        {
            PointF newPosition = new PointF(0, 0);
            PointF newVelocity = new PointF(0, 0);
            newPosition.X = StartPosition.X + StartVelocity.X * Timestamp;
            newPosition.Y = StartPosition.Y + StartVelocity.Y * Timestamp + 0.5f * Physics.Gravity * Timestamp * Timestamp;
            newVelocity.X = StartVelocity.X;
            newVelocity.Y = StartVelocity.Y + Physics.Gravity * Timestamp;
            tryMove(newPosition, newVelocity, wall);
            Acceleration.Y = Physics.Gravity;
        }

        public void Restart(PointF position, PointF velocity)
        {
            Restart(position.X, position.Y, velocity.X, velocity.Y);
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

        virtual
        protected void tryMove(PointF position, PointF velocity, Wall wall)
        {
            if (wall.pointCollides(position))
            {
                // Hit
                PointF wallStart = wall.getWall()[0];
                PointF wallEnd = wall.getWall()[1];
                double angle = Math.Atan2(wallEnd.Y - wallStart.Y, wallEnd.X - wallStart.X);
                PointF newVelocity = new PointF((float)(-velocity.Y * Math.Sin(angle) + velocity.X * Math.Cos(angle)),
                                                (float)(velocity.Y * Math.Cos(angle) + velocity.X * Math.Sin(angle)));
                if (velocity.Y > 0)
                {
                    newVelocity.X *= -1;
                }

                Restart(position, newVelocity);
            }
            else
            {
                Position = position;
                Velocity = velocity;
            }
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
        public void tick(float dt, Wall wall)
        {
            //this.tick(dt, wall);
            this.Timestamp += dt;
            this.HandleGravity(wall);
            BoundValues();
        }

        //override
        //protected void tryMove(PointF position, PointF velocity, Wall wall)
        //{
        //    if (wall.lineCollides(new PointF(position.X - CubeSize / 2, position.Y + CubeSize / 2), new PointF(position.X + CubeSize / 2, position.Y - CubeSize / 2)))
        //    {
        //        // Hit
        //        PointF wallStart = wall.getWall()[0];
        //        PointF wallEnd = wall.getWall()[1];
        //        double angle = Math.Atan2(wallEnd.Y - wallStart.Y, wallEnd.X - wallStart.X);
        //        PointF newVelocity = new PointF((float)(-velocity.Y * Math.Sin(angle) + velocity.X * Math.Cos(angle)),
        //                                        (float)(velocity.Y * Math.Cos(angle) + velocity.X * Math.Sin(angle)));
        //        Restart(position, newVelocity);
        //    }
        //    else
        //    {
        //        Position = position;
        //        Velocity = velocity;
        //    }
        //}

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
