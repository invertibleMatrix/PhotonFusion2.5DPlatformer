using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "UIViewConfig", menuName = "Views/UIConfig")]
public class UIViewConfig : ScriptableObject
{
    public float PanelInDuration;
    public float PanelOutDuration;
    public float DefaultScreenHeight;
    public Ease  EaseInType;
    public Ease  EaseOutType;

    public float EaseInTime;

    public float GetOffScreenOffset()
    {
        return DefaultScreenHeight / Screen.height * Screen.width;
    }
}