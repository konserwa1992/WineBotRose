namespace CodeInject.UIPanels
{
    partial class BackToCenterPanel
    {
        /// <summary> 
        /// Wymagana zmienna projektanta.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Wyczyść wszystkie używane zasoby.
        /// </summary>
        /// <param name="disposing">prawda, jeżeli zarządzane zasoby powinny zostać zlikwidowane; Fałsz w przeciwnym wypadku.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kod wygenerowany przez Projektanta składników

        /// <summary> 
        /// Metoda wymagana do obsługi projektanta — nie należy modyfikować 
        /// jej zawartości w edytorze kodu.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.lX = new System.Windows.Forms.Label();
            this.lY = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.bGetPosition = new System.Windows.Forms.Button();
            this.lRadius = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(48, 14);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 0;
            // 
            // lX
            // 
            this.lX.AutoSize = true;
            this.lX.Location = new System.Drawing.Point(25, 17);
            this.lX.Name = "lX";
            this.lX.Size = new System.Drawing.Size(17, 13);
            this.lX.TabIndex = 1;
            this.lX.Text = "X:";
            // 
            // lY
            // 
            this.lY.AutoSize = true;
            this.lY.Location = new System.Drawing.Point(25, 43);
            this.lY.Name = "lY";
            this.lY.Size = new System.Drawing.Size(17, 13);
            this.lY.TabIndex = 3;
            this.lY.Text = "Y:";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(48, 40);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 20);
            this.textBox2.TabIndex = 2;
            // 
            // bGetPosition
            // 
            this.bGetPosition.Location = new System.Drawing.Point(48, 89);
            this.bGetPosition.Name = "bGetPosition";
            this.bGetPosition.Size = new System.Drawing.Size(100, 23);
            this.bGetPosition.TabIndex = 4;
            this.bGetPosition.Text = "Get Position";
            this.bGetPosition.UseVisualStyleBackColor = true;
            this.bGetPosition.Click += new System.EventHandler(this.bGetPosition_Click);
            // 
            // lRadius
            // 
            this.lRadius.AutoSize = true;
            this.lRadius.Location = new System.Drawing.Point(3, 66);
            this.lRadius.Name = "lRadius";
            this.lRadius.Size = new System.Drawing.Size(43, 13);
            this.lRadius.TabIndex = 6;
            this.lRadius.Text = "Radius:";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(48, 63);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(100, 20);
            this.textBox3.TabIndex = 5;
            this.textBox3.Text = "50";
            // 
            // BackToCenterPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lRadius);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.bGetPosition);
            this.Controls.Add(this.lY);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.lX);
            this.Controls.Add(this.textBox1);
            this.Name = "BackToCenterPanel";
            this.Size = new System.Drawing.Size(182, 150);
            this.Load += new System.EventHandler(this.BackToCenterPanel_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label lX;
        private System.Windows.Forms.Label lY;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button bGetPosition;
        private System.Windows.Forms.Label lRadius;
        private System.Windows.Forms.TextBox textBox3;
    }
}
