using System;
using System.Drawing;
using System.Windows.Forms;

namespace NFe.UI
{
    public partial class FormDummy : MetroFramework.Forms.MetroForm
    {
        public FormDummy()
        {
            InitializeComponent();
        }

        public CEnfeOpcoes opcao { get; set; }

        public static void ShowModulo(CEnfeOpcoes opcao)
        {
            using (FormDummy f = new FormDummy())
            {
                f.opcao = opcao;
                if (f.LoadProcesso())
                    f.ShowDialog();
                else
                    f.Tag = 1;  //ops nao precisa salvar a posicao da tela
            }
        }

        bool LoadProcesso()
        {
            //NFe.UI.uninfeDummy.showError = false;
            try
            {
                UserControl1 control = null;

                switch (opcao)
                {
                    case CEnfeOpcoes.opCadastro:
                        Controls.Add(control = new userCadastro());
                        break;

                    case CEnfeOpcoes.opConfiguracoes:
                        Controls.Add(control = new userConfiguracoes());
                        break;

                    case CEnfeOpcoes.opMunicipios:
                        Controls.Add(control = new Formularios.userMunicipios());
                        break;
                }
                if (control != null)
                {
                    control.Dock = DockStyle.Fill;
                    control.back_button.Click += delegate { Close(); };
                    control.UpdateControles();
                    Invalidate();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                MetroFramework.MetroMessageBox.Show(this, ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Text = NFe.Components.Propriedade.NomeAplicacao + "  (" + NFe.Components.Propriedade.Versao + ")";
            MinimumSize = new Size(800, 600);

            try
            {
                StyleManager = ((MetroFramework.Interfaces.IMetroForm)CEnfeDummy.mainForm).StyleManager.Clone(this) as MetroFramework.Components.MetroStyleManager;
            }
            catch
            {
                StyleManager = CEnfeDummy.mainForm.StyleManager;
            }
            //this.Theme = uninfeDummy.mainForm.uTheme;
            //this.Style = uninfeDummy.mainForm.uStyle;
            //this.StyleManager = uninfeDummy.mainForm.StyleManager;

            CEnfeDummy.xmlParams.LoadForm(this, "\\" + opcao.ToString(), true);
            Icon = Properties.Resources.uninfe_icon;
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (Tag == null)
            {
                if (opcao == CEnfeOpcoes.opConfiguracoes)
                {
                    CEnfeDummy.mainForm.RestartServicos();
                }
                CEnfeDummy.xmlParams.SaveForm(this, "\\" + opcao.ToString());
                CEnfeDummy.xmlParams.Save();
            }
            base.OnFormClosed(e);
        }
    }
}
