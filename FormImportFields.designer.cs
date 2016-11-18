namespace CsvXlsImport {
    partial class FormImportFields<T> {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.splitTop = new System.Windows.Forms.SplitContainer();
            this.gridFields = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.listTargets = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.gridData = new System.Windows.Forms.DataGridView();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitTop)).BeginInit();
            this.splitTop.Panel1.SuspendLayout();
            this.splitTop.Panel2.SuspendLayout();
            this.splitTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridFields)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridData)).BeginInit();
            this.SuspendLayout();
            // 
            // splitTop
            // 
            this.splitTop.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitTop.Location = new System.Drawing.Point(3, 16);
            this.splitTop.Name = "splitTop";
            // 
            // splitTop.Panel1
            // 
            this.splitTop.Panel1.Controls.Add(this.gridFields);
            this.splitTop.Panel1.Controls.Add(this.label1);
            // 
            // splitTop.Panel2
            // 
            this.splitTop.Panel2.Controls.Add(this.listTargets);
            this.splitTop.Panel2.Controls.Add(this.label2);
            this.splitTop.Size = new System.Drawing.Size(455, 163);
            this.splitTop.SplitterDistance = 281;
            this.splitTop.TabIndex = 0;
            // 
            // gridFields
            // 
            this.gridFields.AllowDrop = true;
            this.gridFields.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridFields.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridFields.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2});
            this.gridFields.Location = new System.Drawing.Point(3, 23);
            this.gridFields.Name = "gridFields";
            this.gridFields.Size = new System.Drawing.Size(275, 138);
            this.gridFields.TabIndex = 1;
            this.gridFields.DragDrop += new System.Windows.Forms.DragEventHandler(this.gridFields_DragDrop);
            this.gridFields.DragOver += new System.Windows.Forms.DragEventHandler(this.gridFields_DragOver);
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "FieldName";
            this.Column1.HeaderText = "Field";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "TargetName";
            this.Column2.HeaderText = "Meaning";
            this.Column2.Name = "Column2";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Excel Columns";
            // 
            // listTargets
            // 
            this.listTargets.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listTargets.FormattingEnabled = true;
            this.listTargets.IntegralHeight = false;
            this.listTargets.Location = new System.Drawing.Point(-1, 23);
            this.listTargets.Name = "listTargets";
            this.listTargets.Size = new System.Drawing.Size(168, 140);
            this.listTargets.TabIndex = 1;
            this.listTargets.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listTargets_MouseDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Meaning";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(245, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Drag from the right to the left to define the columns";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(305, 380);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(386, 380);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // splitMain
            // 
            this.splitMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitMain.Location = new System.Drawing.Point(0, 0);
            this.splitMain.Name = "splitMain";
            this.splitMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitMain.Panel1
            // 
            this.splitMain.Panel1.Controls.Add(this.splitTop);
            this.splitMain.Panel1.Controls.Add(this.label3);
            // 
            // splitMain.Panel2
            // 
            this.splitMain.Panel2.Controls.Add(this.gridData);
            this.splitMain.Panel2.Controls.Add(this.label4);
            this.splitMain.Size = new System.Drawing.Size(461, 375);
            this.splitMain.SplitterDistance = 182;
            this.splitMain.TabIndex = 4;
            // 
            // gridData
            // 
            this.gridData.AllowUserToAddRows = false;
            this.gridData.AllowUserToDeleteRows = false;
            this.gridData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridData.Location = new System.Drawing.Point(3, 16);
            this.gridData.Name = "gridData";
            this.gridData.ReadOnly = true;
            this.gridData.Size = new System.Drawing.Size(458, 173);
            this.gridData.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(163, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Sample data as it will be imported";
            // 
            // FormImportFields
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(464, 406);
            this.Controls.Add(this.splitMain);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Name = "FormImportFields";
            this.Text = "Choose fields to import";
            this.splitTop.Panel1.ResumeLayout(false);
            this.splitTop.Panel1.PerformLayout();
            this.splitTop.Panel2.ResumeLayout(false);
            this.splitTop.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitTop)).EndInit();
            this.splitTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridFields)).EndInit();
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel1.PerformLayout();
            this.splitMain.Panel2.ResumeLayout(false);
            this.splitMain.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridData)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitTop;
        private System.Windows.Forms.DataGridView gridFields;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox listTargets;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.SplitContainer splitMain;
        private System.Windows.Forms.DataGridView gridData;
        private System.Windows.Forms.Label label4;
    }
}