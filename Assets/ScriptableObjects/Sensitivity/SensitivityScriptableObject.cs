using UnityEngine;
[CreateAssetMenu(fileName = "Sensitivity", menuName = "ScriptableObjects/Sensitivity")]
public class SensitivityScriptableObject : ScriptableObject
{
    public float tiltIntensity;
    public float tiltSmoothness;
    public float maxRotation;
}
