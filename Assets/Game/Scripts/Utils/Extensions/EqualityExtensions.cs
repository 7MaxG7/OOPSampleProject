using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public static class EqualityExtensions
    {
        private const float EPSILON = 1e-6f;
        private const float SQR_EPSILON = 1e-12f;
        
        public static bool IsEqual(this Vector3 value, Vector3 other)
            => (value - other).sqrMagnitude <= SQR_EPSILON;
   
        public static bool IsEqual(this float value, float other)
            => Math.Abs(value - other) <= EPSILON;
   
        public static bool IsZero(this float value)
            => value.IsEqual(0f);

        public static bool IsEqual<T>(this T first, T second) where T : struct
            => EqualityComparer<T>.Default.Equals(first, second);

        public static bool IsAlmostOne(this float value)
            => value.IsEqual(1);
    }
}