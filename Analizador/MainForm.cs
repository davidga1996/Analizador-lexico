/*
 * Created by SharpDevelop.
 * User: david
 * Date: 17/09/2020
 * Time: 02:53 p. m.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace Analizador
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form {
		Lexico lex = new Lexico();
		OpenFileDialog openFile = new OpenFileDialog();
		public MainForm() {
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		void EjecutarToolStripMenuItemClick(object sender, EventArgs e) {
			String entry = textBox.Text;
			
			List<Token> ltokens = lex.escanear(entry);
			
			dataTable.Rows.Clear();
			
			foreach(Token token in ltokens) {
				dataTable.Rows.Add(token.getStatus(), token.TokenName, token.Row, token.Column, token.Id);
			}
		}
		void ToolStripMenuItem3Click(object sender, EventArgs e) {
			if(openFile.ShowDialog() == DialogResult.OK) {
				textBox.Text = File.ReadAllText(openFile.FileName);
			}
		}
		void ToolStripMenuItem4Click(object sender, EventArgs e) {
			if(textBox.Text != "") {
				//no esta vacio
				MessageBox.Show("se borro");
				textBox.Text = "";
			}
		}
	}
}
