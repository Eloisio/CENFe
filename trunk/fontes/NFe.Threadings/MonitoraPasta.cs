using NFe.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Xml.Linq;

namespace NFe.Threadings
{
    public class MonitoraPasta
    {
        #region Construtores


        /// Construtor

        public MonitoraPasta()
        {
            MonitorarPasta();

        }

        #endregion Construtores

        #region Propriedades


        /// Tipos de arquivos a serem monitorados pelo UniNFe

        public static List<FileSystemWatcher> fsw = new List<FileSystemWatcher>();

        #endregion Propriedades

        #region MonitorarPastas()


        /// Executa as thread´s de monitoração de pastas

        private void MonitorarPasta()
        {
            fsw.Clear();

            for (int i = 0; i < Empresas.Configuracoes.Count; i++)
            {
                List<string> pastas = new List<string>();

                pastas.Add(Empresas.Configuracoes[i].PastaXmlEnvio);

                if (!string.IsNullOrEmpty(Empresas.Configuracoes[i].PastaXmlEmLote) &&
                    Empresas.Configuracoes[i].Servico != Components.TipoAplicativo.Nfse)
                {
                    pastas.Add(Empresas.Configuracoes[i].PastaXmlEmLote);
                }

                pastas.Add(Empresas.Configuracoes[i].PastaValidar);

                #region Pasta impressão dfe em contingência

                if (Directory.Exists(Empresas.Configuracoes[i].PastaContingencia) &&
                    Empresas.Configuracoes[i].tpEmis != (int)Components.TipoEmissao.teNormal)
                {
                    pastas.Add(Empresas.Configuracoes[i].PastaContingencia);
                }

                #endregion Pasta impressão dfe em contingência

                fsw.Add(new FileSystemWatcher(pastas, "*.xml,*.txt"));
                fsw[fsw.Count - 1].OnFileChanged += new FileSystemWatcher.FileChangedHandler(fsw_OnFileChanged);
                fsw[fsw.Count - 1].StartWatch();
            }

            #region Pasta Geral

            fsw.Add(new FileSystemWatcher(Path.Combine(System.Windows.Forms.Application.StartupPath, "Geral"), "*.xml,*.txt"));
            fsw[fsw.Count - 1].OnFileChanged += new FileSystemWatcher.FileChangedHandler(fsw_OnFileChanged);
            fsw[fsw.Count - 1].StartWatch();

            #endregion Pasta Geral
        }
        #endregion MonitorarPastas()
        #region LocalizaEmpresa()
        /// Localiza a empresa da qual o arquivo faz parte para processar com as configurações corretas
        private int LocalizaEmpresa(FileInfo fi)
        {
            int empresa = -1;
            try
            {
                string fullName = ConfiguracaoApp.RemoveEndSlash(fi.Directory.FullName.ToLower());
                if (fullName.EndsWith("\\temp"))
                    fullName = Path.GetDirectoryName(fullName);
                for (int i = 0; i < Empresas.Configuracoes.Count; i++)
                {
                    if (fullName == Empresas.Configuracoes[i].PastaXmlEnvio.ToLower() ||
                        fullName == Empresas.Configuracoes[i].PastaXmlEmLote.ToLower() ||
                        fullName == Empresas.Configuracoes[i].PastaValidar.ToLower() ||
                        fullName == Empresas.Configuracoes[i].PastaContingencia.ToLower())
                    {
                        empresa = i;
                        break;
                    }
                }
            }
            catch
            {
            }
            return empresa;
        }

        #endregion LocalizaEmpresa()
        #region Eventos
        /// Evento que executa thread´s para processar os arquivos que são colocados nas pastas que estão sendo monitoradas pela FileSystemWatcher
        /// Arquivo detectado pela FileSystemWatcher
        private void fsw_OnFileChanged(FileInfo fi)
        {
            try
            {
                int empresa;
                string arq = fi.FullName.ToLower();
                if (fi.Directory.FullName.ToLower().EndsWith("geral\\temp"))
                {
                    var sext = Components.Propriedade.Extensao(Components.Propriedade.TipoEnvio.sair_XML);
                    if (arq.EndsWith(sext.EnvioTXT) || arq.EndsWith(sext.EnvioXML))
                    {
                        File.Delete(fi.FullName);
                        Empresas.ClearLockFiles(false);
                        if (!Components.Propriedade.ExecutandoPeloUniNFe)
                        {
                            if (Components.ServiceProcess.StatusService(Components.Propriedade.ServiceName) == System.ServiceProcess.ServiceControllerStatus.Running)
                                Components.ServiceProcess.StopService(Components.Propriedade.ServiceName, 40000);
                        }
                        else
                            ThreadService.Stop();
                        Environment.Exit(0);
                        return;
                    }
                    string ExtRetorno = null;
                    string finalArqErro = null;
                    Exception exx = null;
                    /// Atualiza WSDL / Schemas
                    var ext = Components.Propriedade.Extensao(Components.Propriedade.TipoEnvio.pedUpdatewsdl);
                    if (arq.EndsWith(ext.EnvioTXT) || arq.EndsWith(ext.EnvioXML))
                    {
                        #region ---Atualiza WSDL e Schemas

                        File.Delete(fi.FullName);

                        Components.Functions.DeletarArquivo(Components.Propriedade.XMLVersaoWSDLXSD);

                        string cerros = "";
                        try
                        {
                            ConfiguracaoApp.ForceUpdateWSDL(false, ref cerros);

                            if (!string.IsNullOrEmpty(cerros)) throw new Exception(cerros);

                            string ExtRet = (arq.EndsWith(".xml") ? ext.RetornoXML : ext.RetornoTXT);
                            string arqRetorno = Components.Propriedade.PastaGeralRetorno + "\\" + Components.Functions.ExtrairNomeArq(fi.FullName, null) + ExtRet;
                            const string rst = "Schemas atualizados com sucesso!!!";

                            if (arq.EndsWith(".xml"))
                            {
                                var xml = new XDocument(new XDeclaration("1.0", "utf-8", null),
                                    new XElement("UPDT",
                                    new XElement("Result", rst)));
                                xml.Save(arqRetorno);
                            }
                            else
                                File.WriteAllText(arqRetorno, rst);
                            return;
                        }
                        catch (Exception ex)
                        {
                            ExtRetorno = (arq.EndsWith(".xml") ? ext.EnvioXML : ext.EnvioTXT);
                            finalArqErro = ext.EnvioXML.Replace(".xml", ".err");
                            exx = ex;
                        }

                        #endregion ---Atualiza WSDL e Schemas
                    }
                    /// restart o CENFe
                    var uext = Components.Propriedade.Extensao(Components.Propriedade.TipoEnvio.pedRestart);
                    if (arq.EndsWith(uext.EnvioTXT) || arq.EndsWith(uext.EnvioXML))
                    {
                        #region ---Reinicia 
                        File.Delete(fi.FullName);
                        try
                        {
                            if (Components.Propriedade.ExecutandoPeloUniNFe)
                            {
                                System.Diagnostics.Process.Start(Components.Propriedade.PastaExecutavel + "\\uninfe.exe", "/restart");
                            }
                            else
                            {
                                Components.ServiceProcess.StopService(Components.Propriedade.ServiceName, 40000);
                                for (int i = 0; i < 10; ++i)
                                {
                                    Thread.Sleep(500);
                                    if (Components.ServiceProcess.StatusService(Components.Propriedade.ServiceName) == System.ServiceProcess.ServiceControllerStatus.Stopped)
                                    {
                                        Components.ServiceProcess.RestartService(Components.Propriedade.ServiceName, 40000);
                                        break;
                                    }
                                }
                            }
                            return;
                        }
                        catch (Exception ex)
                        {
                            ExtRetorno = (arq.EndsWith(".xml") ? uext.EnvioXML : uext.EnvioTXT);
                            finalArqErro = uext.EnvioXML.Replace(".xml", ".err");
                            exx = ex;
                        }
                        #endregion ---Reinicia
                    }

                    if (ExtRetorno != null)
                    {
                        try
                        {
                            Service.TFunctions.GravarArqErroServico(fi.FullName, ExtRetorno, finalArqErro, exx);
                        }
                        catch { }
                        return;
                    }

                    ///
                    /// solicitacao de layouts
                    ///
                    var lext = Components.Propriedade.Extensao(Components.Propriedade.TipoEnvio.pedLayouts);
                    if (arq.EndsWith(lext.EnvioTXT) || arq.EndsWith(lext.EnvioXML))
                    {
                        Service.TaskLayouts l = new Service.TaskLayouts();
                        l.NomeArquivoXML = fi.FullName;
                        l.Execute();
                        return;
                    }
                    empresa = 0; //Vou criar fixo como 0 quando for na pasta geral, pois na pasta geral não tem como detectar qual é a empresa. 
                }
                else
                {
                    empresa = LocalizaEmpresa(fi);
                }

                if (empresa >= 0) new ThreadItem(fi, empresa).Run();
                else Auxiliar.WriteLog(fi.FullName + " - Não localizou a empresa.", true);
            }
            catch (Exception ex)
            {
                if (fi.Directory.Name.ToLower().EndsWith("geral\\temp"))
                    Components.Functions.WriteLog(ex.Message + "\r\n" + ex.StackTrace, false, true, "");
                else
                    Auxiliar.WriteLog(ex.Message + "\r\n" + ex.StackTrace, false);
            }
        }

        #endregion Eventos
    }
}