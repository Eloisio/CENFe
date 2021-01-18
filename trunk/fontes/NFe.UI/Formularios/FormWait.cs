using System;
using System.Windows.Forms;

namespace NFe.UI.Formularios
{
    public partial class FormWait : MetroFramework.Forms.MetroForm
    {
        public FormWait()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            CEnfeDummy.ClearControls(this, true, true);
            metroLabel6.Text = mensagem;

            NFe.Settings.Auxiliar.WriteLog(metroLabel6.Text, false);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            Timer t = new Timer();
            t.Interval = 500;
            t.Tick += delegate
            {
                t.Stop();
                try
                {
                    if (acao1 != null)
                    {
                        acao1();
                        metroLabel6.Text = "Iniciando os serviços";
                        metroLabel6.Update();
                        acao2();
                    }
                    else
                    {
                        if (stopservice)
                            NFe.Components.ServiceProcess.StopService(servicename, 40000);
                        else
                            NFe.Components.ServiceProcess.RestartService(servicename, 40000);
                    }
                    DialogResult = System.Windows.Forms.DialogResult.OK;
                }
                catch (Exception ex)
                {
                    MetroFramework.MetroMessageBox.Show(null, ex.Message, "");
                    DialogResult = System.Windows.Forms.DialogResult.Cancel;
                }
            };
            t.Start();
        }

        public Action acao1 { private get; set; }
        public Action acao2 { private get; set; }
        public bool stopservice { private get; set; }
        public string servicename { private get; set; }
        public string mensagem { private get; set; }
    }
}
