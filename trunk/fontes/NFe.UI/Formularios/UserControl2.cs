using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace NFe.UI.Formularios
{
    [ToolboxItem(false)]
    public partial class UserControl2 : MetroFramework.Controls.MetroUserControl
    {
        public UserControl2()
        {
            InitializeComponent();
        }

        private void UserControl2_Load(object sender, EventArgs e)
        {
            CEnfeDummy.ClearControls(this, true, false);

            switch (CEnfeDummy.opServicos)
            {
                case CEnfeOpcoes2.opStopServico:
                    metroLabel1.Text = "Parando o serviço da Campos Evolution NFe";
                    break;

                case CEnfeOpcoes2.opRestartServico:
                    metroLabel1.Text = "Reiniciando o serviço da Campos Evolution NFe";
                    break;

                case CEnfeOpcoes2.opRestartTasks:
                    metroLabel1.Text = "Parando/Executando os serviços da Campos Evolution NFe";
                    break;
            }

            Invalidate();

            BackgroundWorker bm = new BackgroundWorker();
            bm.DoWork += (_sender, _e) =>
                {
                    switch (CEnfeDummy.opServicos)
                    {
                        case CEnfeOpcoes2.opStopServico:
                            CEnfeDummy.mainForm.PararServicos(true);
                            break;

                        case CEnfeOpcoes2.opRestartServico:
                            CEnfeDummy.mainForm.ExecutaServicos(false);
                            break;

                        case CEnfeOpcoes2.opRestartTasks:
                            CEnfeDummy.mainForm._RestartServicos();
                            break;

                        default:
                            throw new Exception("Opcao invalida");
                    }
                    System.Threading.Thread.Sleep(2000);
                    _e.Result = true;
                };
            bm.RunWorkerCompleted += (_sender, _e) =>
                {
                    ((BackgroundWorker)_sender).Dispose();

                    MetroFramework.Forms.MetroTaskWindow.ForceClose();

                    if (_e.Error != null)
                    {
                        MetroFramework.MetroMessageBox.Show(CEnfeDummy.mainForm, _e.Error.Message, "");
                    }
                    else
                    {
                        switch (CEnfeDummy.opServicos)
                        {
                            case CEnfeOpcoes2.opStopServico:
                                CEnfeDummy.mainForm.updateControleDoServico();
                                MetroFramework.MetroMessageBox.Show(CEnfeDummy.mainForm,
                                    "Serviço da Campos Evolution NFe parado com sucesso!", "",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                                break;

                            case CEnfeOpcoes2.opRestartServico:
                                CEnfeDummy.mainForm.updateControleDoServico();
                                MetroFramework.MetroMessageBox.Show(CEnfeDummy.mainForm,
                                    "Serviço da Campos Evolution NFe reiniciado com sucesso!", "",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                                break;
                        }
                    }
                };

            bm.RunWorkerAsync();
        }
    }
}
