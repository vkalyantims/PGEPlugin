using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PGE.Diagnosis
{
    [RequireComponent(typeof(Toggle))]
    public class SelectableOption : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI OptionText;
        
        public void SetOptionText(string text)
        {
            OptionText.text = text;
        }
    }
}