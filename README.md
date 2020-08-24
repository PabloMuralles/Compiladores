# Mini C#

Es una implementación pequeña de un compilador para el lenguaje C#

## Tabla de Contenido 🚀

- [Fase 1](#Fase-1)
     - [Requerimientos](#Requerimientos-📋)
          - [Objetivo](#Ojetivo)
          - [Estructura Lexicográfica](#Estructura-Lexicográfica)


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

##### - Palabras clave (reservadas)

     - void int double bool string class const interface null this for while foreach if else
       return break New NewArray Console WriteLine 

##### - Identificadores

     - Un identificador es una secuencia de letras, dígitos y guiones bajos siempre comenzando
       con una letra.

#####  - Case Sensitive

- if es una palabra clave pero IF es un identificador

- binky y Binky son dos identificadores distintos. 

#####  Espacios en blanco

El espacio en blanco (es decir, espacios, tabuladores y saltos de línea) sirve para
separar tokens, pero por lo demás debe ser ignorado. Palabras clave y los
identificadores deben estar separados por espacios en blanco, o por una señal de
que no es ni una palabra ni un identificador.

- if ( 23 this se escanea como cuatro tokens, al igual que if(23this

#####  Comentarios

- De una lines: inicio con // y todo lo que siga hasta el final de la linea sera un comentario se permite cualquier simbolo dentro de estos.

- De varias lineas: inicia con /* y termina con */ cualquier simbolo se acepta en un comentario sin tomar en cuenta */ ya que pone fin al comentario, estos comentarios no se anidan.

#####  Constantes

- Booleanas: Estas pueden ser true o false.

- Entera: 


### Instalación 🔧

_Una serie de ejemplos paso a paso que te dice lo que debes ejecutar para tener un entorno de desarrollo ejecutandose_

_Dí cómo será ese paso_

```
Da un ejemplo
```

_Y repite_

```
hasta finalizar
```

_Finaliza con un ejemplo de cómo obtener datos del sistema o como usarlos para una pequeña demo_

## Ejecutando las pruebas ⚙️

_Explica como ejecutar las pruebas automatizadas para este sistema_

### Analice las pruebas end-to-end 🔩

_Explica que verifican estas pruebas y por qué_

```
Da un ejemplo
```

### Y las pruebas de estilo de codificación ⌨️

_Explica que verifican estas pruebas y por qué_

```
Da un ejemplo
```

 📦



## Construido con 🛠️

_Menciona las herramientas que utilizaste para crear tu proyecto_

* [Dropwizard](http://www.dropwizard.io/1.0.2/docs/) - El framework web usado
* [Maven](https://maven.apache.org/) - Manejador de dependencias
* [ROME](https://rometools.github.io/rome/) - Usado para generar RSS

  🖇️

 
  📖

 
 📌


## Autores ✒️

* **Pablo Muralles**   
* **Santiago Bocel** 

  

## Licencia 📄

Este proyecto está bajo la Licencia (Tu Licencia) - mira el archivo [LICENSE.md](LICENSE.md) para detalles

 
