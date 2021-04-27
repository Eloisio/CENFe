using NFe.Components;
using NFe.Settings;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace NFe.UI
{
    [ToolboxItem(false)]
    public partial class menu : MetroFramework.Controls.MetroUserControl
    {
        public menu()
        {
            InitializeComponent();
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ConfiguracaoApp.CarregarDadosSobre();
            CEnfeDummy.mainForm.Text = "CAMPOS EVOLUTION";
            var Components = Controls.Cast<object>()
                                                   .Where(obj => !ReferenceEquals(obj, this))
                                                   .OfType<MetroFramework.Controls.MetroTile>();
            foreach (var c in Components)
            {
                c.Style = (MetroFramework.MetroColorStyle)CEnfeDummy.xmlParams.ReadValue(Name + "\\" + c.Name, "color", Convert.ToInt16(c.Style));
            }
            UpdateControles();
        }
        public void UpdateControles()
        {
        }
        public void Show(CEnfeOpcoes opcao)
        {
            DisposeAllControls();
            switch (opcao)
            {
                case CEnfeOpcoes.opCadastro:
                    metroTile_CAD_Click(null, null);
                    break;

                case CEnfeOpcoes.opConfiguracoes:
                    metroTile_CFG_Click(null, null);
                    break;

                case CEnfeOpcoes.opMunicipios:
                    metroTile_municipios_Click(null, null);
                    break;
            }
        }
        private void menu_VisibleChanged(object sender, EventArgs e)
        {
            if (!Propriedade.EncerrarApp)
            {
                UpdateControles();
                DisposeAllControls();
            }
        }
        private void DisposeAllControls()
        {
            var Components = CEnfeDummy.mainForm.Controls.Cast<object>()
                                                   .Where(obj => !ReferenceEquals(obj, this))
                                                   .OfType<MetroFramework.Controls.MetroUserControl>();
            foreach (var c in Components)
            {
                c.Dispose();
            }
        }
        private bool createControl(Type user)
        {
            bool _novo = false;
            VisibleChanged -= menu_VisibleChanged;
            try
            {
                UserControl1 _uc = null;
                /// processo já existe?
                foreach (Control __uc in CEnfeDummy.mainForm.Controls)
                {
                    if (__uc.GetType().Equals(user))
                    {
                        _uc = __uc as UserControl1;
                        break;
                    }
                }
                _novo = (_uc == null);
                if (_uc == null) _uc = Activator.CreateInstance(user) as UserControl1;/// cria o processo
                _uc.UpdateControles();
                if (_novo)CEnfeDummy.mainForm.Controls.Add(_uc);
                var Components = CEnfeDummy.mainForm.Controls.Cast<object>()
                                                       .Where(obj => !ReferenceEquals(obj, _uc))
                                                       .OfType<MetroFramework.Controls.MetroUserControl>();
                foreach (Control ctrl in Components)
                {
                    ctrl.Visible = false;
                }
                _uc.Visible = true;
            }
            catch (Exception ex)
            {
                MetroFramework.MetroMessageBox.Show(CEnfeDummy.mainForm, ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                VisibleChanged += menu_VisibleChanged;
            }
            return _novo;
        }
        private void metroTile_CFG_Click(object sender, EventArgs e)
        {
        }
        private void metroTile_CAD_Click(object sender, EventArgs e)
        {
            createControl(typeof(userCadastro));
        }
        private void metroTile_municipios_Click(object sender, EventArgs e)
        {
            createControl(typeof(Formularios.userMunicipios));
        }
        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            CurrentTile.Style = (MetroFramework.MetroColorStyle)Convert.ToInt16(e.ClickedItem.Tag);
            CurrentTile.Refresh();
            CEnfeDummy.xmlParams.WriteValue(Name + "\\" + CurrentTile.Name, "color", (int)CurrentTile.Style);
            CurrentTile = null;
        }
        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (CurrentTile != null)
            {
                foreach (var item in contextMenuStrip1.Items)
                {
                    try
                    {
                        if (item.ToString().Equals("Restaurar Padrão"))
                            continue;
                        ((ToolStripMenuItem)item).Checked = Convert.ToInt16(((ToolStripMenuItem)item).Tag) == (int)CurrentTile.Style;
                    }
                    catch
                    {
                    }
                }
            }
        }
        private MetroFramework.Controls.MetroTile CurrentTile = null;
        private void restaurarPadrãoToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void metroLabel1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ConfiguracaoApp.SenhaConfig) && CEnfeDummy.TempoExpirou())
            {
                if (!FormSenha.SolicitaSenha(true))
                    return;
                CEnfeDummy.UltimoAcessoConfiguracao = DateTime.Now;
            }
            createControl(typeof(userConfiguracoes));
        }
    }
}