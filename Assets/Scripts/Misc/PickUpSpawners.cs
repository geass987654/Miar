using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSpawners : MonoBehaviour
{
    [SerializeField] private GameObject goldCoinPrefab, healthGlobePrefab;

    public void DropItems()
    {
        int randomNum = Random.Range(1, 4);

        if(randomNum == 1)
        {
            Instantiate(healthGlobePrefab, transform.position, Quaternion.identity);
        }
        if(randomNum == 2)
        {
            int randomAmountOfGold = Random.Range(1, 4);

            for(int i = 0; i < randomAmountOfGold; i++)
            {
                Instantiate(goldCoinPrefab, transform.position, Quaternion.identity);
            }
        }
    }

}
