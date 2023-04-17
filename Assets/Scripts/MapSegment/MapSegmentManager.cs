using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum MapType { Fight }

public class MapSegmentManager : MonoBehaviour
{
    public FightManager fightManager;
    public UIManager uiManager;
    //Progress bar related
    public GameObject ProgressBarPrefab;
    
    
    public List<BaseCharacter> testAlly;
    public List<BaseCharacter> testEnemy;

    private void Start()
    {
        LoadData(testAlly, testEnemy);
        Debug.Log ("Trying to start fight...");
        StartFight();
        Debug.Log("Fight started");
    }

    public GameObject SpawnCharacter(BaseCharacter character, Vector2 position)
    {
        GameObject go = Instantiate(character.Data.prefab, position, Quaternion.identity);
        return go;
    }

    public void LoadData(List<BaseCharacter> allies, List<BaseCharacter> enemies)
    {
        fightManager = new FightManager(this, uiManager, allies, enemies);
    }

    public void StartFight()
    {
        StartCoroutine(fightManager.Fight());
    }

    private void Update()
    {

    }
}
