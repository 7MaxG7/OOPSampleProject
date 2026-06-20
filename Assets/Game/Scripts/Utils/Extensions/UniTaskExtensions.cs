using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Utils
{
    public static class UniTaskExtensions
    {
        public static void Update<T>(this AsyncReactiveProperty<T> property, T value) where T : struct
        {
            if (property.Value.IsEqual(value))
                return;

            property.Value = value;
        }

        public static void UpdateEnum<T>(this AsyncReactiveProperty<T> property, T value) where T : Enum
        {
            if (EqualityComparer<T>.Default.Equals(property.Value, value))
                return;

            property.Value = value;
        }
    }
}