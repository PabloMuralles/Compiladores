# Mini C#

Es una implementación pequeña de un compilador para el lenguaje C#

## Tabla de Contenido 🚀

- [Fase 1](#Fase-1).
     - [Requerimientos](#Requerimientos-)
          - [Objetivo](#objetivo)
          - [Estructura Lexicográfica](#Estructura-Lexicográfica)
          - [Funcionamiento](#funcionamiento-%EF%B8%8F)
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


## Construido con 🛠️

* [.NET](https://dotnet.microsoft.com/download/dotnet-framework/net472) - Framework usado
 


## Autores ✒️

* **Pablo Muralles**  - Carné:1113818
* **Santiago Bocel**  - Cerné:1076818

  

## Licencia 📄

A short and simple permissive license with conditions only requiring preservation of copyright and license notices. Licensed works, modifications, and larger works may be distributed under different terms and without source code. 
see more in [LICENSE](https://github.com/PabloMuralles/Compiladores/blob/master/LICENSE) for more details.

 
