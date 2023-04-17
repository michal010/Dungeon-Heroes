using SeonerTurnBasedRPG;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Flags]
public enum ControllType { AI, Player}

public enum PlayerInputType { None, Skip, SpellUse }

public class FightManager: IPlayerInputContext
{
    private MapSegmentManager mapSegmentManager;
    private UIManager uiManager;
    private ControllType ControllType;

    List<BaseCharacter> allies;
    List<BaseCharacter> enemies;
    
    List<BaseCharacter> allCharacters;

    public BaseCharacter currentCharacter;

    List<BaseCharacter> charactersInActionOrder;

    private int currentCharacterIndex = -1;

    public PlayerInputType playerInputType;

    private bool FightEnd = false;

    public FightManager(MapSegmentManager mapSegmentManager, UIManager uiManager, List<BaseCharacter> Allies, List<BaseCharacter> Enemies)
    {
        this.mapSegmentManager = mapSegmentManager;
        this.uiManager = uiManager;
        allies = new List<BaseCharacter>();
        enemies = new List<BaseCharacter>();
        charactersInActionOrder = new List<BaseCharacter>();
        allCharacters = new List<BaseCharacter>();

        playerInputType = PlayerInputType.None;

        foreach (var c in Allies)
        {
            BaseCharacter chara = c.Clone<BaseCharacter>();
            chara.Data = c.Data.Clone<BaseCharacterData>();
            allies.Add(chara);
        }

        foreach (var c in Enemies)
        {
            BaseCharacter chara = c.Clone<BaseCharacter>();
            chara.Data = c.Data.Clone<BaseCharacterData>();
            enemies.Add(chara);
        }

        allCharacters.AddRange(allies);
        allCharacters.AddRange(enemies);

        foreach (var c in allCharacters)
        {
            c.OverTimeEffects = new List<BaseOverTimeEffect>();
        }

        for (int i = 0; i < Allies.Count; i++)
        {
            allies[i].Data.characterReference = mapSegmentManager.SpawnCharacter(Allies[i], new Vector2(-3.2f, -1.6f));
            allies[i].Data.characterReference.GetComponent<CharacterReference>().Character = allies[i];
            uiManager.SpawnResourceProgressBar(new Vector2(-3.2f, -1.6f), allies[i].Data.GetResourdeOfType(ResourceType.MaxHealth), allies[i].Data.GetResourdeOfType(ResourceType.Health));

        }

        for (int i = 0; i < Enemies.Count; i++)
        {
            enemies[i].Data.characterReference = mapSegmentManager.SpawnCharacter(Enemies[i], new Vector2(3f, -1.6f));
            enemies[i].Data.characterReference.GetComponent<CharacterReference>().Character = enemies[i];
        }

    }

    private void ResolveControlType()
    {
        if (allies.Contains(currentCharacter))
        {
            playerInputType = PlayerInputType.None;
            ControllType = ControllType.Player;
        }
        else
            ControllType = ControllType.AI;
    }

    public IEnumerator Fight()
    {
        //Int for testing only
        CalculateCharactersMoveOrder();
        while (!FightEnd)
        {
            currentCharacter = GetNextCharacter();
            ResolveControlType();
            yield return OnCharacterMove();
        }
        yield return null;
    }


    private IEnumerator OnCharacterMove()
    {
        yield return OnRoundStarted();
        yield return OnRoundMain();
        yield return OnRoundEnded();

        //end of character move
    }

    private IEnumerator OnRoundStarted()
    {
        Debug.Log("On Round Start for: " + currentCharacter.name);
        foreach (var effect in currentCharacter.OverTimeEffects)
        {
            yield return effect.OnRoundStared(currentCharacter);
        }
        yield return null;
    }
    
    private IEnumerator OnRoundMain()
    {
        Debug.Log("On Round Main for: " + currentCharacter.name);
        foreach (var effect in currentCharacter.OverTimeEffects)
        {
            yield return effect.OnRoundMain(currentCharacter);
        }

        if(ControllType == ControllType.Player)
        {
            Debug.Log("Controlled by player");
            //Await for player input
            yield return AwaitForPlayerInput();
        }
        else if(ControllType == ControllType.AI)
        {
            Debug.Log("Controlled by AI, awaiting 1 second...");
            yield return new WaitForSeconds(1f);
            //Process AI move.
        }
        yield return null;
    }

    private IEnumerator OnRoundEnded()
    {
        Debug.Log("On Round Ended for: " + currentCharacter.name);
        foreach (var effect in currentCharacter.OverTimeEffects)
        {
            yield return effect.OnRoundEnded(currentCharacter);
        }
        yield return null;
    }

    //To be removed?
    public IEnumerator ProcessInput(BasePlayerInput PlayerInput)
    {
        //Give PlayerInput scriptable object to process the input!.
        yield return PlayerInput.Execute(this);
    }

    private IEnumerator AwaitForPlayerInput()
    {
        Debug.Log("Awaiting for player move...");
        while (playerInputType == PlayerInputType.None)
        {
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }

    public IEnumerator UseSpell(BaseSpell spell)
    {
        Debug.Log("Using spell...");
        List<BaseCharacter> selectedTargets = new List<BaseCharacter>();
        yield return SelectTarget(returnValue => selectedTargets = returnValue);
        Debug.Log("Targets has been selected...");
        Debug.Log("Previous: " + selectedTargets[0].Data.GetResourdeOfType(ResourceType.Health).Value);
        yield return spell.Execute(currentCharacter, selectedTargets);
        playerInputType = PlayerInputType.SpellUse;
        Debug.Log("After: " + selectedTargets[0].Data.GetResourdeOfType(ResourceType.Health).Value);
    }

    private IEnumerator SelectTarget(Action<List<BaseCharacter>> callback = null)
    {
        List<BaseCharacter> selectedCharacters = new List<BaseCharacter>();
        Debug.Log("Selecting target...");
        //show indicator
        while(true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                // try get target
                // but how to get the target
                // raycast then if hit, 
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if(hit.collider != null)
                {
                    //Add some schema which character can be selected.
                    BaseCharacter baseChar = hit.collider.gameObject.GetComponent<CharacterReference>().Character;
                    if(baseChar != null)
                    {
                        selectedCharacters.Add(baseChar);
                        callback.Invoke(selectedCharacters);
                        yield break;
                    }
                }
            }
            if (Input.GetMouseButtonDown(1))
            {
                //Break the input
                Debug.Log("Mouse 1");
                yield break;
            }
            yield return null;
        }
    }

    private BaseCharacter GetNextCharacter()
    {
        currentCharacterIndex++;
        if(currentCharacterIndex > charactersInActionOrder.Count - 1)
        {
            //round over.
            CalculateCharactersMoveOrder();
            currentCharacterIndex = 0;
        }
            return charactersInActionOrder[currentCharacterIndex];
    }

    private void CalculateCharactersMoveOrder()
    {
        charactersInActionOrder = allCharacters.OrderByDescending(c => c.Data.GetResourdeOfType(ResourceType.Initiative).Value).ToList();
    }

    public List<BaseCharacter> GetAffectedCharacters(AffectedPosition position, BaseCharacter getFromCharacter = null)
    {
        List<BaseCharacter> getFrom = new List<BaseCharacter>(); 

        switch (position)
        {
            case AffectedPosition.A1:
                getFrom.Add(allies[0]);
                break;
            case AffectedPosition.A2:
                getFrom.Add(allies[1]);
                break;
            case AffectedPosition.A3:
                getFrom.Add(allies[2]);
                break;
            case AffectedPosition.A4:
                getFrom.Add(allies[3]);
                break;
            case AffectedPosition.E1:
                getFrom.Add(enemies[0]);
                break;
            case AffectedPosition.E2:
                getFrom.Add(enemies[1]);
                break;
            case AffectedPosition.E3:
                getFrom.Add(enemies[2]);
                break;
            case AffectedPosition.E4:
                getFrom.Add(enemies[3]);
                break;
            case AffectedPosition.Plus1:
                if(allies.Contains(getFromCharacter))
                {
                    int index = allies.IndexOf(getFromCharacter) + 1;
                    if (index > allies.Count - 1)
                        index = 0;
                    getFrom.Add(allies[index]);
                }
                else if(enemies.Contains(getFromCharacter))
                {
                    int index = enemies.IndexOf(getFromCharacter) + 1;
                    if (index > enemies.Count - 1)
                        index = 0;
                    getFrom.Add(enemies[index]);
                }
                break;
            case AffectedPosition.Plus2:
                if (allies.Contains(getFromCharacter))
                {
                    int index = allies.IndexOf(getFromCharacter) + 2;
                    if (index > allies.Count - 1)
                        index = 0;
                    getFrom.Add(allies[index]);
                }
                else if (enemies.Contains(getFromCharacter))
                {
                    int index = enemies.IndexOf(getFromCharacter) + 2;
                    if (index > enemies.Count - 1)
                        index = 0;
                    getFrom.Add(enemies[index]);
                }
                break;
            case AffectedPosition.Minus1:
                if (allies.Contains(getFromCharacter))
                {
                    int index = allies.IndexOf(getFromCharacter) - 1;
                    if (index < 0) 
                        index = allies.Count - 1;
                    getFrom.Add(allies[index]);
                }
                else if (enemies.Contains(getFromCharacter))
                {
                    int index = enemies.IndexOf(getFromCharacter) - 1;
                    if (index < 0)
                        index = enemies.Count - 1;
                    getFrom.Add(enemies[index]);
                }
                break;
            case AffectedPosition.Minus2:
                if (allies.Contains(getFromCharacter))
                {
                    int index = allies.IndexOf(getFromCharacter) - 2;
                    if (index < 0)
                        index = allies.Count - 1;
                    getFrom.Add(allies[index]);
                }
                else if (enemies.Contains(getFromCharacter))
                {
                    int index = enemies.IndexOf(getFromCharacter) - 2;
                    if (index < 0)
                        index = enemies.Count - 1;
                    getFrom.Add(enemies[index]);
                }
                break;
        }

        return getFrom;
    }
}
