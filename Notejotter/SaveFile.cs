using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notejotter
{
    class SaveFile
    {
        //======================================
        //              CLASS VARIABLES
        //======================================
        private string file;
        private string text;
        private bool isSaved;
        private string errorMessage;

        //======================================
        //              DEFAULT CONSTRUCTORS
        //======================================
        public SaveFile()
        {
            isSaved = false;
            errorMessage = null;
        }
        //======================================
        //              METHODS
        //======================================
        private bool saveFile()
        {
            System.IO.StreamWriter streamWriter = new System.IO.StreamWriter(file);
            streamWriter.WriteLine(text);

            //catch errormessages here and return false with an errormessage here

            streamWriter.Close();

            // or if file has just been saved, return true without error
            return true;
        }

        /*
         * // Compose a string that consists of three lines.
        string lines = "First line.\r\nSecond line.\r\nThird line.";

        // Write the string to a file.
        System.IO.StreamWriter file = new System.IO.StreamWriter("c:\\test.txt");
        file.WriteLine(lines);

        file.Close();
*/
        //======================================
        //              PROPERTIES
        //======================================

        public string File
        {
            set { file = value; }
        }

        public string Text
        {
            set { text = value; }
        }

        public bool IsSaved
        {
            get { return isSaved; }
            set { isSaved = saveFile(); }
        }

        /*
        public string MyProperty
        {
            get { return birthdayMessage; }
            set { birthdayMessage = getMessage(value); }
        }
        */

        public string ErrorMessage
        {
            get { return errorMessage; }
        }







        /* Example thing
        private int myVar;

        public int MyProperty
        {
            get { return myVar; }
            set { myVar = value; }
        }*/


    }
}
