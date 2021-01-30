using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolDisplay : MonoBehaviour
{

    [Header("Elements")]
    public Image toolImage;

    [Header("Sprites (match indices)")]
    public List<Tool> toolList;
    public List<Sprite> toolSprites;

    [Header("Spinny Animation")]
    public RectTransform spinnyArrows;
    public float spinnyTime = 0.5f;
    public AnimationCurve spinnyCurve;

    public void SetTool(Tool tool, bool doAnimation)
    {
        toolImage.sprite = toolSprites[toolList.IndexOf(tool)];

        if(doAnimation)
        {
            StopAllCoroutines();
            StartCoroutine(SpinAnimation());
        }
    }

    IEnumerator SpinAnimation()
    {
        const float startAngle = 0.0f;
        const float endAngle = 360.0f;

        float t = 0.0f;
        while(t <= spinnyTime)
        {
            t += Time.deltaTime;

            float curveTime = spinnyCurve.Evaluate(t / spinnyTime);
            float angle = Mathf.LerpUnclamped(startAngle, endAngle, curveTime);
            spinnyArrows.localRotation = Quaternion.Euler(0.0f, 0.0f, angle);

            yield return null;
        }

        spinnyArrows.localRotation = Quaternion.Euler(0.0f, 0.0f, endAngle);
    }

}
