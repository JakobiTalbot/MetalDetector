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

    public void SetTool(Tool tool)
    {
        toolImage.sprite = toolSprites[toolList.IndexOf(tool)];
    }

}
