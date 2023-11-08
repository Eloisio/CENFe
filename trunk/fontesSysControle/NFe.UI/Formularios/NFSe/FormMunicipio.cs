using NFe.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;


namespace NFe.UI.Formularios.NFSe
{
    public partial class FormMunicipio : MetroFramework.Forms.MetroForm
    {
        public FormMunicipio()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            CEnfeDummy.ClearControls(this, true, true);

            foreach (var estado in Propriedade.Estados)
            {
                edtUF.Items.Add(estado.UF);
            }
            edtUF.SelectedIndex = 0;

            var _PadroesDataSource = new List<PadroesDataSource>();
            XElement axml = XElement.Load(Propriedade.NomeArqXMLWebService_NFSe);
            var xs = (from p in axml.Descendants(NFe.Components.NFeStrConstants.Estado)
                      where p.Attribute(NFe.Components.TpcnResources.UF.ToString()).Value == "XX" &&
                             p.Attribute(NFe.Components.TpcnResources.ID.ToString()).Value == p.Attribute(NFe.Components.NFeStrConstants.Padrao).Value
                      orderby p.Attribute(NFe.Components.NFeStrConstants.Padrao).Value
                      select p);
            foreach (var item in xs)
            {
                PadroesNFSe type = WebServiceNFSe.GetPadraoFromString(item.Attribute(NFe.Components.NFeStrConstants.Padrao).Value);
                _PadroesDataSource.Add(new PadroesDataSource { fromType = type.ToString(), fromDescription = EnumHelper.GetEnumItemDescription(type) });
            }

            edtPadrao.Sorted = false;
            edtPadrao.DataSource = _PadroesDataSource;// WebServiceNFSe.PadroesNFSeListDataSource.Where(p => p.fromType != PadroesNFSe.NaoIdentificado.ToString()).ToList();
            edtPadrao.ValueMember = "fromType";
            edtPadrao.DisplayMember = "fromDescription";

            edtPadrao.SelectedIndex = 0;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            edtCodMun.Focus();
        }

        private void edtCodMun_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void edtCodMun_Leave(object sender, EventArgs e)
        {
            edtCodMun.Text = edtCodMun.Text.PadLeft(7, '0');
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            edtCodMun.Focus();
            if (Functions.CodigoParaUF(Convert.ToInt32(edtCodMun.Text.Substring(0, 2))) != edtUF.SelectedItem.ToString())
            {
                MetroFramework.MetroMessageBox.Show(this, "Código do IBGE diverge da UF", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (edtMunicipio.Text == "")
            {
                MetroFramework.MetroMessageBox.Show(this, "Nome do município deve ser informado", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                edtMunicipio.Focus();
                return;
            }

            foreach (Municipio mun in Propriedade.Municipios)
                if (mun.CodigoMunicipio.ToString() == edtCodMun.Text)
                {
                    MetroFramework.MetroMessageBox.Show(this, "Código IBGE já definido no municipio \"" + mun.Nome + "\"", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

            try
            {
                WebServiceNFSe.SalvarXMLMunicipios(edtUF.SelectedItem.ToString(),
                    edtMunicipio.Text,
                    Convert.ToInt32(edtCodMun.Text),
                    (edtPadrao.SelectedItem as PadroesDataSource).fromType,
                    false);

                DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            catch (Exception ex)
            {
                MetroFramework.MetroMessageBox.Show(null, ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void edtMunicipio_TextChanged(object sender, EventArgs e)
        {
            metroButton1.Enabled = !edtCodMun.Text.PadLeft(7, '0').Equals("0000000") &&
                edtMunicipio.Text.Trim().Length > 0;
        }
    }
}
