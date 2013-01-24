using System.Drawing;
using System;

namespace ProUpgradeEditor.Common
{
    public struct Vector2
    {
        public float X;
        public float Y;

        public Vector2(Vector2 b)
        {
            X = b.X;
            Y = b.Y;
        }
        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }
        public void Normalize()
        {
            var lensq = LengthSq;
            if (lensq != 0 && lensq != 1.0f)
            {
                var l = (float)Math.Sqrt(lensq);
                X = X / l;
                Y = Y / l;
            }
        }

        public Vector2 Normal
        {
            get
            {
                Vector2 ret = this;
                var lensq = LengthSq;
                if (lensq != 0 && lensq != 1.0f)
                {
                    var l = (float)Math.Sqrt(lensq);
                    ret.X = X / l;
                    ret.Y = Y / l;
                }
                return ret;
            }
        }

        public float LengthSq
        {
            get { return X * X + Y * Y; }
        }

        public float Length
        {
            get { return (float)Math.Sqrt(X * X + Y * Y); }
        }

        public float Dot(Vector2 b)
        {
            return X * b.X + Y * b.Y;
        }

        public float DistanceSq(Vector2 b)
        {
            return (float)(Math.Pow(X - b.X, 2) + Math.Pow(Y - b.Y, 2));
        }

        public float Distance(Vector2 b)
        {
            return (float)Math.Sqrt(Math.Pow(X - b.X, 2) + Math.Pow(Y - b.Y, 2));
        }

        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X - b.X, a.Y - b.Y);
        }
        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X + b.X, a.Y + b.Y);
        }

        public static Vector2 operator *(Vector2 a, float f)
        {
            return new Vector2(a.X * f, a.Y * f);
        }
        public static Vector2 operator *(float f, Vector2 a)
        {
            return new Vector2(a.X * f, a.Y * f);
        }
        public static Vector2 operator *(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X * b.X, a.Y * b.Y);
        }


        public static bool PointInBox(Vector2 v, Vector2 boxMin, Vector2 boxMax)
        {
            return (v.X > boxMin.X && v.X < boxMax.X &&
                    v.Y > boxMin.Y && v.Y < boxMax.Y);
        }

        public static bool PointTouchesBox(Vector2 v, Vector2 boxMin, Vector2 boxMax)
        {
            return (v.X >= boxMin.X && v.X <= boxMax.X &&
                    v.Y >= boxMin.Y && v.Y <= boxMax.Y);
        }

        public static Vector2 Min(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X < b.X ? a.X : b.X, a.Y < b.Y ? a.Y : b.Y);
        }

        public static Vector2 Max(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X > b.X ? a.X : b.X, a.Y > b.Y ? a.Y : b.Y);
        }

        public static Vector2 ClosestPointOnBox(Vector2 v, Vector2 boxMin, Vector2 boxMax)
        {
            return new Vector2(
                v.X < boxMin.X ? boxMin.X : v.X > boxMax.X ? boxMax.X : v.X,
                v.Y < boxMin.Y ? boxMin.Y : v.Y > boxMax.Y ? boxMax.Y : v.Y);
        }

        public static float DistanceToBox(Vector2 v, Vector2 boxMin, Vector2 boxMax)
        {
            return PointTouchesBox(v, boxMin, boxMax) ? 0 : ClosestPointOnBox(v, boxMin, boxMax).Distance(v);
        }

        public static Vector2 ClosestPointOnSegment(Vector2 v, Vector2 a, Vector2 b)
        {
            var n = (b - a).Normal;
            float t = n.Dot(v - a);
            if (t < 0)
                return a;
            float d = (b - a).Length;
            if (t > d)
                return b;
            return a + n * t;
        }
    }


}
