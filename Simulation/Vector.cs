﻿using System;
using System.Drawing;

namespace Simulation
{
    internal class Vector
    {
        public double X = 0;
        public double Y = 0;

        public Vector(double x, double y)
        {
            X = x;
            Y = y;
        }

        public static Vector operator +(Vector a, Vector b)
        {
            return new Vector(a.X + b.X, a.Y + b.Y);
        }

        public static Vector operator -(Vector a, Vector b)
        {
            return new Vector(a.X - b.X, a.Y - b.Y);
        }

        public static Vector operator *(Vector a, double b)
        {
            return new Vector(a.X * b, a.Y * b);
        }

        public static Vector operator /(Vector a, double b)
        {
            return new Vector(a.X / b, a.Y / b);
        }

        public static Vector operator -(Vector a)
        {
            return new Vector(-a.X, -a.Y);
        }

        public static Vector FromPoint(Point point)
        {
            return new Vector(point.X, point.Y);
        }

        public double Magnitude()
        {
            return Math.Sqrt(X * X + Y * Y);
        }

        public double Angle()
        {
            return Math.Atan2(Y, X);
        }

        public double Angle(Vector other)
        {
            return Math.Acos(Dot(this + other) / (Magnitude() * other.Magnitude()));
        }

        public double Dot(Vector other)
        {
            return X * other.X + Y * other.Y;
        }

        public double Cross(Vector other)
        {
            return Y * other.X - X * other.Y;
        }
    }
}
