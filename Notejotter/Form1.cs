using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Notejotter
{
    public partial class NoteJotterMain : Form
    {
        string title = " - NoteJotter";
        bool fileSaved = true;

        public NoteJotterMain()
        {
            InitializeComponent();
            this.Text = "Untitled" + title;
        }

        // Open file
        private void mnuOpen_Click(object sender, EventArgs e)
        {
            string openFile = "";

            
            openFD.Title = "Open text file";
            openFD.Filter = "TXT files|*.txt";
            openFD.FileName = "";
            openFD.DefaultExt = ".txt";

            if (openFD.ShowDialog() != DialogResult.Cancel && openFD.CheckFileExists)
            {
                openFile = openFD.FileName;
                richTextBox1.LoadFile(openFile, RichTextBoxStreamType.PlainText);
                this.Text = openFile + title;
                fileSaved = true;
            }
        }

        // Save file
        private void mnuSave_Click(object sender, EventArgs e)
        {
            SaveFile();
        }

        private void NoteJotterMain_Load(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            // Set bool fileChanged to FALSE when something happens
            fileSaved = false;
        }

        // New file
        private void mnuNew_Click(object sender, EventArgs e)
        {
            if (fileSaved)
            {
                // Clear text
                NewFile();
            }
            else
            {
                DialogResult dr = new DialogResult();
                dr = MessageBox.Show("This file has not been saved yet.\nDo you wish to save it now?",
                    "Warning!",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Warning);

                if (dr == DialogResult.Yes)
                {
                    // Save file
                    Console.WriteLine("Save this file");
                    SaveFile("thenNew");
                }
                else if (dr == DialogResult.No)
                {
                    // Don't save file 
                    Console.WriteLine("Discard changes");
                    NewFile();
                }
                else
                {
                    // Stop new file, keep current file
                    Console.WriteLine("Cancel new file");
                }
            }
        }

        // Empty the whole screen for a new file after confirming save of discarding file
        void NewFile()
        {
            richTextBox1.Clear();
            this.Text = "Untitled" + title;
        }

        // Save current file either by clicking the menu option or by warning message
        void SaveFile()
        {
            //because a SaveFile without arguments is okay
            SaveFile("continue");
        }
        void SaveFile(string next)
        {
            string saveFile = "";

            saveFD.Title = "Save text file";
            saveFD.Filter = "TXT files|*.txt";
            saveFD.FileName = "";
            saveFD.DefaultExt = ".txt";

            if (saveFD.ShowDialog() != DialogResult.Cancel)
            {
                saveFile = saveFD.FileName;
                richTextBox1.SaveFile(saveFile, RichTextBoxStreamType.PlainText);
                //MessageBox.Show("File <b>" + saveFile + "</b> saved.",
                 //   "Saved");
                this.Text = saveFile + title;

                fileSaved = true;
            }

            switch (next)
            {
                case "thenNew":
                    // Open new empty file
                    NewFile();
                    break;
                case "thenExit":
                    Quit();
                    break;
                default:
                    // Continue this file

                    break;
            }
        }

        private void mnuQuit_Click(object sender, EventArgs e)
        {
            if (fileSaved)
            {
                // Clear text
                Quit();
            }
            else
            {
                DialogResult dr = new DialogResult();
                dr = MessageBox.Show("This file has not been saved yet.\nDo you wish to save it now?",
                    "Warning!",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Warning);

                if (dr == DialogResult.Yes)
                {
                    // Save file
                    Console.WriteLine("Save this file");
                    SaveFile("thenExit");
                }
                else if (dr == DialogResult.No)
                {
                    // Don't save file 
                    Console.WriteLine("Discard changes");
                    Quit();
                }
                else
                {
                    // Stop new file, keep current file
                    Console.WriteLine("Cancel exiting");
                }
            }
        }

        void Quit()
        {
            //Application.Exit();
            this.Close();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (fileSaved)
            {
                // Just exit
                //Quit();
            }
            else
            {
                DialogResult dr = new DialogResult();
                dr = MessageBox.Show("This file has not been saved yet.\nDo you wish to save it now?",
                    "Warning!",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Warning);

                if (dr == DialogResult.Yes)
                {
                    e.Cancel = true;
                    // Save file
                    Console.WriteLine("Save this file");
                    SaveFile("thenExit");
                }
                else if (dr == DialogResult.No)
                {
                    // Don't save file 
                    Console.WriteLine("Discard changes");
                    //Quit();
                }
                else
                {
                    // Stop new file, keep current file
                    Console.WriteLine("Cancel exiting");
                    e.Cancel = true;
                }
            }


            //e.Cancel = true;
            base.OnFormClosing(e);
        }

        /* protected override void OnFormClosing(FormClosingEventArgs e)
         {
             if (DialogResult == DialogResult.OK)
             {
                 if (!fileSaved)
                 {
                     e.Cancel = true;
                     return;          // Is not calling the base class OnFormClosing okay here?
                 }
             }
             base.OnFormClosing(e);
         }*/
    }
}
