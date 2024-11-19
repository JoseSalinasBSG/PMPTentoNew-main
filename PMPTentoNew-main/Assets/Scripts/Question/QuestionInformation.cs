using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;

namespace Question
{
    public class QuestionInformation : MonoBehaviour
    {
        #region Variables

        [SerializeField] private PopupQuestion _popupIncorrect;
        [SerializeField] private PopupQuestion _popupCorrect;
        [SerializeField] private TextMeshProUGUI _question;
        [SerializeField] private Option _opt1;
        [SerializeField] private Option _opt2;
        [SerializeField] private Option _opt3;
        [SerializeField] private Option _opt4;

        [SerializeField] private Color[] firstWordColors;
        [SerializeField] private int firstWordSize;

        QuestionData _data;
        public Option Opt1
        {
            get => _opt1;
            set => _opt1 = value;
        }

        public Option Opt2
        {
            get => _opt2;
            set => _opt2 = value;
        }

        public Option Opt3
        {
            get => _opt3;
            set => _opt3 = value;
        }

        public Option Opt4
        {
            get => _opt4;
            set => _opt4 = value;
        }

        #endregion

        #region Unity Methods
        #endregion

        #region Methods

        public void SetData(QuestionData questionData)
        {
            _data = questionData;
            _question.text = SeparateAndRejoin(questionData.question);
            _opt1.SetData(questionData.options[0].respuesta,questionData.options[0].id.ToString());
            _opt2.SetData(questionData.options[1].respuesta,questionData.options[1].id.ToString());
            _opt3.SetData(questionData.options[2].respuesta,questionData.options[2].id.ToString());
            _opt4.SetData(questionData.options[3].respuesta,questionData.options[3].id.ToString());
            EnableOptions();

        }

        private string SeparateAndRejoin(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            int spaceIndex = input.IndexOf(' ');
            if (spaceIndex == -1)
            {
                return input; // Si no hay espacios, el string completo es una sola palabra
            }

            string firstWord = input.Substring(0, spaceIndex);
            string restOfString = input.Substring(spaceIndex + 1);
            GetColoredText(ref firstWord);
            return $"{firstWord} {restOfString}";
        }

        private void GetColoredText(ref string input)
        {
            if (string.IsNullOrEmpty(input) || firstWordColors == null || firstWordColors.Length == 0)
            {
                return;
            }

            string result = "";
            int colorCount = firstWordColors.Length;

            for (int i = 0; i < input.Length; i++)
            {
                Color color = firstWordColors[i % colorCount]; // Obtener color cíclicamente
                string hexColor = ColorUtility.ToHtmlStringRGBA(color); // Convertir color a formato hexadecimal
                result += $"<b><size={firstWordSize}><color=#{hexColor}>{input[i]}</color></size></b>";
            }

            input = result;
        }



        public void SetMessage(string message, bool isCorrect)
        {
            if (isCorrect)
            {
                _popupCorrect.SetMessage(message,true);

            }
            else
            {
                _popupIncorrect.SetMessage(message,false);

            }
        }

        // public void SetMessagePower(string message)
        // {
        //     _popupQuestion.SetMessagePower(message);
        // }

        public void  DisableOptions()
        {
            _opt1.DisableOption();
            _opt2.DisableOption();
            _opt3.DisableOption();
            _opt4.DisableOption();
        }
        public void  EnableOptions()
        {
            _opt1.EnableOption();
            _opt2.EnableOption();
            _opt3.EnableOption();
            _opt4.EnableOption();
        }
        
        #endregion

    }

}
