namespace SelectionTest {
    partial class FormSelectTest {
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
            this.btnOpenXls = new System.Windows.Forms.Button();
            this.gridData = new System.Windows.Forms.DataGridView();
            this.btnOpenCsv = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridData)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOpenXls
            // 
            this.btnOpenXls.Location = new System.Drawing.Point(1, 2);
            this.btnOpenXls.Name = "btnOpenXls";
            this.btnOpenXls.Size = new System.Drawing.Size(75, 23);
            this.btnOpenXls.TabIndex = 0;
            this.btnOpenXls.Text = "Open Xls";
            this.btnOpenXls.UseVisualStyleBackColor = true;
            this.btnOpenXls.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // gridData
            // 
            this.gridData.AllowUserToAddRows = false;
            this.gridData.AllowUserToDeleteRows = false;
            this.gridData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridData.Location = new System.Drawing.Point(1, 31);
            this.gridData.Name = "gridData";
            this.gridData.Size = new System.Drawing.Size(437, 264);
            this.gridData.TabIndex = 1;
            // 
            // btnOpenCsv
            // 
            this.btnOpenCsv.Location = new System.Drawing.Point(82, 2);
            this.btnOpenCsv.Name = "btnOpenCsv";
            this.btnOpenCsv.Size = new System.Drawing.Size(75, 23);
            this.btnOpenCsv.TabIndex = 2;
            this.btnOpenCsv.Text = "Open Csv";
            this.btnOpenCsv.UseVisualStyleBackColor = true;
            this.btnOpenCsv.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // FormSelectTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(438, 295);
            this.Controls.Add(this.btnOpenCsv);
            this.Controls.Add(this.gridData);
            this.Controls.Add(this.btnOpenXls);
            this.Name = "FormSelectTest";
            this.Text = "Field Selection Test";
            ((System.ComponentModel.ISupportInitialize)(this.gridData)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOpenXls;
        private System.Windows.Forms.DataGridView gridData;
        private System.Windows.Forms.Button btnOpenCsv;
    }
}

