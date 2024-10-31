using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace QUtility
{
    public static class MathUtility
    {
        public static Vector3 GetRandomVector3(float minX, float maxX, float minY, float maxY)
        {
            return new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), 0);
        }
    }
}
