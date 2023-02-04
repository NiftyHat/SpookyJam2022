using Data.TextSequence;
using UnityEngine;

namespace TextSequence.Data
{
    
    public class TextSequenceSetText : TextSequenceItem
    {
        [SerializeField][TextArea(5,15)] private string _copy;

        [HideInInspector] private string _summaryText = null;
        public string Copy => _copy;
        public string SummaryText {
            get
            {
                if (string.IsNullOrEmpty(_copy) )
                {
                    return name;
                }
                else if (string.IsNullOrEmpty(_summaryText))
                {
                    _summaryText = _copy.Substring(0, Mathf.Min(_copy.Length, 50));
                }
                return _summaryText;
            }
            
        }
    }
}