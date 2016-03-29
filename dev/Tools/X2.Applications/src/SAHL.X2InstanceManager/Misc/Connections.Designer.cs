namespace SAHL.X2InstanceManager.Misc
{
    partial class Connections
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
            this.li = new System.Windows.Forms.ListBox();
            this.txtX2 = new System.Windows.Forms.TextBox();
            this.txt2am = new System.Windows.Forms.TextBox();
            this.txtEngine = new System.Windows.Forms.TextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // li
            // 
            this.li.FormattingEnabled = true;
            this.li.Location = new System.Drawing.Point(12, 12);
            this.li.Name = "li";
            this.li.Size = new System.Drawing.Size(120, 212);
            this.li.TabIndex = 0;
            this.li.SelectedIndexChanged += new System.EventHandler(this.li_SelectedIndexChanged);
            // 
            // txtX2
            // 
            this.txtX2.Location = new System.Drawing.Point(147, 12);
            this.txtX2.Name = "txtX2";
            this.txtX2.Size = new System.Drawing.Size(738, 20);
            this.txtX2.TabIndex = 1;
            // 
            // txt2am
            // 
            this.txt2am.Location = new System.Drawing.Point(147, 38);
            this.txt2am.Name = "txt2am";
            this.txt2am.Size = new System.Drawing.Size(738, 20);
            this.txt2am.TabIndex = 2;
            // 
            // txtEngine
            // 
            this.txtEngine.Location = new System.Drawing.Point(147, 64);
            this.txtEngine.Name = "txtEngine";
            this.txtEngine.Size = new System.Drawing.Size(397, 20);
            this.txtEngine.TabIndex = 3;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(147, 198);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "Done";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // Connections
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(897, 233);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.txtEngine);
            this.Controls.Add(this.txt2am);
            this.Controls.Add(this.txtX2);
            this.Controls.Add(this.li);
            this.Name = "Connections";
            this.Text = "Connections";
            this.Load += new System.EventHandler(this.Connections_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox li;
        private System.Windows.Forms.TextBox txtX2;
        private System.Windows.Forms.TextBox txt2am;
        private System.Windows.Forms.TextBox txtEngine;
        private System.Windows.Forms.Button btnOk;
    }
}