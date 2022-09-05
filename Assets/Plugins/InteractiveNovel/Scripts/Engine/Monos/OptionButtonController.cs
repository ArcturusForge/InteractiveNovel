using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Arcturus.InteractiveNovel
{
    public class OptionButtonController : MonoBehaviour
    {
        public TMP_Text optionDisplay;
        private int sequenceIndex;
        private Action<int> OnSelected;

        private void OptionSelected()
        {
            OnSelected?.Invoke(sequenceIndex);
        }

        public void SetOption(string content, int index, Action<int> OnSelected)
        {
            GetComponent<Button>().onClick.AddListener(OptionSelected);
            optionDisplay.text = content;
            sequenceIndex = index;
            this.OnSelected = OnSelected;
        }
    }
}
