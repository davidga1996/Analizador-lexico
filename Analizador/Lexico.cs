/*
 * Created by SharpDevelop.
 * User: david
 * Date: 17/09/2020
 * Time: 10:15 a. m.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

namespace Analizador{
	
	public class Lexico {
		
		private List<Token> departure;
		private int state;
		private String aux;
		
		public Lexico() { }
		
		public List<Token> escanear(String entry) {
			entry += " $";//fin de la cadena
			departure = new List<Token>();
			int fila=1,columna=1, count = 0;
			state = 0;
			aux = "";
			
			char character = entry[count++];
			
			while(character.CompareTo('$') != 0) {
				switch(state) {
					case 0:
						if(char.IsLetter(character) || character.CompareTo('_')==0) {
							state = 1;
							aux += character;
						}else if(character.CompareTo(';')==0) {
							aux += character;
							agregarToken(Token.States.q2, fila, columna);
						}else if(character.CompareTo(',')==0) {
							aux += character;
							agregarToken(Token.States.q3, fila, columna);
						}else if(character.CompareTo('(')==0) {
							aux += character;
							agregarToken(Token.States.q4, fila, columna);
						}else if(character.CompareTo(')')==0) {
							aux += character;
							agregarToken(Token.States.q5, fila, columna);
						}else if(character.CompareTo('{')==0) {
							aux += character;
							agregarToken(Token.States.q6, fila, columna);
						}else if(character.CompareTo('}')==0) {
							aux += character;
							agregarToken(Token.States.q7, fila, columna);
						}else if(character.CompareTo('=')==0 || character.CompareTo('<')==0 ||character.CompareTo('>')==0 ||character.CompareTo('!')==0) {
							state = 16;
							aux += character;
						}else if(char.IsDigit(character)) {
							state = 9;
							aux += character;
						}else if(character.CompareTo('.')==0) {
							state = 10;
							aux += character;
						}else if(character.CompareTo('+')==0 || character.CompareTo('-')==0) {
							aux += character;
							agregarToken(Token.States.q12, fila, columna);
						}else if(character.CompareTo('&')==0 || character.CompareTo('|')==0) {
							state = 14;
							aux += character;
						}else if(character.CompareTo('*')==0 || character.CompareTo('/')==0 || character.CompareTo('%')==0) {
							aux += character;
							agregarToken(Token.States.q15, fila, columna);
						}else if(!isSpace(character)){
							//en el caso de que no cumpla la condicion y no sea algun tipo de espacis
							aux += character;
							state = -1;//error
						}
						break;
					case 1:
						if(char.IsLetter(character) || character.CompareTo('_')==0 || char.IsDigit(character)) {
							aux += character;
						} else if(!(char.IsDigit(character) && character.CompareTo('.')==0)) {
							//verificar si el token es un tipo de dato o una palabra reservada
							if(isDataType(aux)) {
								agregarToken(Token.States.qTipo, fila, columna);
							}else {
								agregarToken(isReservedWord(aux), fila, columna);
							}
							count--;
						} else {
							aux += character;
							state = -1;
						}
						break;
					case 9:
						if(char.IsDigit(character)) {
							//el estado no cambia
							aux += character;
						}else if(character.CompareTo('.')==0) {
							aux += character;
							state = 10;
						}else if(char.IsLetter(character)) {
							//llevarme a un caso de error
							aux += character;
							state = -1;
						} else if(isSpace(character)) {
							agregarToken(Token.States.q9, fila, columna);
						} else {
							agregarToken(Token.States.q9, fila, columna);
							count--;
							state = 0;
						}
						break;
					case 10:
						if(char.IsDigit(character)) {
							aux += character;
							state = 11;
						} else if(char.IsLetter(character)) {
							//llevarme a un caso de error
							aux += character;
							state = -1;
						} else if(isSpace(character)) {
							aux += '0';
							agregarToken(Token.States.q11, fila, columna);
						} else {
							aux += '0';
							count--;
							agregarToken(Token.States.q11, fila, columna);
						}
						break;
					case 11:
						if(char.IsDigit(character)) {
							//el estado no cambia
							aux += character;
						} else if(char.IsLetter(character) || character.CompareTo('.')==0) {
							//llevarme a un caso de error
							aux += character;
							state = -1;
						} else if(isSpace(character)) {
							agregarToken(Token.States.q11, fila, columna);
						} else {
							agregarToken(Token.States.q11, fila, columna);
							count--;
							state = 0;
						}
						break;
					case 14:
						if((character.CompareTo('&')==0 || character.CompareTo('|')==0) && aux[0] == character) {
							aux += character;
							agregarToken(Token.States.q14, fila, columna);
						}else if(character.CompareTo('&')==0 || character.CompareTo('|')==0) {
							//llevarme a un caso de error
							aux += character;
							state = -1;
							agregarToken(Token.States.qError, fila, columna);
						} else {
							count--;
							agregarToken(Token.States.qError, fila, columna);
						}
						break;
					case 16:
						if(!(character.CompareTo('=') == 0) && aux == "=") {
							//analizar la asignacion
							count--;
							agregarToken(Token.States.q8, fila, columna);
						} else if(!(character.CompareTo('=') == 0)) {
							count--;
							agregarToken(Token.States.q16, fila, columna);
						}else if(character.CompareTo('=')==0) {
							//solo <=, >=, !=, == 
							aux += character;
							agregarToken(Token.States.q16, fila, columna);
						}
						break;
					case -1:
						if(char.IsDigit(character) || char.IsLetter(character) || character.CompareTo('.')==0) {
							aux += character;
						} else {
							count--;
							agregarToken(Token.States.qError, fila, columna);
						}
						break;
				}
				
				if(character.CompareTo('\n')==0) {
					fila++;
					columna=1;
				}else {
					columna++;
				}
				
				//reconocer los Statess	
				character = entry[count++];
			}
			
			return departure;
		}
		
		public void agregarToken(Token.States tipo, int fila, int columna) {
			departure.Add(new Token(tipo, aux, fila, columna-aux.Length));
			aux= "";
			state = 0;
		}
		
		bool isSpace(char c) {
			return c.CompareTo(' ')==0 || c.CompareTo('\t')==0 || c.CompareTo('\n')==0;
		}
		
		bool isDataType(String str) {
			if(str == "int" || str == "string" || str == "char" || str == "bool" || str == "double" || str == "float" || str == "void") {
				return true;
			}
			return false;
		}
		
		Token.States isReservedWord(String cadena) {
			if(cadena == "if") {
				return Token.States.qIf;
			} else if(cadena == "while") {
				return Token.States.qWhile;
			}else if (cadena == "return") {
				return Token.States.qReturn;
			}else if (cadena == "else") {
				return Token.States.qElse;
			}
			return Token.States.q1;
		}
	
	}
}
