using System.Windows.Forms;

namespace uzLib.Lite.ExternalCode.Unity.Extensions
{
    public class PaintArgs
    {
        private bool m_setted;

        public PaintArgs()
        {
        }

        public PaintArgs(Form sender, PaintEventArgs @event)
        {
            Sender = sender;
            Event = @event;

            m_setted = true;
        }

        public Form Sender { get; private set; }
        public PaintEventArgs Event { get; private set; }

        public void SetArgs(Form sender, PaintEventArgs @event)
        {
            if (m_setted)
                return;

            Sender = sender;
            Event = @event;

            m_setted = true;
        }
    }
}