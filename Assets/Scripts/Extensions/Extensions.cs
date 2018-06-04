using System;
using System.Collections;
using UnityEngine;

namespace Unity3DCourse.Helpers
{
    public static class Extensions
    {
        public static bool TryBool(this string self)
        {
            bool result;
            return Boolean.TryParse(self, out result) && result;
        }

        public static IEnumerator WaitForFrames(int frameCount)
        {
            for (int i = 0; i < frameCount; i++)
            {
                yield return null;
            }
        }
    }
}