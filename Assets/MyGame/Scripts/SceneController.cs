
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    [SerializeField] private Button play;
    [SerializeField] private Button info;
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button quitButton;
    // Start is called before the first frame update
    private void Start()
    {
        infoPanel.SetActive(false);
    }
    public void onPlay()
    {
        SceneManager.LoadScene(1);
    }

    public void oInfo()
    {
        infoPanel.SetActive(true);
    }

    public void onExitButton()
    {
        infoPanel.SetActive(false);
    }

    public void OnQuit()
    {
            Application.Quit();
    }
}
