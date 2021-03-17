using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text currentScoreText;
    public Text bestScoreText;

    public Slider slider;
    public Text actualLevel;
    public Text nextLevel;

    public Transform topTransform;
    public Transform goalTransform;

    public Transform ballTransform;



    void Update()
    {
        currentScoreText.text = "Puntaje: " + GameManager.singleton.currentScore;
        bestScoreText.text = "Mejor: " + GameManager.singleton.bestScore;

        ChangeSliderLevelAndProgress();
    }

    public void ChangeSliderLevelAndProgress()
    {
        actualLevel.text = "" + (GameManager.singleton.currentLevel+1);
        nextLevel.text = "" + (GameManager.singleton.currentLevel+2);

        float totalDistance = (topTransform.position.y - goalTransform.position.y);
        float distanceLeft = totalDistance - (ballTransform.position.y - goalTransform.position.y);

        float value = (distanceLeft / totalDistance);

        slider.value = Mathf.Lerp(slider.value,value,5);
    }
}
