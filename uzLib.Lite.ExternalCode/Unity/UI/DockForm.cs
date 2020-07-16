using System;
using System.Windows.Forms;

namespace UnityEngine.UI
{
    public class DockForm : Form
    {
        public DockForm()
        {
            FormBorderStyle = FormBorderStyle.FixedSingle;
            uwfMovable = false;

            Shown += _Shown;
            FormClosed += _Closed;
        }

        protected override void OnResize(EventArgs e)
        {
        }

        private void _Shown(object sender, EventArgs e)
        {
            DockBehaviour.IsShown = true;
        }

        private void _Closed(object sender, EventArgs e)
        {
            DockBehaviour.IsShown = false;
        }
    }
}