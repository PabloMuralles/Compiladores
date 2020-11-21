# Mini C#

Es una implementación pequeña de un compilador para el lenguaje C#

## Tabla de Contenido 🚀

- [Fase 1](#Fase-1)
     - [Requerimientos](#Requerimientos-)
          - [Objetivo](#objetivo)
          - [Estructura Lexicográfica](#Estructura-Lexicográfica)
          - [Funcionamiento](#funcionamiento-%EF%B8%8F)
- [Fase 2](#fase-2)
     - [Requerimientos](#requerimientos--1)
          - [Gramática Implementada](#gramática-implementada--)
          - [Tabla de Análisis](#tabla-de-análisis--)
          - [Manejo de Errores](#manejo-de-errores-)
          - [Ejecutando las pruebas](#ejecutando-las-pruebas-%EF%B8%8F)
 - [Fase 2](#fase-3)
     - [Requerimientos](#requerimientos--1)
           
- [Construido con](#construido-con-%EF%B8%8F)
- [Autores](#autores-%EF%B8%8F)
- [Licencia](#licencia-)
 
 


## Fase 1 

### Requerimientos 📋

#### Objetivo

En el primer proyecto de programación, ustedes iniciarán su compilador con la aplicación
del análisis léxico. Para la primera tarea del front-end, crearán un escáner para el lenguaje
de programación asignado. El escáner irá reconociendo los tokens en el orden en que se
leen, hasta el final del archivo. Para cada lenguaje, el escáner determinará sus atributos
adecuadamente (estos eventualmente serán utilizados por otros componentes de su
compilador) para que la información sobre cada símbolo deba estar correctamente impresa. 

#### Estructura Lexicográfica

##### ● Palabras clave (reservadas)

void int double bool string class const interface null this for while foreach if else 
return break New NewArray Console WriteLine 

##### ● Identificadores

Un identificador es una secuencia de letras, dígitos y guiones bajos siempre comenzando
con una letra.

```
Correcto:
cadeba_1
num1
listNum_

Incorrecto:
12num
_cadena
```
#####  ● Case Sensitive

-Distingue entre mayúsculas y minúsculas
```
if es una palabra clave pero IF es un identificador

binky y Binky son dos identificadores distintos. 
```

##### ● Espacios en blanco

El espacio en blanco (es decir, espacios, tabuladores y saltos de línea) sirve para
separar tokens, pero por lo demás debe ser ignorado. Palabras clave y los
identificadores deben estar separados por espacios en blanco, o por una señal de
que no es ni una palabra ni un identificador.

```if ( 23 this se escanea como cuatro tokens, al igual que if(23this```

##### ● Comentarios

-De una lines: inicio con ```//``` y todo lo que siga hasta el final de la linea sera un comentario se permite cualquier simbolo dentro de estos.

-De varias lineas: inicia con ```/*``` y termina con ```*/``` cualquier simbolo se acepta en un comentario sin tomar en cuenta '*/' ya que pone fin al comentario, estos comentarios no se anidan.

##### ● Constantes

-Booleanas: Estas pueden ser ```true``` o ```false```.

-Entero: Puede expresarse en base 10 o base 16 
     -Los de base 10 deben ser una secuencia de digitos de 0-9
     -Los de base 16 deben de empezar con ```0``` seguido de ```x``` o ```X ```    

```
Correcto:
4
50
0487
0xFf217
0XA41E1

Incorrecto:
FF217
5.2
.5
 ```
-Doubles: Es una secuencia de digitos, un punto, segudio de una secuencia de digitos o nada. Tambien después del puntos puede venir un numero o nada seguido de una notación científica que puede o no tener el signo del exponente si no lo tiene se asume que es un ``+``

```
Correcto:
58.5
13.
0.15
0.2E2
28.2e-2
48.4e+2

Incorrecto:
.78
.44E15
45.3.15
```

-Strings: Es una secuencia de caracterres dentro de commillas dobles ``""``. Estas no pueden contener ``"`` , una linea nueva, o un caracter nulo. Deben de comenzar y terminar en la misma linea no se permite que comiense en una y termine en otra.

```
Correcto: 
"esto cadena es un string correcta"

Incorrecto:
"esta es una parte de la cadena
esto es otra parte de la cadena

"será que esto
esta bueno jejeje"
```

##### ● Operadores y caracteres de puntuación

```
+ - * / % < <= > >= = == != && || ! ; , . [ ] ( ) { } [] () {}
```

#### Funcionamiento ⚙️

Los archivos de salida se guardarán en el directorio del proyecto específicamente en la siguiente ruta: \bin\Debug\Salida

A continuación, un pequeño video del funcionamiento de la fase 1

[![VIDEO](https://img.youtube.com/vi/1HR2VJ3BnYc/0.jpg)](https://www.youtube.com/watch?v=1HR2VJ3BnYc)

## Fase 2

### Requerimientos 📋

#### Gramática Implementada  📃 

```
S' -> Program
Program -> Decl Program
Program -> Decl 
Decl -> Type ident ;
Decl ->Type ident ( Formals ) StmtBlock
Decl ->void ident ( Formals ) StmtBlock
Decl -> class ident Id Id' { Field’ }
Decl -> interface ident { Prototype’ }
Decl -> const ConstType ident ; 
Type -> int
Type -> double
Type -> bool
Type -> string
Type -> ident
Type -> Type []
ConstType -> int
ConstType -> double
ConstType -> bool
ConstType -> string
Formals -> Type ident , Formals
Formals -> Type ident
Id -> : ident
Id -> ε
Id' -> , ident Id'
Id' -> ε
Field’ -> Field Field’
Field’ -> ε
Field -> Type ident ;
Field -> Type ident ( Formals ) StmtBlock
Field -> void ident ( Formals ) StmtBlock
Field -> const ConstType ident ;
Prototype’ ->  Prototype Prototype’
Prototype’ -> ε
Prototype -> Type ident ( Formals ) ;
Prototype -> void ident ( Formals ) ;
StmtBlock -> { Declare }
Declare -> VariableDecl Declare
Declare -> ConstDecl Declare
Declare -> Stmt’ Declare
Declare -> ε
ConstDecl-> const ConstType ident ;
VariableDecl -> Type ident ;
Stmt’ -> Stmt
Stmt -> Expr ;
Stmt -> ;
Stmt ->  if ( Expr ) Stmt IfStmt
Stmt -> while ( Expr ) Stmt
Stmt -> for ( Expr ; Expr ; Expr ) Stmt
Stmt -> break ;
Stmt -> return Expr ;
Stmt -> Console . WriteLine ( Expr’ ) ;
Stmt -> StmtBlock
Stmt -> ident ( Actuals )
Stmt -> ident . ident ( Actuals )
Actuals -> Expr , Actuals
Actuals -> Expr 
IfStmt -> else Stmt
IfStmt -> ε
Expr’ -> Expr , Expr’
Expr’ -> Expr 
Expr -> ident = ConditionAnd
Expr -> ConditionAnd
ConditionAnd -> Equality ConditionAnd'
ConditionAnd' -> && Equality ConditionAnd'
ConditionAnd' -> ε
Equality -> Equality == Relational
Equality -> Relational
Relational -> Relational < Additive
Relational -> Relational <= Additive
Relational -> Additive
Additive -> Additive + Multiplicative
Additive -> Multiplicative
Multiplicative -> Multiplicative * Unary
Multiplicative -> Multiplicative % Unary
Multiplicative -> Unary 
Unary -> - Primary
Unary -> ! Primary 
Unary -> Primary
Primary -> Primary . ident = Expr
Primary -> Primary . ident  
Primary -> Terminal
Terminal -> this
Terminal -> ( Expr )
Terminal -> New ( ident )
Terminal -> intConstant
Terminal -> doubleConstant
Terminal -> boolConstant
Terminal -> stringConstant
Terminal -> null
Terminal -> ident
```


##### Documento con la gramática: [DOC](https://github.com/PabloMuralles/Compiladores/blob/Analizador_sint%C3%A1ctico/Documentacion/Gram%C3%A1tica%20Corregida.pdf)

##### Follows: [DOC](https://github.com/PabloMuralles/Compiladores/blob/Analizador_sint%C3%A1ctico/Documentacion/Follows.pdf)



#### Tabla de Análisis  📜

El analizador es de tipo SLR

##### Documento con la tabla de análisis: [DOC](https://github.com/PabloMuralles/Compiladores/blob/fase3/Documentacion/Tabla%20de%20Analisis.xlsx)

#### Manejo de Errores ❌

El analizar sintáctico a la hora de encontrar un error consume el token donde marco error y guarda el error para posteriormente mostrarlo al usuario, todo lo que resta de esa línea lo ignora y salta de línea, a la hora de saltar de línea el análisis sintáctico empieza de cero. Si todos los tokens de entrada son consumidos y llega al estado de aceptación indicara que el código ingresado es correcto de lo contrario mostrara los errores en pantalla.

#### Ejecutando las pruebas ⚙️

El analizador cargar su tabla de análisis y las producciones de la gramática desde archivos de texto que se encuentran en la carpeta Gramática en la solución de la aplicación por lo tanto para poder realizar pruebas es necesario tener en cuenta que esta carpeta exista con sus archivos para que funcione de la manera correcta.

Ruta de los archivos anteriores: 

Ruta Relativa:

"\Gramatica\Gramatica.txt"

"\Gramatica\Tabla_analisis.txt"

## Fase 3

### Requerimientos 📋
     
#### Objetivo

La tercera fase del proyecto consistirá en generar la tabla de símbolos, realizar la asignación
de valores a variables y constantes, comprobar los tipos de ciertas expresiones para los
lenguajes que están trabajando

#### Estructura de la Tabla de Símbolos
```
TYPE:class| NAME:Parser| VALUE:NULL|AMBIT:NULL 
TYPE:int| NAME:a1| VALUE:8|AMBIT:Programmain 
TYPE:int| NAME:f1| VALUE:3|AMBIT:Parserf1 
TYPE:int| NAME:a| VALUE:NULL|AMBIT:Parserf1 
TYPE:int| NAME:b| VALUE:NULL|AMBIT:Parserf1 
TYPE:int| NAME:c| VALUE:NULL|AMBIT:Parserf1 
TYPE:int| NAME:p1| VALUE:3|AMBIT:Parserf1 
TYPE:void| NAME:proc1| VALUE:NULL|AMBIT:Parserproc1 
TYPE:double| NAME:y| VALUE:NULL|AMBIT:Parserproc1 
TYPE:class| NAME:Program| VALUE:NULL|AMBIT:NULL 
TYPE:void| NAME:main| VALUE:NULL|AMBIT:Programmain 
TYPE:string| NAME:x| VALUE:NULL|AMBIT:Programmain 
TYPE:double| NAME:m1| VALUE:7.5|AMBIT:Programmain 
TYPE:int| NAME:m3| VALUE:NULL|AMBIT:Programmain 
TYPE:bool| NAME:t| VALUE:NULL|AMBIT:Programmain 
TYPE:string| NAME:mensaje| VALUE: hola   mundo |AMBIT:Programmain 
TYPE:ident| NAME:MyParser| VALUE:NULL|AMBIT:Programmain 
TYPE:int| NAME:m2| VALUE:NULL|AMBIT:Programmain 
TYPE:bool| NAME:p| VALUE:False|AMBIT:Programmain 
TYPE:=| NAME:t| VALUE:NULL|AMBIT:Program 
```
#### Mantenimiento de la Símbolos

La tabla de símbolos se le da mantenimiento cada vez que se defina una variable, clase, procedimiento o función. Se verifica que ya exista y sino existe se agrega a la tabla de símbolos, también cuando se realiza una asignación o un return se actualiza el valor del elemento de la tabla de símbolos al que se le esta asignando.

#### Manejo de Errores ❌

 

## Construido con 🛠️

* [.NET](https://dotnet.microsoft.com/download/dotnet-framework/net472) - Framework usado

## Autores ✒️

**Pablo Muralles**  - Carné:1113818

**Santiago Bocel**  - Carné:1076818

## Licencia 📄

A short and simple permissive license with conditions only requiring preservation of copyright and license notices. Licensed works, modifications, and larger works may be distributed under different terms and without source code. 
see more in [LICENSE](https://github.com/PabloMuralles/Compiladores/blob/master/LICENSE) for more details.
