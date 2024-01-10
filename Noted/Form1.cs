using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Noted.DataLayer;


namespace Noted
{
    public partial class Form1 : Form
    {
        private int nextWindowId = 1;
        private NoteRepository noteRepository = new NoteRepository();

        public Form1()
        {
            InitializeComponent();
            LoadNotes();
            listViewNotes.View = View.Details; // Устанавливаем режим отображения в виде таблицы
            listViewNotes.Columns.Add("Ваши заметки ", -2, HorizontalAlignment.Left); // Добавляем столбец "Title" с автоматической шириной
            listViewNotes.SelectedIndexChanged += ListViewNotes_SelectedIndexChanged;
        }
        private void LoadNotes()
        {
            List<Note> notes = noteRepository.GetNotes();

            foreach (var note in notes)
            {
                ListViewItem item = new ListViewItem(new string[] { note.Title });
                listViewNotes.Items.Add(item);
            }
        }

        private void ListViewNotes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewNotes.SelectedItems.Count > 0)
            {
                int selectedNoteId = Convert.ToInt32(listViewNotes.SelectedItems[0].Text);
                Note selectedNote = noteRepository.GetNoteById(selectedNoteId);

                // Открываем ChildForm с выбранной заметкой
                using (ChildForm childForm = new ChildForm(selectedNoteId, selectedNote.Title, Icon, selectedNote))
                {
                    childForm.ShowDialog();
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button2.FlatStyle = FlatStyle.Flat;
            button2.FlatAppearance.BorderSize = 0;
            button2.BackColor = this.BackColor;
        }


        private void button2_Click(object sender, EventArgs e)
        {
            string title = Promt.ShowDialog("Введите заголовок:", "Заголовок", "По умолчанию");

            Icon icon = new Icon("D:\\Code D\\Visual Studio\\Noted\\Noted\\note.ico");
            Note newNote = new Note
            {
                NoteId = nextWindowId,
                Title = title,
                // Дополните объект Note по вашему усмотрению
            };

            var newWindow = new ChildForm(nextWindowId, title, icon, newNote);
            newWindow.TitleChanged += ChildForm_TitleChanged;
            newWindow.Show();
            nextWindowId++;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            //label1.Text = ChildForm.Text;
        }
        private void ChildForm_TitleChanged(object sender, EventArgs e)
        {
            // Обработчик события изменения заголовка
            if (sender is ChildForm childForm)
            {
                // Обновляем Label на главной форме
                label1.Text = childForm.Text;
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewNotes.SelectedItems.Count > 0)
            {
                int selectedNoteId = Convert.ToInt32(listViewNotes.SelectedItems[0].Text);
                Note selectedNote = noteRepository.GetNoteById(selectedNoteId);

                // Если ChildForm уже создан, просто обновите его содержимое
                if (Application.OpenForms["ChildForm"] is ChildForm childForm)
                {
                    childForm.UpdateContent();
                }
                else
                {
                    // Иначе создайте новый ChildForm
                    using (ChildForm newChildForm = new ChildForm(selectedNoteId, selectedNote.Title, Icon, selectedNote))
                    {
                        newChildForm.Show();
                    }
                }
            }
        }
    }

}
