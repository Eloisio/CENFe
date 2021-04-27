using NFe.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

namespace NFe.UI
{
    public enum CEnfeOpcoes
    {
        opCadastro,
        opConfiguracoes,
        opDanfe,
        opLogs,
        opMunicipios,
        opServicos,
        opSobre,
        opValidarXML
    }

    public enum CEnfeOpcoes2
    {
        opRestartServico,
        opStopServico,
        opRestartTasks
    }

    public static class CEnfeDummy
    {
        public static string vStatus = "Ocorreu uma falha ao tentar obter a solicitação junto ao SEFAZ.\r\n\r\n" +
                "O problema pode ter ocorrido por causa dos seguintes fatores:\r\n\r\n" +
                "- Problema com o certificado digital\r\n" +
                "- Necessidade de atualização da cadeia de certificados digitais\r\n" +
                "- Falha de conexão com a internet\r\n" +
                "- Falha nos servidores do SEFAZ\r\n\r\n" +
                "Afirmamos que a produtora do software não se responsabiliza por decisões tomadas e/ou execuções realizadas com base nas informações acima.\r\n\r\n";

        public static DateTime UltimoAcessoConfiguracao { get; set; }
        public static Form_Main mainForm { get; set; }
        public static CEnfeOpcoes2 opServicos { get; set; }
        private static NFe.Components.XMLIniFile _xml;
        public static XMLIniFile xmlParams
        {
            get
            {
                if (_xml == null)
                    _xml = new NFe.Components.XMLIniFile(NFe.Components.Propriedade.NomeArqXMLParams);
                return _xml;
            }
        }
        public static bool TempoExpirou()
        {
            if (CEnfeDummy.UltimoAcessoConfiguracao == DateTime.MinValue)
                return true;

            DateTime stopTime = DateTime.Now;
            TimeSpan elapsedTime = stopTime.Subtract(CEnfeDummy.UltimoAcessoConfiguracao);
            return (int)elapsedTime.TotalMinutes > 60;
        }
        public static string FmtCnpjCpf(string Value, bool juridica)
        {
            string Result = Value.PadLeft(14, '0');

            if (juridica)
                Result = Result.Substring(0, 2) + "." +
                        Result.Substring(2, 3) + "." +
                        Result.Substring(5, 3) + "/" +
                        Result.Substring(8, 4) + "-" +
                        Result.Substring(12, 2);
            else
                Result = Result.Substring(3, 3) + "." +
                            Result.Substring(6, 3) + "." +
                            Result.Substring(9, 3) + "-" +
                            Result.Substring(12, 2);
            return Result;
        }
        public static IList DatasouceTipoAplicativo(bool soConsulta)
        {
            ArrayList list = new ArrayList();
            if (!soConsulta)
            list.Add(new KeyValuePair<int, string>((int)TipoAplicativo.Nfe, EnumHelper.GetDescription(TipoAplicativo.Nfe)));
            return list;
        }
        public static void ClearControls(System.Windows.Forms.Control Xcontrol, bool clear, bool inverteTheme)
        {
            if (Xcontrol == null)  return;
            MetroFramework.MetroThemeStyle uTheme = CEnfeDummy.mainForm.metroStyleManager1.Theme;
            if (Xcontrol.GetType().IsSubclassOf(typeof(MetroFramework.Controls.MetroUserControl)) ||
                Xcontrol.GetType().IsSubclassOf(typeof(MetroFramework.Controls.MetroTabPage)) ||
                Xcontrol.GetType().IsSubclassOf(typeof(MetroFramework.Controls.MetroTabControl)) ||
                Xcontrol.GetType().IsSubclassOf(typeof(MetroFramework.Forms.MetroForm)))
            {
                if (Xcontrol.GetType().IsSubclassOf(typeof(MetroFramework.Forms.MetroForm)))
                {
                    ((MetroFramework.Interfaces.IMetroForm)Xcontrol).Theme = uTheme;
                    ((MetroFramework.Interfaces.IMetroForm)Xcontrol).Style = CEnfeDummy.mainForm.metroStyleManager1.Style;
                }
                else
                {
                    ((MetroFramework.Interfaces.IMetroControl)Xcontrol).Theme = uTheme;
                    ((MetroFramework.Interfaces.IMetroControl)Xcontrol).Style = CEnfeDummy.mainForm.metroStyleManager1.Style;
                }
            }
            foreach (Control control in Xcontrol.Controls)
            {
                if (control.Controls.Count > 0)             ClearControls(control, clear, inverteTheme);
                if (clear)
                {
                    if (control.GetType().IsAssignableFrom(typeof(TextBox)))
                        NFe.Components.Functions.SetProperty(control, "Text", "");
                    if (control.GetType().IsAssignableFrom(typeof(MetroFramework.Controls.MetroTextBox)))
                        NFe.Components.Functions.SetProperty(control, "Text", "");
                    NFe.Components.Functions.SetProperty(control, "PromptText", "");
                }
                NFe.Components.Functions.SetProperty(control, "UseStyleColors", false);
                if (control is MetroFramework.Interfaces.IMetroControl)
                {
                    try
                    {
                        ((MetroFramework.Interfaces.IMetroControl)control).Theme = mainForm.metroStyleManager1.Theme;
                        ((MetroFramework.Interfaces.IMetroControl)control).Style = mainForm.metroStyleManager1.Style;
                    }
                    catch { }
                }
            }
        }
    }
}