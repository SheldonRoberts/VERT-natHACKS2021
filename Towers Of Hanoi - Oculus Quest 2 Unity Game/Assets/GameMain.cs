using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class SaveData
{
    public List<int> Scores = new List<int>();
}

public class GameMain : MonoBehaviour
{
    public List<GameObject> discList;
    public List<GameObject> poleList;
    public List<Transform> poleTranList;
    public float tableHeight;
    public List<List<GameObject>> piles = new List<List<GameObject>>();
    private float timeTaken;


    public GameObject table;

    // Start is called before the first frame update
    void Start()
    {


        tableHeight = table.transform.position.y;

        // Adjusts the height of the poles based on the height of the table
        // WARNING: it adds a float that is completly arbitrary 
        foreach (var pole in poleList)
        {
            float poleHeight = tableHeight + 0.975f;
            pole.transform.position = new Vector3(pole.transform.position.x, poleHeight, pole.transform.position.z);

        }
        int discStartIndex = 0;
        GameObject leftPole = poleList[0];
        float discX = leftPole.transform.position.x;
        foreach (var disc in discList)
        {

            float discY = 0.9f + discStartIndex * 0.022f;

            discStartIndex++;
            disc.transform.position = new Vector3(discX, discY, disc.transform.position.z);


        }







    }

    public List<GameObject> startDiscs()
    {
        return discList;
    }
    public float heightOfTable()
    {
        return tableHeight;
    }
    public void addPile(List<GameObject> thePile)
    {
        piles.Add(thePile);
    }
    public List<GameObject> getPoleList()
    {
        return poleList;
    }

    // Update is called once per frame
    void Update()
    {
        timeTaken = Time.time;
        foreach (var pile in piles)
        {
            int index = pile.Count - 1;
            if (index != null && index >= 0)
            {
                GameObject topDisc = pile[index];
                topDisc.transform.GetChild(0).GetComponent<BoxCollider>().enabled = true;
                index--;
                while (index >= 0)
                {
                    GameObject disc = pile[index];
                    disc.transform.GetChild(0).GetComponent<BoxCollider>().enabled = false;
                    index--;
                }
            }

        }

    }

    private void OnDestroy()
    {
        Debug.Log("-----------------------------------");
        Debug.Log("Game Over");
        Debug.Log("------------------------------------");
        Debug.Log("Time Taken: " + timeTaken);
        Debug.Log("Moves Taken: " + MoveWithHand.MovesTaken);

        string path = (Application.dataPath.Substring(0,Application.dataPath.Length-50) + "Evalutation Portal - Bootstrap Template/game_data.json");
        string jsonString = File.ReadAllText(path);
        SaveData Savedata = JsonUtility.FromJson<SaveData>(jsonString);
        Savedata.Scores.Add((int)(MoveWithHand.MovesTaken / timeTaken * 100000));
        string data = JsonUtility.ToJson(Savedata);
        System.IO.File.WriteAllText(path, data);
    }
}

