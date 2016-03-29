namespace SAHL.X2Designer.Forms
{
    partial class frmCallWorkFlowActivitiesNotSet
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
            this.listViewCallWorkFlows = new System.Windows.Forms.ListView();
            this.label1 = new System.Windows.Forms.Label();
            this.colWorkflow = new System.Windows.Forms.ColumnHeader();
            this.colName = new System.Windows.Forms.ColumnHeader();
            this.btnOk = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listViewCallWorkFlows
            // 
            this.listViewCallWorkFlows.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colWorkflow,
            this.colName});
            this.listViewCallWorkFlows.Location = new System.Drawing.Point(12, 43);
            this.listViewCallWorkFlows.Name = "listViewCallWorkFlows";
            this.listViewCallWorkFlows.Size = new System.Drawing.Size(284, 234);
            this.listViewCallWorkFlows.TabIndex = 0;
            this.listViewCallWorkFlows.UseCompatibleStateImageBehavior = false;
            this.listViewCallWorkFlows.View = System.Windows.Forms.View.Details;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(13, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(267, 31);
            this.label1.TabIndex = 1;
            this.label1.Text = "The Following CallWorkFlow Activities Properties must be set before publishing :";
            // 
            // colWorkflow
            // 
            this.colWorkflow.Text = "WorkFlow";
            this.colWorkflow.Width = 150;
            // 
            // colName
            // 
            this.colName.Text = "CallWorkFlow Activity";
            this.colName.Width = 130;
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(221, 296);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 34;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // frmCallWorkFlowActivitiesNotSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(308, 331);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listViewCallWorkFlows);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmCallWorkFlowActivitiesNotSet";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Call WorkFlow Activities";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listViewCallWorkFlows;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ColumnHeader colWorkflow;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.Button btnOk;
    }
}