namespace Gestionale
{
    partial class Form1
    {
        /// <summary>
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Pulire le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione Windows Form

        /// <summary>
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent() {
            this.createTable = new System.Windows.Forms.Button();
            this.zonaPazienti = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // createTable
            // 
            this.createTable.Location = new System.Drawing.Point(12, 12);
            this.createTable.Name = "createTable";
            this.createTable.Size = new System.Drawing.Size(129, 32);
            this.createTable.TabIndex = 0;
            this.createTable.Text = "Crea Tabelle";
            this.createTable.UseVisualStyleBackColor = true;
            this.createTable.Click += new System.EventHandler(this.createTable_Click);
            // 
            // zonaPazienti
            // 
            this.zonaPazienti.Location = new System.Drawing.Point(471, 12);
            this.zonaPazienti.Name = "zonaPazienti";
            this.zonaPazienti.Size = new System.Drawing.Size(95, 32);
            this.zonaPazienti.TabIndex = 1;
            this.zonaPazienti.Text = "Pazienti";
            this.zonaPazienti.UseVisualStyleBackColor = true;
            this.zonaPazienti.Click += new System.EventHandler(this.zonaPazienti_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(578, 533);
            this.Controls.Add(this.zonaPazienti);
            this.Controls.Add(this.createTable);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Home";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button createTable;
        private System.Windows.Forms.Button zonaPazienti;
    }
}

