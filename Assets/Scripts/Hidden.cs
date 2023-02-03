using UnityEngine;

public class Hidden : MonoBehaviour
{
    public GameObject hiddenObject;

    // Start is called before the first frame update
    void Start()
    {
        hiddenObject.SetActive(false);
    }
}
