# Mini C#

Es una implementación pequeña de un compilador para el lenguaje C#

## Tabla de Contenido 🚀

- [Laboratorio A](#Laboratorio-A).
     - [Requerimientos](#Requerimientos-)
          - [Objetivo](#objetivo)
          - [Estructura Lexicográfica](#Estructura-Lexicográfica)
          - [Gramática](#Gramática)
          - [Funcionamiento](#funcionamiento-%EF%B8%8F)
- [Construido con](#construido-con-%EF%B8%8F)
- [Autores](#autores-%EF%B8%8F)
- [Licencia](#licencia-)
 
 


## Laboratorio A

### Requerimientos 📋

#### Objetivo

Este laboratorio consistirá en analizar sintácticamente un programa escrito en el lenguaje
asignado, implementando el método de análisis sintáctico descendente recursivo. Deberán
hacer uso de su analizador léxico de la fase anterior. La finalidad es poder determinar que
el programa fuente escrito está sintácticamente correcto utilizando este método

#### Estructura Lexicográfica

Todas las especificaciones de la fase #1, con el agregado de la palabra reservada Print
para ambos lenguajes. 

#### Gramática

```
Program → Decl Program’
Program’ → Program | ε
Decl → VariableDecl | FunctionDecl
VariableDecl →  Variable ;
Variable → Type ident
Type → int Type’ | double Type’ | string Type’ | ident Type’
Type’ → [] Type’ | ε
FunctionDecl → Type ident (Formals) Stmt | void ident (Formals) Stmt 
Formals → Variable Variable’  , | ε    
Variable’ → Variable Variable’ | ε
Stmt → Stmt’ Stmt | ε
Stmt´ → ForStmt | ReturnStmt | Expr ; 
ForStmt → for ( Expr’ ; Expr ; Expr’) Stmt
Expr’ → Expr | ε 
ReturnStmt → return Expr’  ;
Expr → LValue = P | P 
P → T P’ 
P’ → || T P’ | ε
T → H T’
T’ → && H T’ | ε
H → F H’
H’ →  == F H’ | != F H’ | ε
F → L F’
F’ → < L F’ | > L F’ | <= L F’ | >= L F’ | ε
L → M L’
L’ → + M L’ | - M L’ | ε
M → N M’
M’ → * N M’ | / N M’ | % N M’ | ε
N → - Expr | ! Expr | G
G → (Expr) |Constant | LValue | this | New(ident) 
Lvalue → ident | ident . ident | ident [ Expr ]
Constant → intConstant | doubleConstant | boolConstant | stringConstant | null


```

#### Funcionamiento ⚙️

Los archivos de salida se guardarán en el directorio del proyecto específicamente en la siguiente ruta: \bin\Debug\Salida

A continuación, un pequeño video del funcionamiento de la fase 1

[![VIDEO](https://img.youtube.com/vi/1HR2VJ3BnYc/0.jpg)](https://www.youtube.com/watch?v=1HR2VJ3BnYc)


## Construido con 🛠️

* [.NET](https://dotnet.microsoft.com/download/dotnet-framework/net472) - Framework usado
 


## Autores ✒️

* **Pablo Muralles**  - Carné:1113818
* **Santiago Bocel**  - Cerné:1076818

  

## Licencia 📄

A short and simple permissive license with conditions only requiring preservation of copyright and license notices. Licensed works, modifications, and larger works may be distributed under different terms and without source code. 
see more in [LICENSE](https://github.com/PabloMuralles/Compiladores/blob/master/LICENSE) for more details.

 
