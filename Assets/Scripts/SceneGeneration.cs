using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneGeneration : MonoBehaviour
{
    [Header("Block Prefabs")]
    [Tooltip("The block that appears at the start of the level")]
    public GameObject startBlock;

    [Tooltip("The block that appears at the end of the level")]
    public GameObject endBlock;

    [Tooltip("Common blocks that appear regularly")]
    public List<GameObject> commonBlocks = new List<GameObject>();

    [Tooltip("Special blocks that appear occasionally and only once")]
    public List<GameObject> specialBlocks = new List<GameObject>();

    [Header("Generation Settings")]
    [Tooltip("Distance between consecutive blocks in unity base units")]
    public float blockSpacing = 32f;

    [Tooltip("How many common blocks before checking for a special block")]
    public int commonBlockInterval = 5;

    [Tooltip("Probability (0-1) of spawning a special block at each interval")]
    [Range(0f, 1f)]
    public float specialBlockProbability = 0.5f;

    [Tooltip("Number of common blocks to spawn after all special blocks are used")]
    public int blocksAfterSpecials = 3;

    [Tooltip("Number of blocks to keep behind the player")]
    public int blocksToKeepBehind = 3;

    [Tooltip("Number of blocks to generate ahead of the player")]
    public int blocksToGenerateAhead = 3;

    private Transform playerTransform;
    private Queue<GameObject> activeBlocks = new Queue<GameObject>();
    private List<GameObject> remainingSpecialBlocks = new List<GameObject>();
    private float nextBlockPosition = 0f;
    private int commonBlockCounter = 0;
    private int blocksAfterSpecialsCounter = 0;
    private bool allSpecialsUsed = false;
    private bool endBlockSpawned = false;
    private bool isInitialized = false;

    private List<GameObject> shuffledCommonBlocks = new List<GameObject>();
    private int currentBlockIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize and generate some blocks on level startup
        Initialize();
    }

    void Initialize()
    {
        if (isInitialized) return;

        playerTransform = GameManager.instance.characterInstance.transform;

        // Copy special blocks to remaining list
        remainingSpecialBlocks = new List<GameObject>(specialBlocks);

        if (startBlock != null)
        {
            GameObject start = Instantiate(startBlock, new Vector3(0, 0, nextBlockPosition), Quaternion.identity);
            activeBlocks.Enqueue(start);
            nextBlockPosition += blockSpacing;
        }

        ShuffleBlocks();

        for (int i = 0; i < blocksToGenerateAhead; i++)
        {
            GenerateNextBlock();
        }

        isInitialized = true;
    }

    void ShuffleBlocks()
    {
        if (commonBlocks.Count == 0)
            return;

        GameObject lastPlaced = null;
        if (shuffledCommonBlocks.Count > 0 && currentBlockIndex > 0)
        {
            lastPlaced = shuffledCommonBlocks[currentBlockIndex - 1];
        }

        shuffledCommonBlocks = ShuffleUtility.SpotifyShuffle(commonBlocks, lastPlaced);

        // Reset index to start of new shuffle
        currentBlockIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTransform == null || !isInitialized) return;

        // Check if we need to generate more blocks ahead
        float playerForwardPosition = playerTransform.position.z;
        float furthestBlockPosition = nextBlockPosition - blockSpacing;

        if (furthestBlockPosition - playerForwardPosition < blocksToGenerateAhead * blockSpacing)
        {
            GenerateNextBlock();
        }

        // Remove blocks that are too far behind the player
        // TODO: May be try to optimize it?
        while (activeBlocks.Count > 0)
        {
            GameObject firstBlock = activeBlocks.Peek();
            if (firstBlock != null && firstBlock.transform.position.z < playerForwardPosition - (blocksToKeepBehind * blockSpacing))
            {
                GameObject blockToRemove = activeBlocks.Dequeue();
                Destroy(blockToRemove);
            }
            else
            {
                break;
            }
        }
    }

    // NOTE: Main level generation logic.
    // At first it should spawn some common blocks from common list
    // After some common blocks with some probability it can spawn special blocks with entities
    // When all special blocks was shown, we can spawn last one
    void GenerateNextBlock()
    {
        // If end block is spawned, don't generate more
        if (endBlockSpawned) return;

        GameObject blockToSpawn = null;

        // Check if all special blocks have been used
        if (!allSpecialsUsed && remainingSpecialBlocks.Count == 0)
        {
            allSpecialsUsed = true;
        }

        // If all specials are used, count down to end block
        if (allSpecialsUsed)
        {
            blocksAfterSpecialsCounter++;

            if (blocksAfterSpecialsCounter >= blocksAfterSpecials)
            {
                // Spawn end block
                if (endBlock != null)
                {
                    blockToSpawn = endBlock;
                    endBlockSpawned = true;
                }
                else
                {
                    blockToSpawn = GetRandomCommonBlock();
                }
            }
            else
            {
                blockToSpawn = GetRandomCommonBlock();
            }
        }
        else
        {
            // Normal generation logic
            commonBlockCounter++;

            // Check if it's time to potentially spawn a special block
            if (commonBlockCounter >= commonBlockInterval && remainingSpecialBlocks.Count > 0)
            {
                commonBlockCounter = 0;

                // Roll for special block
                if (Random.value <= specialBlockProbability)
                {
                    // Spawn a random special block
                    int randomIndex = Random.Range(0, remainingSpecialBlocks.Count);
                    blockToSpawn = remainingSpecialBlocks[randomIndex];
                    remainingSpecialBlocks.RemoveAt(randomIndex);
                }
                else
                {
                    blockToSpawn = GetRandomCommonBlock();
                }
            }
            else
            {
                blockToSpawn = GetRandomCommonBlock();
            }
        }

        // Spawn the selected block
        if (blockToSpawn != null)
        {
            GameObject newBlock = Instantiate(blockToSpawn, new Vector3(0, 0, nextBlockPosition), Quaternion.identity);
            activeBlocks.Enqueue(newBlock);
            nextBlockPosition += blockSpacing;
        }
    }

    GameObject GetRandomCommonBlock()
    {
        if (commonBlocks.Count == 0) return null;

        if (currentBlockIndex >= shuffledCommonBlocks.Count)
        {
            ShuffleBlocks();
        }

        return shuffledCommonBlocks[currentBlockIndex++];
    }

    // I think that this one will be needed in future
    public void ResetLevel()
    {
        while (activeBlocks.Count > 0)
        {
            GameObject block = activeBlocks.Dequeue();
            if (block != null)
                Destroy(block);
        }

        remainingSpecialBlocks = new List<GameObject>(specialBlocks);
        nextBlockPosition = 0f;
        commonBlockCounter = 0;
        blocksAfterSpecialsCounter = 0;
        allSpecialsUsed = false;
        endBlockSpawned = false;
        isInitialized = false;

        Initialize();
    }
}
