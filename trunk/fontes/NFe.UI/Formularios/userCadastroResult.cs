using System;
using System.Windows.Forms;

namespace NFe.UI
{
    public partial class userCadastroResult : UserControl1
    {
        public userCadastroResult()
        {
            InitializeComponent();
        }

        public NFe.Service.RetConsCad vConsCad { private get; set; }

        public override void UpdateControles()
        {
            base.UpdateControles();

            back_button.Tag = 1;   //para nao deixar a UserControl1 executar o 'back'

            try
            {
                metroTabControl.TabPages.Clear();
                for (int i = 0; i < vConsCad.infCad.Count; ++i)
                {
                    MetroFramework.Controls.MetroTabPage tb = new MetroFramework.Controls.MetroTabPage();
                    tb.Padding = new System.Windows.Forms.Padding(2);
                    tb.Text = "Consulta-" + (i + 1).ToString();
                    //tb.Theme = uninfeDummy.mainForm.uTheme;
                    //tb.Style = uninfeDummy.mainForm.uStyle;

                    userCadastroResultDados dd = new userCadastroResultDados();
                    tb.Controls.Add(dd);
                    //dd.Location = new Point(1, 1);
                    //dd.Dock = DockStyle.Fill;
                    dd.Populate(vConsCad.infCad[i]);

                    metroTabControl.TabPages.Add(tb);
                }
            }
            catch (Exception ex)
            {
                MetroFramework.MetroMessageBox.Show(CEnfeDummy.mainForm, ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
