using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Readme", menuName = "Create Readme")]
public class Readme : ScriptableObject
{
    public string Title;
    [TextArea(3, 5)] public string Description;

    public List<ControlEntry> Controls = new List<ControlEntry>();

    [Serializable]
    public class ControlEntry
    {
        public string Action;
        public string Key;
    }
}
