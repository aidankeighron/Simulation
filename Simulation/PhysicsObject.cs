using System;
using System.Drawing;

namespace Simulation
{
    class PhysicsObject
    {
        // Position
        protected Vector StartPosition = new Vector(100, 100);
        protected Vector Position = new Vector(0, 0);

        // Velocity
        protected Vector StartVelocity = new Vector(0, 0);
        protected Vector Velocity = new Vector(0, 0);

        // Acceleration
        protected Vector Acceleration = new Vector(0, 0);

        // Coefficients
        protected double CoefficientOfRestitution = 0.8;

        // State
        protected double Timestamp = 0;

        virtual
        public void tick(double dt, Wall wall)
        {
            Timestamp += dt;
            HandleGravity(wall);
        }

        public void HandleGravity(Wall wall)
        {
            Vector newPosition = new Vector(0, 0);
            Vector newVelocity = new Vector(0, 0);
            newPosition.X = StartPosition.X + StartVelocity.X * Timestamp;
            newPosition.Y = StartPosition.Y + StartVelocity.Y * Timestamp + 0.5f * Simulation.Gravity * Timestamp * Timestamp;
            newVelocity.X = StartVelocity.X;
            newVelocity.Y = StartVelocity.Y + Simulation.Gravity * Timestamp;
            tryMove(newPosition, newVelocity, wall);
            Acceleration.Y = Simulation.Gravity;
        }

        public void Restart(Vector position, Vector velocity)
        {
            Restart(position.X, position.Y, velocity.X, velocity.Y);
        }

        public void Restart(double x, double y, double vx, double vy)
        {
            StartPosition.X = x;
            StartPosition.Y = y;
            Position = StartPosition;
            StartVelocity.X = vx;
            StartVelocity.Y = vy;
            Velocity = StartVelocity;
            Timestamp = 0;
        }

        virtual
        protected void tryMove(Vector position, Vector velocity, Wall wall)
        {
            if (wall.pointCollides(position))
            {
                // Hit
                Vector wallStart = wall.getWall()[0];
                Vector wallEnd = wall.getWall()[1];
                Vector wallAngle = new Vector(wallEnd.Y - wallStart.Y, wallEnd.X - wallStart.X);
                double flip = velocity.Y > (wallStart.Y - wallEnd.Y) / (wallStart.X - wallEnd.X) * (velocity.X - wallStart.X) + wallStart.Y ? -1 : 1;
                double angle = flip * velocity.Angle(wallAngle) - Math.Atan(wallAngle.Y / wallAngle.X);
                //Console.WriteLine($"{velocity.ToString()}|{wallAngle.ToString()} {velocity.Angle(wallAngle)}+{-Math.Atan(wallAngle.Y / wallAngle.X)}={angle * 180.0 / Math.PI}");
                Vector newVelocity = new Vector(velocity.Magnitude() * Math.Cos(angle), velocity.Magnitude() * Math.Sin(angle));
                if (flip == -1)
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

        public Vector getPosition() { return Position; }
        public Vector getStartPosition() { return StartPosition; }
        public Vector getVelocity() { return Velocity; }
        public Vector getStartVelocity() { return StartVelocity; }
        public Vector getAcceleration() { return Acceleration; }
    }

    class CubePhysics : PhysicsObject
    {
        private double CubeSize;

        public CubePhysics(double cubeSize)
        {
            this.CubeSize = cubeSize;
        }

        override
        public void tick(double dt, Wall wall)
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
            double xPos = this.Position.X - CubeSize / 2.0;
            double yPos = this.Position.Y - CubeSize / 2.0;
            g.DrawRectangle(new Pen(Color.Black, 1), (float)xPos, (float)yPos, (float)CubeSize, (float)CubeSize);
        }

        public void BoundValues()
        {
            bool rightWall = Position.X <= 0 + CubeSize / 2.0f;
            bool leftWall = Position.X >= Simulation.ScreenWidth - CubeSize / 2.0f;
            bool topWall = Position.Y <= 0 + CubeSize / 2.0f;
            bool bottomWall = Position.Y >= Simulation.ScreenHeight - CubeSize / 2.0f;
            double posX = rightWall ? 0 + CubeSize / 2.0f : leftWall ? Simulation.ScreenWidth - CubeSize / 2.0f : Position.X;
            double velX = Velocity.X * this.CoefficientOfRestitution * ((rightWall || leftWall) ? -1 : 1);
            double posY = topWall ? 0 + CubeSize / 2.0f : bottomWall ? Simulation.ScreenHeight - CubeSize / 2.0f : Position.Y;
            double velY = Velocity.Y * this.CoefficientOfRestitution * ((topWall || bottomWall) ? -1 : 1);
            if (rightWall || leftWall || topWall || bottomWall)
                this.Restart(posX, posY, velX, velY);
        }
    }
}
