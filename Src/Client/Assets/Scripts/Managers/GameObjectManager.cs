using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Entities;
using Services;
using SkillBridge.Message;

public class GameObjectManager : MonoBehaviour
{
    Dictionary<int, GameObject> Characters = new Dictionary<int, GameObject>();

    // Use this for initialization
    private void Start()
    {
        StartCoroutine(InitGameObjects());
        CharacterManager.Instance.OnCharacterEnter = OnCharacterEnter;
    }

    private void OnDestroy()
    {
        CharacterManager.Instance.OnCharacterEnter = null;
    }

    // Updata is called once per frame
    private void Update()
    {
        
    }

    private void OnCharacterEnter(Character cha)
    {
        CreateCharacterObject(cha);
    }

    IEnumerator InitGameObjects()
    {
        foreach (var cha in CharacterManager.Instance.Characters.Values)
        {
            CreateCharacterObject(cha);
            yield return null;
        }
    }

    private void CreateCharacterObject(Character character)
    {
        if (!Characters.ContainsKey(character.Info.Id) || Characters[character.Info.Id] == null)
        {
            Object obj = Resloader.Load<Object>(character.Define.Resource);
            if(obj == null)
            {
                Debug.LogErrorFormat("Character: [{0}], Resource: [{1}] not existed.", character.Define.TID, character.Define.Resource);
                return;
            }
            GameObject go = (GameObject)Instantiate(obj);
            go.name = "Character_" + character.Info.Id + "_" + character.Info.Name;

            go.transform.position = GameObjectTool.LogicToWorld(character.position);
            go.transform.forward = GameObjectTool.LogicToWorld(character.direction);
            Characters[character.Info.Id] = go;

            EntityController ec = go.GetComponent<EntityController>();
            if (ec != null)
            {
                ec.entity = character;
                ec.isPlayer = character.IsPlayer;
            }
            
            PlayerInputController pc = go.Get
        }
    }
}