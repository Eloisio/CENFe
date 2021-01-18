using System;
using System.Threading;
using System.Windows.Forms;

namespace NFe.UI.Formularios
{
    public partial class FormSpinner : Form
    {
        private string Mensagem = "Carregando, aguarde...";
        public FormSpinner(string mensagem)
        {
            InitializeComponent();

            if (!String.IsNullOrEmpty(mensagem))
            {
                Mensagem = mensagem;
            }
        }
        private void FormSpinner_Load(object sender, EventArgs e)
        {
            lblMensagem.Text = Mensagem;
        }
    }
    public static class Wait
    {
        static FormSpinner wait = null;
        static Thread t = null;
        /// Exibe o form de espera
        public static void Show(string mensagem)
        {
            if (wait != null && wait.Visible) return;
            //-------------------------------------------------------------------------
            // Garantir que existirá sempre apenas uma chamada
            //-------------------------------------------------------------------------
            //assim evita-se o congelamento do componente
            t = new Thread(new ThreadStart(delegate ()
            {
                wait = new FormSpinner(mensagem);
                Application.Run(wait);
            }));
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            Thread.Sleep(100);
        }
        public static void Close()
        {
            if (wait == null || wait.IsDisposed) return;
            wait.Invoke((MethodInvoker)delegate
            {
                wait.Close();
                wait.Dispose();
                Application.DoEvents();
            });
            t.Abort();
            GC.Collect();
        }
    }
}
