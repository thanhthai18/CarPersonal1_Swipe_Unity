using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCar_CarMinigame1 : MonoBehaviour
{
    public Camera mainCamera;
    public Vector2 currentMousePos;
    public Vector2 preMousePos;
    public bool isMoveCompleted;
    public bool isLock;
    public bool isCompleteTutorial1;
    public bool isFinish;
    public bool isWin;

    private void Start()
    {
        isLock = true;
        isCompleteTutorial1 = false;
        isFinish = false;
        isWin = false;
    }

    private void Update()
    {
        if (!GameController_CarMinigame1.instance.isIntro && GameController_CarMinigame1.instance.iLose != 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                preMousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            }

            if (Input.GetMouseButtonUp(0))
            {
                currentMousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                isLock = false;
            }

            if (currentMousePos.y - preMousePos.y > 0 && !isLock)
            {
                if (!GameController_CarMinigame1.instance.isCompleteTutorial1)
                {
                    GameController_CarMinigame1.instance.isCompleteTutorial1 = true;
                    GameController_CarMinigame1.instance.tutorial1.SetActive(false);
                    GameController_CarMinigame1.instance.carEnemy[0].transform.DOPlay();
                    GameController_CarMinigame1.instance.backGround.transform.DOPlay();
                    GameController_CarMinigame1.instance.isOnGame = false;
                }
                if (GameController_CarMinigame1.instance.lane > 0)
                {
                    GameController_CarMinigame1.instance.lane--;
                    transform.DOMove(GameController_CarMinigame1.instance.pos[GameController_CarMinigame1.instance.lane].transform.position, 0.2f);
                    currentMousePos.y = 0;
                    preMousePos.y = 0;
                    isLock = true;
                }             
            }
            if (currentMousePos.y - preMousePos.y < 0 && !isLock)
            {
                if (!GameController_CarMinigame1.instance.isCompleteTutorial1)
                {
                    GameController_CarMinigame1.instance.isCompleteTutorial1 = true;
                    GameController_CarMinigame1.instance.tutorial1.SetActive(false);
                    GameController_CarMinigame1.instance.carEnemy[0].transform.DOPlay();
                    GameController_CarMinigame1.instance.backGround.transform.DOPlay();
                    GameController_CarMinigame1.instance.isOnGame = false;
                }
                if (GameController_CarMinigame1.instance.lane < 2)
                {
                    GameController_CarMinigame1.instance.lane++;
                    transform.DOMove(GameController_CarMinigame1.instance.pos[GameController_CarMinigame1.instance.lane].transform.position, 0.2f);
                    currentMousePos.y = 0;
                    preMousePos.y = 0;
                    isLock = true;
                }
            }

            if (currentMousePos.y == preMousePos.y && !isLock)
            {
                isLock = true;
            }

            if (GameController_CarMinigame1.instance.lane == 0 && isFinish)
            {
                if(currentMousePos.y - preMousePos.y > 0 && !isLock)
                {
                    Debug.Log("Win");
                    isWin = true;
                    GameController_CarMinigame1.instance.iLose = 0;
                    transform.DOMove(new Vector2(transform.position.x, 0.78f), 3);
                    GameController_CarMinigame1.instance.tutArrow.SetActive(false);
                    GameController_CarMinigame1.instance.backGround.transform.DOPause();
                    GameController_CarMinigame1.instance.StopAllCoroutines();
                    currentMousePos.y = 0;
                    preMousePos.y = 0;
                    isLock = true;
                }            
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Path"))
        {
            GameController_CarMinigame1.instance.iLose = 2;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Finish"))
        {
            isFinish = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Finish") && !isWin)
        {
            GameController_CarMinigame1.instance.iLose = 3;
        }
    }
}
