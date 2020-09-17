/*
 * Created by SharpDevelop.
 * User: david
 * Date: 17/09/2020
 * Time: 10:15 a. m.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace Analizador
{
	/// <summary>
	/// Description of Token.
	/// </summary>
	public class Token {
		private States states;
		private String token;
		private int row, colum, id;
		
		
		public Token() { }
		
		public Token(Token token) {
			this.states	= token.states;
			this.token	= token.token;
			this.row	= token.row;
			this.colum= token.colum;
			this.id		= token.id;
		}
		
		public enum States {
			q1,//id
			q2,//;
			q3,//,
			q4,//(
			q5,//)
			q6,//{
			q7,//}
			q8,//=
			q9,//entero
			q10,//flotante
			q11,//(+, -)
			q12,
			q13,
			q14,
			q15,
			q16,
			qTipo,
			qIf, qWhile, qReturn, qElse,
			qFin,
			qError
		}
		
	
		
		public Token(States statusToken, String token, int row, int column) {
			this.states = statusToken;
			this.token  = token;
			this.row    = row;
			this.colum  = column;
		}
		
		public string TokenName {get { return token; } }
		public int Row          {get { return row; } }
		public int Column       {get { return colum; } }
		public int Id           {get { return id; } set {id = value;} }
		

		public String getStatus() {
			switch(states) {
			case States.q1://variable o palabra reservada
				id = 1;
				return "variable";
			case States.q2://punto y coma
				id = 2;
				return  "punto y coma";
			case States.q3://coma
				id = 3;
				return "coma";
			case States.q4://parentesis apertura
				id = 4;
				return  "parentesis izq";
			case States.q5://parentesis cierre
				id = 5;
				return "parentesis der";
			case States.q6://llave apertura
				id = 6;
				return  "llave izq";
			case States.q7://llave cierre
				id = 7;
				return "llave der";
			case States.q8://igual
				id = 8;
				return  "asignacion";
			case States.q9://numero entero
				id = 13;
				return "entero";
			case States.q11://numero flotante
				id = 13;
				return  "flotante";
			case States.q12:// mas o menos
				id = 14;
				return "operador suma";
			case States.q14://operador logico
				id = 15;
				return "operador logico";
			case States.q15://mult, div o mod
				id = 16;
				return "operador mult";
			case States.q16://fin
				id = 17;
				return  "operador relacional";
			case States.qTipo:
				id = 0;
				return "tipo de dato";
			case States.qIf:
				id = 9;
				return "if";
			case States.qWhile:
				id = 10;
				return "while";
			case States.qReturn:
				id = 11;
				return "return";
			case States.qElse:
				id = 12;
				return "else";
			case States.qFin:
				id = 18;
				return "fin ejecucion";
			default:
				id = -1;
				return "error";
			}
		}
	
		
		public override String ToString() {
			return TokenName + " | " + getStatus() + " | " + id.ToString();
		}
	}
}
