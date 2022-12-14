using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author : Wendi Cai
/// </summary>
namespace Minerva.Module
{
    /// <summary>
    /// a utility class for more vector support
    /// </summary>
    public static class VectorUtilities
    {
        public static bool IsVector3(string sVector)
        {
            try
            {
                ToVector3(sVector);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static Vector3 ToVector3(string sVector)
        {
            // Remove the parentheses
            if (sVector.StartsWith("(") && sVector.EndsWith(")"))
            {
                sVector = sVector.Substring(1, sVector.Length - 2);
            }

            // split the items
            string[] sArray = sVector.Split(',');

            // store as a Vector3
            Vector3 result = new Vector3(
                float.Parse(sArray[0]),
                float.Parse(sArray[1]),
                float.Parse(sArray[2]));

            return result;
        }

        public static bool TryParseVector3(string defaultValue, out Vector3 val)
        {
            if (IsVector3(defaultValue))
            {
                val = ToVector3(defaultValue);
                return true;
            }
            val = default;
            return false;
        }


        public static bool IsVector2(string sVector)
        {
            try
            {
                ToVector2(sVector);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static Vector2 ToVector2(string sVector)
        {
            // Remove the parentheses
            if (sVector.StartsWith("(") && sVector.EndsWith(")"))
            {
                sVector = sVector.Substring(1, sVector.Length - 2);
            }

            // split the items
            string[] sArray = sVector.Split(',');

            // store as a Vector3
            Vector2 result = new Vector2(
                float.Parse(sArray[0]),
                float.Parse(sArray[1]));

            return result;
        }

        public static bool TryParseVector2(string defaultValue, out Vector2 val)
        {
            if (IsVector2(defaultValue))
            {
                val = ToVector2(defaultValue);
                return true;
            }
            val = default;
            return false;
        }


        public static Vector3Int ToVector3Int(string sVector)
        {
            // Remove the parentheses
            if (sVector.StartsWith("(") && sVector.EndsWith(")"))
            {
                sVector = sVector.Substring(1, sVector.Length - 2);
            }

            // split the items
            string[] sArray = sVector.Split(',');

            // store as a Vector3
            Vector3Int result = new Vector3Int(
                int.Parse(sArray[0]),
                int.Parse(sArray[1]),
                int.Parse(sArray[2]));

            return result;
        }

        public static Vector2Int ToVector2Int(string sVector)
        {
            // Remove the parentheses
            if (sVector.StartsWith("(") && sVector.EndsWith(")"))
            {
                sVector = sVector.Substring(1, sVector.Length - 2);
            }

            // split the items
            string[] sArray = sVector.Split(',');

            // store as a Vector3
            Vector2Int result = new Vector2Int(
                int.Parse(sArray[0]),
                int.Parse(sArray[1]));

            return result;
        }

        public static Vector2Int OneLeft(this Vector2Int vector2Int)
        {
            return vector2Int + Vector2Int.left;
        }
        public static Vector2Int OneRight(this Vector2Int vector2Int)
        {
            return vector2Int + Vector2Int.right;
        }
        public static Vector2Int OneUp(this Vector2Int vector2Int)
        {
            return vector2Int + Vector2Int.up;
        }
        public static Vector2Int OneDown(this Vector2Int vector2Int)
        {
            return vector2Int + Vector2Int.down;
        }

        public static Vector2 YVector(this Vector2 vector2)
        {
            return new Vector2(0, vector2.y);
        }

        public static Vector2Int YVector(this Vector2Int vector2Int)
        {
            return new Vector2Int(0, vector2Int.y);
        }


        public static Vector2 XVector(this Vector2 vector2)
        {
            return new Vector2(vector2.x, 0);
        }
        public static Vector2Int XVector(this Vector2Int vector2Int)
        {
            return new Vector2Int(vector2Int.x, 0);
        }


        public static Vector2Int ReflectY(this Vector2Int vector2Int)
        {
            return new Vector2Int(vector2Int.x, -vector2Int.y);
        }
        public static Vector2 ReflectY(this Vector2 vector2)
        {
            return new Vector2(vector2.x, -vector2.y);
        }
        public static Vector2Int ReflectX(this Vector2Int vector2Int)
        {
            return new Vector2Int(-vector2Int.x, vector2Int.y);
        }
        public static Vector2 ReflectX(this Vector2 vector2)
        {
            return new Vector2(-vector2.x, vector2.y);
        }


        public static Vector2 Rotate(this Vector2 v, float rad)
        {
            float cos = Mathf.Cos(rad);
            float sin = Mathf.Sin(rad);
            return new Vector2(v.x * cos - v.y * sin, v.x * sin + v.y * cos);
        }
        public static Vector2 Reverse(this Vector2 v)
        {
            return new Vector2(v.y, v.x);
        }

        public static Vector2 Abs(this Vector2 v)
        {
            return new Vector2(Mathf.Abs(v.x), Mathf.Abs(v.y));
        }

        public static Vector3 RotateX(this Vector3 v, float rad)
        {
            float cos = Mathf.Cos(rad);
            float sin = Mathf.Sin(rad);
            return new Vector3(v.x, v.y * cos - v.z * sin, v.y * sin + v.z * cos);
        }
        public static Vector3 RotateY(this Vector3 v, float rad)
        {
            float cos = Mathf.Cos(rad);
            float sin = Mathf.Sin(rad);
            return new Vector3(v.x * cos - v.z * sin, v.y, v.x * sin + v.z * cos);
        }
        public static Vector3 RotateZ(this Vector3 v, float rad)
        {
            float cos = Mathf.Cos(rad);
            float sin = Mathf.Sin(rad);
            return new Vector3(v.x * cos - v.y * sin, v.x * sin + v.y * cos, v.z);
        }

        public static Vector3 Abs(this Vector3 v)
        {
            return new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
        }

        public static Vector3 Modulo(this Vector3 v, float a)
        {
            return new Vector3(v.x % a, v.y % a, v.z % a);
        }
        public static Vector2 Modulo(this Vector2 v, float a)
        {
            return new Vector2(v.x % a, v.y % a);
        }

        public static IEnumerable<Vector2Int> Enumerate(this Vector2Int vector2Int)
        {
            for (int x = 0; x < vector2Int.x; x++)
            {
                for (int y = 0; y < vector2Int.y; y++)
                {
                    yield return new Vector2Int(x, y);
                }
            }
        }
        public static IEnumerable<Vector2Int> Enumerate(this Vector2Int vector3Int, bool x, bool y)
        {
            for (int ix = 0; ix < vector3Int.x; ix++)
            {
                for (int iy = 0; iy < vector3Int.y; iy++)
                {
                    Vector2Int vector = new(vector3Int.x, vector3Int.y);
                    if (x) vector.x = ix;
                    if (y) vector.y = iy;
                    yield return vector;
                }
            }
        }
        public static IEnumerable<Vector3Int> Enumerate(this Vector3Int vector3Int)
        {
            for (int x = 0; x < vector3Int.x; x++)
            {
                for (int y = 0; y < vector3Int.y; y++)
                {
                    for (int z = 0; z < vector3Int.z; z++)
                    {
                        yield return new Vector3Int(x, y, z);
                    }
                }
            }
        }
        public static IEnumerable<Vector3Int> Enumerate(this Vector3Int vector3Int, bool x, bool y, bool z)
        {
            for (int ix = 0; ix < vector3Int.x; ix++)
            {
                for (int iy = 0; iy < vector3Int.y; iy++)
                {
                    for (int iz = 0; iz < vector3Int.z; iz++)
                    {
                        Vector3Int vector = new(vector3Int.x, vector3Int.y, vector3Int.z);
                        if (x) vector.x = ix;
                        if (y) vector.y = iy;
                        if (z) vector.z = iz;
                        yield return vector;
                    }
                }
            }
        }


        public static Vector3Int ToVector3Int(this Vector2Int vector2Int, int z = 0)
        {
            return new Vector3Int(vector2Int.x, vector2Int.y, z);
        }






        public static Vector2 Random(float x, float y)
        {
            return new Vector2(UnityEngine.Random.value * x, UnityEngine.Random.value * y);
        }

        public static Vector2Int Random(int x, int y)
        {
            return new Vector2Int(UnityEngine.Random.Range(0, x), UnityEngine.Random.Range(0, y));
        }

        public static Vector3 Random(float x, float y, float z)
        {
            return new Vector3(UnityEngine.Random.value * x, UnityEngine.Random.value * y, UnityEngine.Random.value * z);
        }

        public static Vector3Int Random(int x, int y, int z)
        {
            return new Vector3Int(UnityEngine.Random.Range(0, x), UnityEngine.Random.Range(0, y), UnityEngine.Random.Range(0, z));
        }




        public static IEnumerable<Vector2Int> FourNeighbors(this Vector2Int vector2)
        {
            yield return vector2 + Vector2Int.right;
            yield return vector2 + Vector2Int.up;
            yield return vector2 + Vector2Int.left;
            yield return vector2 + Vector2Int.down;
        }
        public static IEnumerable<Vector2Int> EightNeighbors(this Vector2Int vector2)
        {
            yield return vector2 + Vector2Int.right;
            yield return vector2 + Vector2Int.right + Vector2Int.up;
            yield return vector2 + Vector2Int.up;
            yield return vector2 + Vector2Int.left + Vector2Int.up;
            yield return vector2 + Vector2Int.left;
            yield return vector2 + Vector2Int.left + Vector2Int.down;
            yield return vector2 + Vector2Int.down;
            yield return vector2 + Vector2Int.down + Vector2Int.right;
        }
        public static IEnumerable<Vector2Int> NeighborInRadiusOf(this Vector2Int vector2, float radius)
        {
            for (int i = (int)(vector2.x - radius); i < (int)(vector2.x + radius); i++)
            {
                for (int j = (int)(vector2.y - radius); j < (int)(vector2.y + radius); j++)
                {
                    Vector2Int vector2Int = new(i, j);
                    if ((vector2Int - vector2).magnitude <= radius)
                    {
                        yield return vector2Int;
                    }
                }
            }
        }
    }
}