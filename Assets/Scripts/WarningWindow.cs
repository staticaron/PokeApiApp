using UnityEngine;

public class WarningWindow : MonoBehaviour
{
    private const string PopupAnimationName = "WindowPopup";
    private const string PopdownAnimationName = "WindowPopdown";

    [SerializeField] GameObject connectionText;
    [SerializeField] GameObject notFoundText;

    private Animator animator;

    [SerializeField] WarningWindowChannelSO warningWindowChannelSO;

    private void Awake()
    {
        warningWindowChannelSO.PopupWarningWindowEvent += PopupWindow;

        animator = GetComponent<Animator>();
    }

    private void OnDisable()
    {
        warningWindowChannelSO.PopupWarningWindowEvent -= PopupWindow;
    }

    public void PopupWindow(WarningType type)
    {
        //Enable appropriate message
        if (type == WarningType.CONNECTION)
        {
            connectionText.SetActive(true);
            notFoundText.SetActive(false);
        }
        else
        {
            notFoundText.SetActive(true);
            connectionText.SetActive(false);
        }

        //Play the warning window animation
        animator.Play(PopupAnimationName);
    }

    public void PopdownWindow()
    {
        animator.Play(PopdownAnimationName);
    }
}