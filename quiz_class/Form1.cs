using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace quiz_class
{
    public partial class Form1 : Form
    {
        List<Question> questions = new List<Question>();
        int totalQuestions;
        int currentQuestion = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string fileName = @"../../test.txt";
            IEnumerable<string> lines = File.ReadLines(fileName);

            foreach (var item in lines)
            {
                var data = item.Split('~').ToList();
                questions.Add(new Question(data));
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            totalQuestions = questions.Count;
            questions[currentQuestion].askQuestion(txtQuestion, panel);
            disableButtons();
            lblCurrent.Text = Convert.ToString(currentQuestion + 1);
            lblTotal.Text = Convert.ToString(totalQuestions);
        }

        private void writeAnswer(int currentQuestion)
        {
            string answer = "";
            foreach (Control selector in panel.Controls.OfType<RadioButton>().ToList())
            {
                RadioButton radioButton = (RadioButton)selector;
                answer += radioButton.Checked ? "1" : "0";
            }
            foreach (Control selector in panel.Controls.OfType<CheckBox>().ToList())
            {
                CheckBox checkBox = (CheckBox)selector;
                answer += checkBox.Checked ? "1" : "0";
            }

            Dictionary<int, int> order = new Dictionary<int, int>();
            int i = 0;
            foreach (Control selector in panel.Controls.OfType<Button>().ToList())
            {
                Button button = (Button)selector;
                order[i] = selector.Location.Y;
                questions[currentQuestion].orderY[i] = selector.Location.Y;
                i++;
            }
            var res = order.OrderBy(item => item.Value);

            foreach (var item in res)
            {
                answer += Convert.ToString(item.Key);
            }
            questions[currentQuestion].GivenAnswer = answer;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            writeAnswer(currentQuestion);
            panel.Controls.Clear();
            currentQuestion++;
            lblCurrent.Text = Convert.ToString(currentQuestion + 1);
            var question = questions[currentQuestion];
            question.askQuestion(txtQuestion, panel);
            if (question.GivenAnswer != "")
            {
                question.setAnswers(panel);
            }
            disableButtons();
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            writeAnswer(currentQuestion);
            panel.Controls.Clear();
            currentQuestion--;
            lblCurrent.Text = Convert.ToString(currentQuestion + 1);
            var question = questions[currentQuestion];
            question.askQuestion(txtQuestion, panel);
            if (question.GivenAnswer != "")
            {
                question.setAnswers(panel);
            }
            disableButtons();
        }

        private void disableButtons()
        {
            btnPrev.Enabled = !(currentQuestion == 0);
            btnNext.Enabled = !(currentQuestion == totalQuestions - 1);
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            writeAnswer(currentQuestion);
            int totalScore = 0;
            foreach (var question in questions)
            {
                totalScore += question.Score;
            }
            MessageBox.Show("Правильных ответов: " + totalScore, "Результат", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
