using System;
using System.Collections;
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
        protected double CoefficientOfRestitution = 0;

        // State
        protected double Timestamp = 0;

        virtual
        public void tick(double dt, Wall wall, Graphics g)
        {
            Timestamp += dt;
            HandleGravity(wall, g);
        }

        public void HandleGravity(Wall wall, Graphics g)
        {
            Vector newPosition = new Vector(0, 0);
            Vector newVelocity = new Vector(0, 0);
            newPosition.X = StartPosition.X + StartVelocity.X * Timestamp;
            newPosition.Y = StartPosition.Y + StartVelocity.Y * Timestamp + 0.5f * Simulation.Gravity * Timestamp * Timestamp;
            newVelocity.X = StartVelocity.X;
            newVelocity.Y = StartVelocity.Y + Simulation.Gravity * Timestamp;
            tryMove(newPosition, newVelocity, wall, g);
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
        public ArrayList Vectors = new ArrayList();
        virtual
        protected void tryMove(Vector position, Vector velocity, Wall wall, Graphics g)
        {
            for (int i = 0; i < Vectors.Count; i++)
            {
                g.FillRectangle((Brush)Brushes.Red, (int)((Vector)Vectors[i]).X, (int)((Vector)Vectors[i]).Y, 5, 5);
            }
            if (wall.pointCollides(position))
            {
                Vectors.Add(position);

                // Hit
                Vector wallStart = wall.getWall()[0];
                Vector wallEnd = wall.getWall()[1];
                Vector wallAngle = new Vector(wallEnd.X - wallStart.X, wallEnd.Y - wallStart.Y);
                double flip = velocity.Y > (wallStart.Y - wallEnd.Y) / (wallStart.X - wallEnd.X) * (velocity.X - wallStart.X) + wallStart.Y ? -1 : 1;
                double angle = flip * velocity.Angle(wallAngle) - Math.Atan(wallAngle.Y / wallAngle.X);
                if (flip == -1)
                {
                    angle *= -1;
                }
                Console.WriteLine(Velocity);
                Console.WriteLine(CoefficientOfRestitution);
                Vector newVelocity = new Vector(Velocity.Magnitude() * Math.Cos(angle) * CoefficientOfRestitution, Velocity.Magnitude() * Math.Sin(angle) * CoefficientOfRestitution);

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
            this.CoefficientOfRestitution = 0.8;
        }

        override
        public void tick(double dt, Wall wall, Graphics g)
        {
            base.tick(dt, wall, g);
            BoundValues();
        }

        //override
        //protected void tryMove(Vector position, Vector velocity, Wall wall, Graphics g)
        //{
        //    if (wall.lineCollides(new Vector(position.X - CubeSize / 2, position.Y + CubeSize / 2), new Vector(position.X + CubeSize / 2, position.Y + CubeSize / 2)))
        //    {

        //        // Hit
        //        Vector wallStart = wall.getWall()[0];
        //        Vector wallEnd = wall.getWall()[1];
        //        Vector wallAngle = new Vector(wallEnd.Y - wallStart.Y, wallEnd.X - wallStart.X);
        //        double flip = velocity.Y > (wallStart.Y - wallEnd.Y) / (wallStart.X - wallEnd.X) * (velocity.X - wallStart.X) + wallStart.Y ? -1 : 1;
        //        double angle = flip * velocity.Angle(wallAngle) - Math.Atan(wallAngle.Y / wallAngle.X);
        //        //Console.WriteLine($"{velocity.ToString()}|{wallAngle.ToString()} {velocity.Angle(wallAngle)}+{-Math.Atan(wallAngle.Y / wallAngle.X)}={angle * 180.0 / Math.PI}");
        //        Vector newVelocity = new Vector(velocity.Magnitude() * Math.Cos(angle), velocity.Magnitude() * Math.Sin(angle));
        //        if (flip == -1)
        //        {
        //            newVelocity.X *= -1;
        //        }

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

    class SpherePhysics : PhysicsObject
    {
        private double SphereRadius;
        private double Angle = 0;
        private double RotationalVelocity = 0;

        public SpherePhysics(double sphereRadius)
        {
            this.SphereRadius = sphereRadius;
            this.CoefficientOfRestitution = 0.1;
        }

        override
        public void tick(double dt, Wall wall, Graphics g)
        {
            Angle = RotationalVelocity * dt;
            base.tick(dt, wall, g);
            BoundValues();
        }

        override
        protected void tryMove(Vector position, Vector velocity, Wall wall, Graphics g)
        {
            double H0 = Simulation.Gravity * this.Position.Y;
            double H1 = Simulation.Gravity * position.Y;
            double W0 = 0.25 * SphereRadius * SphereRadius * RotationalVelocity * RotationalVelocity;
            double V0 = 0.5 * this.Velocity.Magnitude() * this.Velocity.Magnitude();
            double V1 = 0.5 * velocity.Magnitude() * velocity.Magnitude();
            // TODO Plus or minus
            double W1 = Math.Sqrt(Math.Abs((W0 + V0 - V1 + H0 - H1 /*- uFn / M * distance*/) / (0.25 * SphereRadius * SphereRadius)));
            RotationalVelocity = W1;

            if (wall.pointCollides(position, 0.1))
            {
                // Hit
                Vector wallStart = wall.getWall()[0];
                Vector wallEnd = wall.getWall()[1];
                Vector wallAngle = new Vector(wallEnd.X - wallStart.X, wallEnd.Y - wallStart.Y);
                double flip = velocity.Y > (wallStart.Y - wallEnd.Y) / (wallStart.X - wallEnd.X) * (velocity.X - wallStart.X) + wallStart.Y ? -1 : 1;
                double angle = flip * velocity.Angle(wallAngle) - Math.Atan(wallAngle.Y / wallAngle.X);
                if (flip == -1)
                {
                    angle *= -1;
                }
                Vector newVelocity = new Vector(Velocity.Magnitude() * Math.Cos(angle) * CoefficientOfRestitution, Velocity.Magnitude() * Math.Sin(angle) * CoefficientOfRestitution);

                Restart(position, newVelocity);
            }
            else
            {
                this.Velocity = velocity;
                this.Position = position;
            }
        }

        public void DrawSphere(Graphics g)
        {
            double xPos = this.Position.X - SphereRadius / 2.0;
            double yPos = this.Position.Y - SphereRadius / 2.0;
            g.DrawEllipse(new Pen(Color.Black, 1), (float)xPos, (float)yPos, (float)SphereRadius, (float)SphereRadius);
            g.DrawLine(new Pen(Color.Red, 1), Position.ToPoint(), (Position + new Vector(SphereRadius * Math.Cos(Angle), SphereRadius * Math.Sin(Angle))).ToPoint());
        }

        public void BoundValues()
        {
            bool rightWall = Position.X <= 0 + SphereRadius / 2.0f;
            bool leftWall = Position.X >= Simulation.ScreenWidth - SphereRadius / 2.0f;
            bool topWall = Position.Y <= 0 + SphereRadius / 2.0f;
            bool bottomWall = Position.Y >= Simulation.ScreenHeight - SphereRadius / 2.0f;
            double posX = rightWall ? 0 + SphereRadius / 2.0f : leftWall ? Simulation.ScreenWidth - SphereRadius / 2.0f : Position.X;
            double velX = Velocity.X * this.CoefficientOfRestitution * ((rightWall || leftWall) ? -1 : 1);
            double posY = topWall ? 0 + SphereRadius / 2.0f : bottomWall ? Simulation.ScreenHeight - SphereRadius / 2.0f : Position.Y;
            double velY = Velocity.Y * this.CoefficientOfRestitution * ((topWall || bottomWall) ? -1 : 1);
            if (rightWall || leftWall || topWall || bottomWall)
                this.Restart(posX, posY, velX, velY);
        }
    }
}
