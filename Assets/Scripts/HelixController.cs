using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class HelixController : MonoBehaviour
{

    private Vector2 lasTapPosition;

    private Vector3 startRotation;


    public Transform topTransform;
    public Transform goalTransform;

    public GameObject helixLevelPrefab;

    public List<Stage> allStages = new List<Stage>();

    public float helixDistance;

    private List<GameObject> spawnLevels = new List<GameObject>();


    private void Awake()
    {
        startRotation = transform.localEulerAngles;
        helixDistance = topTransform.localPosition.y - (goalTransform.localPosition.y + .1f);
        LoadStage(0);
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 currentTapPosition = Input.mousePosition;

            if (lasTapPosition==Vector2.zero)
            {
                lasTapPosition = currentTapPosition;
            }

            float distance = lasTapPosition.x - currentTapPosition.x;
            lasTapPosition = currentTapPosition;

            transform.Rotate(Vector3.up * distance);
        }

        if (Input.GetMouseButtonUp(0))
        {
            lasTapPosition = Vector2.zero;
        }
    }

    public void LoadStage(int stageNumber)
    {
        Stage stage = allStages[Mathf.Clamp(stageNumber, 0 , allStages.Count-1)];

        if (stage==null)
        {
            Debug.Log("No hay niveles :c");
            return;
        }

        if(stageNumber == allStages.Count) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        }
        else
        {
            Camera.main.backgroundColor = allStages[stageNumber].stageBackgroundColor;

            FindObjectOfType<BallController>().GetComponent<Renderer>().material.color = allStages[stageNumber].stageBallColor;

            transform.localEulerAngles = startRotation;

            foreach (GameObject go in spawnLevels)
            {
                Destroy(go);
            }

            float levelDistance = helixDistance / stage.levels.Count;
            float spawnPosY = topTransform.localPosition.y;

            for (int i = 0; i < stage.levels.Count; i++)
            {
                spawnPosY -= levelDistance;

                GameObject level = Instantiate(helixLevelPrefab, transform);

                level.transform.localPosition = new Vector3(0, spawnPosY, 0);

                spawnLevels.Add(level);

                int partsToDisable = 12 - stage.levels[i].partCount;
                List<GameObject> disabledParts = new List<GameObject>();

                while (disabledParts.Count < partsToDisable)
                {
                    GameObject randomPart = level.transform.GetChild(Random.Range(0, level.transform.childCount)).gameObject;
                    if (!disabledParts.Contains(randomPart))
                    {
                        randomPart.SetActive(false);
                        disabledParts.Add(randomPart);
                    }
                }

                List<GameObject> leftParts = new List<GameObject>();

                foreach (Transform t in level.transform)
                {
                    t.GetComponent<Renderer>().material.color = allStages[stageNumber].stageLevelPartColor;
                    if (t.gameObject.activeInHierarchy)
                    {
                        leftParts.Add(t.gameObject);
                    }
                }

                List<GameObject> deathParts = new List<GameObject>();

                while (deathParts.Count < stage.levels[i].deathPartCount)
                {
                    GameObject randomPart = leftParts[(Random.Range(0, leftParts.Count))];

                    if (!deathParts.Contains(randomPart))
                    {
                        randomPart.gameObject.AddComponent<DeathPart>();
                        deathParts.Add(randomPart);
                    }
                }

            }
        }


        
    }

}
