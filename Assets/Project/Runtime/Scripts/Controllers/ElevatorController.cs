using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    [SerializeField]
    private DoorController doorController;

    [SerializeField]
    bool canMove;

    [SerializeField]
    private float speed;

    [SerializeField]
    private int startPoint;

    [SerializeField]
    private Transform[] points;
    private int i;
    private bool lastPointReached;

    [SerializeField]
    bool endGame;

    private void Awake()
    {
        doorController.activationState = DoorController.DoorActivation.StayClosed;
        transform.position = points[startPoint].position;
        i = startPoint;
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, points[i].position) < 0.01f)
        {
            if (i == points.Length - 1)
            {
                canMove = false;
                doorController.activationState = DoorController.DoorActivation.StayOpen;
                gameObject.transform.parent = GameplayHandler.Instance.mapPuzzle3.transform;
                points[0].parent.gameObject.transform.parent = GameplayHandler
                    .Instance
                    .mapPuzzle3
                    .transform;
                GameplayHandler.Instance.CloseMapPart("Puzzle2");
            }
            else
            {
                i++;
            }
        }

        if (canMove && i < points.Length)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                points[i].position,
                speed * Time.deltaTime
            );
            doorController.activationState = DoorController.DoorActivation.StayClosed;
            GameplayHandler.Instance.OpenMapPart("Puzzle3");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (other.gameObject.CompareTag("PlayerFix"))
        {
            canMove = true;
            if (endGame == true)
            {
                GameplayHandler.Instance.EndGame();
            }
        }
    }
}
