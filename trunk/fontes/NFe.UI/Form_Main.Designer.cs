namespace NFe.UI
{
    partial class Form_Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_Main));
            this.metroStyleManager1 = new MetroFramework.Components.MetroStyleManager(this.components);
            this.metroToolTip1 = new MetroFramework.Components.MetroToolTip();
            this.tbPararServico = new System.Windows.Forms.ToolStripMenuItem();
            this.tbRestartServico = new System.Windows.Forms.ToolStripMenuItem();
            this.tbSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmAbrir = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.cmConfiguracoes = new System.Windows.Forms.ToolStripMenuItem();
            this.cmFechar = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // metroStyleManager1
            // 
            this.metroStyleManager1.Owner = this;
            // 
            // metroToolTip1
            // 
            this.metroToolTip1.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroToolTip1.StyleManager = null;
            this.metroToolTip1.Theme = MetroFramework.MetroThemeStyle.Light;
            // 
            // tbPararServico
            // 
            this.tbPararServico.Name = "tbPararServico";
            this.tbPararServico.Size = new System.Drawing.Size(265, 22);
            this.tbPararServico.Text = "Parar o serviço";
            this.tbPararServico.Click += new System.EventHandler(this.tbPararServico_Click);
            // 
            // tbRestartServico
            // 
            this.tbRestartServico.Name = "tbRestartServico";
            this.tbRestartServico.Size = new System.Drawing.Size(265, 22);
            this.tbRestartServico.Text = "Reiniciar o serviço";
            this.tbRestartServico.Click += new System.EventHandler(this.tbRestartServico_Click);
            // 
            // tbSeparator1
            // 
            this.tbSeparator1.Name = "tbSeparator1";
            this.tbSeparator1.Size = new System.Drawing.Size(262, 6);
            // 
            // cmAbrir
            // 
            this.cmAbrir.Name = "cmAbrir";
            this.cmAbrir.Size = new System.Drawing.Size(265, 22);
            this.cmAbrir.Text = "Abrir Campos Evolution Monitor";
            this.cmAbrir.Click += new System.EventHandler(this.cmAbrir_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(262, 6);
            // 
            // cmConfiguracoes
            // 
            this.cmConfiguracoes.Name = "cmConfiguracoes";
            this.cmConfiguracoes.Size = new System.Drawing.Size(265, 22);
            this.cmConfiguracoes.Text = "Configurações";
            this.cmConfiguracoes.Click += new System.EventHandler(this.cmConfiguracoes_Click);
            // 
            // cmFechar
            // 
            this.cmFechar.Name = "cmFechar";
            this.cmFechar.Size = new System.Drawing.Size(265, 22);
            this.cmFechar.Text = "Fechar o Campos Evolution Monitor";
            this.cmFechar.Click += new System.EventHandler(this.cmFechar_Click);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbPararServico,
            this.tbRestartServico,
            this.tbSeparator1,
            this.cmConfiguracoes,
            this.toolStripSeparator2,
            this.cmAbrir,
            this.cmFechar});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(266, 126);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::NFe.UI.Properties.Resources.NOVO_LOGOGRANDE800;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.ErrorImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(12, 18);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(59, 50);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // Form_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackImagePadding = new System.Windows.Forms.Padding(210, 10, 0, 0);
            this.BackMaxSize = 50;
            this.ClientSize = new System.Drawing.Size(764, 724);
            this.Controls.Add(this.pictureBox1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinimumSize = new System.Drawing.Size(583, 500);
            this.Name = "Form_Main";
            this.Padding = new System.Windows.Forms.Padding(17, 60, 17, 15);
            this.ShadowType = MetroFramework.Forms.MetroFormShadowType.AeroShadow;
            this.Text = "Campos Evolution NFe";
            this.Theme = MetroFramework.MetroThemeStyle.Default;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ControlRemoved += new System.Windows.Forms.ControlEventHandler(this.Form_Main_ControlRemoved);
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private MetroFramework.Components.MetroToolTip metroToolTip1;
        private System.Windows.Forms.ToolStripMenuItem tbPararServico;
        private System.Windows.Forms.ToolStripMenuItem tbRestartServico;
        private System.Windows.Forms.ToolStripSeparator tbSeparator1;
        private System.Windows.Forms.ToolStripMenuItem cmAbrir;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem cmConfiguracoes;
        private System.Windows.Forms.ToolStripMenuItem cmFechar;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.PictureBox pictureBox1;
        public MetroFramework.Components.MetroStyleManager metroStyleManager1;
    }
}

