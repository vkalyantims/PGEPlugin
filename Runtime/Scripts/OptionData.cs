using UnityEngine;

namespace PGE.Diagnosis
{
    [CreateAssetMenu(fileName = "Option", menuName = "Quiz/Create Option")]
    public class OptionData : ScriptableObject,IOptionData
    {
        [TextArea]
        public string OptionText;

        public string optionText { get => OptionText; set => value = OptionText; }
    }

}
public interface IOptionData
{
    string optionText { get; set; }
}