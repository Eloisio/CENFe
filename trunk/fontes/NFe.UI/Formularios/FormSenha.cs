using System;
using System.Drawing;
using System.Windows.Forms;

namespace NFe.UI
{
    public partial class FormSenha : MetroFramework.Forms.MetroForm
    {
        bool frommain;
        public FormSenha(bool fromMain)
        {
            InitializeComponent();
            frommain = fromMain;
        }

        private void FormSenha_Load(object sender, EventArgs e)
        {
            CEnfeDummy.ClearControls(this, true, frommain);
            Text = "Campos Evolution";
            tbSenha.Clear();
            if (frommain)
            {
                Size = new Size(CEnfeDummy.mainForm.Size.Width, Height);
                Location = new Point(CEnfeDummy.mainForm.Location.X, CEnfeDummy.mainForm.Location.Y + (CEnfeDummy.mainForm.Height - Height) / 2);
            }
            else
            {
                StartPosition = FormStartPosition.CenterScreen;
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            Timer t = new Timer();
            t.Interval = 50;
            t.Tick += (sender, _e) =>
            {
                ((Timer)sender).Stop();
                ((Timer)sender).Dispose();

                tbSenha.Focus();
            };
            t.Start();
        }
        private void metroButton1_Click(object sender, EventArgs e)
        {
            tbSenha.Focus();

            string senhaCrip = NFe.Components.Functions.GerarMD5(tbSenha.Text.Trim());

            if (string.IsNullOrEmpty(tbSenha.Text) || senhaCrip != NFe.Settings.ConfiguracaoApp.SenhaConfig)
            {
                MetroFramework.MetroMessageBox.Show(this, "Senha inválida", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }

        private void tbSenha_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                metroButton1_Click(sender, null);
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        public static bool SolicitaSenha(bool fromMain)
        {
            using (FormSenha f = new FormSenha(fromMain))
            {
                return f.ShowDialog() == DialogResult.OK;
            }
        }
    }
}
