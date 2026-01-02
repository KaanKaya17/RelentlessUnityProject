using UnityEngine;

public class StartPanel : MonoBehaviour
{
    public GameObject startPanel;

    public void HideStartPanel()
    {
        startPanel.SetActive(false);
    }
}
