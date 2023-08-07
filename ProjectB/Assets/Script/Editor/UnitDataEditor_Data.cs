using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[Serializable]
public class UnitDataEditor_Data
{
    public int Seed;
    public string Name;
    public Info info = new();

    public UnitDataEditor_Data(int seed, string name)
    {
        Seed = seed;
        Name = name;
    }

    [Serializable]
    public class Info
    {
        public int hp;
        public Vector3 pos;
        public Texture2D texture;
        public UnityEngine.Object objectValue;
    }
}
