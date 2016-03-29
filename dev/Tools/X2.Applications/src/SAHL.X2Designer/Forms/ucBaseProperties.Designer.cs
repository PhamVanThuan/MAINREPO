namespace SAHL.X2Designer.Forms
{
    partial class ucBaseProperties
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.li = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // li
            // 
            this.li.GridLines = true;
            this.li.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.li.Location = new System.Drawing.Point(3, 3);
            this.li.MultiSelect = false;
            this.li.Name = "li";
            this.li.Size = new System.Drawing.Size(369, 133);
            this.li.TabIndex = 3;
            this.li.UseCompatibleStateImageBehavior = false;
            // 
            // ucBaseProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.li);
            this.Name = "ucBaseProperties";
            this.Size = new System.Drawing.Size(375, 301);
            this.Resize += new System.EventHandler(this.ucBaseProperties_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView li;

    }
}
