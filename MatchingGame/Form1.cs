using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace MatchingGame
{
    public partial class Form1 : Form
    {
        Random random = new Random();

        List<string> icons = new List<string>()
        {
            "!", "!", "N", "N", ",", ",", "k", "k",
            "b", "b", "v", "v", "w", "w", "z", "z"
        };

        Label firstClicked = null, secondClicked = null;
        public int playTime;
        SoundPlayer soundPlayer1, soundPlayer2;

        private void AssignIconsToSquares()
        {
            // The TableLayoutPanel has 16 labels,
            // and the icon list has 16 icons,
            // so an icon is pulled at random from the list
            // and added to each label
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label iconLabel = control as Label;
                if (iconLabel != null)
                {
                    int randomNum = random.Next(icons.Count);
                    iconLabel.Text = icons[randomNum];
                    iconLabel.ForeColor = iconLabel.BackColor;
                    icons.RemoveAt(randomNum);
                }
            }
        }
        public Form1()
        {
            InitializeComponent();
            AssignIconsToSquares();
            timer2.Start();
            soundPlayer1 = new SoundPlayer("correct.wav");
            soundPlayer2 = new SoundPlayer("miss.wav");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            // init backcolor
            firstClicked.BackColor = Color.CornflowerBlue;
            secondClicked.BackColor = Color.CornflowerBlue;
            // Hide both icons
            firstClicked.ForeColor = firstClicked.BackColor;
            secondClicked.ForeColor = secondClicked.BackColor;
            // Reset firstClicked and secondClicked 
            // so the next time a label is
            // clicked, the program knows it's the first click
            firstClicked = null;
            secondClicked = null;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            // The timer is only on after two non-matching 
            // icons have been shown to the player, 
            // so ignore any clicks if the timer is running
            if (timer1.Enabled == true)
            {
                return;
            }

            Label clickedLabel = sender as Label;

            if (clickedLabel != null)
            {
                // If the clicked label is black, the player clicked
                // an icon that's already been revealed --
                // ignore the click
                if (clickedLabel.ForeColor == Color.Black)
                {
                    return;
                }
                // If firstClicked is null, this is the first icon
                // in the pair that the player clicked, 
                // so set firstClicked to the label that the player 
                // clicked, change its color to black, and return
                if (firstClicked == null)
                {
                    firstClicked = clickedLabel;
                    firstClicked.ForeColor = Color.Black;
                    firstClicked.BackColor = Color.Pink;
                    return;
                }
                // If the player gets this far, the timer isn't
                // running and firstClicked isn't null,
                // so this must be the second icon the player clicked
                // Set its color to black
                secondClicked = clickedLabel;
                secondClicked.ForeColor = Color.Black;
                secondClicked.BackColor = Color.Pink;

                CheckForWinner();

                if (firstClicked.Text == secondClicked.Text)
                {
                    firstClicked = null;
                    secondClicked = null;
                    soundPlayer1.Play();
                    return;
                }
                soundPlayer2.Play();
                // If the player gets this far, the player 
                // clicked two different icons, so start the 
                // timer (which will wait three quarters of 
                // a second, and then hide the icons)
                timer1.Start();
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            playTime += 1;
            this.Text = "Matching Game (" + playTime + " seconds)";
        }

        private void CheckForWinner()
        {
            // Go through all of the labels in the TableLayoutPanel, 
            // checking each one to see if its icon is matched
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label selectedLabel = control as Label;
                if (selectedLabel != null)
                {
                    if (selectedLabel.ForeColor == selectedLabel.BackColor)
                    {
                        return;
                    }
                }
            }
            // If the loop didn’t return, it didn't find
            // any unmatched icons
            // That means the user won. Show a message and close the form
            timer2.Stop();
            MessageBox.Show("You matched all the icons! (" + playTime + " seconds)", "Congratulations");
            Close();
        }
    }
}
