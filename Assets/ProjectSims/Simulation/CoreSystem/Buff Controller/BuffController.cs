using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Simulation.BuffSystem
{
    public class BuffController
    {
        public List<Buff> Buffs { get; private set; } = new();

        public void Update(float dt)
        {
            var count = Buffs.Count;
            for (int i = count - 1; i >= 0; i--)
            {
                Buffs[i].Update(dt);
            }
        }

        public void Add(Buff buff)
        {
            Buffs.Add(buff);
        }

        public void Remove(Buff buff)
        {
            if (!Buffs.Contains(buff))
            {
                return;
            }
            Buffs.Remove(buff);
        }
    }
}