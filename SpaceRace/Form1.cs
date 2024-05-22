//Aaron Marchanton
//May 22, 2024
//Space Race

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

namespace SpaceRace
{
    public partial class Form1 : Form
    {
        //Create Objects and Brushes
        Rectangle player1 = new Rectangle(182, 480, 10, 25);
        Rectangle player2 = new Rectangle(406, 480, 10, 25);
        Rectangle zoneborder1 = new Rectangle(0, 530, 600, 3);
        Rectangle zoneborder2 = new Rectangle(0, 455, 600, 3);

        SolidBrush p1Brush = new SolidBrush(Color.DodgerBlue);
        SolidBrush p2Brush = new SolidBrush(Color.Goldenrod);
        SolidBrush grayBrush = new SolidBrush(Color.LightGray);
        SolidBrush whiteBrush = new SolidBrush(Color.White);

        //New Randoms and Integers
        Random randGen = new Random();
        int randValue = 0;

        int player1Score = 0;
        int player2Score = 0;

        int playerSpeed = 4;
        int meteorSize = 6;


        bool wPressed = false;
        bool sPressed = false;
        bool upPressed = false;
        bool downPressed = false;

        //List of balls
        List<Rectangle> meteorList = new List<Rectangle>();
        List<int> meteorSpeeds = new List<int>();

        //Sounds Create
        SoundPlayer PointSound = new SoundPlayer(Properties.Resources.PointSound);
        SoundPlayer WinSound = new SoundPlayer(Properties.Resources.WinSound);
        SoundPlayer ExplosionSound = new SoundPlayer(Properties.Resources.ExplosionSound);

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            //Keys Input (Unpress)
            switch (e.KeyCode)
            {
                case Keys.W:
                    wPressed = false;
                    break;
                case Keys.S:
                    sPressed = false;
                    break;
                case Keys.Up:
                    upPressed = false;
                    break;
                case Keys.Down:
                    downPressed = false;
                    break;
                case Keys.Space:
                    gameTimer.Enabled = true;
                    break;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //Keys Input (Press)
            switch (e.KeyCode)
            {
                case Keys.W:
                    wPressed = true;
                    break;
                case Keys.S:
                    sPressed = true;
                    break;
                case Keys.Up:
                    upPressed = true;
                    break;
                case Keys.Down:
                    downPressed = true;
                    break;
                case Keys.Space:
                    if (gameTimer.Enabled == false)
                    {
                        ResetGame();
                    }
                    break;
                case Keys.Escape:
                    if (gameTimer.Enabled == false)
                    {
                        Application.Exit();
                    }
                    break;
            }
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            //Complete Actions
            PlayerMovement();
            MeteorsSpawn();
            MeteorHitsPlayer();
            ScoresOrWins();

            Refresh();
        }

        public void PlayerMovement()
        {
            //move player 1
            if (wPressed == true && player1.Y > 0)
            {
                player1.Y = player1.Y - playerSpeed;
            }
            if (sPressed == true && player1.Y < 505)
            {
                player1.Y = player1.Y + playerSpeed;
            }

            //move player 2
            if (upPressed == true && player2.Y > 0)
            {
                player2.Y = player2.Y - playerSpeed;
            }
            if (downPressed == true && player2.Y < 505)
            {
                player2.Y = player2.Y + playerSpeed;
            }
        }

        public void MeteorsSpawn()
        {
            //create balls
            for (int i = 0; i < meteorList.Count(); i++)
            {
                int x = meteorList[i].X + meteorSpeeds[i];

                meteorList[i] = new Rectangle(x, meteorList[i].Y, meteorSize, meteorSize);
            }

            randValue = randGen.Next(0, 100);

            //spawn balls with various speeds (Left to Right)
            if (randValue < 17)
            {
                    randValue = randGen.Next(10, 455 - meteorSize * 2);

                    Rectangle ball = new Rectangle(0, randValue, meteorSize, meteorSize);
                    meteorList.Add(ball);
                    meteorSpeeds.Add(randGen.Next(5, 10));
            }

            //spawn balls with various speeds (Right to Left)
            if (randValue > 20 && randValue < 34)
            {
                randValue = randGen.Next(10, 455 - meteorSize * 2);

                Rectangle ball = new Rectangle(600, randValue, meteorSize, meteorSize);
                meteorList.Add(ball);
                meteorSpeeds.Add(randGen.Next(-10, -5));
            }
        }

        public void MeteorHitsPlayer()
        {
            //Reset Player To Starting Position if Hit by a Meteor
            for (int i = 0; i < meteorList.Count(); i++)
            {
                if (meteorList[i].IntersectsWith(player1))
                {
                    player1 = new Rectangle(182, 480, 10, 25);

                    ExplosionSound.Play();
                }
                else if (meteorList[i].IntersectsWith(player2))
                {
                    player2 = new Rectangle(406, 480, 10, 25);

                    ExplosionSound.Play();
                }
            }
        }

        public void ScoresOrWins()
        {
            //If Playr Scores Add a Point
            if (player1.Y < 1)
            {
                player1.Y = 480;
                player1Score++;
                p1ScoreLabel.Text = $"{player1Score}";

                PointSound.Play();
            }

            if (player2.Y < 1)
            {
                player2.Y = 480;
                player2Score++;
                p2ScoreLabel.Text = $"{player2Score}";

                PointSound.Play();
            }

            //If Player Wins Display End Screen
            if (player1Score == 3 || player2Score == 3)
            {
                gameTimer.Enabled = false;

                WinSound.Play();
            }
        }

        public void ResetGame()
        {
            //Reset Everything To Allow Restart
            titleLabel.Text = "";
            subtitleLabel.Text = "";

            gameTimer.Enabled = true;

            player1Score = 0;
            player2Score = 0;

            meteorList.Clear();
            meteorSpeeds.Clear();

            Rectangle player1 = new Rectangle(182, 480, 10, 25);
            Rectangle player2 = new Rectangle(406, 480, 10, 25);
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (gameTimer.Enabled == false && player1Score == 0 && player2Score == 0)
            {
                titleLabel.Text = "Space Race!";
                subtitleLabel.Text = "Press Space to Start or Esc to End";
            }
            else if (gameTimer.Enabled == true)
            {
                //draw balls
                for (int i = 0; i < meteorList.Count(); i++)
                {
                    e.Graphics.FillEllipse(whiteBrush, meteorList[i]);
                }

                //Draw All Other Objects
                e.Graphics.FillRectangle(grayBrush, zoneborder1);
                e.Graphics.FillRectangle(grayBrush, zoneborder2);
                e.Graphics.FillRectangle(p1Brush, player1);
                e.Graphics.FillRectangle(p2Brush, player2);
            }
            else
            {
                if (player1Score == 3)
                {
                    titleLabel.Text = $"P1 Wins!!!";
                    subtitleLabel.Text = "Press Sace to Start and Escape to End";
                }

                if (player2Score == 3)
                {
                    titleLabel.Text = $"P2 Wins!!!";
                    subtitleLabel.Text = "Press Sace to Start and Escape to End";
                }
            }
        }
    }
}