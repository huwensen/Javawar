namespace Javawar
{
    partial class JSONViewer
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
            this.jsonViewer1 = new EPocalipse.Json.Viewer.JsonViewer();
            this.SuspendLayout();
            // 
            // jsonViewer1
            // 
            this.jsonViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.jsonViewer1.Json = null;
            this.jsonViewer1.Location = new System.Drawing.Point(0, 0);
            this.jsonViewer1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.jsonViewer1.Name = "jsonViewer1";
            this.jsonViewer1.Size = new System.Drawing.Size(1397, 997);
            this.jsonViewer1.TabIndex = 0;
            // 
            // JSONViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1397, 997);
            this.Controls.Add(this.jsonViewer1);
            this.Name = "JSONViewer";
            this.Text = "JSONViewer";
            this.ResumeLayout(false);

        }

        #endregion

        private EPocalipse.Json.Viewer.JsonViewer jsonViewer1;
    }
}