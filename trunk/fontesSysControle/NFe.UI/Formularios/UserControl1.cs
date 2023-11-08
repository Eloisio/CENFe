using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace NFe.UI
{
    [ToolboxItem(false)]
    public partial class UserControl1 : MetroFramework.Controls.MetroUserControl
    {
        public UserControl1()
        {
            InitializeComponent();
        }

        private bool clearAll = true;
        public virtual void UpdateControles()
        {
            Dock = DockStyle.Fill;
            try
            {
                CEnfeDummy.ClearControls(this, clearAll, false);
                clearAll = false;
                try
                {
                    StyleManager = ((MetroFramework.Interfaces.IMetroForm)CEnfeDummy.mainForm).StyleManager.Clone(this) as MetroFramework.Components.MetroStyleManager;
                }
                catch
                {
                    StyleManager = CEnfeDummy.mainForm.StyleManager;
                }

                pictureBox1.Image = NFe.UI.Properties.Resources.e112_Back_48;
            }
            catch { }
        }

        public PictureBox back_button { get { return pictureBox1; } }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (CEnfeDummy.mainForm.WindowState == FormWindowState.Minimized)
                return;

            if (pictureBox1.Tag == null)
            {
                BackFuncao();
            }
        }

        public void BackFuncao()
        {
            foreach (var uc in CEnfeDummy.mainForm.Controls)
            {
                if (uc.GetType().Equals(typeof(menu)))
                {
                    ((menu)uc).Visible = true;
                    continue;
                }
                if (uc is MetroFramework.Controls.MetroUserControl)
                    (uc as MetroFramework.Controls.MetroUserControl).Visible = false;
            }
        }
    }
}
