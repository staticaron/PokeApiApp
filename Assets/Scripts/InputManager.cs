using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    [SerializeField] LoadDataChannelSO loadDataChannelSO;

    public void Submit(InputField nameField)
    {
        string inputName = nameField.text;

        if (string.IsNullOrEmpty(inputName))
        {
            return;
            //TODO : Display Some Kind of alert 
        }

        loadDataChannelSO.RaiseEvent(inputName);
    }
}