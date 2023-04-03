using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public MMap map;
    public float moveSpeed = 2.5f;
    public float moveTime = 0.5f;

    private MapNode currentNode;

    //其他属性和方法

    void Start()
    {
        currentNode = map.firstNode;
        transform.position = currentNode.position + new Vector3(0,0.5f,0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MoveToNextNode();
        }
    }

    void MoveToNextNode()
    {
        if (currentNode.nextNode != null)
        {
            currentNode = currentNode.nextNode;
            Debug.Log(currentNode.position);
            Vector3 targetPosition = currentNode.position + new Vector3(0,0.5f,0);
            StartCoroutine(MoveToNode(targetPosition));
        }
    }

    IEnumerator MoveToNode(Vector3 targetPosition)
    {
        // 等待 0.25 秒，使玩家停顿一下
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