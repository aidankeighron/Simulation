using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

namespace Simulation
{
    class Simulation : System.Windows.Forms.Form
    {
        private static ArrayList Actions = new ArrayList();
        public static float TimestampSeconds = 0;

        // Constants
        public const int Interval = 20;
        // INIT //
        public Simulation()
        {
            InitializeComponent();
            Physics physics = new Physics();
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = Interval; // ms
            timer.Tick += MainLoop;
            timer.Start();
        }

        private void MainLoop(object sender, EventArgs e)
        {
            this.Refresh();
            TimestampSeconds += Interval / 1000.0f;
        }

        private void InitializeComponent()
        {
            this.Text = "Simulation";
            this.DoubleBuffered = true;
            // this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.PaintSimulation);
        }

        static void Main(string[] args)
        {
            Application.Run(new Simulation());
        }

        // INIT //

        private void PaintSimulation(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            Graphics g = e.Graphics;// this.CreateGraphics();
            foreach (Action<Graphics> action in Actions)
            {
                action.Invoke(g);
            }
        }

        public static void RegesterDrawer(Action<Graphics> action)
        {
            Actions.Add(action);

        }
    }
}