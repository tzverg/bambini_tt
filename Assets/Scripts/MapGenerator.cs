using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Range(0, 100)] [SerializeField] private int stoneNumber;

    [SerializeField] private int minTailLenght;
    [SerializeField] private int maxTailLenght;

    [SerializeField] private GameObject centipedeGO;
    [SerializeField] private GameObject spiderGO;
    [SerializeField] private GameObject stoneGO;

    public static bool[,] mapGreed;
    private int mapSize;

    // Start is called before the first frame update
    void Start()
    {
        CreateSpider();
        CreateCentipede();

        CreateStoneField();
    }

    private void CreateSpider()
    {
        SpiderC spiderC = Instantiate(spiderGO).GetComponent<SpiderC>();
    }

    private void CreateCentipede()
    {
        CentipedeC centipedeHead = Instantiate(centipedeGO).GetComponent<CentipedeC>();

        if (centipedeHead != null)
        {
            centipedeHead.centipedeLenght = Random.Range(minTailLenght, maxTailLenght);

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
        }
    }

    private void CreateStoneField()
    {
        int halfMapSize = (int)Camera.main.orthographicSize;
        mapSize = halfMapSize * 2;
        mapGreed = new bool[mapSize, mapSize];
        Vector3 stoneOffset = new Vector3(0.5F, 0.5F, 0F);

        for (int cnt = 0; cnt < mapSize; cnt++)
        {
            mapGreed[cnt, 0] = true;
            mapGreed[cnt, 1] = true;
            mapGreed[cnt, mapSize - 1] = true;
        }

        for (int cnt = 0; cnt < stoneNumber; cnt++)
        {
            StoneC newStone = Instantiate(stoneGO, transform).GetComponent<StoneC>();
            if (newStone != null)
            {
                SetRandomPositionInGreed(cnt, halfMapSize, stoneOffset, newStone);
                newStone.mapIDX = (int)newStone.transform.position.x;
                newStone.mapIDY = (int)newStone.transform.position.y;
            }
        }
    }

    private void SetRandomPositionInGreed(int stoneID, int halfMapSize, Vector3 stoneOffset, StoneC newStone)
    {
        int targetCol = 0;
        int targetRow = 0;

        do
        {
            targetCol = Random.Range(0, mapSize);
            targetRow = Random.Range(0, mapSize);
        }
        while (mapGreed[targetCol, targetRow]);

        Vector3 newPosition = new Vector3(targetCol - halfMapSize, targetRow - halfMapSize);
        mapGreed[targetCol, targetRow] = true;

        newStone.mapIDX = targetCol;
        newStone.mapIDY = targetRow;

        newStone.transform.position = newPosition + stoneOffset;
    }
}
