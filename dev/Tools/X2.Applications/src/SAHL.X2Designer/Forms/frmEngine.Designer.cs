namespace SAHL.X2Designer.Forms
{
  partial class frmEngine
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
        this.ddlEngine = new System.Windows.Forms.ComboBox();
        this.label3 = new System.Windows.Forms.Label();
        this.btnClose = new System.Windows.Forms.Button();
        this.btnDone = new System.Windows.Forms.Button();
        this.btnTest = new System.Windows.Forms.Button();
        this.label1 = new System.Windows.Forms.Label();
        this.txtPortNo = new System.Windows.Forms.TextBox();
        this.SuspendLayout();
        // 
        // ddlEngine
        // 
        this.ddlEngine.FormattingEnabled = true;
        this.ddlEngine.Items.AddRange(new object[] {
            "sahls14",
            "sahls118",
            "sahls318",
            "192.168.108.146",
            "192.168.108.164"});
        this.ddlEngine.Location = new System.Drawing.Point(83, 12);
        this.ddlEngine.Name = "ddlEngine";
        this.ddlEngine.Size = new System.Drawing.Size(240, 21);
        this.ddlEngine.TabIndex = 32;
        // 
        // label3
        // 
        this.label3.AutoSize = true;
        this.label3.Location = new System.Drawing.Point(16, 15);
        this.label3.Name = "label3";
        this.label3.Size = new System.Drawing.Size(62, 13);
        this.label3.TabIndex = 33;
        this.label3.Text = "X2 Engine :";
        // 
        // btnClose
        // 
        this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        this.btnClose.Location = new System.Drawing.Point(248, 89);
        this.btnClose.Name = "btnClose";
        this.btnClose.Size = new System.Drawing.Size(75, 23);
        this.btnClose.TabIndex = 36;
        this.btnClose.Text = "Cancel";
        this.btnClose.UseVisualStyleBackColor = true;
        this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
        // 
        // btnDone
        // 
        this.btnDone.DialogResult = System.Windows.Forms.DialogResult.OK;
        this.btnDone.Location = new System.Drawing.Point(167, 89);
        this.btnDone.Name = "btnDone";
        this.btnDone.Size = new System.Drawing.Size(75, 23);
        this.btnDone.TabIndex = 35;
        this.btnDone.Text = "OK";
        this.btnDone.UseVisualStyleBackColor = true;
        this.btnDone.Click += new System.EventHandler(this.btnDone_Click);
        // 
        // btnTest
        // 
        this.btnTest.Location = new System.Drawing.Point(3, 89);
        this.btnTest.Name = "btnTest";
        this.btnTest.Size = new System.Drawing.Size(75, 23);
        this.btnTest.TabIndex = 34;
        this.btnTest.Text = "Test";
        this.btnTest.UseVisualStyleBackColor = true;
        this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
        // 
        // label1
        // 
        this.label1.AutoSize = true;
        this.label1.Location = new System.Drawing.Point(16, 47);
        this.label1.Name = "label1";
        this.label1.Size = new System.Drawing.Size(32, 13);
        this.label1.TabIndex = 37;
        this.label1.Text = "Port: ";
        // 
        // txtPortNo
        // 
        this.txtPortNo.Location = new System.Drawing.Point(83, 40);
        this.txtPortNo.Name = "txtPortNo";
        this.txtPortNo.Size = new System.Drawing.Size(100, 20);
        this.txtPortNo.TabIndex = 38;
        this.txtPortNo.Text = "8089";
        // 
        // frmEngine
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(332, 118);
        this.ControlBox = false;
        this.Controls.Add(this.txtPortNo);
        this.Controls.Add(this.label1);
        this.Controls.Add(this.btnClose);
        this.Controls.Add(this.btnDone);
        this.Controls.Add(this.btnTest);
        this.Controls.Add(this.ddlEngine);
        this.Controls.Add(this.label3);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        this.Name = "frmEngine";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "Connect to X2Engine";
        this.ResumeLayout(false);
        this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ComboBox ddlEngine;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Button btnClose;
    private System.Windows.Forms.Button btnDone;
    private System.Windows.Forms.Button btnTest;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox txtPortNo;
  }
}