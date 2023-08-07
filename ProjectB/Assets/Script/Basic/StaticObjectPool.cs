using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    public class StaticeObjectPool
    {
        private static class Pool<E>
        {
            private static Lazy<Stack<E>> lazyLargeObject = new Lazy<Stack<E>>();
            private static Stack<E> pool => lazyLargeObject.Value;

            public static int Lent { get; private set; }
            public static int Return { get; private set; }

            static Pool()
            {

            }

            public static void Push(E obj)
            {
                if (obj == null) return;
                if (typeof(E).IsInterface) throw new Exception("Is Interface");

                lock (pool)
                {
                    ++Return;
                    pool.Push(obj);
                }
            }

            public static E Pop()
            {
                if (typeof(E).IsInterface) throw new Exception("Is Interface");

                lock (pool)
                {
                    ++Lent;
                    return pool.Count > 0 ? pool.Pop() : Activator.CreateInstance<E>();
                }
            }
        }

        public static void Push<E>(E obj)
        {
            Pool<E>.Push(obj);
        }
        public static E Pop<E>()
        {
            return Pool<E>.Pop();
        }
    }
}

