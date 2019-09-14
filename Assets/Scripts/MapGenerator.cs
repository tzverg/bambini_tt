using System;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private CentipedeC centipedeHead;
    [SerializeField] private GameObject stoneGO;

    [Range(0,100)][SerializeField] private int stoneNumber;

    public bool[,] mapGreed;
    private int mapSize;
    private int halfMapSize;

    // Start is called before the first frame update
    void Start()
    {
        mapSize = (int)Camera.main.orthographicSize * 2;
        halfMapSize = (int)Camera.main.orthographicSize;
        mapGreed = new bool[mapSize,mapSize];

        for (int cnt = 0; cnt < mapSize; cnt++)
        {
            mapGreed[cnt, 0] = true;
            mapGreed[cnt, 1] = true;
            mapGreed[cnt, mapSize - 1] = true;
        }

        // create centipede tail
        for (int cnt = 0; cnt < centipedeHead.centipedeLenght; cnt++)
        {
            GameObject bodySection = Instantiate(centipedeHead.centipedeBodyGO);
            Vector3 posOffset = new Vector3(centipedeHead.bodyOffset, 0F, 0F);

            var centBC = bodySection.GetComponent<CentipedeBodyC>();
            if (centBC)
            {
                centBC.bodyID = cnt;
                centBC.head = centipedeHead;
            }
            bodySection.transform.position = centipedeHead.transform.position + posOffset * (cnt + 1);
            centipedeHead.centipedeBody.Add(bodySection);
        }

        CreateStoneField();
    }

    private void CreateStoneField()
    {
        for (int cnt = 0; cnt < stoneNumber; cnt++)
        {
            GameObject newStone = stoneGO;
            newStone.transform.position = RandomPositionInGreed(cnt);
            Instantiate(newStone, transform);
        }
    }

    private Vector3 RandomPositionInGreed(int stoneID)
    {
        int targetCol = 0;
        int targetRow = 0;

        do
        {
            targetCol = UnityEngine.Random.Range(0, mapSize);
            targetRow = UnityEngine.Random.Range(0, mapSize);
        }
        while (mapGreed[targetCol, targetRow]);

        Vector3 newPosition = new Vector3(targetCol - halfMapSize, targetRow - halfMapSize);
        mapGreed[targetCol, targetRow] = true;

        return newPosition;
    }
}
