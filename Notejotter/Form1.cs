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
        // Declare the main title for the top screen bar thingy
        string title = " - NoteJotter";
        // Is the file saved in its current form? New files == true because empty files don't need to be saved on exit.
        bool fileSaved = true;
        // Does the file that is open exist in a saved form, not current but earlier?
        // If so, saving functions should skip saving dialog on saving.
        bool fileExists = false;
        string fileExistsInName;

        public NoteJotterMain()
        {
            InitializeComponent();
            this.Text = "Untitled" + title;

            if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Text))
            {
                mnuPaste.Enabled = true;
            }
        }

        //####################################################
        //                      ONCLICKS
        //####################################################

        // File menu
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
                fileExists = true;
                fileExistsInName = openFile;
            }
        }

        private void mnuSave_Click(object sender, EventArgs e)
        {
            SaveFile();
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
                    fileSaved = true;
                    Quit();
                }
                else
                {
                    // Stop new file, keep current file
                    Console.WriteLine("Cancel exiting");
                }
            }
        }

        // Edit menu
        private void mnuUndo_Click(object sender, EventArgs e)
        {
            Undo();
        }

        private void mnuRedo_Click(object sender, EventArgs e)
        {
            Redo();
        }

        private void mnuCut_Click(object sender, EventArgs e)
        {
            Cut();
        }

        private void mnuCopy_Click(object sender, EventArgs e)
        {
            Copy();
        }

        private void mnuPaste_Click(object sender, EventArgs e)
        {
            Paste();
        }

        // View menu

        // Tools menu

        // Others
        private void richTextBox1_SelectionChanged(object sender, EventArgs e)
        {

            if (richTextBox1.SelectionLength > 0)
            {
                mnuCopy.Enabled = true;
                mnuCut.Enabled = true;

            }
            else
            {
                mnuCopy.Enabled = false;
                mnuCut.Enabled = false;
            }

            if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Text))
            {
                mnuPaste.Enabled = true;
            }
            else
            {
                mnuPaste.Enabled = false;
            }

            if (richTextBox1.CanUndo)
            {
                mnuUndo.Enabled = true;
            }
            else
            {
                mnuUndo.Enabled = false;
            }

            if (richTextBox1.CanRedo)
            {
                mnuRedo.Enabled = true;
            }
            else
            {
                mnuRedo.Enabled = false;
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            // Set bool fileChanged to FALSE when something happens
            fileSaved = false;

            FillUndoArray();
        }

        //############################################################
        //                          METHODS
        //############################################################

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
            string saveFile;

            saveFD.Title = "Save text file";
            saveFD.Filter = "TXT files|*.txt";
            saveFD.FileName = "";
            saveFD.DefaultExt = ".txt";

            if (!fileExists)
            {
                if (saveFD.ShowDialog() != DialogResult.Cancel)
                {
                    saveFile = saveFD.FileName;
                    richTextBox1.SaveFile(saveFile, RichTextBoxStreamType.PlainText);
                    //MessageBox.Show("File <b>" + saveFile + "</b> saved.",
                    //   "Saved");
                    this.Text = saveFile + title;

                    fileSaved = true;
                }
            }
            else // If the file has been saved before, store it there
            {
                saveFile = fileExistsInName;
                Console.WriteLine(saveFile);
                richTextBox1.SaveFile(saveFile, RichTextBoxStreamType.PlainText);

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

        // Quit application
        void Quit()
        {
            //Application.Exit();
            this.Close();
        }

        void Undo()
        {
            if (richTextBox1.CanUndo)
            {
                richTextBox1.Undo();
                //richTextBox1.ClearUndo();
            }
        }

        void Redo()
        {
            Console.WriteLine("Redo method. CanRedo == " + richTextBox1.CanRedo);

            richTextBox1.Redo();
        }
        void Cut()
        {
            if (richTextBox1.SelectedText.Length > 0)
            {
                richTextBox1.Cut();
            }
        }

        void Copy()
        {
            if (richTextBox1.SelectedText.Length > 0)
            {
                richTextBox1.Copy();
            }
        }

        void Paste()
        {
            // Suspend layout to avoid blinking
            richTextBox1.SuspendLayout();

            string selectedText = richTextBox1.SelectedText;

            // If text is selected
            if (selectedText.Length > 0)
            {
                // Get start of selection
                int insPt = richTextBox1.SelectionStart;

                // Get Length of selection
                int exPt = richTextBox1.SelectionLength;

                // Get Length of full text
                int endPt = richTextBox1.TextLength;

                // Preserve text beyond selection
                string postRTFContent = richTextBox1.Text.Substring(insPt + exPt, endPt - (insPt + exPt));

                // Remove selection and text beyond selection
                richTextBox1.Text = richTextBox1.Text.Substring(0, insPt);

                // add the clipboard content and then the preserved postRTF content
                richTextBox1.Text += (string)Clipboard.GetData("Text") + postRTFContent;

                // adjust the insertion point to just after the inserted text
                richTextBox1.SelectionStart = richTextBox1.TextLength - postRTFContent.Length;

                //Console.WriteLine("Ins: " + insPt + " Ex: " + exPt + " End: " + endPt);
                //Console.WriteLine(postRTFContent);
            }

            // If there is no text selected
            else
            {
                // get insertion point
                int insPt = richTextBox1.SelectionStart;

                // preserve text from after insertion pont to end of RTF content
                string postRTFContent = richTextBox1.Text.Substring(insPt);

                // remove the content after the insertion point
                richTextBox1.Text = richTextBox1.Text.Substring(0, insPt);

                // add the clipboard content and then the preserved postRTF content
                richTextBox1.Text += (string)Clipboard.GetData("Text") + postRTFContent;

                // adjust the insertion point to just after the inserted text
                richTextBox1.SelectionStart = richTextBox1.TextLength - postRTFContent.Length;
            }


            // Restore layout
            richTextBox1.ResumeLayout();
        }

        /*void Paste()
        {
            // suspend layout to avoid blinking
            richTextBox1.SuspendLayout();

            // get insertion point
            int insPt = richTextBox1.SelectionStart;

            // preserve text from after insertion pont to end of RTF content
            string postRTFContent = richTextBox1.Text.Substring(insPt);

            // remove the content after the insertion point
            richTextBox1.Text = richTextBox1.Text.Substring(0, insPt);

            // add the clipboard content and then the preserved postRTF content
            richTextBox1.Text += (string)Clipboard.GetData("Text") + postRTFContent;

            // adjust the insertion point to just after the inserted text
            richTextBox1.SelectionStart = richTextBox1.TextLength - postRTFContent.Length;

            // restore layout
            richTextBox1.ResumeLayout();

            
        }

        /*void Paste()
        {
            if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Text))
            {
                richTextBox1.Paste();
            }
        }*/

        // On pressing red X, hold the quitting until save/no save/cancel
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

        void FillUndoArray()
        {
            //Console.WriteLine("Fill ");
        }

        private void NoteJotterMain_Load(object sender, EventArgs e)
        {

        }
    }
}
