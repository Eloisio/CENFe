using System.ComponentModel;

namespace NFe.UI
{
    [ToolboxItem(false)]
    public partial class userCadastroResultDados : MetroFramework.Controls.MetroUserControl
    {
        public userCadastroResultDados()
        {
            InitializeComponent();
        }

        public void Populate(NFe.Service.infCad infCad)
        {
            CEnfeDummy.ClearControls(this, true, false);
            if (!string.IsNullOrEmpty(infCad.CNPJ))
            {
                label1.Text = NFe.Components.TpcnResources.CNPJ.ToString();
                txtCNPJ_CPF.Text = CEnfeDummy.FmtCnpjCpf(infCad.CNPJ, true);
            }
            else
            {
                label1.Text = "CPF";
                txtCNPJ_CPF.Text = CEnfeDummy.FmtCnpjCpf(infCad.CPF, false);
            }
            txtxIE.Text = infCad.IE;
            txtxNome.Text = infCad.xNome;
            txtxFant.Text = infCad.xFant;
            txtxEnder.Text = infCad.ender.xLgr;
            txtxBairro.Text = infCad.ender.xBairro;
            txtnro.Text = infCad.ender.nro;
            txtxCpl.Text = infCad.ender.xCpl;
            txtUF.Text = infCad.UF;
            txtxMun.Text = infCad.ender.xMun;
            txtCEP.Text = infCad.ender.CEP.ToString("00000000");
            txtdBaixa.Text = infCad.dBaixa.ToShortDateString();
            txtdUltSit.Text = infCad.dUltSit.ToShortDateString();
            txtdIniAtiv.Text = infCad.dIniAtiv.ToShortDateString();
            txtxRegApur.Text = infCad.xRegApur;
            txtCNAE.Text = infCad.CNAE.ToString();
            txtIEAtual.Text = infCad.IEAtual;
            txtIEUnica.Text = infCad.IEUnica;
            txtcSit.Text = infCad.cSit;
        }

    }
}
