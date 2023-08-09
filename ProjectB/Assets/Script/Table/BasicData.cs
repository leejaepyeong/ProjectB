using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MessagePack;
using UnityEngine;

namespace Data
{
    public interface IDataKey<E>
    {
        public E Key { get; }
    }

    [MessagePackObject(true)]
    public class ArrayMeta
    {
        public Dummy[] Dummy { get; }
    }
    [MessagePackObject(true)]
    public class ListMeta
    {
        public List<Dummy> Dummy { get; }
    }


    [MessagePackObject(true)]
    public class Dummy : IDataKey<int>
    {
        public int Seed;
        public float Value;

        public Dummy(int seed, float value)
        {
            Seed = seed;
            Value = value;
        }

        [IgnoreMember]
        public int Key => Seed;
    }

}