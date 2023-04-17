using SeonerTurnBasedRPG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerInputContext
{
    public IEnumerator ProcessInput(BasePlayerInput PlayerInput);
}
