using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BckgTextBox
{
    /// <summary>
    /// Textbox, имеющий серый фоновый текст, если в него не введён пользовательский текст
    /// </summary>
    public class TextBoxWithBckgText : TextBox
    {
        #region Constructors
        /// <summary>
        /// Textbox, имеющий серый фоновый текст, если в него не введён пользовательский текст
        /// </summary>
        /// <param name="TextVariants">Возможные варианты фонового текста (первый выставляется изначально)</param>
        public TextBoxWithBckgText
            (params string[] TextVariants)
        {
            textVariants = TextVariants;
            BckgText = textVariants[0];
            ForeColor = Color.Gray;
            textColor = Color.Black;
            bckgTextColor = Color.Gray;
            GotFocus += TextBoxWithBckgText_GotFocus;
            LostFocus += TextBoxWithBckgText_LostFocus;
            TextChanged += TextBoxWithBckgText_TextChanged;
        }
        #endregion

        #region Fields
        string bckgText;
        string[] textVariants;
        Color
            textColor,
            bckgTextColor;
        #endregion

        #region Properties
        /// <summary>
        /// Возвращает фоновый текст
        /// </summary>
        public string BckgText
        {
            get { return bckgText; }
            private set { Text = bckgText = value; }
        }
        /// <summary>
        /// Возвращает или устанавливает индекс текущего фонового текста
        /// </summary>
        public int CurrentBckgTextNumber
        {
            get
            {
                for (int i = 0; i < textVariants.Length; i++)
                {
                    if (textVariants[i] == bckgText)
                        return i;
                }
                throw new MissingFieldException("TextBoxWithBckgText", "bckgText");
            }
            set
            {
                if (value >= textVariants.Length)
                    throw new ArgumentException(
                        String.Format(
                            "Номер текущего текста должен быть меньше, чем {0}",
                            textVariants.Length));
                else
                    BckgText = textVariants[value];
            }
        }
        /// <summary>
        /// Возвращает true, если отсутствует пользовательский текст. 
        /// В противном случае, false.
        /// </summary>
        public bool Clean
        {
            get
            {
                return (
                    Text == BckgText && ForeColor == bckgTextColor ||
                    Text == String.Empty);
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns Background Text
        /// </summary>
        public new void Clear()
        {
            InvokeGotFocus(this, new EventArgs());
            Text = String.Empty;
            InvokeLostFocus(this, new EventArgs());
        }
        /// <summary>
        /// Returns Background Text with nesessary number
        /// </summary>
        /// <param name="textNumber">
        /// Number of Background Text that will be used
        /// </param>
        public void Clear(int textNumber)
        {
            InvokeGotFocus(this, new EventArgs());
            CurrentBckgTextNumber = textNumber;
            Text = String.Empty;
            InvokeLostFocus(this, new EventArgs());
        }
        #endregion

        #region EventHandlers
        void TextBoxWithBckgText_GotFocus(object sender, EventArgs e)
        {
            if (Text == bckgText)
            {
                Text = String.Empty;
                ForeColor = textColor;
            }
            else
                SelectAll();
        }
        void TextBoxWithBckgText_LostFocus(object sender, EventArgs e)
        {
            if (Text == String.Empty)
            {
                Text = bckgText;
                ForeColor = bckgTextColor;
            }
        }
        void TextBoxWithBckgText_TextChanged(object sender, EventArgs e)
        {
            if (ForeColor == bckgTextColor && Text != BckgText)
                ForeColor = textColor;
        }
        #endregion
    }
}
