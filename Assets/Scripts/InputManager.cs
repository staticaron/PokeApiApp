using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    [SerializeField] LoadDataChannelSO loadDataChannelSO;

    public void Submit(InputField nameField)
    {
        string inputName = nameField.text.Trim();

        if (string.IsNullOrEmpty(inputName))
        {
            return;
        }

        loadDataChannelSO.RaiseEvent(inputName);
    }
}