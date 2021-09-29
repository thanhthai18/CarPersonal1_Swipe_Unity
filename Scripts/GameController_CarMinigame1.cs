using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController_CarMinigame1 : MonoBehaviour
{
    public static GameController_CarMinigame1 instance;

    public GameObject backGround;
    public List<Transform> pos = new List<Transform>();
    public List<Transform> posEnemy = new List<Transform>();
    public int lane;
    public MyCar_CarMinigame1 myCar;
    public List<CarEnemy_CarMinigame1> carEnemy = new List<CarEnemy_CarMinigame1>();
    public CarEnemy_CarMinigame1 carEnemyPrefab;
    public List<int> laneEnemyCanUse = new List<int>();
    public bool isIntro;
    public bool isOnGame;
    public int iLose;
    public int timeOnMainGame;
    public Button btnBrake;
    public int iBrake;
    public GameObject brake, trafficLight, police1, police2;
    public bool isLockStageSelf;
    public Coroutine brakeCoroutine;
    public GameObject tutorial1, tutorial2, tutArrow, tutWarring;
    public bool isCompleteTutorial1, isCompleteTutorial2;
    public Camera mainCamera;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(instance);
    }

    private void Start()
    {
        SetSizeCamera();
        brake.SetActive(false);
        trafficLight.SetActive(false);
        police1.SetActive(false);
        tutorial1.SetActive(false);
        tutorial2.SetActive(false);
        tutArrow.SetActive(false);
        tutWarring.SetActive(false);
        iLose = 0; // iLose = 0: cant control car / iLose = -1: can control, / 1 or 2 or 3 losegame1 or losegame2 or losegame3
        timeOnMainGame = 3;
        isLockStageSelf = false;
        isIntro = true;
        isOnGame = true;
        isCompleteTutorial1 = false;
        isCompleteTutorial2 = false;
        iBrake = 0;
        laneEnemyCanUse.AddRange(new int[] { 0, 1, 2 });
        btnBrake.onClick.AddListener(GetBrake);
    }

    void SetSizeCamera()
    {
        float f1 = 16.0f / 9;
        float f2 = Screen.width * 1.0f / Screen.height;

        if (f1 > f2)
        {
            mainCamera.orthographicSize *= f1 / f2;
            mainCamera.orthographicSize = Mathf.Max(7, mainCamera.orthographicSize);
        }
    }

    CarEnemy_CarMinigame1 SpawnCarEnemy(int laneIndex)
    {
        CarEnemy_CarMinigame1 car2Object = Instantiate(carEnemyPrefab, posEnemy[laneIndex].position, Quaternion.identity);
        car2Object.enemyLane = laneIndex;
        if (laneEnemyCanUse.Count > 0)
        {
            laneEnemyCanUse.Remove(laneIndex);
        }
        return car2Object;
    }

    IEnumerator Intro()
    {
        myCar.transform.DOMoveX(9.62f, 1);
        yield return new WaitForSeconds(1);
        myCar.transform.DOMove(new Vector2(8.56f, pos[1].position.y), 1).OnComplete(() =>
        {
            myCar.transform.DOMoveX(pos[1].position.x, 1f).OnComplete(() =>
            {
                iLose = -1;
                backGround.transform.DOMoveX(194.7f, 50).SetEase(Ease.Linear);
                carEnemy.Add(SpawnCarEnemy(laneEnemyCanUse[1]));
                Invoke(nameof(GetTutorial1), 1);
            });
            lane = 1;
        });
    }

    IEnumerator CountingTime()
    {
        while (timeOnMainGame <= 48)
        {
            timeOnMainGame++;
            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator OnMainGame()
    {
        yield return new WaitForSeconds(2);
        timeOnMainGame += 2;
        while (timeOnMainGame <= 30)
        {
            if (laneEnemyCanUse.Count > 0)
            {

                if (laneEnemyCanUse.Count == 2 && carEnemy.Count < 3)
                {
                    int ranBit = Random.Range(0, 2);
                    if (ranBit == 1)
                    {
                        int ran = Random.Range(0, laneEnemyCanUse.Count);
                        carEnemy.Add(SpawnCarEnemy(laneEnemyCanUse[ran]));
                        yield return new WaitForSeconds(Random.Range(1, 3));
                    }
                }
                if (laneEnemyCanUse.Count != 2 && carEnemy.Count < 3)
                {
                    int ran = Random.Range(0, laneEnemyCanUse.Count);
                    carEnemy.Add(SpawnCarEnemy(laneEnemyCanUse[ran]));
                    yield return new WaitForSeconds(Random.Range(1, 3));
                }
            }
            else
                yield return new WaitForSeconds(1);
        }
    }

    void GetTutorial1()
    {
        if (!isCompleteTutorial1)
        {
            tutorial1.SetActive(true);
            backGround.transform.DOPause();
            carEnemy[0].transform.DOPause();
            tutorial1.transform.DOMoveY(tutorial1.transform.position.y + 5, 1).OnComplete(() =>
            {
                tutorial1.transform.DOMoveY(tutorial1.transform.position.y - 5, 1).OnComplete(() =>
                {
                    ReplayTutorial1();

                });

            });
        }
    }
    void ReplayTutorial1()
    {
        tutorial1.transform.DOMoveY(tutorial1.transform.position.y + 5, 1).OnComplete(() =>
        {
            tutorial1.transform.DOMoveY(tutorial1.transform.position.y - 5, 1).OnComplete(() =>
            {
                if (tutorial1.activeSelf == true)
                {
                    ReplayTutorial1();
                }
                else
                    Destroy(tutorial1);
            });
        });
    }

    void GetBrake()
    {
        if (iBrake == 1)
        {
            backGround.transform.DOPause();
            iBrake = -1;
            iLose = 0;
            StopCoroutine(brakeCoroutine);
            Destroy(tutorial2);
            Destroy(brake);
        }
    }
    void GetResume()
    {
        backGround.transform.DOPlay();
        iLose = -1;
    }
    IEnumerator BrakeIdle()
    {
        while (brake != null)
        {
            brake.transform.DOScale(new Vector3(2.5f, 2.5f, 0), 0.5f);
            yield return new WaitForSeconds(0.5f);
            brake.transform.DOScale(new Vector3(2, 2, 0), 0.5f);
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void Update()
    {
        if (!isIntro)
        {
            if (!isOnGame)
            {
                isOnGame = true;
                StartCoroutine(OnMainGame());
                StartCoroutine(CountingTime());
            }

            if (timeOnMainGame == 30 && !isLockStageSelf)
            {
                isLockStageSelf = true;
                trafficLight.SetActive(true);
                police1.SetActive(true);
                trafficLight.transform.GetChild(2).gameObject.SetActive(false);
            }
            if (timeOnMainGame == 34 && isLockStageSelf)
            {
                isLockStageSelf = false;
                trafficLight.transform.GetChild(2).gameObject.SetActive(true);
                trafficLight.transform.GetChild(1).gameObject.SetActive(false);
            }
            if (timeOnMainGame == 35 && !isLockStageSelf)
            {
                isLockStageSelf = true;
                trafficLight.transform.GetChild(1).gameObject.SetActive(true);
                trafficLight.transform.GetChild(0).gameObject.SetActive(false);
                trafficLight.transform.GetChild(3).gameObject.SetActive(true);
                trafficLight.transform.GetChild(3).transform.DOScale(2, 0.5f).OnComplete(() => { trafficLight.transform.GetChild(3).transform.DOScale(1, 0.5f); });
                if (brake != null)
                {
                    brake.SetActive(true);
                }
                iBrake = 1;
                brakeCoroutine = StartCoroutine(BrakeIdle());
                tutorial2.SetActive(true);
                tutorial2.transform.DOMove(new Vector2(-8.11f, -5.16f), 1).SetLoops(-1);
            }
            if (timeOnMainGame == 42 && isLockStageSelf)
            {
                isLockStageSelf = false;
                GetResume();
                carEnemy.Add(SpawnCarEnemy(laneEnemyCanUse[Random.Range(0, 3)]));
                trafficLight.GetComponent<BoxCollider2D>().enabled = false;
                trafficLight.transform.GetChild(2).gameObject.SetActive(false);
                trafficLight.transform.GetChild(0).gameObject.SetActive(true);
                trafficLight.transform.GetChild(3).gameObject.SetActive(false);
            }

            if (timeOnMainGame == 45 && !isLockStageSelf)
            {
                isLockStageSelf = true;
                tutWarring.SetActive(true);
                tutArrow.SetActive(true);
            }
        }

        if (isIntro)
        {
            isIntro = false;
            StartCoroutine(Intro());
        }

        if (iLose == 1)
        {
            iLose = 0;
            backGround.transform.DOPause();
            StopAllCoroutines();
            Debug.Log("Thua: va cham xe");
        }
        if (iLose == 2)
        {
            iLose = 0;
            backGround.transform.DOPause();
            Destroy(tutorial2);
            Destroy(brake);
            StopAllCoroutines();
            police1.transform.GetChild(0).gameObject.SetActive(false);
            police1.transform.GetChild(1).gameObject.SetActive(true);
            police1.transform.GetChild(1).transform.DOMove(new Vector2(police1.transform.GetChild(1).transform.position.x + 2, myCar.transform.position.y + 1), 3);
            Debug.Log("Thua: pham luat giao thong 1");
        }

        if (iLose == 3)
        {
            iLose = 0;
            backGround.transform.DOPause();
            StopAllCoroutines();
            police2.transform.GetChild(0).gameObject.SetActive(false);
            police2.transform.GetChild(1).gameObject.SetActive(true);
            police2.transform.GetChild(1).transform.DOMove(new Vector2(police2.transform.GetChild(1).transform.position.x + 2, myCar.transform.position.y + 1), 3);
            tutWarring.SetActive(false);
            Debug.Log("Thua: pham luat giao thong 2");
        }
    }
}

