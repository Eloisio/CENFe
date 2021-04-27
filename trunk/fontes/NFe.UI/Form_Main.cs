using MetroFramework.Forms;
using NFe.Components;
using NFe.Settings;
using NFe.Threadings;
using System;
using System.Windows.Forms;

namespace NFe.UI
{
    public partial class Form_Main : MetroForm
    {
        private const string Empresa = "Campos Evolution Monitor - NFe";
        private bool first = false;
        private bool servicoInstaladoErodando = false;
        private string srvName = Propriedade.ServiceName;
        private menu _menu;
        private bool _maximized;
        private bool _formloaded = false;

        public Form_Main()
        {
            InitializeComponent();

            CEnfeDummy.mainForm = this;
            CEnfeDummy.UltimoAcessoConfiguracao = DateTime.MinValue;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            bool error = false;
            try
            {
                //servico está instalado e rodando?
                servicoInstaladoErodando = Propriedade.ServicoRodando;
                tbSeparator1.Visible =
                tbRestartServico.Visible =
                tbPararServico.Visible = servicoInstaladoErodando;
                //Trazer minimizado e no systray
                notifyIcon1.Text = Empresa;
                notifyIcon1.Visible = true;
                WindowState = FormWindowState.Minimized;
                Visible = false;
                ShowInTaskbar = false;
                ConfiguracaoApp.StartVersoes();
                _menu = new menu();
                Controls.Add(_menu);
                _menu.Dock = DockStyle.Fill;
                cmAbrir.Text = "Abrir "+Empresa;
                cmFechar.Text = "Fechar " + Empresa;
                string msg = "";
                Propriedade.VerificaArquivos(out error, out msg);
                if (error)
                {
                    MetroFramework.MetroMessageBox.Show(this, msg, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                    return;
                }
                if (!servicoInstaladoErodando)
                    new ThreadControlEvents();
                ExecutaServicos();
            }
            finally
            {
                if (!error)
                    updateControleDoServico();
            }
        }
        protected override void OnResize(EventArgs e)
        {
            if (first)
            {
                if (WindowState == FormWindowState.Minimized)
                {
                    ShowInTaskbar = false;
                }
            }
            first = true;
            base.OnResize(e);
        }
        protected override void OnClosed(EventArgs e)
        {
            PararServicos(false);
            Empresas.ClearLockFiles(false);
            foreach (var uc in Controls)
            {
                if (uc is MetroFramework.Controls.MetroUserControl)
                    (uc as MetroFramework.Controls.MetroUserControl).Dispose();
            }
            base.OnClosed(e);
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (_formloaded)
                SaveForm();
            if (e.CloseReason == CloseReason.UserClosing && !Propriedade.EncerrarApp)
            {
                e.Cancel = true;
                foreach (var uc in Controls)
                {
                    if (uc.GetType().Equals(typeof(userConfiguracoes)))
                    {
                        if (!((userConfiguracoes)uc).VerificaSeAbandona())
                            return;
                        break;
                    }
                }
                Form form = this;
                do
                {
                    form.Hide();
                } while ((form = form.Owner) != null);

                notifyIcon1.Visible = true;
            }
            else
            {
                e.Cancel = false;  
            }
            base.OnFormClosing(e);
        }
        private void Form_Main_ControlRemoved(object sender, ControlEventArgs e)
        {
            if (e.Control != null && !Propriedade.EncerrarApp)
            {
                if (e.Control.GetType().Equals(typeof(userConfiguracoes)))
                {
                    RestartServicos();
                }
                if (e.Control.GetType().Equals(typeof(Formularios.userMunicipios)))
                {
                    Refresh();
                    WebServiceProxy.reloadWebServicesList();
                }
            }
        }
        #region Métodos gerais
        private void SaveForm()
        {
            CEnfeDummy.xmlParams.SaveForm(this, "\\main");
            CEnfeDummy.xmlParams.Save();
        }
        public void updateControleDoServico()
        {
            if (servicoInstaladoErodando)
            {
                tbPararServico.Enabled = ServiceProcess.StatusService(srvName) == System.ServiceProcess.ServiceControllerStatus.Running;
                tbRestartServico.Enabled = ServiceProcess.StatusService(srvName) == System.ServiceProcess.ServiceControllerStatus.Stopped;
            }
        }
        #region ExecutaServicos()
        /// Metodo responsável por iniciar os serviços do CENfe em threads diferentes
        public void ExecutaServicos(bool updateOptions = true)
        {
            if (servicoInstaladoErodando)
            {
                Empresas.CarregaConfiguracao();
                switch (ServiceProcess.StatusService(srvName))
                {
                    case System.ServiceProcess.ServiceControllerStatus.Stopped:
                        ServiceProcess.StartService(srvName, 40000);
                        break;

                    case System.ServiceProcess.ServiceControllerStatus.Paused:
                        ServiceProcess.RestartService(srvName, 40000);
                        break;
                }
                if (updateOptions)
                    updateControleDoServico();
            }
            else
            {
                ThreadService.Start();
            }
        }
        #endregion ExecutaServicos()
        #region PararServicos()
        public void PararServicos(bool fechaServico)
        {
            if (servicoInstaladoErodando)
            {
                if (fechaServico)
                {
                    ServiceProcess.StopService(srvName, 40000);
                }
            }
            else
            {
                ThreadService.Stop();
            }
        }
        #endregion PararServicos()
        #endregion Métodos gerais
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (!_formloaded)
            {
                _maximized = false;
                try
                {
                    if (CEnfeDummy.xmlParams.ValueExists(Name, "WindowState"))
                    {
                        switch (CEnfeDummy.xmlParams.ReadValue(Name + "\\main", "WindowState", 0))
                        {
                            case 2:
                                _maximized = true;
                                break;
                        }
                    }
                }
                catch { }
            }
            BringToFront();
            Show();
            if (_maximized)
                WindowState = FormWindowState.Maximized;
            else
            {
                WindowState = FormWindowState.Normal;
                if (!_formloaded)
                    try
                    {
                        CEnfeDummy.xmlParams.LoadForm(this, "\\main", true);
                    }
                    catch { }
            }
            ShowInTaskbar = true;
            _formloaded = true;
            _maximized = false;
            Activate();
        }
        private void cmAbrir_Click(object sender, EventArgs e)
        {
            notifyIcon1_MouseDoubleClick(sender, null);
        }
        private void tbPararServico_Click(object sender, EventArgs e)
        {
            CEnfeDummy.opServicos = CEnfeOpcoes2.opStopServico;
            MetroTaskWindow.ShowTaskWindow(this, "", new NFe.UI.Formularios.UserControl2());
        }
        private void tbRestartServico_Click(object sender, EventArgs e)
        {
            CEnfeDummy.opServicos = CEnfeOpcoes2.opRestartServico;
            MetroTaskWindow.ShowTaskWindow(this, "", new NFe.UI.Formularios.UserControl2());
        }
        private bool MainVisible
        {
            get
            {
                return WindowState != FormWindowState.Minimized && Visible;
            }
        }
        private void cmConfiguracoes_Click(object sender, EventArgs e)
        {
            if (!MainVisible)
            {
                if (!string.IsNullOrEmpty(ConfiguracaoApp.SenhaConfig) && CEnfeDummy.TempoExpirou())
                {
                    if (!FormSenha.SolicitaSenha(false))
                        return;

                    CEnfeDummy.UltimoAcessoConfiguracao = DateTime.Now;
                }
                FormDummy.ShowModulo(CEnfeOpcoes.opConfiguracoes);
            }
            else
                _menu.Show(CEnfeOpcoes.opConfiguracoes);
        }
        private void cmFechar_Click(object sender, EventArgs e)
        {
            if (ConfiguracaoApp.ConfirmaSaida)
            {
                if (MetroFramework.MetroMessageBox.Show(this,
                            "Confirma o encerramento da Campos Evolution Monitor - NFe ?",
                            "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    Propriedade.EncerrarApp = true;
                    notifyIcon1.Visible = false;
                    Close();
                }
            }
            else
            {
                Propriedade.EncerrarApp = true;
                notifyIcon1.Visible = false;
                Close();
            }
        }
        public void _RestartServicos()
        {
            PararServicos(true);
            ExecutaServicos();
        }
        public void RestartServicos()
        {
            if (Empresas.Configuracoes.Count == 0)
            {
                MetroFramework.MetroMessageBox.Show(this,
                    "É necessário cadastrar e configurar a(s) empresa(s) que será(ão) gerenciada(s) pelo aplicativo.", "",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            CEnfeDummy.opServicos = CEnfeOpcoes2.opRestartTasks;
            MetroTaskWindow.ShowTaskWindow(this, "", new NFe.UI.Formularios.UserControl2());
        }
    }
}