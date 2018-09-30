using System;
using System.Collections.Specialized;
using System.Windows.Forms;

namespace BILLmanager_app
{
    internal class Program
    {
        void button_handler()
        {
            
        }
        
        public static void Main(string[] args)
        {
            Form mainForm = new Form();
            Panel box = new Panel();
            box.Dock = DockStyle.Fill;
            Button button = new Button();
            button.Text = "Button"; 
            button.Click += ButtonHandler;
            button.Dock = DockStyle.Fill;
            box.Controls.Add(button);
            mainForm.Controls.Add(box);
            mainForm.ShowDialog();
        }

        private static void ButtonHandler(object sender, EventArgs e)
        {
            Console.WriteLine("Kek!");
        }
    }
}