
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace PGE.Diagnosis
{
    [CreateAssetMenu(menuName = "Quiz/Create Question", fileName = "Question")]
    public class QuestionData : ScriptableObject, IQuestionData
    {
        [SerializeField]
        private string questionPrefix;

        [TextArea]
        [SerializeField]
        private string questionText;

        [SerializeField]
        private List<Option> allOptions;

        [SerializeField]
        private bool useDefaultOptions;
        
        [SerializeField]
        private bool shouldSendBothIdealAndAcceptable;

        [SerializeField]
        private bool isThisQuestionPartOfEvaluation;

        

        public string QuestionPrefix { get => questionPrefix; set => questionPrefix = value; }
        public bool IsThisQuestionPartOfEvaluation
        {
            get => isThisQuestionPartOfEvaluation;
            set => isThisQuestionPartOfEvaluation = value;
        }
        public bool ShouldSendIdealAndAcceptableStatements
        {
            get => shouldSendBothIdealAndAcceptable;
            set => shouldSendBothIdealAndAcceptable = value;
        }
        public bool UseDefaultOptions
        {
            get => useDefaultOptions;
            set => useDefaultOptions = value;
        }

        public List<IOption> AllOptions
        {
            get => allOptions.Cast<IOption>().ToList();
            set
            {
                if (value == null)
                {
                    allOptions = new List<Option>();
                    return;
                }

                // convert each IOption back to its concrete Option instance
                allOptions = new List<Option>(value
                    .OfType<Option>()
                    .Select(o => o)                
                );
            }
        }

        public string QuestionText { get => questionText; set => questionText = value; }
    }
}

public interface IQuestionData
{
    public string QuestionText { get; set; }
    public string QuestionPrefix { get; set; }
    public bool IsThisQuestionPartOfEvaluation { get; set; }
    public bool ShouldSendIdealAndAcceptableStatements { get; set; }
    public bool UseDefaultOptions { get; set; }

    public List<IOption> AllOptions { get; set; }
}