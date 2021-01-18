using NFe.Components;
using NFe.Service;
using NFe.Settings;
using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace NFe.UI
{
    [ToolboxItem(false)]
    public partial class userCadastro : UserControl1
    {
        public userCadastro()
        {
            InitializeComponent();
        }

        private userCadastroResult rst;
        private int Emp;

        public override void UpdateControles()
        {
            base.UpdateControles();

            cbEmpresa.SelectedIndexChanged -= cbEmpresa_SelectedIndexChanged;
            try
            {
                cbEmpresa.DisplayMember = NFe.Components.NFeStrConstants.Nome;
                cbEmpresa.ValueMember = "Key";
                cbEmpresa.DataSource = Auxiliar.CarregaEmpresa(true);

                comboUf.DisplayMember = NFe.Components.NFeStrConstants.Nome.ToLower();
                comboUf.ValueMember = "valor";
                comboUf.DataSource = Functions.CarregaEstados();

                int posicao = CEnfeDummy.xmlParams.ReadValue(GetType().Name, "last_empresa", 0);
                if (posicao > (cbEmpresa.DataSource as System.Collections.ArrayList).Count)
                    posicao = 0;

                cbEmpresa.SelectedIndex = posicao;
                cbVersao.SelectedIndex = 0;
                cbVersao.Enabled = false;

                cbEmpresa.SelectedIndexChanged += cbEmpresa_SelectedIndexChanged;

                cbEmpresa_SelectedIndexChanged(null, null);
            }
            finally
            {
                FocusF();
            }
        }

        private void FocusF()
        {
            System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();
            t.Interval = 50;
            t.Tick += (sender, e) =>
            {
                ((System.Windows.Forms.Timer)sender).Stop();
                ((System.Windows.Forms.Timer)sender).Dispose();

                textConteudo.Focus();
            };
            t.Start();
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            FocusF();

            textResultado.Text = "Consultando o servidor. Aguarde....";
            Refresh();

            RetConsCad vConsCad = null;
            try
            {
                if (string.IsNullOrEmpty(textConteudo.Text))
                    throw new Exception("Conteúdo para pesquisa deve ser informado");

                object vvConsCad = null;

                if (rbCNPJ.Checked)
                    vvConsCad = ConsultaCadastro((string)comboUf.SelectedValue, textConteudo.Text, string.Empty, string.Empty);
                else
                    if (rbCPF.Checked)
                    vvConsCad = ConsultaCadastro((string)comboUf.SelectedValue, string.Empty, string.Empty, textConteudo.Text);
                else
                    vvConsCad = ConsultaCadastro((string)comboUf.SelectedValue, string.Empty, textConteudo.Text, string.Empty);

                if (vvConsCad is RetConsCad)
                {
                    vConsCad = (vvConsCad as RetConsCad);
                    if (vConsCad == null)
                        textResultado.Text = "Não pode obter a resposta do Sefaz";
                }
                else
                {
                    throw new Exception((string)vvConsCad);
                }
            }
            catch (Exception ex)
            {
                textResultado.Text = ex.Message;
                vConsCad = null;
            }
            finally
            {
                if (vConsCad != null)
                {
                    textResultado.Text = "";

                    if (vConsCad.infCad.Count > 0)
                    {
                        Visible = false;

                        Form parent = (Form)Parent;

                        var icc = rst == null ? -1 : parent.Controls.IndexOf(rst);
                        if (icc >= 0)
                        {
                            (parent.Controls[icc] as UserControl1).UpdateControles();
                            parent.Controls[icc].Visible = true;
                        }
                        else
                        {
                            rst = new userCadastroResult();
                            rst.vConsCad = vConsCad;
                            rst.UpdateControles();

                            rst.back_button.Click += delegate
                            {
                                rst.Visible = false;
                                icc = parent.Controls.IndexOf(rst);
                                parent.Controls.RemoveAt(icc);
                                rst.Dispose();

                                Visible = true;
                                FocusF();
                            };
                            parent.Controls.Add(rst);
                            rst.Visible = true;
                        }
                    }
                    else
                        textResultado.Text = vConsCad.xMotivo;
                }
            }
        }

        private void cbEmpresa_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                comboUf.Enabled = cbEmpresa.SelectedValue != null;

                buttonPesquisa.Enabled = false;

                if (cbEmpresa.SelectedValue != null)
                {
                    var list = (cbEmpresa.DataSource as System.Collections.ArrayList)[cbEmpresa.SelectedIndex] as NFe.Components.ComboElem;
                    Emp = Empresas.FindConfEmpresaIndex(list.Valor, NFe.Components.EnumHelper.StringToEnum<TipoAplicativo>(list.Servico));
                    if (Emp >= 0)
                    {
                        if (Empresas.Configuracoes[Emp].Servico == TipoAplicativo.Nfse)
                        {
                            throw new Exception("NFS-e não dispõe do serviço de consulta a cadastro de contribuinte.");
                        }

                        CEnfeDummy.xmlParams.WriteValue(GetType().Name, "last_empresa", cbEmpresa.SelectedIndex);
                        CEnfeDummy.xmlParams.Save();

                        comboUf.SelectedValue = Functions.CodigoParaUF(Empresas.Configuracoes[Emp].UnidadeFederativaCodigo).Trim();

                        buttonPesquisa.Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MetroFramework.MetroMessageBox.Show(CEnfeDummy.mainForm, ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// VerConsultaCadastroClass
        /// </summary>
        /// <param name="XmlNfeDadosMsg"></param>
        /// <returns></returns>
        public object VerConsultaCadastro(string XmlNfeDadosMsg)
        {
            string ArqXMLRetorno = Empresas.Configuracoes[Emp].PastaXmlRetorno + "\\" +
                       Functions.ExtrairNomeArq(XmlNfeDadosMsg, Propriedade.Extensao(Propriedade.TipoEnvio.ConsCad).EnvioXML) +
                       Propriedade.Extensao(Propriedade.TipoEnvio.ConsCad).RetornoXML;

            string ArqERRRetorno = Empresas.Configuracoes[Emp].PastaXmlRetorno + "\\" +
                      Functions.ExtrairNomeArq(XmlNfeDadosMsg, Propriedade.Extensao(Propriedade.TipoEnvio.ConsCad).EnvioXML) +
                      Propriedade.ExtRetorno.ConsCad_ERR;

            object vRetorno = null;
            try
            {
                vRetorno = EnviaArquivoERecebeResposta(ArqXMLRetorno, ArqERRRetorno);
            }
            finally
            {
                Functions.DeletarArquivo(ArqERRRetorno);
                Functions.DeletarArquivo(ArqXMLRetorno);
            }
            return vRetorno;
        }

        #region ConsultaCadastro()
        /// <summary>
        /// Verifica um cadastro no site da receita.
        /// Voce deve preencher o estado e mais um dos tres itens: CPNJ, IE ou CPF.
        /// </summary>
        /// <param name="uf">Sigla do UF do cadastro a ser consultado. Tem que ter dois dígitos. SU para suframa.</param>
        /// <param name="cnpj"></param>
        /// <param name="ie"></param>
        /// <param name="cpf"></param>
        /// <returns>String com o resultado da consuta do cadastro</returns>
        /// <by>Marcos Diez</by>
        /// <date>29/08/2009</date>
        public object ConsultaCadastro(string uf, string cnpj, string ie, string cpf)
        {
            GerarXML oGerar = new GerarXML(Emp);

            //Criar XML para obter o cadastro do contribuinte
            string XmlNfeConsultaCadastro = oGerar.ConsultaCadastro(string.Empty, uf, cnpj, ie, cpf, cbVersao.SelectedItem.ToString());

            return VerConsultaCadastro(XmlNfeConsultaCadastro);
        }
        #endregion

        /// <summary>
        /// Envia um arquivo para o webservice da NFE e recebe a resposta. 
        /// </summary>
        /// <returns>Retorna uma string com a mensagem obtida do webservice de status do serviço da NFe</returns>
        /// <example>string vPastaArq = this.CriaArqXMLStatusServico();</example>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>17/06/2009</date>
        private object EnviaArquivoERecebeResposta(string arqXMLRetorno, string arqERRRetorno)
        {
            DateTime startTime;
            DateTime stopTime;
            TimeSpan elapsedTime;

            long elapsedMillieconds;
            startTime = DateTime.Now;

            object vStatus = CEnfeDummy.vStatus;

            while (true)
            {
                stopTime = DateTime.Now;
                elapsedTime = stopTime.Subtract(startTime);
                elapsedMillieconds = (int)elapsedTime.TotalMilliseconds;

                if (elapsedMillieconds >= 60000)
                {
                    break;
                }

                if (File.Exists(arqXMLRetorno))
                {
                    if (!Functions.FileInUse(arqXMLRetorno))
                    {
                        try
                        {
                            //Ler o status do serviço no XML retornado pelo WebService
                            try
                            {
                                vStatus = new GerarXML(Emp).ProcessaConsultaCadastro(arqXMLRetorno);
                            }
                            catch (Exception ex)
                            {
                                vStatus = ex.Message;
                                break;
                                //Se não conseguir ler o arquivo vai somente retornar ao loop para tentar novamente, pois 
                                //pode ser que o arquivo esteja em uso ainda.
                            }

                            //Detetar o arquivo de retorno
                            try
                            {
                                FileInfo oArquivoDel = new FileInfo(arqXMLRetorno);
                                oArquivoDel.Delete();
                                break;
                            }
                            catch
                            {
                                //Somente deixa fazer o loop novamente e tentar deletar
                            }
                        }
                        catch
                        {
                        }
                    }
                }
                else if (File.Exists(arqERRRetorno))
                {
                    //Retornou um arquivo com a extensão .ERR, ou seja, deu um erro,
                    //futuramente tem que retornar esta mensagem para a MessageBox do usuário.

                    //Detetar o arquivo de retorno
                    try
                    {
                        vStatus += System.IO.File.ReadAllText(arqERRRetorno, Encoding.Default);
                        System.IO.File.Delete(arqERRRetorno);
                        break;
                    }
                    catch
                    {
                        //Somente deixa fazer o loop novamente e tentar deletar
                    }
                }

                Thread.Sleep(100);
            }
            //Retornar o status do serviço
            return vStatus;
        }

        private void rbCNPJ_CheckedChanged(object sender, EventArgs e)
        {
            textConteudo.MaxLength = 14;
        }

        private void rbCPF_CheckedChanged(object sender, EventArgs e)
        {
            textConteudo.MaxLength = 11;
        }

        private void rbIE_CheckedChanged(object sender, EventArgs e)
        {
            textConteudo.MaxLength = 20;
        }
    }
}
