using UnityEngine;

[CreateAssetMenu(fileName = "Sway", menuName = "ScriptableObjects/Sway")]
public class SwayScriptableObject : ScriptableObject
{
    public float swayAmountA = 1;
    public float swayAmountB = 2;
    public float swayScale = 100;
    public float swayLerpSpeed = 14;
    public float swaySpeed = 1f;
}
