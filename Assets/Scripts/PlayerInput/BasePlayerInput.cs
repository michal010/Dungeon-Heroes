using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeonerTurnBasedRPG
{
    public abstract class BasePlayerInput : ScriptableObject
    {
        public abstract IEnumerator Execute(IPlayerInputContext InputContext);
    }
}

