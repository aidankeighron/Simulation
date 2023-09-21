using System;
using System.Drawing;
using System.Windows.Forms;

namespace Simulation
{
    class Simulation : System.Windows.Forms.Form
    {
        static void Main(string[] args)
        {
            Application.Run(new Simulation());
        }

        public const int Interval = 20;
        public Physics physics = new Physics();

        public Simulation()
        {
            InitializeComponent();
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = Interval; // ms
            timer.Tick += MainLoop;
            timer.Start();
        }

        private void MainLoop(object sender, EventArgs e)
        {
            this.Refresh();
        }

        private void InitializeComponent()
        {
            this.Text = "Simulation";
            this.DoubleBuffered = true;
            // this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.PaintSimulation); // TODO combine
        }

        private void PaintSimulation(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            Graphics g = e.Graphics;// this.CreateGraphics();
            physics.DrawPhysics(g);
        }
    }
}