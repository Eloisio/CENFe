using NFe.Components;
using NFe.Settings;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace NFe.UI.Formularios
{
    public partial class FormNova : MetroFramework.Forms.MetroForm
    {
        Form _owner;

        public FormNova(Form parente)
        {
            InitializeComponent();
            _owner = parente;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (_owner != null)
            {
                Size = new Size(_owner.Size.Width, Height);
                Location = new Point(_owner.Location.X, _owner.Location.Y + (_owner.Height - Height) / 2);
            }
            CEnfeDummy.ClearControls(this, true, true);

            Text = NFe.Components.Propriedade.NomeAplicacao + " - Nova";

            cbServico.DataSource = CEnfeDummy.DatasouceTipoAplicativo(false);
            cbServico.DisplayMember = "Value";
            cbServico.ValueMember = "Key";
            cbServico.Enabled = true;
            cbServico.SelectedIndex = 0;
            edtCNPJ.Text = edtNome.Text = "";
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

                edtCNPJ.Focus();
            };
            t.Start();
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            FormatarCPFCNPJ();

            if (edtCNPJ.Text.Length == 14)
            {
                if (!CPF.Validate(edtCNPJ.Text, false))
                {
                    MetroFramework.MetroMessageBox.Show(this, "CPF inválido", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    edtCNPJ.Focus();
                    return;
                }
            }
            else
            {
                if (!CNPJ.Validate(edtCNPJ.Text))
                {
                    MetroFramework.MetroMessageBox.Show(this, "CNPJ inválido", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    edtCNPJ.Focus();
                    return;
                }
            }

            string cnpj = Functions.OnlyNumbers(edtCNPJ.Text, ".-/").ToString();

            if (edtNome.Text.Trim().Length == 0)
            {
                edtNome.Focus();
                MetroFramework.MetroMessageBox.Show(this, "Nome deve ser informado", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            TipoAplicativo servico = (TipoAplicativo)cbServico.SelectedValue;

            Empresa empresa = null;
            switch (servico)
            {
                case TipoAplicativo.Todos:
                case TipoAplicativo.Nfe:

                    //Serviço todos e NFe utilizam a mesma pasta de configurações, então não posso permitir configurar o mesmo CNPJ para os dois serviços.
                    if ((empresa = Empresas.FindConfEmpresa(cnpj, TipoAplicativo.Todos)) == null)
                        empresa = Empresas.FindConfEmpresa(cnpj, TipoAplicativo.Nfe);
                    break;

                default:
                    empresa = Empresas.FindConfEmpresa(cnpj, servico);
                    break;
            }

            if (empresa != null)
            {
                string msgErro = "Já existe uma Empresa/CNPJ configurada para atender este serviço, conforme dados abaixo: " +
                                 "\r\n\r\nEmpresa configurada: " + empresa.Nome +
                                 "\r\nServiço configurado: " + NFe.Components.EnumHelper.GetDescription(empresa.Servico);

                MetroFramework.MetroMessageBox.Show(this, msgErro, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult = DialogResult.OK;
        }

        private void edtCNPJ_TextChanged(object sender, EventArgs e)
        {
            metroButton1.Enabled = edtCNPJ.Text.Length > 0 && edtNome.Text.Trim().Length > 0;
        }

        private void edtCNPJ_Enter(object sender, EventArgs e)
        {
            edtCNPJ.Text = (string)NFe.Components.Functions.OnlyNumbers(edtCNPJ.Text, ".-/");
        }

        private void edtCNPJ_Leave(object sender, EventArgs e)
        {
            FormatarCPFCNPJ();
        }

        private void FormatarCPFCNPJ()
        {
            string cnpj = Functions.OnlyNumbers(edtCNPJ.Text, ".,-/").ToString();

            if (string.IsNullOrEmpty(cnpj))
                return;

            if (Convert.ToInt64(cnpj).Equals(0))
            {
                edtCNPJ.Clear();
                edtCNPJ.Focus();
                return;
            }

            if (cnpj.Length < 13)
                edtCNPJ.Text = CEnfeDummy.FmtCnpjCpf(cnpj, false);
            else
                edtCNPJ.Text = CEnfeDummy.FmtCnpjCpf(cnpj, true);
        }
    }
}