using NFe.Components;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace NFe.UI.Formularios.NFSe
{
    [ToolboxItem(false)]
    public partial class userGrid2 : MetroFramework.Controls.MetroUserControl
    {
        public userGrid2()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            CEnfeDummy.ClearControls(this, true, false);

            //this.colPadrao_D.Items.AddRange(WebServiceNFSe.PadroesNFSeList);
            colPadrao_D.Sorted = false;
            colPadrao_D.DataSource = WebServiceNFSe.PadroesNFSeListDataSource;
            colPadrao_D.ValueMember = "fromType";
            colPadrao_D.DisplayMember = "fromDescription";
        }

        private string currpadrao;

        private void metroGrid1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            currpadrao = (sender as DataGridView).Rows[e.RowIndex].Cells[colPadrao_D.Index].Value.ToString();
        }

        private void dgvDireto_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int codmun = Convert.ToInt32((sender as DataGridView).Rows[e.RowIndex].Cells[colIBGE_D.Index].Value);
                string cidade = (sender as DataGridView).Rows[e.RowIndex].Cells[colNome_D.Index].Value.ToString().Trim();
                string padrao = (sender as DataGridView).Rows[e.RowIndex].Cells[colPadrao_D.Index].Value.ToString();
                string uf = Functions.CodigoParaUF(Convert.ToInt32(codmun.ToString().Substring(0, 2)));

                if (!padrao.Equals(currpadrao))
                {
                    var geral = WebServiceNFSe.PadroesNFSeUnicoWSDLDataSource.FirstOrDefault(w => w.fromType == padrao);

                    if (geral != null)/*padrao == PadroesNFSe.NaoIdentificado.ToString() ||
                         padrao == PadroesNFSe.BETHA.ToString() ||
                         padrao == PadroesNFSe.GINFES.ToString() ||
                         padrao == PadroesNFSe.EQUIPLANO.ToString() ||
                         padrao == PadroesNFSe.ABASE.ToString())*/
                    {
                        WebServiceNFSe.SalvarXMLMunicipios(uf, cidade, codmun, padrao, true);
                        ///
                        /// remove o municipio da grade
                        metroGrid1.Rows.RemoveAt(e.RowIndex);
                    }
                    else
                    {
                        ///
                        /// novo [uf+padrao+municipio] está no arquivo webservice.xml?
                        /// 
                        if (System.IO.File.Exists(Propriedade.NomeArqXMLWebService_NFSe))
                        {
                            var axml = XElement.Load(Propriedade.NomeArqXMLWebService_NFSe);
                            var s = (from p in axml.Descendants(NFeStrConstants.Estado)
                                     where (string)p.Attribute(TpcnResources.UF.ToString()) == uf &&
                                             (string)p.Attribute(TpcnResources.ID.ToString()) == codmun.ToString() &&
                                             (string)p.Attribute(NFeStrConstants.Padrao) == WebServiceNFSe.GetPadraoFromString(padrao).ToString()
                                     select p).FirstOrDefault();
                            if (s == null)
                                throw new Exception(@"Padrão não pode ser alterado, já que ele não está configurado no arquivo 'NFSe\WebService.xml'.");

                            WebServiceNFSe.SalvarXMLMunicipios(uf, cidade, codmun, padrao, true);
                        }
                        else
                            throw new Exception("Padrão não pode ser alterado, entre em contato com a Unimake.");
                    }
                }
            }
            catch (Exception ex)
            {
                (sender as DataGridView).Rows[e.RowIndex].Cells[colPadrao_D.Index].Value = currpadrao;
                MetroFramework.MetroMessageBox.Show(CEnfeDummy.mainForm, ex.Message, "");
            }
        }

        private void dgvDireto_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }

        public void RefreshMunicipiosDefinidos()
        {
            var final = from p in Propriedade.Municipios orderby p.Nome select p;
            municipioBindingSource.DataSource = final.ToList();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (FormMunicipio f = new FormMunicipio())
            {
                if (f.ShowDialog() == DialogResult.OK)
                    RefreshMunicipiosDefinidos();
            }
        }
    }
}
