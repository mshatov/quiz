using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace quiz_class
{
    class Question
    {
        private string text;
        private string rightAnswer;
        private string givenAnswer;
        private int score;
        private string type;
        private List<string> options;
        public List<int> orderY;
        private Point MouseDownLocation;

        public Question(List<string> list)
        {
            
            text = list[0];
            type = list[1];
            rightAnswer = list[2];
            orderY = new List<int>(new int[rightAnswer.Length]);
            givenAnswer = "";
            options = new List<string>();
            for (int i = 0; i < rightAnswer.Length; i++)
            {
                options.Add(list[i + 3]);
            }
        }

        public string GivenAnswer
        {
            set
            {
                givenAnswer = value;
                score = givenAnswer == rightAnswer ? 1 : 0;

            }
            get { return givenAnswer; }
        }

        public int Score
        {
            get
            {
                return score;
            }
        }

        public string Text
        {
            get { return text; }
        }

        public void setAnswers(Control panel)
        {
            int i = 0;
            foreach (Control selector in panel.Controls.OfType<CheckBox>().ToList())
            {
                CheckBox checkBox = (CheckBox)selector;
                if (givenAnswer[i] == '1')
                {
                    checkBox.Checked = true;
                }
                i++;
            }
            foreach (Control selector in panel.Controls.OfType<RadioButton>().ToList())
            {
                RadioButton radioButton = (RadioButton)selector;
                if (givenAnswer[i] == '1')
                {
                    radioButton.Checked = true;
                }
                i++;
            }
            foreach (Control selector in panel.Controls.OfType<Button>().ToList())
            {
                Button button = (Button)selector;
                button.Top = orderY[i];
                i++;
            }

        }

        public void askQuestion(RichTextBox questionText, Control panel)
        {
            questionText.Text = text;
            addOptions(panel);
        }

        public void addRadio(Control panel)
        {
            for (int i = 0; i < rightAnswer.Length; i++)
            {
                RadioButton radioButton = new RadioButton()
                {
                    Text = options[i],
                    Location = new Point(10, 40 * i + 10),
                    AutoSize = true,
                };
                panel.Controls.Add(radioButton);
            }
        }

        public void addCheck(Control panel)
        {
            for (int i = 0; i < rightAnswer.Length; i++)
            {
                CheckBox checkBox = new CheckBox()
                {
                    Text = options[i],
                    Location = new Point(10, 40 * i + 10),
                    AutoSize = true,
                };
                panel.Controls.Add(checkBox);
            }
        }

        public void addButton(Control panel)
        {
            for (int i = 0; i < rightAnswer.Length; i++)
            {
                Button button = new Button()
                {
                    Text = options[i],
                    Location = new Point(10, 40 * i + 40),
                    AutoSize = true,
                };

                button.MouseDown += button_MouseDown;
                button.MouseMove += button_MouseMove;

                panel.Controls.Add(button);
            }
        }

        public void addOptions(Control panel)
        {
            switch (type)
            {
                case "s":
                    addRadio(panel);
                    break;
                case "m":
                    addCheck(panel);
                    break;
                case "o":
                    addButton(panel);
                    break;
                default:
                    break;
            }
        }

        private void button_MouseMove(object sender, MouseEventArgs e)
        {
            Button btn = sender as Button;
            if (e.Button == MouseButtons.Left)
            {
                //btn.Left = e.X + btn.Left - MouseDownLocation.X;
                btn.Top = e.Y + btn.Top - MouseDownLocation.Y;
            }
        }

        private void button_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MouseDownLocation = e.Location;
            }
        }


    }
}
