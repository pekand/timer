using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Timer
{
    public partial class FormTimer : Form
    {
        private long cTimer = 0;
        private long aTimer = 0;
        private bool paused = false;

        public FormTimer()
        {
            InitializeComponent(); 
        }

        private void FormTimer_Load(object sender, EventArgs e)
        {
            cTimer = Properties.Settings.Default.cTime;
            aTimer = new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds();

            this.timer_Tick(this, null);
            timer.Enabled = true;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            refreshLabel();
        }

        private void refreshLabel() {
            labelTimer.Text = formatSeconds(cTimer + countTimerDiff());
        }


        private long countTimerDiff()
        {
            var timeNow = new DateTimeOffset(DateTime.Now);
            return (timeNow.ToUnixTimeMilliseconds() - aTimer);
        }

        private string formatSeconds(long totalSeconds = 0) {
            long miliseconds = totalSeconds % 1000;
            totalSeconds = totalSeconds / 1000;
            
            long seconds = totalSeconds % 60;
            totalSeconds = totalSeconds / 60;

            long minutes = totalSeconds % 60;
            totalSeconds = totalSeconds / 60;

            long hours = totalSeconds % 60;
            totalSeconds = totalSeconds / 60;

            return padInt(hours, 2) + ":" + padInt(minutes, 2) + ":" + padInt(seconds, 2);
        }

        private string padInt(long i, int pad) { 
            return i.ToString().PadLeft(pad, '0');
        }

        private void FormTimer_MouseClick(object sender, MouseEventArgs e)
        {
            pauseTimer();
        }

        private void labelTimer_MouseClick(object sender, MouseEventArgs e)
        {
            pauseTimer();
        }

        private void pauseTimer() {
            if (paused)
            {
                this.Text = "Timer";
                aTimer = new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds();
                timer.Enabled = true;
                paused = false;
            }
            else
            {
                this.Text = "Timer - Paused";
                cTimer += countTimerDiff();
                timer.Enabled = false;
                paused = true;
            }
        }

        private void FormTimer_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.cTime = cTimer + countTimerDiff();

            Properties.Settings.Default.Save();
        }

        private void resetTimer() {
            aTimer = new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds();
            cTimer = 0;
            Properties.Settings.Default.cTime = 0;
            Properties.Settings.Default.Save();
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            resetTimer();
            refreshLabel();
        }

        private void pauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pauseTimer();
        }
    }
}
