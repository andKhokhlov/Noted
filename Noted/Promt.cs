using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Noted
{
    internal class Promt
    {
        public static string ShowDialog(string text, string caption, string defaultValue)
        {
            Form prompt = new Form()
            {
                Width = 300,
                Height = 300,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Название заметки",
                StartPosition = FormStartPosition.CenterScreen
            };
            Label textLabel = new Label() { Left = 15, Top = 25, Width = 150 , Text = text };
            TextBox textBox = new TextBox() { Left = 15, Top = 50, Width = 250 };
            Button confirmation = new Button() { Text = "OK", Left = 15, Width = 250, Top = 100, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : defaultValue;
        }
    }
}
