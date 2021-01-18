using NFe.Components;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace NFe.UI.Formularios
{
    [ToolboxItem(false)]
    public partial class userConfiguracao_ftp : MetroFramework.Controls.MetroUserControl
    {
        public userConfiguracao_ftp()
        {
            InitializeComponent();
        }

        public event EventHandler changeEvent;
        private NFe.Settings.Empresa empresa;
        private bool ErrorFtp = false;

        public void Populate(NFe.Settings.Empresa empresa)
        {
            this.empresa = empresa;
            CEnfeDummy.ClearControls(this, true, false);

            edtFTP_Ativo.Checked = empresa.FTPAtivo;
            edtFTP_Passivo.Checked = empresa.FTPPassivo;
            edtFTP_GravaXMLPastaUnica.Checked = empresa.FTPGravaXMLPastaUnica;
            edtFTP_Password.Text = empresa.FTPSenha;
            edtFTP_PastaDestino.Text = empresa.FTPPastaAutorizados;
            edtFTP_PastaRetornos.Text = empresa.FTPPastaRetornos;
            edtFTP_Porta.Text = empresa.FTPPorta.ToString();
            edtFTP_Server.Text = empresa.FTPNomeDoServidor;
            edtFTP_UserName.Text = empresa.FTPNomeDoUsuario;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        public void Validar()
        {
            if (ErrorFtp && edtFTP_Ativo.Checked)
                throw new Exception("Erros foram encontrados no teste de FTP que não foram sanados");

            empresa.FTPAtivo = edtFTP_Ativo.Checked;
            empresa.FTPPassivo = edtFTP_Passivo.Checked;
            empresa.FTPGravaXMLPastaUnica = edtFTP_GravaXMLPastaUnica.Checked;
            empresa.FTPSenha = edtFTP_Password.Text;
            empresa.FTPPastaAutorizados = edtFTP_PastaDestino.Text;
            empresa.FTPPastaRetornos = edtFTP_PastaRetornos.Text;
            empresa.FTPPorta = Convert.ToInt32(edtFTP_Porta.Text);
            empresa.FTPNomeDoServidor = edtFTP_Server.Text;
            empresa.FTPNomeDoUsuario = edtFTP_UserName.Text;
        }

        public void FocusFirstControl()
        {
            Timer t = new Timer();
            t.Interval = 50;
            t.Tick += (sender, e) =>
            {
                ((Timer)sender).Stop();
                ((Timer)sender).Dispose();

                edtFTP_Server.Focus();
            };
            t.Start();
        }

        private void btnTestarFTP_Click(object sender, EventArgs e)
        {
            ErrorFtp = false;
            FTP ftp = null;
            try
            {
                ftp = new FTP(edtFTP_Server.Text, Convert.ToInt32("0" + edtFTP_Porta.Text), edtFTP_UserName.Text, edtFTP_Password.Text);
                ftp.Connect();
                if (ftp.IsConnected)
                {
                    string vCurrente = ftp.GetWorkingDirectory();

                    const string error = "Pasta {0} '{1}' não existe no FTP.\r\nDeseja criá-la agora?";

                    if (!ftp.changeDir(edtFTP_PastaDestino.Text))
                    {
                        if (MetroFramework.MetroMessageBox.Show(CEnfeDummy.mainForm, string.Format(error, "destino", edtFTP_PastaDestino.Text), "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            ftp.makeDir(edtFTP_PastaDestino.Text);
                            if (!ftp.changeDir(edtFTP_PastaDestino.Text))
                                throw new Exception("Pasta de destino criada mas não foi possivel acessá-la");
                        }
                        else
                            ErrorFtp = true;
                    }

                    ftp.ChangeDir(vCurrente);

                    if (!string.IsNullOrEmpty(edtFTP_PastaRetornos.Text))
                        if (!ftp.changeDir(edtFTP_PastaRetornos.Text))
                        {
                            if (MetroFramework.MetroMessageBox.Show(CEnfeDummy.mainForm, string.Format(error, "para retornos", edtFTP_PastaRetornos.Text), "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                ftp.makeDir(edtFTP_PastaRetornos.Text);
                                if (!ftp.changeDir(edtFTP_PastaRetornos.Text))
                                    throw new Exception("Pasta de retornos criada mas não foi possivel acessá-la");
                            }
                            else
                                ErrorFtp = true;
                        }

                    MetroFramework.MetroMessageBox.Show(CEnfeDummy.mainForm, "FTP conectado com sucesso," + (ErrorFtp ? "\r\nmas houve erro da definição da(s) pasta(s)" : ""), "");
                }
                else
                    throw new Exception("FTP não conectado");
            }
            catch (Exception ex)
            {
                ErrorFtp = true;
                MetroFramework.MetroMessageBox.Show(CEnfeDummy.mainForm, ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (ftp != null)
                    if (ftp.IsConnected)
                        ftp.Disconnect();
            }
        }

        private void edtFTP_Server_TextChanged(object sender, EventArgs e)
        {
            if (changeEvent != null)
                changeEvent(sender, e);
        }

        private void edtFTP_Ativo_CheckedChanged(object sender, EventArgs e)
        {
            if (changeEvent != null)
                changeEvent(sender, e);
        }
    }
}
