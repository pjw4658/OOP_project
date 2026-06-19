using System;
using System.Windows.Forms;

namespace project_cs.Source.UI.Components
{
    public partial class ItemButtonControl : UserControl
    {
        public event EventHandler OnShuffleClicked;
        public event EventHandler OnHintClicked;
        public event EventHandler OnJokerClicked;

        public ItemButtonControl()
        {
            InitializeComponent();
        }

        private void btnShuffle_Click(object sender, EventArgs e)
        {
            OnShuffleClicked?.Invoke(this, EventArgs.Empty);
        }

        private void btnHint_Click(object sender, EventArgs e)
        {
            OnHintClicked?.Invoke(this, EventArgs.Empty);
        }

        private void btnJoker_Click(object sender, EventArgs e)
        {
            OnJokerClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}