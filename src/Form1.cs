using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MovementChecklist
{
    public partial class Form1 : Form
    {
        public List<Movement> movements = new List<Movement>();
        private GameVersion version = new GameVersion();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // If the Mirror's Edge process was not found, show messagebox with retry and cancel button
            while (Process.GetProcessesByName("MirrorsEdge").FirstOrDefault() == null)
            {
                DialogResult dialog = MessageBox.Show("I had problems finding the Mirror's Edge process...", "MirrorsEdge.exe not found!", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);

                if (dialog == DialogResult.Cancel)
                {
                    Environment.Exit(0);
                }
            }
            
            // Adds the movements to a List
            AddMovementsToList();

            // Add the movements name and it's completed state
            for (int i = 0; i < movements.Count; i++)
            {
                checkedListBox1.Items.Add(movements[i].Name, false);
            }
            
            // Get game version
            while (true)
            {
                // If it return null, then the proccess wasn't found
                if (version.DetectGameVersion() == null)
                {
                    Environment.Exit(0);
                }

                // User is using an unsupported or a nknown version :(
                if (GameVersion.Current == Version.Unknown)
                {
                    // Display a message box telling user that it doesn't support their game version
                    string message = $"This program doesn't support your game version. It only supports Steam, GoG, Origin, Reloaded, Dvd, Origin (Asia), and Origin (DLC) version.\n\nVersion: {GameVersion.Current}\nMemorySize: {GameVersion.MemorySize}";
                    DialogResult dialogResult = MessageBox.Show(message, "Unsupported Version Detected!", MessageBoxButtons.RetryCancel, MessageBoxIcon.Information);

                    // Exit the program
                    if (dialogResult == DialogResult.Cancel)
                    {
                        Environment.Exit(0);
                    }
                }
                else
                {
                    break;
                }
            }

            InitializeMemory();

            // Sets the title
            Text = "Movement Checklist (0/" + movements.Count + ")";

            // Starts the BackgroundWorker
            BGWorker.RunWorkerAsync();
        }

        private void BGWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // Sleeps for 0.1 seconds
            // Change this if it's using too much cpu
            Thread.Sleep(100);

            // Check if Mirror's Edge is running
            if (Process.GetProcessesByName("MirrorsEdge").FirstOrDefault() != null)
            {
                // If Mirror's Edge is still running, continue
                BGWorker.ReportProgress(0);
            }
            else
            {
                // Close the program
                Environment.Exit(0);
            }
        }

        private void BGWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ReadMemory();
        }

        private void BGWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            BGWorker.RunWorkerAsync();
        }

        private void ResetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Set everything to false
            for (int i = 0; i < movements.Count; i++)
            {
                movements[i].Completed = false;
                checkedListBox1.SetItemChecked(i, false);
            }

            // Reset counter
            completedStates = 0;

            // Update title
            Text = "Movement Checklist (0/" + movements.Count + ")";
        }

        private void AddMovementsToList()
        {
            // Sorted by the id
            // Most of these names are the same found in TdGame.u

            movements.Add(new Movement(1, "Walking"));
            movements.Add(new Movement(2, "Falling"));
            movements.Add(new Movement(3, "Grabbing"));
            movements.Add(new Movement(4, "WallRunningRight"));
            movements.Add(new Movement(5, "WallRunningLeft"));
            movements.Add(new Movement(6, "WallClimbing"));
            movements.Add(new Movement(7, "Springboard"));
            movements.Add(new Movement(9, "VaultOver"));
            movements.Add(new Movement(10, "GrabPullUp"));
            movements.Add(new Movement(11, "Jump"));
            movements.Add(new Movement(12, "WallrunJump"));
            movements.Add(new Movement(13, "GrabJump"));
            // movements.Add(new Movement(14, "IntoGrab"));
            movements.Add(new Movement(15, "Crouch"));
            movements.Add(new Movement(16, "Slide"));
            movements.Add(new Movement(17, "Melee"));
            movements.Add(new Movement(18, "Disarm"));
            movements.Add(new Movement(19, "Barge"));
            movements.Add(new Movement(20, "HardLanding")); 
            movements.Add(new Movement(21, "Climb"));
            // movements.Add(new Movement(22, "IntoClimb"));
            movements.Add(new Movement(24, "180Turn"));
            movements.Add(new Movement(25, "180TurnInAir"));
            movements.Add(new Movement(26, "LayOnGround"));
            // movements.Add(new Movement(27, "IntoZipLine"));
            movements.Add(new Movement(28, "ZipLine"));
            movements.Add(new Movement(29, "Balance"));
            movements.Add(new Movement(30, "LedgeWalk"));
            movements.Add(new Movement(31, "GrabTransfer"));
            movements.Add(new Movement(32, "MeleeAir"));
            movements.Add(new Movement(33, "DodgeJump"));
            movements.Add(new Movement(34, "WallRunDodgeJump"));
            movements.Add(new Movement(35, "Stumble"));
            movements.Add(new Movement(38, "RumpSlide"));
            movements.Add(new Movement(39, "Interact"));
            movements.Add(new Movement(47, "Vertigo"));
            movements.Add(new Movement(48, "MeleeSlide"));
            movements.Add(new Movement(49, "WallClimbDodgeJump"));
            movements.Add(new Movement(50, "WallClimb180TurnJump"));
            movements.Add(new Movement(60, "Swing"));
            movements.Add(new Movement(61, "Coil"));
            movements.Add(new Movement(62, "MeleeWallrun"));
            movements.Add(new Movement(63, "MeleeCrouch"));
            movements.Add(new Movement(72, "FallingUncontrolled"));
            movements.Add(new Movement(73, "SwingJump"));
            movements.Add(new Movement(78, "SoftLanding"));
            movements.Add(new Movement(82, "MeleeAirAbove"));
            movements.Add(new Movement(85, "AirBarge"));
            movements.Add(new Movement(91, "SkillRoll"));
        }
    }
}
