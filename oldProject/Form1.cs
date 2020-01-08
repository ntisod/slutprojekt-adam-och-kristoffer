using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace flappyplane
{
    public partial class Form1 : Form
    { 
        
        bool goup;
        bool godown;
        bool shot = false;
        int score = 0;
        int speed = 8; 
        Random rand = new Random();
        int playerSpeed = 7;
        int index;
        bool jumping = false;
        int gravity = 2;

        public Form1()
        {
            InitializeComponent();
           

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void changeUFO()
        {
            index += 1;
            if (index > 3)
            {
                index = 1;
            }

            switch (index)
            {
                case 1:
                    ufo.Image = Properties.Resources.alien1;
                    break;

                case 2:
                    ufo.Image = Properties.Resources.alien2;
                    break;

                case 3:
                    ufo.Image = Properties.Resources.alien3;
                    break;

            }

        }

        private void makeBullet()
        {
            PictureBox bullet = new PictureBox();
            bullet.BackColor = System.Drawing.Color.DarkOrange;
            bullet.Height = 5;
            bullet.Width = 10;
            bullet.Left = player.Left + player.Width;
            bullet.Top = player.Top + player.Height / 2;
            bullet.Tag = "bullet";
            this.Controls.Add(bullet);
        }

        private void keyisdown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                goup = true;
            }
            if (e.KeyCode == Keys.Down)
            {
                godown = true;
            }
            if (e.KeyCode == Keys.R && shot == false)
            {
                makeBullet();
                shot = true;
            }
            if (e.KeyCode == Keys.Space)
            {
                jumping = true;
                gravity = -5;

            }
        }

        private void keyisup(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                goup = false;
            }
            if (e.KeyCode == Keys.Down)
            {
                godown = false;
            }
            if (shot == true)
            {
                shot = false;
            }
            if (e.KeyCode == Keys.Space)
            {
                jumping = false;
                gravity = 5;

            }
        }

        private void gametick(object sender, EventArgs e)
        {
            pillar1.Left -= speed;
            pillar2.Left -= speed;
            ufo.Left -= speed;
            label1.Text = "Score: " + score;
            player.Top+= gravity;

            if (goup)
            {
                player.Top -= playerSpeed;
            }

            if (godown)
            {
                player.Top += playerSpeed;
            }

            if (pillar1.Left < -150)
            {
                pillar1.Left = 900;
            }

            if (pillar2.Left < -150)
            {
                pillar2.Left = 1000;
            }

            if (ufo.Left < -5 ||
                player.Bounds.IntersectsWith(ufo.Bounds) ||
                player.Bounds.IntersectsWith(pillar1.Bounds) ||
                player.Bounds.IntersectsWith(pillar2.Bounds)
                 )
            {
                gameTimer.Stop();
                MessageBox.Show("Du misslyckades med uppdraget, du dödade " + score + " rymdskepp");
                this.Close();

            }

            foreach (Control X in this.Controls)
            {

                if (X is PictureBox && X.Tag == "bullet")
                {
                    X.Left += 15;

                    if (X.Left > 900)
                    {
                        this.Controls.Remove(X);

                        X.Dispose();
                    }
                    if (X.Bounds.IntersectsWith(ufo.Bounds))
                    {
                        score += 1;
                        this.Controls.Remove(X);
                        X.Dispose();
                        ufo.Left = 1000;
                        ufo.Top = rand.Next(5, 330) - ufo.Height;
                        changeUFO();
                    }
                }

            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {


            SoundPlayer s = new SoundPlayer(@"C:\Users\adam.nasser1\Desktop\Music\DesiJourney.wav");

                s.Play();         
        }

      
    }
}
