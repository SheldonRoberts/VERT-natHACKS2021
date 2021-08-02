using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pile : MonoBehaviour
{
    public List<GameObject> pileList;

    public GameMain gameMain;

    public float tableHeight;

    // Start is called before the first frame update
    void Start()
    {
        gameMain.addPile(pileList);

        tableHeight = gameMain.heightOfTable();
    }

    public float getBaseHeight(GameObject disc)
    {

        //int discPosition = pileList.FindIndex(a => a == disc);
        int index = 0;
        int discPosition = 0;
        foreach (var discIN in pileList)
        {
            if (disc == discIN)
            {
                discPosition = index;
            }
            index++;
        }
        if (discPosition != -1)
        {
            float baseHeight = 0.9f + discPosition * 0.022f;
            return baseHeight;
        }


        return -1;

    }

    public void addDisc(GameObject disc)
    {
        if (!pileList.Contains(disc))
        {
            pileList.Add(disc);

        }


    }
    public void removeDisc(GameObject disc)
    {
        pileList.Remove(disc);
    }
}
