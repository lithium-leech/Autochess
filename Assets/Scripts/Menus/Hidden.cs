using UnityEngine;

/// <summary>
/// A behavior which makes a game object hidden by default
/// </summary>
public class Hidden : MonoBehaviour
{
    void Start() => gameObject.SetActive(false);
}
