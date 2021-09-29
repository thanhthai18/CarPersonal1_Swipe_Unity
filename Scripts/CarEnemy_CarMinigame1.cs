using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarEnemy_CarMinigame1 : MonoBehaviour
{
    public int enemyLane;

    private void Start()
    {
        transform.DOMoveX(transform.position.x + 27, 3.375f).SetEase(Ease.Linear).OnComplete(() => 
        {
            GameController_CarMinigame1.instance.laneEnemyCanUse.Add(enemyLane);
            Destroy(this.gameObject);
            GameController_CarMinigame1.instance.carEnemy.Remove(this);
            
        });
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            transform.DOPause();
            transform.DOMoveX(transform.position.x - 2, 1f);
            GameController_CarMinigame1.instance.myCar.transform.DOMoveX(GameController_CarMinigame1.instance.myCar.transform.position.x + 1.5f, 1f);
            GameController_CarMinigame1.instance.iLose = 1;
    
        }
    }
}
