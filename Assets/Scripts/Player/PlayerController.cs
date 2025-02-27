using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public MMap map;
    public float moveSpeed = 2.5f;
    public float moveTime = 0.75f;
    private float moveWaitTime;

    private MapNode currentNode;
    private bool diceInHand = false;
    private bool iWannaToDice = false;

    private int tmpDiceResult;

    public bool isAI = false;

    //其他属性和方法

    void Start()
    {
        currentNode = map.firstNode;
        transform.position = currentNode.position + new Vector3(0,0.5f,0);
        moveWaitTime = moveTime + 0.25f;
    }

    // void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.Space))
    //     {
    //         MoveToNextNode();
    //     }
    // }

    public void GiveMeDice()
    {
        this.iWannaToDice = false;
        this.diceInHand = true;
    }

    public void LoseMyDice()
    {
        this.iWannaToDice = false;
        this.diceInHand = false;
    }

    public void IWannaToDice()
    {
        this.iWannaToDice = true;
    }

    public int getTmpDiceResult()
    {
        return tmpDiceResult;
    }

    public void iAmAI()
    {
        this.isAI = true;
    }

    public bool amIAI()
    {
        return this.isAI;
    }

    public IEnumerator WaitForRollDice(Dice dice)
    {
        while (!diceInHand || !iWannaToDice)
        {
            yield return null;
        }

        LoseMyDice();

        int result = dice.PlayerRoll("Player");
        tmpDiceResult = result;
        // play animation

        //
        for (int i = 0; i < result; i++)
        {
            yield return new WaitForSeconds(moveWaitTime);
            MoveToNextNode();
        }

        // Event triggered
        currentNode.PlayerEntered();
        
    }

    public IEnumerator AIWaitForRollDice(Dice dice)
    {
        while (!diceInHand)
        {
            yield return null;
        }

        LoseMyDice();

        int result = dice.PlayerRoll("AI");
        tmpDiceResult = result;
        // play animation

        //
        for (int i = 0; i < result; i++)
        {
            yield return new WaitForSeconds(moveWaitTime);
            MoveToNextNode();
        }

        // Event triggered
        currentNode.PlayerEntered();
        
    }

    void MoveToNextNode()
    {
        if (!currentNode.NextNodesIsEmpty())
        {
            currentNode = currentNode.GetRandomNextNode();
            currentNode.PlayerPassed();
            Vector3 targetPosition = currentNode.position + new Vector3(0,0.5f,0);
            StartCoroutine(MoveToNode(targetPosition));
            
        }
    }

    IEnumerator MoveToNode(Vector3 targetPosition)
    {
        // 等待 x 秒，使玩家停顿一下
        yield return new WaitForSeconds(moveTime);

        // 移动玩家到目标位置
        float journeyLength = Vector3.Distance(transform.position, targetPosition);
        float startTime = Time.time;
        float speed = moveSpeed;

        while (transform.position != targetPosition)
        {
            float distCovered = (Time.time - startTime) * speed;
            float fracJourney = distCovered / journeyLength;
            transform.position = Vector3.Lerp(transform.position, targetPosition, fracJourney);
            yield return null;
        }

    }
}
