namespace dataPump.det
{
    partial class F99
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(F99));
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Деинсталляция FB 15");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Установка FB 25");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Восстановление системных пользователей");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Анализ");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Восстановление первых метаданных");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Восстановление данных");
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("Восстановление вторых метаданных");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("Замена базы данных");
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("Конвертирование базы данных", new System.Windows.Forms.TreeNode[] {
            treeNode4,
            treeNode5,
            treeNode6,
            treeNode7,
            treeNode8});
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("Восстановление пользователей");
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.b_next = new System.Windows.Forms.Button();
            this.b_prev = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.t_sysdba_new = new System.Windows.Forms.TextBox();
            this.is_new_sysdba = new System.Windows.Forms.CheckBox();
            this.t_sysdba = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.t_pass = new System.Windows.Forms.TextBox();
            this.is_user = new System.Windows.Forms.CheckBox();
            this.button5 = new System.Windows.Forms.Button();
            this.t_programm = new System.Windows.Forms.TextBox();
            this.is_install = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.is_replace = new System.Windows.Forms.CheckBox();
            this.button4 = new System.Windows.Forms.Button();
            this.t_database = new System.Windows.Forms.TextBox();
            this.is_convert = new System.Windows.Forms.CheckBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.progressBar2 = new System.Windows.Forms.ProgressBar();
            this.l_count = new System.Windows.Forms.Label();
            this.l_ = new System.Windows.Forms.Label();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.l_error = new System.Windows.Forms.Label();
            this.b_save = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SelectDB = new System.Windows.Forms.OpenFileDialog();
            this.SelectDir = new System.Windows.Forms.FolderBrowserDialog();
            this.saveAs = new System.Windows.Forms.SaveFileDialog();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.p_load = new System.Windows.Forms.PictureBox();
            this.image_bad = new System.Windows.Forms.PictureBox();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.p_load)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.image_bad)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Controls.Add(this.b_next);
            this.groupBox1.Controls.Add(this.b_prev);
            this.groupBox1.Location = new System.Drawing.Point(1, 458);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Size = new System.Drawing.Size(544, 59);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(117, 20);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(35, 25);
            this.button1.TabIndex = 3;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(5, 18);
            this.button3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(87, 28);
            this.button3.TabIndex = 2;
            this.button3.Text = "Выход";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // b_next
            // 
            this.b_next.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.b_next.Location = new System.Drawing.Point(446, 18);
            this.b_next.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.b_next.Name = "b_next";
            this.b_next.Size = new System.Drawing.Size(87, 28);
            this.b_next.TabIndex = 1;
            this.b_next.Text = "Далее>>";
            this.b_next.UseVisualStyleBackColor = true;
            this.b_next.Click += new System.EventHandler(this.b_next_Click);
            // 
            // b_prev
            // 
            this.b_prev.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.b_prev.Enabled = false;
            this.b_prev.Location = new System.Drawing.Point(351, 18);
            this.b_prev.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.b_prev.Name = "b_prev";
            this.b_prev.Size = new System.Drawing.Size(87, 28);
            this.b_prev.TabIndex = 0;
            this.b_prev.Text = "<<Назад";
            this.b_prev.UseVisualStyleBackColor = true;
            this.b_prev.Click += new System.EventHandler(this.b_prev_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl1.ItemSize = new System.Drawing.Size(20, 20);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(543, 466);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.pictureBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabPage1.Size = new System.Drawing.Size(535, 438);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "1";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(8, 166);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(519, 141);
            this.label1.TabIndex = 1;
            this.label1.Text = resources.GetString("label1.Text");
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox3);
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabPage2.Size = new System.Drawing.Size(535, 438);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.t_sysdba_new);
            this.groupBox3.Controls.Add(this.is_new_sysdba);
            this.groupBox3.Controls.Add(this.t_sysdba);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.checkBox4);
            this.groupBox3.Controls.Add(this.t_pass);
            this.groupBox3.Controls.Add(this.is_user);
            this.groupBox3.Controls.Add(this.button5);
            this.groupBox3.Controls.Add(this.t_programm);
            this.groupBox3.Controls.Add(this.is_install);
            this.groupBox3.Location = new System.Drawing.Point(2, 90);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox3.Size = new System.Drawing.Size(522, 191);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Настройки";
            // 
            // t_sysdba_new
            // 
            this.t_sysdba_new.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.t_sysdba_new.Enabled = false;
            this.t_sysdba_new.Location = new System.Drawing.Point(227, 91);
            this.t_sysdba_new.Name = "t_sysdba_new";
            this.t_sysdba_new.Size = new System.Drawing.Size(260, 22);
            this.t_sysdba_new.TabIndex = 10;
            // 
            // is_new_sysdba
            // 
            this.is_new_sysdba.AutoSize = true;
            this.is_new_sysdba.Location = new System.Drawing.Point(74, 91);
            this.is_new_sysdba.Name = "is_new_sysdba";
            this.is_new_sysdba.Size = new System.Drawing.Size(124, 20);
            this.is_new_sysdba.TabIndex = 9;
            this.is_new_sysdba.Text = "Сменить пароль";
            this.is_new_sysdba.UseVisualStyleBackColor = true;
            this.is_new_sysdba.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged_1);
            // 
            // t_sysdba
            // 
            this.t_sysdba.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.t_sysdba.Location = new System.Drawing.Point(227, 63);
            this.t_sysdba.Name = "t_sysdba";
            this.t_sysdba.Size = new System.Drawing.Size(260, 22);
            this.t_sysdba.TabIndex = 8;
            this.t_sysdba.Text = "masterkey";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(71, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(110, 16);
            this.label2.TabIndex = 7;
            this.label2.Text = "Пароль SYSDBA";
            // 
            // checkBox4
            // 
            this.checkBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox4.AutoSize = true;
            this.checkBox4.Checked = true;
            this.checkBox4.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox4.Location = new System.Drawing.Point(227, 162);
            this.checkBox4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(127, 20);
            this.checkBox4.TabIndex = 6;
            this.checkBox4.Text = "Показать пароль";
            this.checkBox4.UseVisualStyleBackColor = true;
            this.checkBox4.CheckedChanged += new System.EventHandler(this.checkBox4_CheckedChanged);
            // 
            // t_pass
            // 
            this.t_pass.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.t_pass.Location = new System.Drawing.Point(227, 132);
            this.t_pass.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.t_pass.Name = "t_pass";
            this.t_pass.Size = new System.Drawing.Size(260, 22);
            this.t_pass.TabIndex = 5;
            this.t_pass.Text = "12345";
            // 
            // is_user
            // 
            this.is_user.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.is_user.AutoSize = true;
            this.is_user.Checked = true;
            this.is_user.CheckState = System.Windows.Forms.CheckState.Checked;
            this.is_user.Location = new System.Drawing.Point(27, 132);
            this.is_user.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.is_user.Name = "is_user";
            this.is_user.Size = new System.Drawing.Size(189, 20);
            this.is_user.TabIndex = 4;
            this.is_user.Text = "Воссоздать пользователей";
            this.is_user.UseVisualStyleBackColor = true;
            this.is_user.CheckedChanged += new System.EventHandler(this.checkBox3_CheckedChanged);
            // 
            // button5
            // 
            this.button5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button5.Location = new System.Drawing.Point(493, 20);
            this.button5.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(28, 23);
            this.button5.TabIndex = 3;
            this.button5.Text = "...";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // t_programm
            // 
            this.t_programm.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.t_programm.Location = new System.Drawing.Point(227, 20);
            this.t_programm.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.t_programm.Name = "t_programm";
            this.t_programm.Size = new System.Drawing.Size(260, 22);
            this.t_programm.TabIndex = 1;
            this.t_programm.Text = "C:\\Program Files\\Firebird\\Firebird_2_5";
            // 
            // is_install
            // 
            this.is_install.AutoSize = true;
            this.is_install.Checked = true;
            this.is_install.CheckState = System.Windows.Forms.CheckState.Checked;
            this.is_install.Location = new System.Drawing.Point(7, 23);
            this.is_install.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.is_install.Name = "is_install";
            this.is_install.Size = new System.Drawing.Size(174, 20);
            this.is_install.TabIndex = 0;
            this.is_install.Text = "Установить сервер FB25";
            this.toolTip1.SetToolTip(this.is_install, "Будет произведены следующие действия\r\n- Деисталляция более старых версий сервера\r" +
        "\n- Установка сервера FB25\r\nЕсли не выбрать - будет использоваться встроенный сер" +
        "вер");
            this.is_install.UseVisualStyleBackColor = true;
            this.is_install.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.is_replace);
            this.groupBox2.Controls.Add(this.button4);
            this.groupBox2.Controls.Add(this.t_database);
            this.groupBox2.Controls.Add(this.is_convert);
            this.groupBox2.Location = new System.Drawing.Point(2, 4);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox2.Size = new System.Drawing.Size(522, 78);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "База данных";
            // 
            // is_replace
            // 
            this.is_replace.AutoSize = true;
            this.is_replace.Checked = true;
            this.is_replace.CheckState = System.Windows.Forms.CheckState.Checked;
            this.is_replace.Location = new System.Drawing.Point(27, 50);
            this.is_replace.Name = "is_replace";
            this.is_replace.Size = new System.Drawing.Size(84, 20);
            this.is_replace.TabIndex = 3;
            this.is_replace.Text = "Заменить";
            this.is_replace.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button4.Location = new System.Drawing.Point(493, 20);
            this.button4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(28, 24);
            this.button4.TabIndex = 2;
            this.button4.Text = "...";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // t_database
            // 
            this.t_database.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.t_database.Location = new System.Drawing.Point(227, 20);
            this.t_database.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.t_database.Name = "t_database";
            this.t_database.Size = new System.Drawing.Size(260, 22);
            this.t_database.TabIndex = 1;
            this.t_database.Text = "D:\\Talisman_SQL\\Base\\tsql.gdb";
            // 
            // is_convert
            // 
            this.is_convert.AutoSize = true;
            this.is_convert.Checked = true;
            this.is_convert.CheckState = System.Windows.Forms.CheckState.Checked;
            this.is_convert.Location = new System.Drawing.Point(7, 23);
            this.is_convert.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.is_convert.Name = "is_convert";
            this.is_convert.Size = new System.Drawing.Size(122, 20);
            this.is_convert.TabIndex = 0;
            this.is_convert.Text = "Конвертировать";
            this.is_convert.UseVisualStyleBackColor = true;
            this.is_convert.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.p_load);
            this.tabPage3.Controls.Add(this.progressBar2);
            this.tabPage3.Controls.Add(this.l_count);
            this.tabPage3.Controls.Add(this.l_);
            this.tabPage3.Controls.Add(this.treeView1);
            this.tabPage3.Controls.Add(this.progressBar1);
            this.tabPage3.Location = new System.Drawing.Point(4, 24);
            this.tabPage3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(535, 438);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "3";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // progressBar2
            // 
            this.progressBar2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar2.Location = new System.Drawing.Point(4, 244);
            this.progressBar2.Name = "progressBar2";
            this.progressBar2.Size = new System.Drawing.Size(523, 28);
            this.progressBar2.TabIndex = 4;
            this.progressBar2.Visible = false;
            // 
            // l_count
            // 
            this.l_count.AutoSize = true;
            this.l_count.Location = new System.Drawing.Point(8, 225);
            this.l_count.Name = "l_count";
            this.l_count.Size = new System.Drawing.Size(50, 16);
            this.l_count.TabIndex = 3;
            this.l_count.Text = "l_count";
            this.l_count.Visible = false;
            // 
            // l_
            // 
            this.l_.AutoSize = true;
            this.l_.Location = new System.Drawing.Point(8, 275);
            this.l_.Name = "l_";
            this.l_.Size = new System.Drawing.Size(29, 16);
            this.l_.TabIndex = 2;
            this.l_.Text = "___";
            // 
            // treeView1
            // 
            this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.treeView1.ImageIndex = 0;
            this.treeView1.ImageList = this.imageList1;
            this.treeView1.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.treeView1.LineColor = System.Drawing.Color.DimGray;
            this.treeView1.Location = new System.Drawing.Point(3, 4);
            this.treeView1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.treeView1.Name = "treeView1";
            treeNode1.Name = "Node0";
            treeNode1.Text = "Деинсталляция FB 15";
            treeNode2.Name = "Node1";
            treeNode2.Text = "Установка FB 25";
            treeNode3.Name = "Node2";
            treeNode3.Text = "Восстановление системных пользователей";
            treeNode4.Name = "Node3_1";
            treeNode4.Text = "Анализ";
            treeNode5.Name = "Node3_2";
            treeNode5.Text = "Восстановление первых метаданных";
            treeNode6.Name = "Node3_3";
            treeNode6.Text = "Восстановление данных";
            treeNode7.Name = "Node3_4";
            treeNode7.Text = "Восстановление вторых метаданных";
            treeNode8.Name = "Node3_5";
            treeNode8.Text = "Замена базы данных";
            treeNode9.Name = "Node3";
            treeNode9.Text = "Конвертирование базы данных";
            treeNode10.Name = "Node4";
            treeNode10.Text = "Восстановление пользователей";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3,
            treeNode9,
            treeNode10});
            this.treeView1.SelectedImageIndex = 0;
            this.treeView1.Size = new System.Drawing.Size(523, 206);
            this.treeView1.TabIndex = 1;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "disabled.png");
            this.imageList1.Images.SetKeyName(1, "wait.png");
            this.imageList1.Images.SetKeyName(2, "current.png");
            this.imageList1.Images.SetKeyName(3, "done_2.png");
            this.imageList1.Images.SetKeyName(4, "fault.png");
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(7, 295);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(523, 33);
            this.progressBar1.TabIndex = 0;
            this.progressBar1.Visible = false;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.l_error);
            this.tabPage4.Controls.Add(this.b_save);
            this.tabPage4.Controls.Add(this.richTextBox1);
            this.tabPage4.Controls.Add(this.label3);
            this.tabPage4.Controls.Add(this.image_bad);
            this.tabPage4.Location = new System.Drawing.Point(4, 24);
            this.tabPage4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(535, 438);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "4";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // l_error
            // 
            this.l_error.AutoSize = true;
            this.l_error.Location = new System.Drawing.Point(8, 61);
            this.l_error.Name = "l_error";
            this.l_error.Size = new System.Drawing.Size(100, 16);
            this.l_error.TabIndex = 4;
            this.l_error.Text = "Список ошибок";
            this.l_error.Visible = false;
            // 
            // b_save
            // 
            this.b_save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.b_save.Location = new System.Drawing.Point(443, 399);
            this.b_save.Name = "b_save";
            this.b_save.Size = new System.Drawing.Size(87, 28);
            this.b_save.TabIndex = 2;
            this.b_save.Text = "Сохранить";
            this.b_save.UseVisualStyleBackColor = true;
            this.b_save.Visible = false;
            this.b_save.Click += new System.EventHandler(this.b_save_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox1.Location = new System.Drawing.Point(11, 80);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(516, 313);
            this.richTextBox1.TabIndex = 1;
            this.richTextBox1.Text = "";
            this.richTextBox1.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 38);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(284, 16);
            this.label3.TabIndex = 0;
            this.label3.Text = "Спасибо за использование нашей программы";
            // 
            // SelectDB
            // 
            this.SelectDB.Filter = "*.gdb|*.gdb";
            // 
            // SelectDir
            // 
            this.SelectDir.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // saveAs
            // 
            this.saveAs.Filter = "*.txt|*.txt";
            // 
            // timer1
            // 
            this.timer1.Interval = 3000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Image = global::dataPump.det.Properties.Resources.пример;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(535, 122);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // p_load
            // 
            this.p_load.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.p_load.Image = ((System.Drawing.Image)(resources.GetObject("p_load.Image")));
            this.p_load.InitialImage = ((System.Drawing.Image)(resources.GetObject("p_load.InitialImage")));
            this.p_load.Location = new System.Drawing.Point(7, 294);
            this.p_load.Name = "p_load";
            this.p_load.Size = new System.Drawing.Size(50, 50);
            this.p_load.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.p_load.TabIndex = 5;
            this.p_load.TabStop = false;
            this.p_load.Visible = false;
            // 
            // image_bad
            // 
            this.image_bad.Image = global::dataPump.det.Properties.Resources.l_done;
            this.image_bad.Location = new System.Drawing.Point(461, 13);
            this.image_bad.Name = "image_bad";
            this.image_bad.Size = new System.Drawing.Size(53, 50);
            this.image_bad.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.image_bad.TabIndex = 3;
            this.image_bad.TabStop = false;
            this.image_bad.Visible = false;
            // 
            // F99
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(543, 519);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "F99";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Переход на версию FB 2.5";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.F99_FormClosing);
            this.Load += new System.EventHandler(this.F99_Load);
            this.groupBox1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.p_load)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.image_bad)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button b_next;
        private System.Windows.Forms.Button b_prev;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TextBox t_database;
        private System.Windows.Forms.CheckBox is_convert;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.TextBox t_programm;
        private System.Windows.Forms.CheckBox is_install;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.OpenFileDialog SelectDB;
        private System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.TextBox t_pass;
        private System.Windows.Forms.CheckBox is_user;
        private System.Windows.Forms.FolderBrowserDialog SelectDir;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Label l_;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox is_replace;
        private System.Windows.Forms.Label l_count;
        private System.Windows.Forms.Button b_save;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.SaveFileDialog saveAs;
        private System.Windows.Forms.ProgressBar progressBar2;
        private System.Windows.Forms.PictureBox image_bad;
        private System.Windows.Forms.Label l_error;
        private System.Windows.Forms.TextBox t_sysdba;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TextBox t_sysdba_new;
        private System.Windows.Forms.CheckBox is_new_sysdba;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox p_load;
    }
}