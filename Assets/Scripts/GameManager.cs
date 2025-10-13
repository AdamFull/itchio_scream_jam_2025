using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject characterPrefab;

    [SerializeField]
    private Vector3 characterSpawnPoint = Vector3.zero;
    public static GameManager instance { get; private set; }

    public GameObject characterInstance { get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            //InitializeSystems();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        characterInstance = Instantiate(characterPrefab);
        characterInstance.transform.position = characterSpawnPoint;
        characterInstance.SetActive(true);
        //newCharacter.transform.SetParent(transform);
    }
}
