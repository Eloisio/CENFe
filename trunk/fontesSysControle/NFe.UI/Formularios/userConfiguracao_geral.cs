using NFe.Components;
using NFe.Settings;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace NFe.UI.Formularios
{
    [ToolboxItem(false)]
    public partial class userConfiguracao_geral : MetroFramework.Controls.MetroUserControl
    {
        public bool Modificado { get; private set; }

        public userConfiguracao_geral()
        {
            InitializeComponent();
        }

        public void PopulateConfGeral()
        {
            CEnfeDummy.ClearControls(this, true, false);

            cbProxy.Checked = ConfiguracaoApp.Proxy;
            tbServidor.Text = ConfiguracaoApp.ProxyServidor;
            tbUsuario.Text = ConfiguracaoApp.ProxyUsuario;
            tbSenha.Text = ConfiguracaoApp.ProxySenha;
            nudPorta.Text = ConfiguracaoApp.ProxyPorta.ToString();
            tbSenhaConfig.Text = ConfiguracaoApp.SenhaConfig;
            tbSenhaConfig2.Text = ConfiguracaoApp.SenhaConfig;
            cbChecaConexaoInternet.Checked = ConfiguracaoApp.ChecarConexaoInternet;
            chkGravarLogOperacao.Checked = ConfiguracaoApp.GravarLogOperacoesRealizadas;
            chkConfProxyAuto.Checked = ConfiguracaoApp.DetectarConfiguracaoProxyAuto;
            chkConfirmaSaida.Checked = ConfiguracaoApp.ConfirmaSaida;
            cbProxy_CheckedChanged(null, null);

            tbUsuario.Focus();
            metroButton1.Enabled = false;
            Modificado = false;
        }

        public void Validar()
        {
            //Verificar se as senhas são idênticas
            if (tbSenhaConfig.Text.Trim() != tbSenhaConfig2.Text.Trim())
            {
                tbSenhaConfig.Focus();
                throw new Exception("As senhas de acesso a tela de configurações devem ser idênticas.");
            }
            if (cbProxy.Checked &&
                ((Convert.ToInt32("0" + nudPorta.Text) == 0 && !ConfiguracaoApp.DetectarConfiguracaoProxyAuto) ||
                //Caso a propriedade referente a detecção de proxy automatico esteja selecionada
                (string.IsNullOrEmpty(tbServidor.Text) && !ConfiguracaoApp.DetectarConfiguracaoProxyAuto) ||
                string.IsNullOrEmpty(tbUsuario.Text) ||
                string.IsNullOrEmpty(tbSenha.Text)))
            {
                tbServidor.Focus();
                throw new Exception(NFeStrConstants.proxyError);
            }
            ConfiguracaoApp.DetectarConfiguracaoProxyAuto = chkConfProxyAuto.Checked;
            ConfiguracaoApp.Proxy = cbProxy.Checked;
            ConfiguracaoApp.ProxyServidor = tbServidor.Text;
            ConfiguracaoApp.ProxyUsuario = tbUsuario.Text;
            ConfiguracaoApp.ProxySenha = tbSenha.Text;
            ConfiguracaoApp.ProxyPorta = Convert.ToInt32("0" + nudPorta.Text);
            ConfiguracaoApp.SenhaConfig = tbSenhaConfig2.Text;
            ConfiguracaoApp.ChecarConexaoInternet = cbChecaConexaoInternet.Checked;
            ConfiguracaoApp.GravarLogOperacoesRealizadas = chkGravarLogOperacao.Checked;
            ConfiguracaoApp.ConfirmaSaida = chkConfirmaSaida.Checked;
        }

        public void FocusFirstControl()
        {
            Timer t = new Timer();
            t.Interval = 50;
            t.Tick += (sender, e) =>
            {
                ((Timer)sender).Stop();
                ((Timer)sender).Dispose();
                if (tbUsuario.Enabled)
                    tbUsuario.Focus();
                else
                    tbSenhaConfig.Focus();
            };
            t.Start();
        }

        private void nudPorta_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsNumber(e.KeyChar);
        }

        private void tbUsuario_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SelectNextControl(ActiveControl, true, true, true, false);
                e.Handled = true;
            }
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            tbUsuario.Focus();

            try
            {
                Validar();
                new ConfiguracaoApp().GravarConfigGeral();
                metroButton1.Enabled = false;
                Modificado = false;
            }
            catch (Exception ex)
            {
                MetroFramework.MetroMessageBox.Show(CEnfeDummy.mainForm, ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tbUsuario_TextChanged(object sender, EventArgs e)
        {
            Modificado = true;
            metroButton1.Enabled = true;
        }

        private void cbProxy_CheckedChanged(object sender, EventArgs e)
        {
            tbUsuario_TextChanged(sender, e);

            lblPorta.Enabled =
                lblSenha.Enabled =
                lblUsuario.Enabled =
                lblServidor.Enabled =
                tbUsuario.Enabled =
                tbSenha.Enabled =
                nudPorta.Enabled =
                tbServidor.Enabled = chkConfProxyAuto.Enabled = cbProxy.Checked;
        }

        private void chkConfProxyAuto_CheckedChanged(object sender, EventArgs e)
        {
            ConfiguracaoApp.DetectarConfiguracaoProxyAuto = chkConfProxyAuto.Checked;
            if (chkConfProxyAuto.Checked)
            {
                nudPorta.Clear();
                tbServidor.Clear();
                nudPorta.Enabled = false;
                tbServidor.Enabled = false;
            }
            else
            {
                nudPorta.Enabled = true;
                tbServidor.Enabled = true;
            }
        }
    }
}

