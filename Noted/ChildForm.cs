using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Noted.Form1;

namespace Noted
{
    public partial class ChildForm : Form
    {
        private TextBox noteTextBox;
        private Note currentNote;

        public int WindowId { get; }
        public event EventHandler TitleChanged;
        public event EventHandler NoteUpdated;

        public ChildForm(int windowId, string title,Icon icon, Note note)
        {
            InitializeComponent();
            WindowId = windowId;
            Text = title;

            currentNote = note;
            UpdateContent();

            //note = selectedNote;
            //DisplayNote();
            InitializeNoteTextBox();

            this.Icon = icon;
            this.currentNote = currentNote;
        }
        /*private void DisplayNote()
        {
            // Здесь вы можете установить значения для элементов управления (например, Label или TextBox),
            // чтобы отобразить заголовок и содержимое заметки.
            titleLabel.Text = note.Title;
            contentTextBox.Text = note.Content;
        }*/

        public void UpdateContent()
        {
            // Обновление содержимого TextBox или других элементов управления в соответствии с текущей заметкой
            if (currentNote != null)
            {
                titleTextBox.Text = currentNote.Title;
                contentTextBox.Text = currentNote.Content;
                
            }
        }

        protected virtual void OnNoteUpdated(EventArgs e)
        {
            NoteUpdated?.Invoke(this, e);
        }

        private void InitializeNoteTextBox()
        {
            noteTextBox = new TextBox
            {
                Multiline = true,
                Dock = DockStyle.Fill,
                ScrollBars = ScrollBars.Vertical,
                WordWrap = true
            };

            Controls.Add(noteTextBox);
        }
        private void ChildForm_Load(object sender, EventArgs e)
        {
            // Настройка содержимого нового окна
           // Text = $"Child Window {WindowId}";
            // Добавьте здесь код для настройки содержимого нового окна, если необходимо
            OnTitleChanged();
        }
        protected virtual void OnTitleChanged()
        {
            TitleChanged?.Invoke(this, EventArgs.Empty);
        }


        private void SaveNoteToDatabase(int windowId, string title, string content)
        {
            // Код подключения и сохранения данных в базу данных PostgreSQL
            // ...

            string connectionString = "Host=localhost;Username=postgres;Password=root;Database=Noted";
            string query = "INSERT INTO notes (window_id, title, content) VALUES (@WindowId, @Title, @Content)";

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@WindowId", windowId);
                    command.Parameters.AddWithValue("@Title", title);
                    command.Parameters.AddWithValue("@Content", content);

                    command.ExecuteNonQuery();
                }
            }
        }



        //private void SaveButton_Click(object sender, EventArgs e)
        //{
          //  SaveNoteToDatabase(WindowId, Text, noteTextBox.Text);
        //}


        private void button1_Click(object sender, EventArgs e)
        {
            SaveNoteToDatabase(WindowId, Text, noteTextBox.Text);
            this.Hide();
            OnNoteUpdated(EventArgs.Empty);
            
        }
    }
}
    