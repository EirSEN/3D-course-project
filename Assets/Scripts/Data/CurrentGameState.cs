using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity3DCourse.Data
{
    public class CurrentGameState
    {
        public string Name;
        public float Hp;
        public bool IsVisible;
        public List<HealthPack> MedKits;
    }
}
