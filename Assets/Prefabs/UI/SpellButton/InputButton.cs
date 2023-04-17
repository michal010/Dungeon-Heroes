using SeonerTurnBasedRPG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputButton : MonoBehaviour
{
    public BaseSpell spell;
    public IPlayerInputContext playerInputContext;
    public MapSegmentManager MSM;

    private void Start()
    {
        if (playerInputContext == null)
           playerInputContext =  MSM.fightManager as IPlayerInputContext;
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    public void Setup(BasePlayerInput playerInput, IPlayerInputContext context)
    {
        this.playerInputContext = context;
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        MSM.StartCoroutine( (playerInputContext as FightManager).UseSpell(spell));
    }
}
